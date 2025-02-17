using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private ObservableCollection<PatchEntryChanges> selectedStagedChanges = [];

        [ObservableProperty]
        private ObservableCollection<PatchEntryChanges> selectedUnstagedChanges = [];

        [ObservableProperty]
        private ObservableCollection<DiffViewModel> selectedChangesViewModels = [];

        public StagingViewModel(Repository repository)
        {
            this.repository = repository;

            StagedChanges = this.repository.GetStagedChanges();
            UnstagedChanges = this.repository.GetUnstagedChanges();

            SelectedStagedChanges.CollectionChanged += OnCollectionChanged;
            SelectedUnstagedChanges.CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object? o, NotifyCollectionChangedEventArgs e)
        {
            SelectedChangesViewModels.Clear();

            foreach (var change in SelectedStagedChanges)
            {
                SelectedChangesViewModels.Add(new DiffViewModel(change));
            }

            foreach (var change in SelectedUnstagedChanges)
            {
                SelectedChangesViewModels.Add(new DiffViewModel(change));
            }
        }

        [RelayCommand]
        public void StageAll()
        {
            Commands.Stage(repository, "*");
        }

        [RelayCommand]
        public void StageSelected()
        {
            string[] paths = new string[SelectedUnstagedChanges.Count];

            for (int i = 0; i < SelectedUnstagedChanges.Count; i++)
            {
                paths[i] = SelectedUnstagedChanges[i].Path;
            }

            Commands.Stage(repository, paths);
        }

        [RelayCommand]
        public void UnstageAll()
        {
            Commands.Unstage(repository, "*");
        }

        [RelayCommand]
        public void UnstageSelected()
        {
            string[] paths = new string[SelectedStagedChanges.Count];

            for (int i = 0; i < SelectedStagedChanges.Count; i++)
            {
                paths[i] = SelectedStagedChanges[i].Path;
            }

            Commands.Unstage(repository, paths);
        }
    }
}
