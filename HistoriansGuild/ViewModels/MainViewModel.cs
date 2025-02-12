using CommunityToolkit.Mvvm.ComponentModel;
using HistoriansGuild.ViewModels.Repositories;

namespace HistoriansGuild.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase currentViewModel;

    public MainViewModel()
    {
        CurrentViewModel = new RepositoriesViewModel();
    }
}
