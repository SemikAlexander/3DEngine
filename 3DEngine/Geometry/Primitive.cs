using System.Drawing;
using System.Collections.Generic;

namespace _3DEngine
{
    public class Primitive
    {
        public Color Color;
        public Point3D BasePoint;

        public List<Face> Faces { get; protected set; } = new List<Face>();

        public Primitive(Point3D basePoint, Color color)
        {
            BasePoint = basePoint;
            Color = color;
        }
    }
}
