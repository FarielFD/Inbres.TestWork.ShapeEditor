using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;


namespace GraphicShapeEditor.Models
{
    public partial class RectangleShape: ObservableObject, IShape
    {
        [ObservableProperty] private double x;
        [ObservableProperty] private double y;
        [ObservableProperty] private double width = 100;
        [ObservableProperty] private double height = 50;
        [ObservableProperty] private IBrush fill = new SolidColorBrush(Colors.MediumOrchid);
        [ObservableProperty] private double strokeThickness = 4;
        [ObservableProperty] private bool isSelected;
        [ObservableProperty] private bool isResizing;
        [ObservableProperty] private Point[]? points;
        public string ShapeType => "Rectangle";
    }
}
