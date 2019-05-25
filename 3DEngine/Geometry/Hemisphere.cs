using System;
using System.Drawing;

namespace _3DEngine
{
    class Hemisphere : Primitive
    {
        private bool closeBottom = false;
        public double Radius { get; private set; }
        public int SegmentsCount { get; private set; }

        public Hemisphere(Point3D basePoint, double radius, int segmentsCount, Color color, bool closeBottom = true) : base(basePoint, color)
        {
            Radius = radius;
            SegmentsCount = Math.Max(segmentsCount, 4);
            this.closeBottom = closeBottom;
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            int quater = Math.Max(SegmentsCount / 4 - 1, 1);
            double angle = 360.0 / SegmentsCount;
            double sin = Math.Sin(MathHelp.DegreesToRadians(-angle));
            double cos = Math.Cos(MathHelp.DegreesToRadians(-angle));

            Point3D[,] points = new Point3D[quater, SegmentsCount + 1];
            points[0, 0] = new Point3D(-Radius, 0, 0);
            for (int i = 1; i < quater; ++i)
            {
                double x = points[i - 1, 0].X * cos - points[i - 1, 0].Y * sin;
                double y = points[i - 1, 0].X * sin + points[i - 1, 0].Y * cos;
                points[i, 0] = new Point3D(x, y, 0);
            }
            Faces.Clear();

            for (int i = 1; i <= SegmentsCount; ++i)
            {
                for (int j = 0; j < quater; ++j)
                {
                    double x = points[j, i - 1].X * cos - points[j, i - 1].Z * sin;
                    double z = points[j, i - 1].X * sin + points[j, i - 1].Z * cos;
                    points[j, i] = new Point3D(x, points[j, i - 1].Y, z);
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            Faces.Add(new Face(new Point3D[] {
                                new Point3D(points[j - 1, i - 1].X, points[j - 1, i - 1].Y, points[j - 1, i - 1].Z),
                                new Point3D(points[j - 1, i].X, points[j - 1, i].Y, points[j - 1, i].Z),
                                new Point3D(points[j, i - 1].X, points[j, i - 1].Y, points[j, i - 1].Z)
                            }));
                            Faces.Add(new Face(new Point3D[] {
                                new Point3D(points[j, i - 1].X, points[j, i - 1].Y, points[j, i - 1].Z),
                                new Point3D(points[j, i].X, points[j, i].Y, points[j, i].Z),
                                new Point3D(points[j - 1, i].X, points[j - 1, i].Y, points[j - 1, i].Z)
                            }));
                        }
                        else if (closeBottom)
                        {
                            Faces.Add(new Face(new Point3D[] {
                                new Point3D(points[j, i - 1].X, points[j, i - 1].Y, points[j, i - 1].Z),
                                new Point3D(points[j, i].X, points[j, i].Y, points[j, i].Z),
                                new Point3D(0, 0, 0)
                            }));
                        }
                        if (j == quater - 1)
                        {
                            Faces.Add(new Face(new Point3D[] {
                                new Point3D(points[j, i - 1].X, points[j, i - 1].Y, points[j, i - 1].Z),
                                new Point3D(points[j, i].X, points[j, i].Y, points[j, i].Z),
                                new Point3D(0, Radius, 0)
                            }));
                        }
                    }
                }
            }
        }

        public void ModifyRadius(double radius)
        {
            Radius = radius;
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
