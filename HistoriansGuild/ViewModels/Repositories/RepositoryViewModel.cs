using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HistoriansGuild.Helpers;
using LibGit2Sharp;
using System.Diagnostics;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class RepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private HistoryViewModel historyViewModel;

        [ObservableProperty]
        private StagingViewModel stagingViewModel;

        private readonly Repository repository;
        private readonly GitRepositoryWatcher watcher;

        public RepositoryViewModel(Repository repository) 
        {
            this.repository = repository;
            watcher = new GitRepositoryWatcher(repository, UpdateUI);

            HistoryViewModel = new HistoryViewModel(this.repository);
            StagingViewModel = new StagingViewModel(this.repository);
        }

        void UpdateUI()
        {
            HistoryViewModel = new HistoryViewModel(this.repository);
            StagingViewModel = new StagingViewModel(this.repository);
        }

        ~RepositoryViewModel()
        {
            watcher.Dispose();
        }
    }
}
