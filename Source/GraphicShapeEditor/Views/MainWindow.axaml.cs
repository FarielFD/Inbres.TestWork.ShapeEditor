using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GraphicShapeEditor.ViewModels;

namespace GraphicShapeEditor.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}