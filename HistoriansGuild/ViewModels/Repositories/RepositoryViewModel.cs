using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibGit2Sharp;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class RepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private HistoryViewModel historyViewModel;

        [ObservableProperty]
        private StagingViewModel stagingViewModel;

        private readonly Repository repository;

        public RepositoryViewModel(Repository repository) 
        {
            this.repository = repository;
            HistoryViewModel = new HistoryViewModel(this.repository);
            StagingViewModel = new StagingViewModel(this.repository);
        }
    }
}
