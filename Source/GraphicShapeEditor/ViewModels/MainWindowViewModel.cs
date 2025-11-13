using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphicShapeEditor.Models;
using System.Runtime.InteropServices.Marshalling;
using System.Collections.ObjectModel;
using Avalonia.Metadata;

namespace GraphicShapeEditor.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection <IShape> figures = new();

        [ObservableProperty]
        private IShape? selectedFigure;

        [RelayCommand]
        private void AddRectangle()
        {
            Figures.Add(new RectangleShape
            {
                X = 50,
                Y = 50,
                Width = 80,
                Height = 80,
                Color = Avalonia.Media.Colors.CornflowerBlue,
                StrokeThickness = 2
            });
        }

        [RelayCommand]
        private void AddEllipse()
        {
            Figures.Add(new EllipseShape
            {
                X = 150,
                Y = 80,
                Width = 100,
                Height = 100,
                Color = Avalonia.Media.Colors.Bisque,
                StrokeThickness = 2
            });
        }
        [RelayCommand]
        private void Delete()
        {
            if (SelectedFigure != null)
            {
                Figures.Remove(SelectedFigure);
                SelectedFigure = null;
            }
        }
        [RelayCommand]

        private void SelectedShape(IShape? shape)
        {
            SelectedFigure = shape;
        }
    }
}
