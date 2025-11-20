using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphicShapeEditor.Models;
using Avalonia.Media;
using System;
using System.Collections.ObjectModel;

namespace GraphicShapeEditor.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<IShape> Shapes { get; } = new();

        [ObservableProperty]
        private IShape? selectedShape;

        public IRelayCommand AddRectangleCommand { get; }
        public IRelayCommand AddEllipseCommand { get; }
        public IRelayCommand AddBezierCommand { get; }
        public IRelayCommand DeleteCommand { get; }

        public MainWindowViewModel()
        {
            AddRectangleCommand = new RelayCommand(AddRectangle);
            AddEllipseCommand = new RelayCommand(AddEllipse);
            AddBezierCommand = new RelayCommand(AddBezier);
            DeleteCommand = new RelayCommand(DeleteShape, () => SelectedShape != null);
        }

        partial void OnSelectedShapeChanged(IShape? value)
        {
            DeleteCommand.NotifyCanExecuteChanged();
        }

        private static IBrush GetRandomBrush()
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var color = Color.FromArgb(
                255,
                (byte)rand.Next(32, 224),
                (byte)rand.Next(32, 224),
                (byte)rand.Next(32, 224));
            return new SolidColorBrush(color);
        }

        private void AddRectangle()
        {
            Shapes.Add(new RectangleShape
            {
                X = 100,
                Y = 100,
                Fill = GetRandomBrush()
            });
        }

        private void AddEllipse()
        {
            Shapes.Add(new EllipseShape
            {
                X = 300,
                Y = 100,
                Fill = GetRandomBrush()
            });
        }

        private void AddBezier()
        {
            Shapes.Add(new BezierCurveShape
            {
                Fill = GetRandomBrush()
            });
        }

        private void DeleteShape()
        {
            if (SelectedShape != null)
            {
                Shapes.Remove(SelectedShape);
                SelectedShape = null;
            }
        }
    }
}
