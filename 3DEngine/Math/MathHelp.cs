using System;

namespace _3DEngine
{
    public class MathHelp
    {
        public static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static double RadiansToDegrees(double angle)
        {
            return angle * 180.0 / Math.PI;
        }
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static double[] Interpolate(double sx, double sy, double ex, double ey)
        {
            double[] values;
            if (sx == ex)
            {
                values = new double[] { sy };
            }
            else
            {
                values = new double[(int)Math.Abs(ex - sx + 1)];
                double step = (ey - sy) / (ex - sx);
                double value = sy;
                for (var i = sx; i <= ex; ++i)
                {
                    values[(int)(i - sx)] = value;
                    value += step;
                }
            }
            return values;
        }
    }
}
