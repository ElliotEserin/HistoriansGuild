using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.ViewModels.Repositories.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class RepositoriesViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase currentViewModel;

        public RepositoriesViewModel()
        {
            CurrentViewModel = new NewRepositoryViewModel();
        }
    }
}
