using LibGit2Sharp;
using System.IO;
using System.Threading;
using System;
using System.Diagnostics;

namespace HistoriansGuild.Helpers;

public class GitRepositoryWatcher : IDisposable
{
    private readonly Repository repository;
    private readonly FileSystemWatcher watcher;
    private readonly FileSystemWatcher gitDirWatcher;
    private readonly Timer _debounceTimer;
    private readonly Action _onRepositoryChanged;
    private bool _changeDetected = false;
    private string _lastKnownHeadCommitId;
    private DateTime _lastIndexModifiedTime;

    private bool isReadyForUpdate = true;

    public GitRepositoryWatcher(Repository repository, Action onRepositoryChanged)
    {
        this.repository = repository;
        _onRepositoryChanged = onRepositoryChanged;

        // Store initial state
        _lastKnownHeadCommitId = repository.Head.Tip?.Sha;
        _lastIndexModifiedTime = GetIndexFileModifiedTime();

        // Create a file system watcher for the working directory
        watcher = new FileSystemWatcher
        {
            Path = this.repository.Info.WorkingDirectory,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        // Setup event handlers
        watcher.Changed += OnFileSystemChanged;
        watcher.Created += OnFileSystemChanged;
        watcher.Deleted += OnFileSystemChanged;
        watcher.Renamed += OnFileSystemRenamed;

        // Create a file system watcher for the .git directory
        gitDirWatcher = new FileSystemWatcher
        {
            Path = this.repository.Info.Path,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            EnableRaisingEvents = true
        };

        // Setup event handlers for git directory changes
        gitDirWatcher.Changed += OnGitDirectoryChanged;

        // Create a timer for debouncing
        _debounceTimer = new Timer(OnDebounceTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }

    private DateTime GetIndexFileModifiedTime()
    {
        string indexPath = Path.Combine(repository.Info.Path, "index");
        return File.Exists(indexPath) ? File.GetLastWriteTime(indexPath) : DateTime.MinValue;
    }

    private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
    {
        // Skip .git directory changes as they're handled separately
        if (e.FullPath.Contains(Path.Combine(repository.Info.WorkingDirectory, ".git")))
            return;

        RestartDebounceTimer();
    }

    private void OnFileSystemRenamed(object sender, RenamedEventArgs e)
    {
        // Skip .git directory changes as they're handled separately
        if (e.FullPath.Contains(Path.Combine(repository.Info.WorkingDirectory, ".git")))
            return;

        RestartDebounceTimer();
    }

    private void OnGitDirectoryChanged(object sender, FileSystemEventArgs e)
    {
        // Specifically look for changes to HEAD (commits) or index (staging)
        string fileName = Path.GetFileName(e.FullPath).ToLowerInvariant();

        RestartDebounceTimer();
    }

    private void RestartDebounceTimer()
    {
        _changeDetected = true;
        _debounceTimer.Change(10, Timeout.Infinite);
    }

    private void OnDebounceTimerElapsed(object? state)
    {
        if (_changeDetected)
        {
            _changeDetected = false;

            // Check if HEAD has changed (new commit)
            string currentHead = repository.Head.Tip?.Sha;
            bool headChanged = currentHead != _lastKnownHeadCommitId;

            // Check if index has been modified (staging/unstaging)
            DateTime currentIndexTime = GetIndexFileModifiedTime();
            bool indexChanged = currentIndexTime != _lastIndexModifiedTime;

            if (headChanged || indexChanged)
            {
                // Update stored state
                _lastKnownHeadCommitId = currentHead;
                _lastIndexModifiedTime = currentIndexTime;
            }

            // Call the change handler
            _onRepositoryChanged();
        }
    }

    public void Dispose()
    {
        watcher.Changed -= OnFileSystemChanged;
        watcher.Created -= OnFileSystemChanged;
        watcher.Deleted -= OnFileSystemChanged;
        watcher.Renamed -= OnFileSystemRenamed;
        watcher.Dispose();

        gitDirWatcher.Changed -= OnGitDirectoryChanged;
        gitDirWatcher.Dispose();

        _debounceTimer.Dispose();
    }
}