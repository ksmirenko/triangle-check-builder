using System;

namespace CoreLib {
    public class TriangleCheck {
        private const double Tolerance = 1e-9;
        private const int IntersectTrue = 1;
        private const int IntersectFalse = -1;
        private const int IntersectVertex = 0;


        public static PointLocation CalcPointLocation(Point a, Point b, Point c,
            Point p) {
            var sides = new[] {
                Line.FromPoints(a, b), Line.FromPoints(b, c),
                Line.FromPoints(c, a)
            };
            if (p.Equals(a) || p.Equals(b) || p.Equals(c))
                return PointLocation.OnVertex;
            if (CheckPointOnSegment(p, sides[0]) ||
                CheckPointOnSegment(p, sides[1]) ||
                CheckPointOnSegment(p, sides[2])) return PointLocation.OnBorder;
            var vectors = new[] {
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
            var dx = point.X - segment.Start.X;
            var dy = point.Y - segment.Start.Y;
            var tx = dx / segment.Vector.X;
            var ty = dy / segment.Vector.Y;
            if (Math.Abs(segment.Vector.X) < Tolerance) {
                return (Math.Abs(dx) < Tolerance) && (ty > 0) && (ty < 1);
            }
            if (Math.Abs(segment.Vector.Y) < Tolerance) {
                return (Math.Abs(dy) < Tolerance) && (tx > 0) && (tx < 1);
            }
            return (Math.Abs(tx - ty) < Tolerance) && (tx > 0) && (tx < 1);
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
            if (Math.Abs(s) < Tolerance || Math.Abs(s - 1.0) < Tolerance) {
                return IntersectVertex;
            }
            return IntersectFalse;
        }
    }
}