using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.ViewModels.Repositories.Commits;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class RepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Commit[] commits;

        [ObservableProperty]
        private Commit? selectedCommit;

        [ObservableProperty]
        private CommitViewModel? selectedCommitViewModel;

        private readonly Repository repository;

        public RepositoryViewModel(Repository repository) 
        {
            this.repository = repository;
            this.commits = repository.Commits.ToArray();
        }

        partial void OnSelectedCommitChanged(Commit? value)
        {
            SelectedCommitViewModel = value != null ? new CommitViewModel(value, repository) : null;
        }
    }
}
