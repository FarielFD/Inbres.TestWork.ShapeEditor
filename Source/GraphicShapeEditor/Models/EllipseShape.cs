
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia;

namespace GraphicShapeEditor.Models
{
    public partial class EllipseShape : ObservableObject, IShape
    {
        [ObservableProperty] private double x;
        [ObservableProperty] private double y;
        [ObservableProperty] private double width = 100;
        [ObservableProperty] private double height = 50;
        [ObservableProperty] private IBrush fill = new SolidColorBrush(Colors.CadetBlue);
        [ObservableProperty] private double strokeThickness = 4;
        [ObservableProperty] private bool isSelected;
        [ObservableProperty] private bool isResizing;
        [ObservableProperty] private Point[]? points;
        public string ShapeType => "Ellipse";
    }
}
