using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MagicPoolBoard
{
    public class GridContext
    {
        private Size window;
        public GridContext(Size window)
        {
            this.window = window;
        }
        public IEnumerable<Point> ReflectPoints(Point p1, Point p2)
        {
            var xPoints = ComputePositions(window.Width, p1.X, p2.X, Orientation.Horizontal);
            var yPoints = ComputePositions(window.Height, p1.Y, p2.Y, Orientation.Vertical);

            var points =
                from item in xPoints.Concat(yPoints)
                orderby item.Position
                select item;

            var originalVector = p2 - p1;
            var direction = new Vector(1, 1);
            var currentPoint = p1;
            var lastPosition = 0.0;

            yield return p1;

            foreach (var p in points)
            {
                var v = originalVector * (p.Position - lastPosition);
                v = new Vector(v.X * direction.X, v.Y * direction.Y);

                lastPosition = p.Position;
                if (p.Orientation == Orientation.Horizontal)
                    direction.X *= -1;
                else
                    direction.Y *= -1;

                var nextPoint = currentPoint + v;
                currentPoint = nextPoint;

                yield return nextPoint;
            }
        }
        private IEnumerable<(double Position, Orientation Orientation)> ComputePositions(double window, double start, double end, Orientation orientation)
        {
            if (end > start)
                return ComputePositionsAscending(window, start, end, orientation);
            else
                return ComputePositionsAscending(window, end, start, orientation, true);
        }
        private IEnumerable<(double Position, Orientation Orientation)> ComputePositionsAscending(double window, double start, double end, Orientation orientation, bool reverse = false)
        {
            var length = end - start;
            var x = (Math.Floor(start / window) + 1) * window;
            while (x < end)
            {
                var position = (x - start) / length;
                if (reverse)
                    position = 1 - position;
                yield return (position, orientation);
                x += window;
            }

            yield return (1, orientation);
        }
    }
}
