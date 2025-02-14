using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.ViewModels.Repositories.New;
using LibGit2Sharp;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class RepositoriesViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase currentViewModel;

        [ObservableProperty]
        private AvaloniaList<Repository> repositories;

        string testRepo = "C:\\Repos\\Personal\\HistoriansGuild";

        public RepositoriesViewModel()
        {
            //TODO load repositories from saved data
            Repositories = [new Repository(testRepo)];

            if(Repositories.Count <= 0)
            {
                var newRepositoryViewModel = new NewRepositoryViewModel();
                newRepositoryViewModel.NewRepository += OnNewRepository;
                CurrentViewModel = newRepositoryViewModel;
            }
            else
            {
                //TODO display current repository
                //CurrentViewModel = new RepositoryViewModel(Repositories[0]);
                CurrentViewModel = new RepositoryViewModel(repositories[0]);
            }
        }

        void OnNewRepository(object? sender, NewRepositoryViewModel.NewRepositoryEventArgs e)
        {
            Repositories.Add(e.NewRepo);
            CurrentViewModel = new RepositoryViewModel(e.NewRepo);
        }
    }
}
