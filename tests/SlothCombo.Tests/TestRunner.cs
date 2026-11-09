using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SlothCombo.Tests;

/// <summary>
/// A very small test runner.
/// </summary>
/// <remarks>
/// The tests of this project are about the parts that can be checked without
/// a running game, so the runner only needs to run a handful of them and
/// report what happened.
/// </remarks>
internal sealed class TestRunner
{
    private readonly List<(string Name, Action Test)> _tests = [];

    /// <summary>Adds a test to the run.</summary>
    /// <param name="name">The name of the test.</param>
    /// <param name="test">The test itself.</param>
    public void Add(string name, Action test) => _tests.Add((name, test));

    /// <summary>
    /// Runs every test and reports the result.
    /// </summary>
    /// <returns>Zero when every test passed.</returns>
    public int Run()
    {
        var failed = 0;
        var skipped = 0;

        foreach (var (name, test) in _tests)
        {
            var watch = Stopwatch.StartNew();

            try
            {
                test();
                watch.Stop();
                Console.WriteLine($"  passed  {name} ({watch.ElapsedMilliseconds} ms)");
            }
            catch (SkipException ex)
            {
                watch.Stop();
                skipped++;
                Console.WriteLine($"  skipped  {name} ({ex.Message})");
            }
            catch (Exception ex)
            {
                watch.Stop();
                failed++;
                Console.WriteLine($"  FAILED  {name} ({watch.ElapsedMilliseconds} ms)");
                Console.WriteLine($"          {ex.GetType().Name}: {ex.Message}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"{_tests.Count - failed - skipped} of {_tests.Count} tests passed, {skipped} skipped.");
        return failed == 0 ? 0 : 1;
    }
}

/// <summary>The reason a test was not run.</summary>
internal sealed class SkipException : Exception
{
    /// <summary>Creates the exception with the given message.</summary>
    /// <param name="message">The reason of the skip.</param>
    public SkipException(string message) : base(message)
    {
    }
}

/// <summary>The failure of a test.</summary>
internal sealed class TestFailure : Exception
{
    /// <summary>Creates the failure with the given message.</summary>
    /// <param name="message">The reason of the failure.</param>
    public TestFailure(string message) : base(message)
    {
    }
}

/// <summary>The checks the tests are written with.</summary>
internal static class Assert
{
    /// <summary>Fails the test when the condition does not hold.</summary>
    /// <param name="condition">The condition that has to hold.</param>
    /// <param name="message">The reason of the failure.</param>
    public static void True(bool condition, string message)
    {
        if (!condition)
        {
            throw new TestFailure(message);
        }
    }

    /// <summary>Fails the test when the two values are not the same.</summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="expected">The value that was expected.</param>
    /// <param name="actual">The value that was found.</param>
    public static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new TestFailure($"'{expected}' was expected, but '{actual}' was found.");
        }
    }
}
