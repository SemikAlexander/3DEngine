using System;

namespace _3DEngine
{
    public class Camera
    {
        public bool IsCentralProjection { get; set; } = true;

        public const double MIN_FOV = 60.0;
        public const double MAX_FOV = 135.0;

        public const double MIN_THETA = 1;
        public const double MAX_THETA = 179;

        public Point3DSpherical Position;
        public Point3DSpherical Target;

        public int AnglePhi { get; private set; } = 270;
        public int AngleTheta { get; private set; } = 90;

        private Matrix cameraMatrix = ModificationMatrix.GetIdentityMatrix(4);

        private double fov = 90;
        public double FOV
        {
            get
            {
                return fov;
            }
            set
            {
                fov = Math.Min(Math.Max(value, MIN_FOV), MAX_FOV);
            }
        }

        public double NearClippingPlaneZ { get; set; }
        public double FarClippingPlaneZ { get; set; }

        public int FrameWidth { get; private set; }
        public int FrameHalfWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public int FrameHalfHeight { get; private set; }

        public double FocusDistance { get { return FrameWidth / 2.0 * Math.Tan(MathHelp.DegreesToRadians(fov / 2)); } }

        public Camera()
        {

        }

        public Camera(
            Point3D position, Point3D target,
            double nearClippingPaneZ, double farClippingPaneZ,
            int frameWidth, int frameHeight)
        {
            Position = new Point3DSpherical(position);
            Target = new Point3DSpherical(target);
            NearClippingPlaneZ = nearClippingPaneZ;
            FarClippingPlaneZ = farClippingPaneZ;
            ResizeFrame(frameWidth, frameHeight);
        }

        public void ResizeFrame(int width, int height)
        {
            FrameWidth = width;
            FrameHalfWidth = FrameWidth / 2;
            FrameHeight = height;
            FrameHalfHeight = FrameHeight / 2;
        }

        public Matrix GetCameraMatrix()
        {
            Matrix matrixInverted = ModificationMatrix.GetMoveMatrix(-Position.X, -Position.Y, -Position.Z);

            Vector3D N = new Vector3D(Target.X - Position.X, Target.Y - Position.Y, Target.Z - Position.Z);
            Vector3D V = new Vector3D(0, 1, 0);
            Vector3D U = V ^ N;
            V = U ^ N;

            U.Normalize();
            V.Normalize();
            N.Normalize();

            cameraMatrix = ModificationMatrix.GetUVNProjectionMatrix(
                U.X, U.Y, U.Z, 
                V.X, V.Y, V.Z,
                N.X, N.Y, N.Z);
            cameraMatrix = cameraMatrix.Multiply(matrixInverted);

            return cameraMatrix;
        }

        public void RotatePositionUpDown(double angle)
        {
            Matrix rotateMatrix = ModificationMatrix.GetMoveMatrix(-Target.X, -Target.Y, -Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelp.RadiansToDegrees(Position.Theta);
            angle = Math.Min(Math.Max(angle, MIN_THETA), MAX_THETA);
            Position.SetTheta(MathHelp.DegreesToRadians(angle));
            AngleTheta = 180 - (int)angle;

            rotateMatrix = ModificationMatrix.GetMoveMatrix(Target.X, Target.Y, Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        public void RotatePositionLeftRight(double angle)
        {
            Matrix rotateMatrix = ModificationMatrix.GetMoveMatrix(-Target.X, -Target.Y, -Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelp.RadiansToDegrees(Position.Phi);
            angle %= 360;
            angle = angle < 0 ? 360 + angle : angle;

            Position.SetPhi(MathHelp.DegreesToRadians(angle));
            AnglePhi = (int)angle;

            rotateMatrix = ModificationMatrix.GetMoveMatrix(Target.X, Target.Y, Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        public void RotateTargetUpDown(double angle)
        {
            angle = MathHelp.DegreesToRadians(angle);

            Matrix rotateMatrix = ModificationMatrix.GetMoveMatrix(-Position.X, -Position.Y, -Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelp.RadiansToDegrees(Target.Theta);
            angle = Math.Min(Math.Max(angle, MIN_THETA), MAX_THETA);
            Target.SetTheta(MathHelp.DegreesToRadians(angle));
            AngleTheta = 180 - (int)angle;

            rotateMatrix = ModificationMatrix.GetMoveMatrix(Position.X, Position.Y, Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        public void RotateTargetLeftRight(double angle)
        {
            Matrix rotateMatrix = ModificationMatrix.GetMoveMatrix(-Position.X, -Position.Y, -Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelp.RadiansToDegrees(Target.Phi);
            angle %= 360;
            angle = angle < 0 ? 360 + angle : angle;

            Target.SetPhi(MathHelp.DegreesToRadians(angle));
            AnglePhi = (int)angle;

            rotateMatrix = ModificationMatrix.GetMoveMatrix(Position.X, Position.Y, Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        public void MoveCameraAlongVector(double distance)
        {
            Vector3D vector = new Vector3D(Target) - new Vector3D(Position);
            vector.Normalize();
            vector = vector * distance;
            Target.Add(vector.X, vector.Y, vector.Z);
            Position.Add(vector.X, vector.Y, vector.Z);
        }

        public void MoveCameraLeftRight(double distance)
        {
            Vector3D vector = new Vector3D(Target) - new Vector3D(Position);
            vector.Normalize();
            vector = vector ^ new Vector3D(0, 1, 0);
            vector = vector * distance;
            Target.Add(vector.X, vector.Y, vector.Z);
            Position.Add(vector.X, vector.Y, vector.Z);
        }
        public void MoveCameraUpDown(double distance)
        {
            Vector3D vector = new Vector3D(Target) - new Vector3D(Position);
            vector.Normalize();
            vector = vector ^ (vector ^ new Vector3D(0, 1, 0));
            vector = vector * distance;
            Target.Add(vector.X, vector.Y, vector.Z);
            Position.Add(vector.X, vector.Y, vector.Z);
        }
    }
}
