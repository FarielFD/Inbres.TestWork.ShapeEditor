using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GraphicShapeEditor.Models
{
    public partial class RectangleShape: ObservableObject, IShape
    {
        [ObservableProperty] private double x;

        [ObservableProperty] private double y;

        [ObservableProperty] private double width;

        [ObservableProperty] private double height;

        [ObservableProperty] private Avalonia.Media.Color color;

        [ObservableProperty] private double strokeThickness;

        public string Name => "Rectangle";
    }
}
