using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicPoolBoard
{
    public enum ResolveMethod { UsingCode, UsingReflect }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private ResolveMethod mode = ResolveMethod.UsingReflect;
        
        private GridContext context;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (mode == ResolveMethod.UsingCode)
                context = new GridContext(new Size(240, 120));
            else
            {
                var r = new Rect(0, 0, 240, 120);
                var q =
                    from x in Enumerable.Range(0, 5)
                    from y in Enumerable.Range(0, 5)
                    select new { Box = Rect.Offset(r, x * 240, y * 120), ScaleX = Math.Pow(-1, x % 2), ScaleY = Math.Pow(-1, y % 2) };
                DataContext = q.ToArray();
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var p1 = new Point(drawingLine.X1, drawingLine.Y1);
            var p2 = new Point(drawingLine.X2, drawingLine.Y2);

            var v = (p2 - p1) * e.NewValue;
            p2 = p1 + v;

            if (mode == ResolveMethod.UsingCode)
            {
                polyline.Points = new PointCollection(context.ReflectPoints(p1, p2));
            }
            else
            {
                line.X1 = p1.X; line.Y1 = p1.Y;
                line.X2 = p2.X; line.Y2 = p2.Y;
            }
        }

        #region Drawing
        private bool drawing;
        private void mainBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            drawing = true;
            var start = e.GetPosition(drawingCanvas);
            drawingLine.X1 = start.X;
            drawingLine.Y1 = start.Y;
            drawingLine.X2 = start.X;
            drawingLine.Y2 = start.Y;
            slider.Value = 0;
        }

        private void mainBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                var pos = e.GetPosition(drawingCanvas);
                drawingLine.X2 = pos.X;
                drawingLine.Y2 = pos.Y;
            }
        }

        private void mainBoard_MouseUp(object sender, MouseButtonEventArgs e)
        {
            drawing = false;
        }
        #endregion
    }
}
