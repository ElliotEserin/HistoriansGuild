using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.Helpers;
using LibGit2Sharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace HistoriansGuild.ViewModels.Repositories.Commits
{
    public partial class CommitViewModel : ViewModelBase
    {
        [ObservableProperty]
        private PatchEntryChanges[] diffs;

        [ObservableProperty]
        private ObservableCollection<PatchEntryChanges> selectedChanges = [];

        [ObservableProperty]
        private ObservableCollection<DiffViewModel> selectedChangesViewModels = [];

        private readonly Commit commit;
        private readonly Repository repository;

        public CommitViewModel(Commit commit, Repository repository)
        {
            this.commit = commit;
            this.repository = repository;

            diffs = this.repository.GetDiff(this.commit);

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
