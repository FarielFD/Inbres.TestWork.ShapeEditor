using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using GraphicShapeEditor.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace GraphicShapeEditor.Views
{
    public class ShapeCanvas : Control
    {
        public static readonly StyledProperty<ObservableCollection<IShape>?> FiguresProperty =
            AvaloniaProperty.Register<ShapeCanvas, ObservableCollection<IShape>?>(nameof(Figures));
        public ObservableCollection<IShape>? Figures
        {
            get => GetValue(FiguresProperty);
            set => SetValue(FiguresProperty, value);
        }

        public static readonly StyledProperty<IShape?> SelectedFigureProperty =
            AvaloniaProperty.Register<ShapeCanvas, IShape?>(nameof(SelectedFigure),
               defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public IShape? SelectedFigure
        {
            get => GetValue(SelectedFigureProperty);
            set => SetValue(SelectedFigureProperty, value);
        }

        private IShape? _activeFigure;
        private Point _lastPoint;
        private bool _isDragging;
        private bool _isResizing;
        private const double ResizeMargin = 24.0; // Расширили область!
        private int? _activePointIndex = null; // Для перемещения узлов Безье

        static ShapeCanvas()
        {
            AffectsRender<ShapeCanvas>(FiguresProperty, SelectedFigureProperty);
        }
        public ShapeCanvas()
        {
            FiguresProperty.Changed.AddClassHandler<ShapeCanvas>((s, e) => s.OnFiguresChanged(e));
        }
        private void OnFiguresChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if(e.OldValue is ObservableCollection<IShape> oldCollection)
                oldCollection.CollectionChanged -= OnFiguresCollectionChanged;
            if(e.NewValue is ObservableCollection<IShape> NewCollection)
                NewCollection.CollectionChanged += OnFiguresCollectionChanged;
        }

        private void OnFiguresCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            if (Figures is null) return;

            foreach (var fig in Figures)
            {
                var geometry = GetGeometryForType(fig);
                var pen = new Pen(Brushes.Black, fig.StrokeThickness);

                context.DrawGeometry(fig.ShapeType == "Bezier" ? null : fig.Fill, pen, geometry);

                if (fig == SelectedFigure)
                {
                    context.DrawGeometry(null, new Pen(Brushes.CornflowerBlue, 2), geometry);

                    if (fig.ShapeType == "Bezier" && fig.Points != null)
                    {
                        var offset = new Point(fig.X, fig.Y);
                        foreach (var pt in fig.Points)
                        {
                            context.DrawGeometry(
                                Brushes.Yellow,
                                null,
                                new EllipseGeometry(new Rect(pt + offset - new Point(5, 5), new Size(10, 10)))
                            );
                        }
                    }
                }
            }
        }
        private Geometry GetGeometryForType(IShape fig)
        {
            var rect = new Rect(fig.X, fig.Y, fig.Width, fig.Height);
            return fig.ShapeType switch
            {
                "Ellipse" => new EllipseGeometry(rect),
                "Bezier" => CreateBezierGeometry(fig),
                 _ => new RectangleGeometry(rect),
            };
        }

        private Geometry CreateBezierGeometry(IShape fig)
        {
            if (fig.Points == null || fig.Points.Length < 4)
                return new PathGeometry();

            var offset = new Point(fig.X, fig.Y);

            var pathFigure = new PathFigure
            {
                StartPoint = fig.Points[0] + offset,
                IsClosed = false,
                Segments = new PathSegments
                {
                    new BezierSegment
                    {
                        Point1 = fig.Points[1] + offset,
                        Point2 = fig.Points[2] + offset,
                        Point3 = fig.Points[3] + offset
                    }
                }
            };

            return new PathGeometry
            {
                Figures = new PathFigures { pathFigure }
            };
        }
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (Figures is null) return;

            var mouse = e.GetPosition(this);
            foreach (var shape in Figures)
            {
                if (shape.ShapeType == "Bezier" && shape.Points is { Length: > 0 })
                {
                    var offset = new Point(shape.X, shape.Y);
                    for (int i = 0; i < shape.Points.Length; i++)
                    {
                        var pt = shape.Points[i] + offset;
                        var dx = mouse.X - pt.X;
                        var dy = mouse.Y - pt.Y;
                        if (dx * dx + dy * dy <= 64)
                        {
                            _activeFigure = shape;
                            SelectedFigure = shape;
                            _activePointIndex = i;
                            _lastPoint = mouse;
                            e.Handled = true;
                            return;
                        }
                    }
                }
            }
            _activePointIndex = null;
            var figure = Figures.LastOrDefault(f => GetGeometryForType(f).FillContains(mouse));
            if (figure == null)
            {
                SelectedFigure = null;
                return;
            }
            bool isResize = false;
            if (figure.ShapeType == "Rectangle")
            {
                var rect = new Rect(figure.X, figure.Y, figure.Width, figure.Height);
                isResize = rect.Inflate(ResizeMargin).Contains(mouse) && !rect.Inflate(-ResizeMargin).Contains(mouse);
            }
            else if (figure.ShapeType == "Ellipse")

            {
                double rx = figure.Width / 2.0, ry = figure.Height / 2.0;
                double dx = mouse.X - (figure.X + rx), dy = mouse.Y - (figure.Y + ry);
                var norm = dx * dx / (rx * rx) + dy * dy / (ry * ry);
                isResize = norm <= 1.0 && norm > 0.77;
            }

            _isResizing = isResize;
            _isDragging = !isResize;
            _activeFigure = figure;
            SelectedFigure = figure;
            _lastPoint = mouse;
            e.Handled = true;
        }
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (_activeFigure is null) return;

            var p = e.GetPosition(this);
            var deltaX = p.X - _lastPoint.X;
            var deltaY = p.Y - _lastPoint.Y;

            if (_activePointIndex != null && _activeFigure.ShapeType == "Bezier" && _activeFigure.Points != null)
            {
                var points = _activeFigure.Points.ToArray();
                points[_activePointIndex.Value] = points[_activePointIndex.Value] + new Point(deltaX, deltaY);
                _activeFigure.Points = points;
                _lastPoint = p;
                InvalidateVisual();
                return;
            }

            if (_isResizing)
            {
                _activeFigure.Width = Math.Max(10, _activeFigure.Width + deltaX);
                _activeFigure.Height = Math.Max(10, _activeFigure.Height + deltaY);
            }
            else if (_isDragging)
            {
                _activeFigure.X += deltaX;
                _activeFigure.Y += deltaY;
            }
            _lastPoint = p;
            InvalidateVisual();
        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            _activeFigure = null;
            _isDragging = false;
            _isResizing = false;
            _activePointIndex = null;
        }
    }
}
