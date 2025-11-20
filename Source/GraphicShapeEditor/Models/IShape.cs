using Avalonia;
using Avalonia.Media;

namespace GraphicShapeEditor.Models
{
    public interface IShape
    {
        double X { get; set; }
        double Y { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        IBrush Fill { get; set; }
        double StrokeThickness { get; set; }
        string ShapeType { get; }
        bool IsSelected { get; set; }
        bool IsResizing { get; set; }
        Point[] Points { get; set; }
    }
}
