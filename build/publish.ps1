<#
.SYNOPSIS
    Assembles the installation of the standalone program.

.DESCRIPTION
    The installation is laid out the way the runtime of the system is laid
    out, because that is the only shape the library that starts the runtime
    understands:

        <output>/
            SlothCombo.Host.exe          the program that injects the runtime
            SlothCombo.Boot.dll          the library that is loaded into the game
            SlothCombo.Loader.dll        the loader that keeps the runtime replaceable
            hostfxr.dll                  the library that starts the runtime
            host/fxr/<version>/hostfxr.dll
            shared/Microsoft.NETCore.App/<version>/   the runtime itself
            game/                        the managed part of the runtime

    The library that starts the runtime works out where the runtime lives
    from its own place, so the game uses the very same files as the program.

.PARAMETER Configuration
    The configuration of the build. Release by default.

.PARAMETER Runtime
    The runtime the build is made for. win-x64 by default.

.PARAMETER Output
    The folder the installation is written to. dist by default.
#>
[CmdletBinding()]
param(
    [string] $Configuration = 'Release',
    [string] $Runtime = 'win-x64',
    [string] $Output = 'dist'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$global:LASTEXITCODE = 0

$root = Split-Path -Parent $PSScriptRoot
$output = if ([System.IO.Path]::IsPathRooted($Output)) { $Output } else { Join-Path $root $Output }
$staging = Join-Path $output '.staging'

function Invoke-Step([string] $message, [scriptblock] $action) {
    Write-Host "==> $message" -ForegroundColor Cyan
    & $action
    if ($LASTEXITCODE -ne 0) {
        throw "The step '$message' failed with the code $LASTEXITCODE."
    }
}

# The game keeps the files it has loaded open, so an installation that is in
# use cannot be written from scratch. The new build is then laid over the old
# one, and a file that did not change is left alone: it is already what the
# new build wants it to be.
$script:lockedFiles = @()
$script:skippedFiles = @()

function Test-SameFile([string] $source, [string] $target) {
    if (-not [System.IO.File]::Exists($target)) {
        return $false
    }

    if ([System.IO.FileInfo]::new($source).Length -ne [System.IO.FileInfo]::new($target).Length) {
        return $false
    }

    $sourceHash = (Get-FileHash -LiteralPath $source -Algorithm SHA256).Hash
    $targetHash = (Get-FileHash -LiteralPath $target -Algorithm SHA256).Hash
    return $sourceHash -eq $targetHash
}

function Copy-FileSafe([string] $source, [string] $target) {
    $folder = [System.IO.Path]::GetDirectoryName($target)
    [System.IO.Directory]::CreateDirectory($folder) | Out-Null

    try {
        [System.IO.File]::Copy($source, $target, $true)
        return
    }
    catch [System.IO.IOException] {
        if (Test-SameFile $source $target) {
            $script:skippedFiles += $target
            return
        }

        $script:lockedFiles += $target
    }
    catch [System.UnauthorizedAccessException] {
        if (Test-SameFile $source $target) {
            $script:skippedFiles += $target
            return
        }

        $script:lockedFiles += $target
    }
}

function Copy-Tree([string] $source, [string] $target) {
    [System.IO.Directory]::CreateDirectory($target) | Out-Null

    foreach ($file in [System.IO.Directory]::GetFiles($source)) {
        Copy-FileSafe $file (Join-Path $target ([System.IO.Path]::GetFileName($file)))
    }

    foreach ($folder in [System.IO.Directory]::GetDirectories($source)) {
        Copy-Tree $folder (Join-Path $target ([System.IO.Path]::GetFileName($folder)))
    }
}

function Remove-File([string] $path) {
    if ([System.IO.File]::Exists($path)) {
        [System.IO.File]::Delete($path)
    }
}

# A file that a program has loaded cannot be opened for writing without
# sharing. Looking before the installation is touched keeps a failure from
# leaving it half deleted.
function Test-InstallationInUse([string] $folder) {
    foreach ($file in [System.IO.Directory]::GetFiles($folder, '*', [System.IO.SearchOption]::AllDirectories)) {
        try {
            $stream = [System.IO.File]::Open(
                $file,
                [System.IO.FileMode]::Open,
                [System.IO.FileAccess]::Write,
                [System.IO.FileShare]::None)
            $stream.Dispose()
        }
        catch [System.IO.IOException] {
            return $true
        }
        catch [System.UnauthorizedAccessException] {
            return $true
        }
    }

    return $false
}

Write-Host "Building the installation in '$output'." -ForegroundColor Green

# A leftover of a failed build is thrown away before anything else, so it is
# neither mistaken for a file of the installation nor reported as in use.
if ([System.IO.Directory]::Exists($staging)) {
    try {
        [System.IO.Directory]::Delete($staging, $true)
    }
    catch [System.IO.IOException] {
        # The publish steps overwrite it; there is nothing else to do.
    }
}

# The installation is written from scratch whenever that is possible. A file
# that is no longer part of it would otherwise stay behind and be carried
# along. When the game holds a part of the installation, the new build is
# laid over the old one instead; a file that is in use and did not change is
# skipped, and one that changed is reported.
if ([System.IO.Directory]::Exists($output)) {
    $left = @([System.IO.Directory]::GetFileSystemEntries($output) |
        Where-Object { [System.IO.Path]::GetFileName($_) -ne '.staging' })

    $mine = @($left | Where-Object {
        $name = [System.IO.Path]::GetFileName($_)
        $name -eq 'SlothCombo.Host.exe' -or $name -eq 'SlothCombo.Boot.dll'
    })

    if ($left.Count -gt 0 -and $mine.Count -eq 0) {
        throw "The folder '$output' holds something that is not an installation of this program, so it was left alone."
    }

    if (Test-InstallationInUse $output) {
        Write-Host '==> The installation is in use. The new build is laid over it; files that did not change are left alone.' -ForegroundColor Yellow
    }
    else {
        try {
            [System.IO.Directory]::Delete($output, $true)
        }
        catch [System.IO.IOException] {
            Write-Host '==> The installation cannot be removed. The new build is laid over it; files that did not change are left alone.' -ForegroundColor Yellow
        }
        catch [System.UnauthorizedAccessException] {
            Write-Host '==> The installation cannot be removed. The new build is laid over it; files that did not change are left alone.' -ForegroundColor Yellow
        }
    }
}

[System.IO.Directory]::CreateDirectory($staging) | Out-Null

# The program that injects the runtime into the game.
Invoke-Step 'Publishing the program.' {
    dotnet publish (Join-Path $root 'src\SlothCombo.Host\SlothCombo.Host.csproj') `
        -c $Configuration -r $Runtime --self-contained false --nologo -v minimal `
        -o (Join-Path $staging 'program')
}

# The managed part of the runtime, which is loaded into the game on its own.
Invoke-Step 'Publishing the managed part of the runtime.' {
    dotnet publish (Join-Path $root 'src\SlothCombo.Runtime\SlothCombo.Runtime.csproj') `
        -c $Configuration -r $Runtime --self-contained false --nologo -v minimal `
        -o (Join-Path $staging 'game')
}

# The loader is the one managed assembly the game keeps for the rest of the
# session. It holds the runtime in a context that can be unloaded, which is
# what lets a new build take the place of the old one without a restart.
Invoke-Step 'Publishing the loader of the runtime.' {
    dotnet publish (Join-Path $root 'src\SlothCombo.Loader\SlothCombo.Loader.csproj') `
        -c $Configuration -r $Runtime --self-contained false --nologo -v minimal `
        -o (Join-Path $staging 'loader')
}

# The library that is loaded into the game. It is built ahead of time, so
# that it can be loaded into a process that has no runtime of its own.
Invoke-Step 'Building the library that is loaded into the game.' {
    dotnet publish (Join-Path $root 'src\SlothCombo.Boot\SlothCombo.Boot.csproj') `
        -c $Configuration -r $Runtime --nologo -v minimal `
        -o (Join-Path $staging 'boot')
}

# The page of the settings is built from the sources of the frontend. The
# build writes its files into the folder of the frontend, which is then taken
# over by the installation.
$uiFolder = Join-Path $root 'XIVSlothComboX.UI'
$wwwroot = Join-Path $root 'XIVSlothComboX.UI\wwwroot'

if (Get-Command npm -ErrorAction SilentlyContinue) {
    Invoke-Step 'Building the page of the settings.' {
        Push-Location $uiFolder
        try {
            if (-not (Test-Path (Join-Path $uiFolder 'node_modules'))) {
                npm ci --no-audit --no-fund
                if ($LASTEXITCODE -ne 0) { throw 'The packages of the frontend could not be installed.' }
            }

            npm run build:only
            if ($LASTEXITCODE -ne 0) { throw 'The frontend could not be built.' }
        }
        finally {
            Pop-Location
        }
    }
}
else {
    Write-Host '==> Node.js was not found, so the page of the settings is taken as it is.' -ForegroundColor Yellow
}

# The runtime of the system is the source of the files the installation
# carries, because the installation has to work on a machine that has
# nothing but the game on it.
$dotnet = (Get-Command dotnet).Source
$dotnetRoot = Split-Path -Parent $dotnet
$frameworkFolder = Join-Path $dotnetRoot 'shared\Microsoft.NETCore.App'

$version = Get-ChildItem $frameworkFolder -Directory |
    Where-Object { $_.Name -match '^10\.' } |
    Sort-Object { [version] $_.Name } -Descending |
    Select-Object -First 1

if ($null -eq $version) {
    throw "No runtime of version 10 was found under '$frameworkFolder'."
}

Write-Host "==> Taking the runtime $($version.Name) from '$dotnetRoot'." -ForegroundColor Cyan

$fxrFolder = Join-Path $dotnetRoot "host\fxr\$($version.Name)"
if (-not (Test-Path (Join-Path $fxrFolder 'hostfxr.dll'))) {
    throw "The library that starts the runtime was not found under '$fxrFolder'."
}

[System.IO.Directory]::CreateDirectory($output) | Out-Null

# The program and everything it needs.
Copy-Tree (Join-Path $staging 'program') $output

# The library that is loaded into the game.
Copy-Tree (Join-Path $staging 'boot') $output

# The loader of the runtime, which stays in the game until it ends.
Copy-Tree (Join-Path $staging 'loader') $output

# The managed part of the runtime.
$game = Join-Path $output 'game'
Copy-Tree (Join-Path $staging 'game') $game

# The page of the settings.
if (Test-Path $wwwroot) {
    Copy-Tree $wwwroot (Join-Path $output 'wwwroot')
}
else {
    Write-Host '==> The page of the settings was not built, so the installation has none.' -ForegroundColor Yellow
}

# The library that starts the runtime. It is placed twice, because the
# program looks for it next to itself and the library works out the place of
# the runtime from its own place.
Copy-FileSafe (Join-Path $fxrFolder 'hostfxr.dll') (Join-Path $output 'hostfxr.dll')

$fxr = Join-Path $output "host\fxr\$($version.Name)"
[System.IO.Directory]::CreateDirectory($fxr) | Out-Null
Copy-FileSafe (Join-Path $fxrFolder 'hostfxr.dll') (Join-Path $fxr 'hostfxr.dll')

# The runtime itself.
$shared = Join-Path $output "shared\Microsoft.NETCore.App\$($version.Name)"
Copy-Tree (Join-Path $frameworkFolder $version.Name) $shared

# The symbols and the documentation of the assemblies are of no use in an
# installation and take up a fair amount of room.
Get-ChildItem $output -Recurse -File -Include '*.pdb', '*.xml' | ForEach-Object { Remove-File $_.FullName }

[System.IO.Directory]::Delete($staging, $true)

if ($script:skippedFiles.Count -gt 0) {
    Write-Host "==> $($script:skippedFiles.Count) files did not change and are held by a running program." -ForegroundColor Yellow
}

if ($script:lockedFiles.Count -gt 0) {
    $list = ($script:lockedFiles | ForEach-Object { "  $_" }) -join [Environment]::NewLine
    throw "These files changed but are held by a running program. Close SlothCombo.Host and the game, then build again:$([Environment]::NewLine)$list"
}

Write-Host ''
Write-Host "The installation is ready in '$output'." -ForegroundColor Green
Write-Host 'Start SlothCombo.Host.exe to load the runtime into the game.'
