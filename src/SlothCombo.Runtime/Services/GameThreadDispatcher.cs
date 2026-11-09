using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// Marshals work from the threads of the outside world onto the thread of the game.
/// </summary>
/// <remarks>
/// The work of the interface has to run on the thread of the game, because
/// that is the only place where the state of the game may be read and
/// written.
/// </remarks>
internal sealed class GameThreadDispatcher
{
    private readonly ConcurrentQueue<PendingWork> _queue = new();
    private readonly IFramework _framework;
    private bool _subscribed;

    /// <summary>Creates the dispatcher for the given update loop.</summary>
    /// <param name="framework">The update loop of the game.</param>
    public GameThreadDispatcher(IFramework framework) => _framework = framework;

    /// <summary>Subscribes to the update loop of the game.</summary>
    public void Start()
    {
        if (_subscribed)
        {
            return;
        }

        _framework.Update += OnUpdate;
        _subscribed = true;
    }

    /// <summary>Unsubscribes from the update loop of the game.</summary>
    public void Stop()
    {
        if (!_subscribed)
        {
            return;
        }

        _framework.Update -= OnUpdate;
        _subscribed = false;

        // Everything that is still waiting fails, so that no request hangs.
        while (_queue.TryDequeue(out var pending))
        {
            pending.Completion.TrySetException(new InvalidOperationException("The runtime is shutting down."));
        }
    }

    /// <summary>Queues work and waits for its result.</summary>
    /// <typeparam name="T">The kind of the result.</typeparam>
    /// <param name="work">The work to do on the thread of the game.</param>
    /// <returns>The result of the work.</returns>
    public async Task<T> RunAsync<T>(Func<T> work)
    {
        var pending = new PendingWork { Work = () => work() };
        _queue.Enqueue(pending);
        var result = await pending.Completion.Task.ConfigureAwait(false);
        return (T)result!;
    }

    /// <summary>Queues work and waits for it to finish.</summary>
    /// <param name="work">The work to do on the thread of the game.</param>
    public async Task RunAsync(Action work)
    {
        var pending = new PendingWork { Work = () => { work(); return null; } };
        _queue.Enqueue(pending);
        await pending.Completion.Task.ConfigureAwait(false);
    }

    /// <summary>Does the work that was queued.</summary>
    private void OnUpdate(IFramework framework)
    {
        while (_queue.TryDequeue(out var pending))
        {
            try
            {
                pending.Completion.TrySetResult(pending.Work());
            }
            catch (Exception ex)
            {
                pending.Completion.TrySetException(ex);
            }
        }
    }

    /// <summary>One piece of work that waits for its turn.</summary>
    private sealed class PendingWork
    {
        /// <summary>Gets or sets the work itself.</summary>
        public Func<object?> Work { get; init; } = null!;

        /// <summary>Gets the place the result of the work is written to.</summary>
        public TaskCompletionSource<object?> Completion { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }
}
