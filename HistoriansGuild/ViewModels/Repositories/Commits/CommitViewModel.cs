using CommunityToolkit.Mvvm.ComponentModel;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HistoriansGuild.ViewModels.Repositories.Commits
{
    public partial class CommitViewModel : ViewModelBase
    {
        [ObservableProperty]
        private PatchEntryChanges[] diffs;

        [ObservableProperty]
        private PatchEntryChanges? selectedDiff;
        
        [ObservableProperty]
        private DiffViewModel? selectedDiffViewModel;

        private readonly Commit commit;
        private readonly Repository repository;

        public CommitViewModel(Commit commit, Repository repository)
        {
            this.commit = commit;
            this.repository = repository;

            diffs = GetDiff();
        }

        public PatchEntryChanges[] GetDiff()
        {
            var parentCommit = commit.Parents.FirstOrDefault();
            var changes = repository.Diff.Compare<Patch>(parentCommit?.Tree, commit.Tree);
            return changes.ToArray();
        }

        partial void OnSelectedDiffChanged(PatchEntryChanges? value)
        {
            SelectedDiffViewModel = value != null ? new DiffViewModel(value) : null;
        }
    }
}
