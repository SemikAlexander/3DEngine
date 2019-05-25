using System;

namespace _3DEngine
{
    public class ModificationMatrix
    {
        private static Matrix moveMatrix = GetIdentityMatrix(4);
        private static Matrix scaleMatrix = GetIdentityMatrix(4);
        private static Matrix rotateXMatrix = GetIdentityMatrix(4);
        private static Matrix rotateYMatrix = GetIdentityMatrix(4);
        private static Matrix rotateZMatrix = GetIdentityMatrix(4);
        private static Matrix uvnProjectionMatrix = GetIdentityMatrix(4);

        public static Matrix GetIdentityMatrix(int n)
        {
            double[,] result = new double[n, n];
            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < n; ++j) {
                    if (i == j) {
                        result[i, j] = 1.0;
                    } else {
                        result[i, j] = 0.0;
                    }
                }
            }
            return new Matrix(result);
        }

        public static Matrix GetMoveMatrix(double dx, double dy, double dz)
        {
            moveMatrix[0, 3] = dx;
            moveMatrix[1, 3] = dy;
            moveMatrix[2, 3] = dz;

            return moveMatrix;
        }

        public static Matrix GetScaleMatrix(double sx, double sy, double sz)
        {
            scaleMatrix[0, 0] = sx;
            scaleMatrix[1, 1] = sy;
            scaleMatrix[2, 2] = sz;

            return scaleMatrix;
        }

        public static Matrix GetScaleMatrix(double scale)
        {
            return GetScaleMatrix(scale, scale, scale);
        }

        public static Matrix GetRotateXMatrix(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            rotateXMatrix[1, 1] = cos;
            rotateXMatrix[2, 1] = -sin;
            rotateXMatrix[1, 2] = sin;
            rotateXMatrix[2, 2] = cos;

            return rotateXMatrix;
        }

        public static Matrix GetRotateYMatrix(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            rotateYMatrix[0, 0] = cos;
            rotateYMatrix[0, 2] = -sin;
            rotateYMatrix[2, 0] = sin;
            rotateYMatrix[2, 2] = cos;

            return rotateYMatrix;
        }

        public static Matrix GetRotateZMatrix(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            rotateZMatrix[0, 0] = cos;
            rotateZMatrix[1, 0] = -sin;
            rotateZMatrix[0, 1] = sin;
            rotateZMatrix[1, 1] = cos;

            return rotateZMatrix;
        }

        public static Matrix GetUVNProjectionMatrix(double ux, double uy, double uz, double vx, double vy, double vz, double nx, double ny, double nz)
        {
            uvnProjectionMatrix[0, 0] = ux;
            uvnProjectionMatrix[1, 0] = vx;
            uvnProjectionMatrix[2, 0] = nx;

            uvnProjectionMatrix[0, 1] = uy;
            uvnProjectionMatrix[1, 1] = vy;
            uvnProjectionMatrix[2, 1] = ny;

            uvnProjectionMatrix[0, 2] = uz;
            uvnProjectionMatrix[1, 2] = vz;
            uvnProjectionMatrix[2, 2] = nz;

            return uvnProjectionMatrix;
        }
    }
}
