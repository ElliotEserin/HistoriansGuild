using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.Helpers;
using LibGit2Sharp;
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

            diffs = this.repository.GetDiff(this.commit);
        }

        partial void OnSelectedDiffChanged(PatchEntryChanges? value)
        {
            SelectedDiffViewModel = value != null ? new DiffViewModel(value) : null;
        }
    }
}
