using LibGit2Sharp;
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
            var selectedFilePaths = new string[selectedFiles.Length];

            for (int i = 0; i < selectedFiles.Length; i++)
            {
                selectedFilePaths[i] = selectedFiles[i].FilePath;
            }

            var changes = repository.Diff.Compare<Patch>(selectedFilePaths, true);
            return changes.ToArray();
        }

        public static PatchEntryChanges[] GetStagedChanges(this Repository repository)
        {
            StatusEntry[] status = repository.RetrieveStatus().Staged.ToArray();

            if(status.Length > 0)
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
