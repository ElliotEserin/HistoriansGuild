using LibGit2Sharp;
using System.IO;
using System.Threading;
using System;
using System.Diagnostics;

namespace HistoriansGuild.Helpers;
public class GitRepositoryWatcher : IDisposable
{
    private readonly Repository _repository;
    private readonly FileSystemWatcher _watcher;
    private readonly FileSystemWatcher _gitDirWatcher;
    private readonly Timer _debounceTimer;
    private readonly Action _onRepositoryChanged;
    private readonly int _debounceMs;
    private bool _pendingChanges = false;
    private bool _timerRunning = false;

    public GitRepositoryWatcher(Repository repository, Action onRepositoryChanged, int debounceMs = 500)
    {
        _repository = repository;
        _onRepositoryChanged = onRepositoryChanged;
        _debounceMs = debounceMs;

        // Create a file system watcher for the working directory
        _watcher = new FileSystemWatcher
        {
            Path = _repository.Info.WorkingDirectory,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        // Setup event handlers
        _watcher.Changed += OnFileSystemChanged;
        _watcher.Created += OnFileSystemChanged;
        _watcher.Deleted += OnFileSystemChanged;
        _watcher.Renamed += OnFileSystemRenamed;

        // Create a file system watcher for the .git directory
        _gitDirWatcher = new FileSystemWatcher
        {
            Path = _repository.Info.Path,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            EnableRaisingEvents = true
        };

        // Setup event handlers for git directory changes
        _gitDirWatcher.Changed += OnGitDirectoryChanged;

        // Create a timer for debouncing
        _debounceTimer = new Timer(OnDebounceTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }

    public void Dispose()
    {
        _watcher.Changed -= OnFileSystemChanged;
        _watcher.Created -= OnFileSystemChanged;
        _watcher.Deleted -= OnFileSystemChanged;
        _watcher.Renamed -= OnFileSystemRenamed;
        _watcher.Dispose();

        _gitDirWatcher.Changed -= OnGitDirectoryChanged;
        _gitDirWatcher.Dispose();

        _debounceTimer.Dispose();
    }

    private void HandleChange()
    {
        if (!_timerRunning)
        {
            // No timer running, update immediately and start the timer
            _timerRunning = true;
            _onRepositoryChanged();
            _debounceTimer.Change(_debounceMs, Timeout.Infinite);
        }
        else
        {
            // Timer is running, mark that we have pending changes
            _pendingChanges = true;
        }
    }

    private void OnDebounceTimerElapsed(object? state)
    {
        _timerRunning = false;

        if (_pendingChanges)
        {
            // Changes occurred during the timer, process them now
            _pendingChanges = false;
            _onRepositoryChanged();
        }
    }

    private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
    {
        if (e.FullPath.Contains(Path.Combine(_repository.Info.WorkingDirectory, ".git")))
            return;

        HandleChange();
    }

    private void OnFileSystemRenamed(object sender, RenamedEventArgs e)
    {
        if (e.FullPath.Contains(Path.Combine(_repository.Info.WorkingDirectory, ".git")))
            return;

        HandleChange();
    }

    private void OnGitDirectoryChanged(object sender, FileSystemEventArgs e)
    {
        string fileName = Path.GetFileName(e.FullPath).ToLowerInvariant();

        HandleChange();
    }
}