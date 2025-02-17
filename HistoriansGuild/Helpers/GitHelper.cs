using LibGit2Sharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HistoriansGuild.Helpers
{
    public static class GitHelper
    {
        public static PatchEntryChanges[] GetDiff(this Repository repository, Commit commit)
        {
            var parentCommit = commit.Parents.FirstOrDefault();
            var changes = repository.Diff.Compare<Patch>(parentCommit?.Tree, commit.Tree);
            return changes.ToArray();
        }

        public static PatchEntryChanges[] GetDiff(this Repository repository, StatusEntry[] selectedFiles)
        {
            if (selectedFiles == null || selectedFiles.Length == 0)
                return [];

            var result = new List<PatchEntryChanges>();

            // Group files by their status
            var stagedFiles = selectedFiles
                .Where(file => file.State.HasFlag(FileStatus.NewInIndex) ||
                                file.State.HasFlag(FileStatus.ModifiedInIndex) ||
                               file.State.HasFlag(FileStatus.DeletedFromIndex))
                .Select(file => file.FilePath)
                .ToArray();

            var unstagedFiles = selectedFiles
                .Where(file => file.State.HasFlag(FileStatus.ModifiedInWorkdir) ||
                               file.State.HasFlag(FileStatus.DeletedFromWorkdir) ||
                               file.State.HasFlag(FileStatus.NewInWorkdir))
                .Select(file => file.FilePath)
                .ToArray();

            // Get staged changes by comparing index with HEAD
            if (stagedFiles.Length > 0)
            {
                var changes = repository.Diff.Compare<Patch>(
                    repository.Head.Tip.Tree,
                    DiffTargets.Index,
                    stagedFiles
                );
                result.AddRange(changes);
            }

            // Get unstaged changes by comparing working directory with index
            if (unstagedFiles.Length > 0)
            {
                var changes = repository.Diff.Compare<Patch>(unstagedFiles, true);
                result.AddRange(changes);
            }

            return result.ToArray();
        }

        public static PatchEntryChanges[] GetStagedChanges(this Repository repository)
        {
            StatusEntry[] status = repository.RetrieveStatus().Where(e => 
            e.State.HasFlag(FileStatus.NewInIndex) ||
            e.State.HasFlag(FileStatus.ModifiedInIndex) ||
            e.State.HasFlag(FileStatus.DeletedFromIndex)
            ).ToArray();

            if (status.Length > 0)
                return repository.GetDiff(status);

            return [];
        }

        public static PatchEntryChanges[] GetUnstagedChanges(this Repository repository)
        {
            var status = repository.RetrieveStatus().Where(e =>
                e.State.HasFlag(FileStatus.ModifiedInWorkdir) ||
                e.State.HasFlag(FileStatus.NewInWorkdir) ||
                e.State.HasFlag(FileStatus.DeletedFromWorkdir)
            ).ToArray();

            if (status.Length > 0)
                return repository.GetDiff(status);

            return [];
        }
    }
}
