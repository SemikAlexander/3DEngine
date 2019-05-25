using System;
using System.Drawing;

namespace _3DEngine
{
    public class Tube : Primitive
    {
        private const int BOTTOM = 0;
        private const int TOP = 1;
        private const int INSIDE = 0;
        private const int OUTSIDE = 1;

        public double TopRadius { get; private set; }
        public double BottomRadius { get; private set; }
        public double Height { get; private set; }
        public double Thickness { get; private set; }
        public int SegmentsCount { get; private set; }

        public Tube(Point3D basePoint, double topRadius, double bottomRadius, double height, double thickness, int segmentsCount, Color color) : base(basePoint, color)
        {
            TopRadius = topRadius;
            BottomRadius = bottomRadius;
            Height = height;
            Thickness = thickness;
            SegmentsCount = Math.Max(segmentsCount, 3);
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            double angle = 360.0 / SegmentsCount;
            double sin = Math.Sin(MathHelp.DegreesToRadians(angle));
            double cos = Math.Cos(MathHelp.DegreesToRadians(angle));

            Point3D[,,] points = new Point3D[2, 2, SegmentsCount + 1];
            points[BOTTOM, INSIDE, 0] = new Point3D(-BottomRadius, 0, 0);
            points[BOTTOM, OUTSIDE, 0] = new Point3D(-(BottomRadius + Thickness), 0, 0);
            points[TOP, INSIDE, 0] = new Point3D(-TopRadius, Height, 0);
            points[TOP, OUTSIDE, 0] = new Point3D(-(TopRadius + Thickness), Height, 0);
            Faces.Clear();

            for (int i = 1; i <= SegmentsCount; ++i)
            {
                double x = points[BOTTOM, INSIDE, i - 1].X * cos - points[BOTTOM, INSIDE, i - 1].Z * sin;
                double z = points[BOTTOM, INSIDE, i - 1].X * sin + points[BOTTOM, INSIDE, i - 1].Z * cos;
                points[BOTTOM, INSIDE, i] = new Point3D(x, 0, z);

                x = points[BOTTOM, OUTSIDE, i - 1].X * cos - points[BOTTOM, OUTSIDE, i - 1].Z * sin;
                z = points[BOTTOM, OUTSIDE, i - 1].X * sin + points[BOTTOM, OUTSIDE, i - 1].Z * cos;
                points[BOTTOM, OUTSIDE, i] = new Point3D(x, 0, z);

                x = points[TOP, INSIDE, i - 1].X * cos - points[TOP, INSIDE, i - 1].Z * sin;
                z = points[TOP, INSIDE, i - 1].X * sin + points[TOP, INSIDE, i - 1].Z * cos;
                points[TOP, INSIDE, i] = new Point3D(x, Height, z);

                x = points[TOP, OUTSIDE, i - 1].X * cos - points[TOP, OUTSIDE, i - 1].Z * sin;
                z = points[TOP, OUTSIDE, i - 1].X * sin + points[TOP, OUTSIDE, i - 1].Z * cos;
                points[TOP, OUTSIDE, i] = new Point3D(x, Height, z);

                if (i > 0)
                {
                    Faces.Add(new Face(new Point3D[] {
                        points[TOP, OUTSIDE, i - 1],
                        points[TOP, OUTSIDE, i],
                        points[BOTTOM, OUTSIDE, i - 1]
                    }));
                    Faces.Add(new Face(new Point3D[] {
                        points[BOTTOM, OUTSIDE, i - 1],
                        points[BOTTOM, OUTSIDE, i],
                        points[TOP, OUTSIDE, i]
                    }));

                    Faces.Add(new Face(new Point3D[] {
                        points[TOP, INSIDE, i - 1],
                        points[TOP, INSIDE, i],
                        points[BOTTOM, INSIDE, i - 1]
                    }));
                    Faces.Add(new Face(new Point3D[] {
                        points[BOTTOM, INSIDE, i - 1],
                        points[BOTTOM, INSIDE, i],
                        points[TOP, INSIDE, i]
                    }));

                    Faces.Add(new Face(new Point3D[] {
                        points[TOP, INSIDE, i - 1],
                        points[TOP, INSIDE, i],
                        points[TOP, OUTSIDE, i - 1]
                    }));
                    Faces.Add(new Face(new Point3D[] {
                        points[TOP, OUTSIDE, i - 1],
                        points[TOP, OUTSIDE, i],
                        points[TOP, INSIDE, i]
                    }));

                    Faces.Add(new Face(new Point3D[] {
                        points[BOTTOM, INSIDE, i - 1],
                        points[BOTTOM, INSIDE, i],
                        points[BOTTOM, OUTSIDE, i - 1]
                    }));
                    Faces.Add(new Face(new Point3D[] {
                        points[BOTTOM, OUTSIDE, i - 1],
                        points[BOTTOM, OUTSIDE, i],
                        points[BOTTOM, INSIDE, i]
                    }));
                }
            }
        }

        public void ModifyTopRadius(double radius)
        {
            TopRadius = radius;
            UpdatePoints();
        }

        public void ModifyBottomRadius(double radius)
        {
            BottomRadius = radius;
            UpdatePoints();
        }

        public void ModifyThickness(double thickness)
        {
            Thickness = thickness;
            UpdatePoints();
        }

        public void ModifyHeight(double height)
        {
            Height = height;
            UpdatePoints();
        }

        public void ModifySegmentsCount(int segmentsCount)
        {
            SegmentsCount = segmentsCount;
            UpdatePoints();
        }

        public void ModifyBasePoint(Point3D basePoint)
        {
            BasePoint = basePoint;
            UpdatePoints();
        }
    }
}
