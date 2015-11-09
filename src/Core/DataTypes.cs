namespace Core {
    public enum PointLocation {
        Inside,
        Outside,
        OnBorder,
        OnVertex
    }

    public class Point {
        public Point(double x, double y) {
            X = x;
            Y = y;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
    }

    public class Line {
        public Line(Point start, Point vector) {
            Start = start;
            Vector = vector;
        }

        public static Line FromPoints(Point start, Point end) {
            return new Line(start, new Point(end.X - start.X, end.Y - start.Y));
        }

        public Point Start { get; private set; }
        public Point Vector { get; private set; }
    }
}