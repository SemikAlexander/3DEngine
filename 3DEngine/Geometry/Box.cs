using System.Drawing;
using System.Collections.Generic;

namespace _3DEngine
{
    public class Box : Primitive
    {
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double Length { get; private set; }
        
        public Box(Point3D basePoint, double width, double height, double length, Color color) : base(basePoint, color)
        {
            Width = width;
            Height = height;
            Length = length;
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            double x = Width / 2;
            double z = Length / 2;

            List<Point3D> points = new List<Point3D>();

            points.Add(new Point3D(-x, 0, -z));
            points.Add(new Point3D(-x, 0, z));
            points.Add(new Point3D(x, 0, z));
            points.Add(new Point3D(x, 0, -z));
            points.Add(new Point3D(-x, Height, -z));
            points.Add(new Point3D(-x, Height, z));
            points.Add(new Point3D(x, Height, z));
            points.Add(new Point3D(x, Height, -z));

            Faces.Clear();
            Faces.Add(new Face(new Point3D[] { points[7], points[4], points[0] }));
            Faces.Add(new Face(new Point3D[] { points[0], points[3], points[7] }));
            Faces.Add(new Face(new Point3D[] { points[6], points[7], points[3] }));
            Faces.Add(new Face(new Point3D[] { points[2], points[6], points[3] }));
            Faces.Add(new Face(new Point3D[] { points[5], points[6], points[2] }));
            Faces.Add(new Face(new Point3D[] { points[1], points[5], points[2] }));
            Faces.Add(new Face(new Point3D[] { points[4], points[5], points[1] }));
            Faces.Add(new Face(new Point3D[] { points[0], points[4], points[1] }));
            Faces.Add(new Face(new Point3D[] { points[2], points[3], points[0] }));
            Faces.Add(new Face(new Point3D[] { points[1], points[2], points[0] }));
            Faces.Add(new Face(new Point3D[] { points[6], points[4], points[7] }));
            Faces.Add(new Face(new Point3D[] { points[6], points[5], points[4] }));
        }

        public void ModifyWidth(double width)
        {
            Width = width;
            UpdatePoints();
        }

        public void ModifyHeight(double height)
        {
            Height = height;
            UpdatePoints();
        }

        public void ModifyLength(double length)
        {
            Length = length;
            UpdatePoints();
        }

        public void ModifyBasePoint(Point3D basePoint)
        {
            BasePoint = basePoint;
            UpdatePoints();
        }
    }
}
