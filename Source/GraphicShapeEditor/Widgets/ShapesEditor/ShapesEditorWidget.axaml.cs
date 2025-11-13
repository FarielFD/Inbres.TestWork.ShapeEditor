using Avalonia;
using Avalonia.Controls;
using GraphicShapeEditor.Models;
using System.Collections.ObjectModel;

namespace GraphicShapeEditor.Widgets
{
    public partial class ShapesEditorWidget : UserControl
    {
        public ShapesEditorWidget()
        {
            InitializeComponent();
        }

        public static readonly StyledProperty<ObservableCollection<IShape>> FiguresProperty =
            AvaloniaProperty.Register<ShapesEditorWidget, ObservableCollection<IShape>>(nameof(Figures));
        public ObservableCollection<IShape> Figures
        {
            get => GetValue(FiguresProperty);
            set => SetValue(FiguresProperty, value);
        }
     
        public static readonly StyledProperty<IShape?> SelectedFigureProperty =
            AvaloniaProperty.Register<ShapesEditorWidget, IShape?>(nameof(SelectedFigure));
        public IShape? SelectedFigure
        {
            get => GetValue(SelectedFigureProperty);
            set => SetValue(SelectedFigureProperty, value);
        }
    }
}