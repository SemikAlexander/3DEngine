using System.Drawing;

namespace _3DEngine
{
    public class Face
    {
        public Point3D[] Points { get; private set; }
        public bool Visible { get; set; } = true;

        public Face(Point3D[] points)
        {
            Points = points;
        }

        public Vector3D GetNormalVector()
        {
            double ux = Points[1].X - Points[0].X;
            double uy = Points[1].Y - Points[0].Y;
            double uz = Points[1].Z - Points[0].Z;

            double vx = Points[2].X - Points[0].X;
            double vy = Points[2].Y - Points[0].Y;
            double vz = Points[2].Z - Points[0].Z;

            Vector3D u = new Vector3D(ux, uy, uz);
            Vector3D v = new Vector3D(vx, vy, vz);

            return (u ^ v).GetNormalized();
        }
    }
}