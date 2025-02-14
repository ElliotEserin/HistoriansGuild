using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibGit2Sharp;
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

        void OnRepositoryAdded(object? sender, AddRepositoryViewModel.RepositoryAddedEventArgs e)
        {
            NewRepository?.Invoke(this, new NewRepositoryEventArgs(e.AddedRepo));
        }

        public class NewRepositoryEventArgs(Repository newRepo) : EventArgs
        {
            public Repository NewRepo { get; } = newRepo;
        }
    }
}
