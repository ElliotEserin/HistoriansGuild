using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HistoriansGuild.Helpers;
using HistoriansGuild.ViewModels.Repositories.Commits;
using LibGit2Sharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class StagingViewModel : ViewModelBase
    {
        private readonly Repository repository;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UnstageAllCommand))]
        private PatchEntryChanges[] stagedChanges;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DiscardAllCommand), nameof(StageAllCommand))]
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

            StageSelectedCommand.NotifyCanExecuteChanged();
            UnstageSelectedCommand.NotifyCanExecuteChanged();
        }

        public bool CanStageAll() => UnstagedChanges.Length > 0;
        [RelayCommand(CanExecute = nameof(CanStageAll))]
        public void StageAll()
        {
            Commands.Stage(repository, "*");
        }

        public bool CanStageSelected() => SelectedUnstagedChanges.Count > 0;
        [RelayCommand(CanExecute = nameof(CanStageSelected))]
        public void StageSelected()
        {
            string[] paths = new string[SelectedUnstagedChanges.Count];

            for (int i = 0; i < SelectedUnstagedChanges.Count; i++)
            {
                paths[i] = SelectedUnstagedChanges[i].Path;
            }

            Commands.Stage(repository, paths);
        }

        public bool CanUnstageAll() => StagedChanges.Length > 0;
        [RelayCommand(CanExecute = nameof(CanUnstageAll))]
        public void UnstageAll()
        {
            Commands.Unstage(repository, "*");
        }

        public bool CanUnstageSelected() => SelectedStagedChanges.Count > 0;
        [RelayCommand(CanExecute = nameof(CanUnstageSelected))]
        public void UnstageSelected()
        {
            string[] paths = new string[SelectedStagedChanges.Count];

            for (int i = 0; i < SelectedStagedChanges.Count; i++)
            {
                paths[i] = SelectedStagedChanges[i].Path;
            }

            Commands.Unstage(repository, paths);
        }

        //public bool CanDiscardAll() => UnstagedChanges.Length > 0;
        
        public bool CanDiscardAll() => false;
        [RelayCommand(CanExecute = nameof(CanDiscardAll))]
        public void DiscardAll()
        {
            var trackedFilepaths = UnstagedChanges.Where(change => !change.Status.HasFlag(ChangeKind.Untracked)).Select(change => change.Path).ToArray();

            repository.CheckoutPaths("HEAD", trackedFilepaths, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force });

            var untrackedFilepaths = UnstagedChanges.Where(change => change.Status.HasFlag(ChangeKind.Added)).Select(change => change.Path).ToArray();

            foreach (var filepath in untrackedFilepaths)
            {
                Commands.Remove(repository, filepath);
            }
        }
    }
}
