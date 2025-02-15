using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace HistoriansGuild.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (VisualRoot is Window window)
        {
            window.BeginMoveDrag(e);
        }
    }

    private void MinimiseButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.GetVisualRoot() is Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    private void MaximiseButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.GetVisualRoot() is Window window)
        {
            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        var window = this.GetVisualRoot() as Window;
        window?.Close();
    }
}
