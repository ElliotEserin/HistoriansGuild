using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.Models;
using HistoriansGuild.ViewModels.Repositories;
using HistoriansGuild.ViewModels.Users;

namespace HistoriansGuild.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase currentViewModel;

    public MainViewModel()
    {
        if(AppConfig.config.Users.Count == 0)
        {
            var newUserViewModel = new NewUserViewModel();
            newUserViewModel.NewUserCreated += OnFirstUserCreated;

            CurrentViewModel = newUserViewModel;
            return;
        }

        CurrentViewModel = new RepositoriesViewModel();
    }

    void OnFirstUserCreated(object? o, NewUserViewModel.NewUserEventArgs e)
    {
        CurrentViewModel = new RepositoriesViewModel();
    }
}
