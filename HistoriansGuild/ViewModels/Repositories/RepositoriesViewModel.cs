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

        public RepositoriesViewModel()
        {
            //TODO load repositories from saved data
            Repositories = [];

            if(Repositories.Count <= 0)
            {
                var newRepositoryViewModel = new NewRepositoryViewModel();
                newRepositoryViewModel.NewRepository += OnNewRepository;
                CurrentViewModel = newRepositoryViewModel;
            }

            //TODO display current repository
            //CurrentViewModel = new RepositoryViewModel(Repositories[0]);
            CurrentViewModel = null;
        }

        void OnNewRepository(object? sender, NewRepositoryViewModel.NewRepositoryEventArgs e)
        {
            Repositories.Add(e.NewRepo);
            //CurrentViewModel = new RepositoryViewModel(e.NewRepo);
        }
    }
}
