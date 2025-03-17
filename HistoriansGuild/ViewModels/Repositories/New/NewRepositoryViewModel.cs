using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HistoriansGuild.Models;
using System;

namespace HistoriansGuild.ViewModels.Repositories.New
{
    public partial class NewRepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase? currentViewModel;

        public event EventHandler<NewRepositoryEventArgs>? NewRepository;

        [RelayCommand]
        void AddRepository()
        {
            var addRepositoryViewModel = new AddRepositoryViewModel();
            addRepositoryViewModel.RepositoryAdded += OnRepositoryAdded;

            CurrentViewModel = addRepositoryViewModel;
        }

        void OnRepositoryAdded(object? sender, NewRepositoryEventArgs e)
        {
            NewRepository?.Invoke(this, e);
        }
    }

    public class NewRepositoryEventArgs(Repository newRepo) : EventArgs
    {
        public Repository Repository { get; } = newRepo;
    }
}
