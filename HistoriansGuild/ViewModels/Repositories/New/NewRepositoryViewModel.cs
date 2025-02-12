using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoriansGuild.ViewModels.Repositories.New
{
    public partial class NewRepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase? currentViewModel;

        [RelayCommand]
        void AddRepository()
        {
            CurrentViewModel = new AddRepositoryViewModel();
        }
    }
}
