using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Avalonia.Media;

namespace GraphicShapeEditor.Models
{
    public interface IShape
    {
        string Name { get; }
        double X { get; set; }
        double Y { get; set; }
        double Width {  get; set; }
        double Height { get; set; }
        Avalonia.Media.Color Color { get; set; }
        double StrokeThickness {  get; set; }    
    }
}
