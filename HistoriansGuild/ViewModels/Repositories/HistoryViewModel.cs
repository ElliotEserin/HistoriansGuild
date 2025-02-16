using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.ViewModels.Repositories.Commits;
using LibGit2Sharp;
using System.Linq;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class HistoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Commit[] commits;

        [ObservableProperty]
        private Commit? selectedCommit;

        [ObservableProperty]
        private CommitViewModel? selectedCommitViewModel;

        Repository repository;

        public HistoryViewModel(Repository repository)
        {
            this.repository = repository;
            commits = repository.Commits.ToArray();
        }

        partial void OnSelectedCommitChanged(Commit? value)
        {
            SelectedCommitViewModel = value != null ? new CommitViewModel(value, repository) : null;
        }
    }
}
