using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.ViewModels.Repositories.New;
using LibGit2Sharp;
using System;

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
            var savedRepos = Models.AppConfig.config.Repositories;
            Repositories = [];

            foreach (var repo in savedRepos)
            {
                Repositories.Add(new Repository(repo.Location));
            }

            if (Repositories.Count <= 0)
            {
                var newRepositoryViewModel = new NewRepositoryViewModel();
                newRepositoryViewModel.NewRepository += OnNewRepository;
                CurrentViewModel = newRepositoryViewModel;
            }
            else
            {
                GoToCurrentRepository();
            }
        }

        void OnNewRepository(object? sender, NewRepositoryEventArgs e)
        {
            var repository = new Repository(e.Repository.Location);
            var user = e.Repository.User;

            if(user != null)
            {
                repository.Config.Set("user.name", user.Name);
                repository.Config.Set("user.email", user.Email);
                repository.Config.Set("user.password", user.Password);
            }

            Repositories.Add(repository);
            Models.AppConfig.SetCurrentRepository(Repositories.Count - 1);
            GoToCurrentRepository();
        }

        void GoToCurrentRepository()
        {
            var repoIndex = Math.Clamp(Models.AppConfig.config.CurrentRepository, 0, Repositories.Count - 1);
            CurrentViewModel = new RepositoryViewModel(Repositories[repoIndex]);
        }

        ~RepositoriesViewModel()
        {
            foreach (var repo in Repositories)
            {
                repo.Dispose();
            }
        }
    }
}
