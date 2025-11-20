using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GraphicShapeEditor.Models
{
    public partial class BezierCurveShape : ObservableObject, IShape
    {
        [ObservableProperty] private double x = 150;
        [ObservableProperty] private double y = 150;
        [ObservableProperty] private double width;
        [ObservableProperty] private double height;
        [ObservableProperty] private IBrush fill = new SolidColorBrush(Colors.Transparent); 
        [ObservableProperty] private double strokeThickness = 10;
        [ObservableProperty] private bool isSelected;
        [ObservableProperty] private bool isResizing;
        [ObservableProperty]
        private Point[] points =
        {
            new Point(0, 50),   
            new Point(50, 0),   
            new Point(100, 100), 
            new Point(150, 50)  
        };

        public string ShapeType => "Bezier";
    }
}
