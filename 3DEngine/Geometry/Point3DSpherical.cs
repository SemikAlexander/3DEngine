using System;

namespace _3DEngine
{
    public class Point3DSpherical : Point3D
    {
        public double Radius { get; set; }
        public double Theta { get; private set; }

        public void SetTheta(double value)
        {
            Theta = Math.Max(value, 0);
            UpdateXYZ();
        }

        public double Phi { get; private set; }

        public void SetPhi(double value)
        {
            Phi = value;
            UpdateXYZ(false);
        }

        public Point3DSpherical(double radius, double theta, double phi) : base(0, 0, 0)
        {
            Radius = radius;
            Theta = Math.Max(Theta, 0);
            Phi = phi;
            UpdateXYZ();
        }

        private void UpdateXYZ(bool updateY = true)
        {
            X = Radius * Math.Sin(Theta) * Math.Cos(Phi);
            if (updateY)
            {
                Y = Radius * Math.Cos(Theta);
            }
            Z = Radius * Math.Sin(Theta) * Math.Sin(Phi);
        }

        private void UpdateRTP()
        {
            Radius = Math.Sqrt(X * X + Y * Y + Z * Z);
            Theta = Math.Max(Math.Acos(Y / Radius), 0);
            Phi = Math.Atan(Z / X);
        }

        public Point3DSpherical(Point3D point) : base(point)
        {
            Set(point.X, point.Y, point.Z);
        }

        public override void Add(double dx, double dy, double dz)
        {
            base.Add(dx, dy, dz);
            UpdateRTP();
        }

        public override void Set(double x, double y, double z)
        {
            base.Set(x, y, z);
            UpdateRTP();
            if (X < 0)
            {
                Phi = Math.PI + Phi;
            }
            else if (Z < 0)
            {
                Phi = 2 * Math.PI + Phi;
            }
        }
    }
}
