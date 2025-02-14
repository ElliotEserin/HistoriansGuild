using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibGit2Sharp;
using System;

namespace HistoriansGuild.ViewModels.Repositories.New
{
    public partial class AddRepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string repositoryLocation = String.Empty;

        public event EventHandler<RepositoryAddedEventArgs>? RepositoryAdded;

        [RelayCommand]
        void AddRepository()
        {
            if (!Repository.IsValid(RepositoryLocation))
            {
                //TODO handle invalid repository
                return;
            }

            var repo = new Repository(RepositoryLocation);

            RepositoryAdded?.Invoke(this, new RepositoryAddedEventArgs(repo));
        }

        public class RepositoryAddedEventArgs(Repository addedRepo) : EventArgs
        {
            public Repository AddedRepo { get; } = addedRepo;
        }
    }
}
