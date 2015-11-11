namespace CoreLib {
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

        public double X { get; set; }
        public double Y { get; set; }

        public override bool Equals(object obj) {
            if (!(obj is Point)) return false;
            var other = (Point) obj;
            return (X.Equals(other.X) && Y.Equals(other.Y));
        }

        protected bool Equals(Point other) {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode() {
            var x = X;
            var y = Y;
            return (x.GetHashCode() * 397) ^ y.GetHashCode();
        }
    }

    public class Line {
        public Line(Point start, Point vector) {
            Start = start;
            Vector = vector;
        }

        public Point Start { get; private set; }
        public Point Vector { get; private set; }

        public static Line FromPoints(Point start, Point end) {
            return new Line(start, new Point(end.X - start.X, end.Y - start.Y));
        }
    }
}