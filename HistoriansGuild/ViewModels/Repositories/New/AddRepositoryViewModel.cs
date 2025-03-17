using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HistoriansGuild.Models;
using System;
using System.Collections.Generic;

namespace HistoriansGuild.ViewModels.Repositories.New
{
    public partial class AddRepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string repositoryLocation = String.Empty;

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private List<User> users;

        public event EventHandler<NewRepositoryEventArgs>? RepositoryAdded;

        public AddRepositoryViewModel()
        {
            Users = AppConfig.config.Users;
            User = Users[0];
        }

        [RelayCommand]
        void AddRepository()
        {
            if (!LibGit2Sharp.Repository.IsValid(RepositoryLocation))
            {
                //TODO handle invalid repository
                return;
            }

            var repo = AppConfig.AddRepository(RepositoryLocation, User.Id);

            RepositoryAdded?.Invoke(this, new NewRepositoryEventArgs(repo));
        }
    }
}
