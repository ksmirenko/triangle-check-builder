using System;

namespace Core {
    public class TriangleCheck {
        private const int IntersectTrue = 1;
        private const int IntersectFalse = -1;
        private const int IntersectVertex = 0;
        private const double Tolerance = 1e-9;

        public static PointLocation CalcPointLocation(Point a, Point b, Point c,
            Point p) {
            var sides = new Line[3] {
                Line.FromPoints(a, b), Line.FromPoints(b, c),
                Line.FromPoints(c, a)
            };
            if (CheckPointOnSegment(p, sides[0]) ||
                CheckPointOnSegment(p, sides[1]) ||
                CheckPointOnSegment(p, sides[2])) return PointLocation.OnBorder;
            var vectors = new Point[4] {
                new Point(1f, 0f), new Point(0f, 1f), new Point(-1f, 0f),
                new Point(0f, -1f)
            };
            foreach (var vector in vectors) {
                var ray = new Line(p, vector);
                var intersectionsCount = 0;
                var nextVector = false;
                foreach (var side in sides) {
                    switch (CheckRaySegmentIntersection(ray, side)) {
                        case IntersectVertex:
                            nextVector = true;
                            break;
                        case IntersectTrue:
                            intersectionsCount++;
                            break;
                    }
                }
                if (nextVector) break;
                return (intersectionsCount % 2 == 0)
                    ? PointLocation.Outside
                    : PointLocation.Inside;
            }
            return PointLocation.Outside;
        }

        private static bool CheckPointOnSegment(Point point, Line segment) {
            return (point.X - segment.Start.X) / segment.Vector.X -
                   (point.Y - segment.Start.Y) / segment.Vector.Y < Tolerance;
        }

        private static int CheckRaySegmentIntersection(Line ray, Line segment) {
            var a = ray.Vector.X;
            var b = -segment.Vector.X;
            var c = ray.Vector.Y;
            var d = -segment.Vector.Y;
            var e = segment.Start.X - ray.Start.X;
            var f = segment.Start.Y - ray.Start.Y;

            var determinant = a * d - b * c;
            if (!(Math.Abs(determinant) > Tolerance)) return IntersectFalse;
            var t = (e * d - b * f) / determinant;
            var s = (a * f - e * c) / determinant;
            if (!(t >= 0.0)) return IntersectFalse;
            if (s > 0.0 && s < 1.0) {
                return IntersectTrue;
            }
            if (Math.Abs(s) < Tolerance || Math.Abs(s) - 1.0 < Tolerance) {
                return IntersectVertex;
            }
            return IntersectFalse;
        }
    }
}