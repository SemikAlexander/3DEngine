using System;

namespace _3DEngine
{
    public class Vector3D
    {
        public double X;
        public double Y;
        public double Z;

        public double Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public Vector3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D(Vector3D Vector3D)
        {
            X = Vector3D.X;
            Y = Vector3D.Y;
            Z = Vector3D.Z;
        }

        public Vector3D(Point3D point3D)
        {
            X = point3D.X;
            Y = point3D.Y;
            Z = point3D.Z;
        }

        public void Normalize()
        {
            double length = Length;
            double scale = 1 / length;
            if (length != 0)
            {
                X *= scale;
                Y *= scale;
                Z *= scale;
            }
        }

        public Vector3D GetNormalized()
        {
            Vector3D result = new Vector3D(X, Y, Z);
            double length = Length;
            double scale = 1 / length;
            if (length != 0)
            {
                result.X *= scale;
                result.Y *= scale;
                result.Z *= scale;
            }
            return result;
        }

        public Vector3D CrossProduct(Vector3D vector)
        {
            return new Vector3D(
                Y * vector.Z - Z * vector.Y,
                Z * vector.X - X * vector.Z,
                X * vector.Y - Y * vector.X
            );
        }

        public static Vector3D CrossProduct(Vector3D first, Vector3D second)
        {
            return new Vector3D(
                first.Y * second.Z - first.Z * second.Y,
                first.Z * second.X - first.X * second.Z,
                first.X * second.Y - first.Y * second.X
            );
        }

        public static Vector3D operator ^(Vector3D first, Vector3D second)
        {
            Vector3D result = new Vector3D(
                first.Y * second.Z - first.Z * second.Y,
                first.Z * second.X - first.X * second.Z,
                first.X * second.Y - first.Y * second.X
            );
            return result;
        }

        public double DotProduct(Vector3D vector)
        {
            return X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        public static double DotProduct(Vector3D first, Vector3D second)
        {
            return first.X * second.X + first.Y * second.Y + first.Z * second.Z;
        }

        public static double operator *(Vector3D first, Vector3D second)
        {
            return first.X * second.X + first.Y * second.Y + first.Z * second.Z;
        }

        public static double AngleCos(Vector3D first, Vector3D second)
        {
            return (first * second) / Math.Max((first.Length * second.Length), 1);
        }

        public static double Angle(Vector3D first, Vector3D second)
        {
            return Math.Acos(AngleCos(first, second));
        }

        public static Vector3D operator +(Vector3D first, Vector3D second)
        {
            Vector3D result = new Vector3D(
                first.X + second.X,
                first.Y + second.Y,
                first.Z + second.Z
            );
            return result;
        }

        public static Vector3D operator -(Vector3D first, Vector3D second)
        {
            Vector3D result = new Vector3D(
                first.X - second.X,
                first.Y - second.Y,
                first.Z - second.Z
            );
            return result;
        }

        public static Vector3D operator *(Vector3D Vector3D, double scale)
        {
            return new Vector3D(Vector3D.X * scale, Vector3D.Y * scale, Vector3D.Z * scale);
        }

        public static Vector3D operator /(Vector3D Vector3D, double scale)
        {
            return new Vector3D(Vector3D.X / scale, Vector3D.Y / scale, Vector3D.Z / scale);
        }
    }
}