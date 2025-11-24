using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using FLEET_MANAGER.Models;

namespace FLEET_MANAGER.Views
{
    public partial class SimpleLineChart : UserControl
    {
        public static readonly DependencyProperty DataPointsProperty =
            DependencyProperty.Register(nameof(DataPoints), typeof(ObservableCollection<PointGraphique>),
                typeof(SimpleLineChart), new PropertyMetadata(null, OnDataPointsChanged));

        public ObservableCollection<PointGraphique> DataPoints
        {
            get => (ObservableCollection<PointGraphique>)GetValue(DataPointsProperty);
            set => SetValue(DataPointsProperty, value);
        }

        public SimpleLineChart()
        {
            InitializeComponent();
            Loaded += (s, e) => DrawChart();
            SizeChanged += (s, e) => DrawChart();
        }

        private static void OnDataPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleLineChart chart)
            {
                chart.DrawChart();
            }
        }

        private void DrawChart()
        {
            ChartCanvas.Children.Clear();

            if (DataPoints == null || DataPoints.Count < 2 || ActualWidth == 0 || ActualHeight == 0)
                return;

            var points = DataPoints.ToList();
            var width = ActualWidth;
            var height = ActualHeight;

            // Marges
            double marginLeft = 40;
            double marginRight = 20;
            double marginTop = 20;
            double marginBottom = 40;

            double chartWidth = width - marginLeft - marginRight;
            double chartHeight = height - marginTop - marginBottom;

            // Trouver min/max
            int minValue = points.Min(p => p.Valeur);
            int maxValue = points.Max(p => p.Valeur);
            int valueRange = maxValue - minValue;

            if (valueRange == 0) valueRange = 1;

            // Dessiner les axes
            var axisLine = new Line
            {
                X1 = marginLeft,
                Y1 = marginTop,
                X2 = marginLeft,
                Y2 = height - marginBottom,
                Stroke = new SolidColorBrush(Color.FromRgb(226, 232, 240)),
                StrokeThickness = 2
            };
            ChartCanvas.Children.Add(axisLine);

            var axisLineX = new Line
            {
                X1 = marginLeft,
                Y1 = height - marginBottom,
                X2 = width - marginRight,
                Y2 = height - marginBottom,
                Stroke = new SolidColorBrush(Color.FromRgb(226, 232, 240)),
                StrokeThickness = 2
            };
            ChartCanvas.Children.Add(axisLineX);

            // Créer les points de la polyline
            var polyline = new Polyline
            {
                Stroke = new SolidColorBrush(Color.FromRgb(79, 70, 229)),
                StrokeThickness = 3,
                StrokeLineJoin = PenLineJoin.Round
            };

            for (int i = 0; i < points.Count; i++)
            {
                double x = marginLeft + (i * chartWidth / (points.Count - 1));
                double y = height - marginBottom - ((points[i].Valeur - minValue) * chartHeight / valueRange);

                polyline.Points.Add(new Point(x, y));

                // Dessiner un point
                var ellipse = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = new SolidColorBrush(Color.FromRgb(79, 70, 229)),
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(ellipse, x - 4);
                Canvas.SetTop(ellipse, y - 4);
                ChartCanvas.Children.Add(ellipse);

                // Label de date
                var label = new TextBlock
                {
                    Text = points[i].Label,
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139))
                };
                Canvas.SetLeft(label, x - 15);
                Canvas.SetTop(label, height - marginBottom + 5);
                ChartCanvas.Children.Add(label);

                // Valeur
                var valueLabel = new TextBlock
                {
                    Text = $"{points[i].Valeur:N0} km",
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139))
                };
                Canvas.SetLeft(valueLabel, x - 20);
                Canvas.SetTop(valueLabel, y - 20);
                ChartCanvas.Children.Add(valueLabel);
            }

            ChartCanvas.Children.Add(polyline);
        }
    }
}
