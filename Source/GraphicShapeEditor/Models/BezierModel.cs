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
        [ObservableProperty] private IBrush fill = new SolidColorBrush(Colors.Transparent); // Кривая не имеет заливки
        [ObservableProperty] private double strokeThickness = 10;
        [ObservableProperty] private bool isSelected;
        [ObservableProperty] private bool isResizing;

        // Точки для кривой Безье: P0, P1, P2, P3
        [ObservableProperty]
        private Point[] points =
        {
            new Point(0, 50),    // P0 - Начальная точка
            new Point(50, 0),    // P1 - Контрольная точка 1
            new Point(100, 100), // P2 - Контрольная точка 2
            new Point(150, 50)   // P3 - Конечная точка
        };

        public string ShapeType => "Bezier";
    }
}
