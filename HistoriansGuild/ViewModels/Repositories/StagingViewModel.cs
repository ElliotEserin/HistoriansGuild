using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.Helpers;
using HistoriansGuild.ViewModels.Repositories.Commits;
using LibGit2Sharp;
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
        private PatchEntryChanges? selectedChange;

        [ObservableProperty]
        private DiffViewModel? selectedChangeViewModel;

        public StagingViewModel(Repository repository)
        {
            this.repository = repository;

            stagedChanges = this.repository.GetStagedChanges();
            unstagedChanges = this.repository.GetUnstagedChanges();
        }

        partial void OnSelectedChangeChanged(PatchEntryChanges? value)
        {
            SelectedChangeViewModel = value != null ? new DiffViewModel(value) : null;
        }
    }
}
