using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class RepositoryViewModel : ViewModelBase
    {
        private readonly Repository repository;

        public Commit[] Commits => repository.Commits.ToArray();

        public RepositoryViewModel(Repository repository) 
        {
            this.repository = repository;
        }
    }
}
