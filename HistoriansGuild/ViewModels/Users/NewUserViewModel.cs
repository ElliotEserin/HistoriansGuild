using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HistoriansGuild.Models;
using System;

namespace HistoriansGuild.ViewModels.Users
{
    partial class NewUserViewModel : ViewModelBase
    {
        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateUserCommand))]
        private string username = string.Empty;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateUserCommand))]
        private string email = string.Empty;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateUserCommand))]
        private string password = string.Empty;

        public event EventHandler<NewUserEventArgs>? NewUserCreated;

        bool CanCreateUser() { return Username != string.Empty && Email != string.Empty && Password != string.Empty; }

        [RelayCommand(CanExecute = nameof(CanCreateUser))]
        private void CreateUser()
        {
            var user = AppConfig.AddUser(Username, Email, Password);

            NewUserCreated?.Invoke(this, new NewUserEventArgs(user));
        }
        
        public class NewUserEventArgs : EventArgs
        {
            public NewUserEventArgs(User user)
            {
                User = user;
            }

            public User User { get; }
        }
    }
}
