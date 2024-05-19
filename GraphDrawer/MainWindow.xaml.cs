using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Annotations;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace GraphDrawer
{
    public partial class MainWindow : System.Windows.Window
    {
        public PlotModel PlotModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            PlotModel = new PlotModel { Title = "Function Graph" };
        }

        private void DrawGraph(object sender, RoutedEventArgs e)
        {
            PlotModel.Series.Clear();
            PlotModel.Annotations.Clear();

            var functionSeries = new FunctionSeries();
            double x0 = -10, x1 = 10, step = 0.1;

            Func<double, double> func = x => EvaluateFunction(x);

            var roots = FindRoots(func, x0, x1, step);

            foreach (var root in roots)
            {
                var verticalLine = new LineAnnotation
                {
                    Type = LineAnnotationType.Vertical,
                    X = root,
                    StrokeThickness = 2,
                    Color = OxyColors.Green, // Asymptoty pionowe na zielono
                    LineStyle = LineStyle.Dash
                };
                PlotModel.Annotations.Add(verticalLine);
            }

            for (double x = x0; x <= x1; x += step)
            {
                double y = func(x);
                functionSeries.Points.Add(new DataPoint(x, y));
            }

            PlotModel.Series.Add(functionSeries);
            PlotModel.InvalidatePlot(true);
        }

        private double EvaluateFunction(double x)
        {
            // Przykład funkcji: (17*x^2) / (3*x + 1)
            // Wprowadź tutaj swoje obliczenia lub użyj dynamicznego kompilowania jeśli potrzebne
            return (17 * Math.Pow(x, 2)) / (3 * x + 1);
        }

        private List<double> FindRoots(Func<double, double> func, double x0, double x1, double step)
        {
            var roots = new List<double>();
            double previousValue = func(x0);
            for (double x = x0 + step; x <= x1; x += step)
            {
                double currentValue = func(x);
                if (Math.Sign(currentValue) != Math.Sign(previousValue))
                {
                    // Użyj metody bisekcji lub dowolnej innej metody numerycznej
                    roots.Add(FindRoot(func, x - step, x));
                }
                previousValue = currentValue;
            }
            return roots;
        }

        private double FindRoot(Func<double, double> func, double x0, double x1, double tolerance = 0.0001)
        {
            double root = x0;
            while (x1 - x0 > tolerance)
            {
                root = (x0 + x1) / 2;
                if (Math.Sign(func(root)) == Math.Sign(func(x0)))
                    x0 = root;
                else
                    x1 = root;
            }
            return root;
        }
    }
}
