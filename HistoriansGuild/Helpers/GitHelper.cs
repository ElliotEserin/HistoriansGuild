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

        public static PatchEntryChanges[] GetDiff(this Repository repository, StatusEntry[] selectedFiles, bool staged)
        {
            if (selectedFiles == null || selectedFiles.Length == 0)
                return [];

            var result = new List<PatchEntryChanges>();

            var files = selectedFiles.Select(e => e.FilePath).ToArray();

            if (staged)
            {
                // Get staged changes by comparing index with HEAD
                if (selectedFiles.Length > 0)
                {
                    var changes = repository.Diff.Compare<Patch>(
                        repository.Head.Tip.Tree,
                        DiffTargets.Index,
                        files
                    );
                    result.AddRange(changes);
                }
            }
            else
            {
                // Get unstaged changes by comparing working directory with index
                if (selectedFiles.Length > 0)
                {
                    var changes = repository.Diff.Compare<Patch>(files, true);
                    result.AddRange(changes);
                }
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

            return repository.GetDiff(status, true);
        }

        public static PatchEntryChanges[] GetUnstagedChanges(this Repository repository)
        {
            var status = repository.RetrieveStatus().Where(e =>
                e.State.HasFlag(FileStatus.ModifiedInWorkdir) ||
                e.State.HasFlag(FileStatus.NewInWorkdir) ||
                e.State.HasFlag(FileStatus.DeletedFromWorkdir)
            ).ToArray();

            return repository.GetDiff(status, false);
        }
    }
}