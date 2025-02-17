using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.Helpers;
using HistoriansGuild.ViewModels.Repositories.Commits;
using LibGit2Sharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class StagingViewModel : ViewModelBase
    {
        private readonly Repository repository;

        [ObservableProperty]
        private PatchEntryChanges[] stagedChanges;

        [ObservableProperty]
        private PatchEntryChanges[] unstagedChanges;

        [ObservableProperty]
        private ObservableCollection<PatchEntryChanges> selectedChanges = [];

        [ObservableProperty]
        private ObservableCollection<DiffViewModel> selectedChangesViewModels = [];

        public StagingViewModel(Repository repository)
        {
            this.repository = repository;

            stagedChanges = this.repository.GetStagedChanges();
            unstagedChanges = this.repository.GetUnstagedChanges();

            selectedChanges.CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object? o, NotifyCollectionChangedEventArgs e)
        {
            SelectedChangesViewModels.Clear();

            for (int i = 0; i < SelectedChanges.Count; i++)
            {
                SelectedChangesViewModels.Add(new DiffViewModel(SelectedChanges[i]));
            }
        }
    }
}
