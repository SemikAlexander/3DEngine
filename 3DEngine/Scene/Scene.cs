using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3DEngine
{
    public class Scene
    {
        private const int GIZMO_SIZE = 15;
        private const int GIZMO_ARROW_SIZE = 2;

        private PictureBox canvas;

        public enum MODE
        {
            WIREFRAME = 0,
            SOLID = 1,
            COMBINE = 2
        }

        public SceneObject PanObject = null;

        public Vector3D Light { get; set; }
        public List<Vector3D> Lights { get; set; } = new List<Vector3D>();
        public Camera Camera { get; set; }
        public List<Camera> Cameras { get; set; } = new List<Camera>();
        public MODE Mode { get; set; } = MODE.WIREFRAME;
        public bool IsGizmoVisible { get; set; } = true;
        public List<SceneObject> Objects { get; private set; } = new List<SceneObject>();

        public Scene(PictureBox canvas)
        {
            this.canvas = canvas;
            Camera = new Camera();
            ResetCamera();
            Cameras.Add(Camera);
            Lights.Add(new Vector3D(0, 150, -150));
            Light = Lights[0];
        }

        public void ResetCamera()
        {
            Camera = new Camera(
                new Point3D(0, 0, -200),
                new Point3D(0, 0, 200),
                10,
                1000.0,
                canvas.ClientSize.Width,
                canvas.ClientSize.Height
            );
        }
        public void ResetLight()
        {
            Light = new Vector3D(0, 150, -150);
        }

        public void AddObject(SceneObject sceneObject)
        {
            Objects.Add(sceneObject);
        }

        public SceneObject GetObjectByName(string name)
        {
            foreach (SceneObject sceneObject in Objects)
            {
                if (name.Equals(sceneObject.Name))
                {
                    return sceneObject;
                }
            }
            return null;
        }

        public void PaintObjects()
        {
            FastBitmap bitmap = new FastBitmap(new Bitmap(Camera.FrameWidth, Camera.FrameHeight));

            double[,] buffer = new double[Camera.FrameWidth, Camera.FrameHeight];
            for (int i = 0; i < Camera.FrameWidth; ++i)
                for (int j = 0; j < Camera.FrameHeight; ++j)
                    buffer[i, j] = double.MaxValue;

            Matrix cameraMatrix = Camera.GetCameraMatrix();

            if (PanObject == null)
            {
                if (IsGizmoVisible && IsVisibleForCamera(new Point3D(0, 0, 0), GIZMO_SIZE, cameraMatrix))
                {
                    DrawGizmo(bitmap, buffer, ModificationMatrix.GetIdentityMatrix(4), new Point3D(0, 0, 0), cameraMatrix);
                }

                Matrix lightMatrix = ModificationMatrix.GetIdentityMatrix(4);
                foreach (Vector3D light in Lights)
                {
                    Point3D lightPosition = new Point3D(light);
                    if (IsVisibleForCamera(lightPosition, lightPosition.Length, cameraMatrix))
                    {
                        Sphere lightSphere = new Sphere(lightPosition, 3, 4, Color.Yellow);
                        foreach (Face face in lightSphere.Faces)
                        {
                            List<Point3D> points = new List<Point3D>();
                            foreach (Point3D point in face.Points)
                            {
                                points.Add(point.Clone());
                            }
                            DrawLines(bitmap, buffer, points, lightSphere.Color, lightMatrix, lightPosition, lightMatrix, lightPosition, cameraMatrix);
                        }
                    }
                }
            }

            foreach (SceneObject sceneObject in Objects)
            {
                if (IsVisibleForCamera(sceneObject.BasePoint, sceneObject.MaxLength, cameraMatrix) && (PanObject == null || PanObject.Equals(sceneObject)))
                {
                    Matrix objectMatrix = sceneObject.GetModificationMatrix();
                    Matrix gizmoModificationMatrix = sceneObject.GetGizmoModificationMatrix();
                    if (Mode == MODE.WIREFRAME && IsGizmoVisible)
                    {
                        DrawGizmo(bitmap, buffer, gizmoModificationMatrix, sceneObject.BasePoint, cameraMatrix);
                    }
                    foreach (ScenePrimitive scenePrimitive in sceneObject.ScenePrimitives)
                    {
                        Matrix primitiveMatrix = scenePrimitive.GetModificationMatrix();
                        foreach (Face face in scenePrimitive.Primitive.Faces)
                        {
                            List<Point3D> points = new List<Point3D>();
                            foreach (Point3D point in face.Points)
                            {
                                points.Add(point.Clone());
                            }

                            if (Mode == MODE.WIREFRAME)
                            {
                                DrawLines(bitmap, buffer, points, scenePrimitive.Primitive.Color, primitiveMatrix, scenePrimitive.Primitive.BasePoint, objectMatrix, sceneObject.BasePoint, cameraMatrix);
                            }
                            else
                            {
                                ApplyMatrix(points, primitiveMatrix, scenePrimitive.Primitive.BasePoint, objectMatrix, sceneObject.BasePoint);

                                double brightness = CalculateLight(new Face(new Point3D[] { points[0], points[1], points[2] }));
                                Color color = Color.FromArgb(
                                    (int)(scenePrimitive.Primitive.Color.R * brightness),
                                    (int)(scenePrimitive.Primitive.Color.G * brightness),
                                    (int)(scenePrimitive.Primitive.Color.B * brightness)
                                );

                                ConvertLocalToCamera(points, cameraMatrix);
                                ConvertCameraToScreen(points);

                                Triangle(new Vector3D(points[0]), new Vector3D(points[1]), new Vector3D(points[2]), color, bitmap, buffer);
                            }
                        }
                    }
                }
            }

            FastBitmap bufferImage = new FastBitmap(new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height));
            double minimum = double.MaxValue;
            double maximum = double.MinValue;
            for (int i = 0; i < Camera.FrameWidth; ++i)
                for (int j = 0; j < Camera.FrameHeight; ++j)
                {
                    minimum = Math.Min(minimum, Math.Max(buffer[i, j], double.MinValue));
                    maximum = Math.Max(maximum, Math.Min(buffer[i, j], double.MaxValue));
                }

            canvas.Image = bitmap.GetBitmap();
        }

        private double CalculateLight(Face face)
        {
            double result = 0;
            Vector3D normal = face.GetNormalVector();
            foreach (Vector3D light in Lights)
            {
                result += Math.Abs(Vector3D.AngleCos(normal, light));
            }
            return Math.Min(result, 1);

        }

        private void DrawGizmo(FastBitmap bitmap, double[,] buffer, Matrix objectMatrix, Point3D basePoint, Matrix cameraMatrix)
        {
            List<Point3D> axisX = new List<Point3D>()
            {
                new Point3D(-GIZMO_SIZE, 0, 0),
                new Point3D(GIZMO_SIZE, 0, 0)
            };
            DrawLines(bitmap, buffer, axisX, Color.Red, objectMatrix, basePoint, cameraMatrix);
            List<Point3D> axisXArrow = new List<Point3D>()
            {
                new Point3D(GIZMO_SIZE, 0, 0),
                new Point3D(GIZMO_SIZE - GIZMO_ARROW_SIZE, GIZMO_ARROW_SIZE, 0),
                new Point3D(GIZMO_SIZE - GIZMO_ARROW_SIZE, -GIZMO_ARROW_SIZE, 0)
            };
            DrawLines(bitmap, buffer, axisXArrow, Color.Red, objectMatrix, basePoint, cameraMatrix);

            List<Point3D> axisY = new List<Point3D>()
            {
                new Point3D(0, -GIZMO_SIZE, 0),
                new Point3D(0, GIZMO_SIZE, 0)
            };
            DrawLines(bitmap, buffer, axisY, Color.Green, objectMatrix, basePoint, cameraMatrix);
            List<Point3D> axisYArrow = new List<Point3D>()
            {
                new Point3D(0, GIZMO_SIZE, 0),
                new Point3D(GIZMO_ARROW_SIZE, GIZMO_SIZE - GIZMO_ARROW_SIZE, 0),
                new Point3D(-GIZMO_ARROW_SIZE, GIZMO_SIZE - GIZMO_ARROW_SIZE, 0)
            };
            DrawLines(bitmap, buffer, axisYArrow, Color.Green, objectMatrix, basePoint, cameraMatrix);

            List<Point3D> axisZ = new List<Point3D>()
            {
                new Point3D(0, 0, -GIZMO_SIZE),
                new Point3D(0, 0, GIZMO_SIZE)
            };
            DrawLines(bitmap, buffer, axisZ, Color.Blue, objectMatrix, basePoint, cameraMatrix);
            List<Point3D> axisZArrow = new List<Point3D>()
            {
                new Point3D(0, 0, GIZMO_SIZE),
                new Point3D(GIZMO_ARROW_SIZE, 0, GIZMO_SIZE - GIZMO_ARROW_SIZE),
                new Point3D(-GIZMO_ARROW_SIZE, 0, GIZMO_SIZE - GIZMO_ARROW_SIZE)
            };
            DrawLines(bitmap, buffer, axisZArrow, Color.Blue, objectMatrix, basePoint, cameraMatrix);
        }

        private void DrawLines(FastBitmap bitmap, double[,] buffer, List<Point3D> points, Color color, Matrix objectMatrix, Point3D basePoint, Matrix cameraMatrix)
        {
            ConvertLocalToCamera(points, objectMatrix, basePoint, cameraMatrix);
            ConvertCameraToScreen(points);
            for (int pi = 0; pi < points.Count - 1; ++pi)
            {
                for (int pj = pi + 1; pj < points.Count; ++pj)
                {
                    DrawLine(bitmap, buffer, points[pi], points[pj], color);
                }
            }
        }

        private void DrawLines(FastBitmap bitmap, double[,] buffer, List<Point3D> points, Color color, Matrix primitiveMatrix, Point3D primitiveBasePoint, Matrix objectMatrix, Point3D objectBasePoint, Matrix cameraMatrix)
        {
            ConvertLocalToCamera(points, primitiveMatrix, primitiveBasePoint, objectMatrix, objectBasePoint, cameraMatrix);
            ConvertCameraToScreen(points);
            for (int pi = 0; pi < points.Count - 1; ++pi)
            {
                for (int pj = pi + 1; pj < points.Count; ++pj)
                {
                    DrawLine(bitmap, buffer, points[pi], points[pj], color);
                }
            }
        }

        private void ConvertLocalToCamera(List<Point3D> points, Matrix objectMatrix, Point3D basePoint, Matrix cameraMatrix)
        {
            foreach (Point3D point in points)
            {
                Matrix result = objectMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + basePoint.X, result[1, 0] + basePoint.Y, result[2, 0] + basePoint.Z);
                result = cameraMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0], result[1, 0], result[2, 0]);
            }
        }

        private void ConvertLocalToCamera(List<Point3D> points, Matrix primitiveMatrix, Point3D primitiveBasePoint, Matrix objectMatrix, Point3D objectBasePoint, Matrix cameraMatrix)
        {
            foreach (Point3D point in points)
            {
                Matrix result = primitiveMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + primitiveBasePoint.X, result[1, 0] + primitiveBasePoint.Y, result[2, 0] + primitiveBasePoint.Z);
                result = objectMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + objectBasePoint.X, result[1, 0] + objectBasePoint.Y, result[2, 0] + objectBasePoint.Z);
                result = cameraMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0], result[1, 0], result[2, 0]);
            }
        }

        private void ConvertLocalToCamera(List<Point3D> points, Matrix cameraMatrix)
        {
            foreach (Point3D point in points)
            {
                Matrix result = cameraMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0], result[1, 0], result[2, 0]);
            }
        }

        private void ApplyMatrix(List<Point3D> points, Matrix primitiveMatrix, Point3D primitiveBasePoint, Matrix objectMatrix, Point3D objectBasePoint)
        {
            foreach (Point3D point in points)
            {
                Matrix result = primitiveMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + primitiveBasePoint.X, result[1, 0] + primitiveBasePoint.Y, result[2, 0] + primitiveBasePoint.Z);
                result = objectMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + objectBasePoint.X, result[1, 0] + objectBasePoint.Y, result[2, 0] + objectBasePoint.Z);
            }
        }

        private void ConvertCameraToScreen(List<Point3D> points)
        {
            foreach (Point3D point in points)
            {
                double x = point.X;
                double y = point.Y;
                double z = point.Z;

                if (Camera.IsCentralProjection && z != 0)
                {
                    x = Camera.FocusDistance * x / z;
                    y = Camera.FocusDistance * y / z;
                }

                x += Camera.FrameHalfWidth;
                y += Camera.FrameHalfHeight;

                point.Set((int)x, (int)y, z);
            }
        }

        public bool CheckObjectsIntersection()
        {
            bool intersecting = false;
            for (int i = 0; i < Objects.Count - 1 && !intersecting; ++i)
            {
                SceneObject first = Objects[i];
                Point3D a = first.BasePoint;

                for (int j = i + 1; j < Objects.Count && !intersecting; ++j)
                {
                    SceneObject second = Objects[j];
                    Point3D b = second.BasePoint;

                    double distance = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z));

                    double size = first.MaxLength + second.MaxLength;
                    intersecting = distance < size;
                }
            }

            return intersecting;
        }

        private void Triangle(Vector3D p0, Vector3D p1, Vector3D p2, Color color, FastBitmap bitmap, double[,] buffer)
        {
            if (p0.Y != p1.Y || p0.Y != p2.Y)
            {
                if (p0.Y > p1.Y)
                {
                    MathHelp.Swap(ref p0, ref p1);
                }
                if (p0.Y > p2.Y)
                {
                    MathHelp.Swap(ref p0, ref p2);
                }
                if (p1.Y > p2.Y)
                {
                    MathHelp.Swap(ref p1, ref p2);
                }
                int totalHeight = (int)Math.Round(p2.Y - p0.Y);
                Parallel.For(0, totalHeight - 1, i =>
                {
                    bool secondHalf = i > p1.Y - p0.Y || p1.Y == p0.Y;
                    int segmentHeight = (int)Math.Round(secondHalf ? p2.Y - p1.Y : p1.Y - p0.Y);
                    double alpha = (double)i / totalHeight;
                    double beta = (i - (secondHalf ? p1.Y - p0.Y : 0.0)) / segmentHeight;
                    Vector3D a = p0 + (p2 - p0) * alpha;
                    Vector3D b = (secondHalf ? p1 + (p2 - p1) * beta : p0 + (p1 - p0) * beta);
                    if (a.X > b.X)
                    {
                        MathHelp.Swap(ref a, ref b);
                    }
                    for (int j = (int)Math.Round(a.X); j <= (int)Math.Round(b.X); ++j)
                    {
                        double scale = (a.X == b.X) ? 1 : (j - a.X) / (b.X - a.X);
                        Vector3D p = a + (b - a) * scale;
                        int x = (int)Math.Round(p.X);
                        int y = (int)Math.Round(p.Y);
                        if (IsOnImage(x, y) && buffer[x, y] >= p.Z)
                        {
                            buffer[x, y] = p.Z;
                            bitmap.SetPixel(x, y, color);
                        }
                    }
                });
            }
        }
        private bool IsVisibleForCamera(Point3D basePoint, double maxLength, Matrix cameraMatrix)
        {
            bool result = false;

            Point3D point = basePoint.Clone();
            Matrix cameraToBase = cameraMatrix.Multiply(point.GetProjectiveCoordinates());
            point.Set(cameraToBase[0, 0], cameraToBase[1, 0], cameraToBase[2, 0]);

            if (point.Z + maxLength < Camera.FarClippingPlaneZ && point.Z - maxLength > Camera.NearClippingPlaneZ)
            {
                if (Camera.IsCentralProjection)
                {
                    double testZ = Camera.FrameHalfWidth * point.Z / Camera.FocusDistance;

                    if (point.X - maxLength < testZ && point.X + maxLength > -testZ)
                    {
                        testZ = Camera.FrameHalfHeight * point.Z / Camera.FocusDistance;

                        if (point.Y - maxLength < testZ && point.Y + maxLength > -testZ)
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    if (Math.Abs(point.X) + maxLength < Camera.FrameHalfWidth && Math.Abs(point.Y) + maxLength < Camera.FrameHalfHeight)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool IsObjectIntersectWithClippingPlanes(SceneObject sceneObject)
        {
            bool result = true;

            Point3D point = sceneObject.BasePoint.Clone();
            Matrix cameraToBase = Camera.GetCameraMatrix().Multiply(point.GetProjectiveCoordinates());
            point.Set(cameraToBase[0, 0], cameraToBase[1, 0], cameraToBase[2, 0]);

            if (point.Z + sceneObject.MaxLength < Camera.FarClippingPlaneZ && point.Z - sceneObject.MaxLength > Camera.NearClippingPlaneZ)
            {
                result = false;
            }

            return result;
        }

        private bool IsOnImage(int x, int y)
        {
            return x > 0 && x < canvas.ClientSize.Width && y > 0 && y < canvas.ClientSize.Height;
        }

        private void DrawLine(FastBitmap bitmap, double[,] buffer, Point3D start, Point3D end, Color color)
        {
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx < 0)
                {
                    MathHelp.Swap(ref start, ref end);
                }
                double[] ys = MathHelp.Interpolate(start.X, start.Y, end.X, end.Y);
                double[] zs = MathHelp.Interpolate(start.X, start.Z, end.X, end.Z);
                Parallel.For((int)start.X, (int)end.X, x =>
                {
                    int y = (int)ys[(int)(x - start.X)];
                    double z = zs[(int)(x - start.X)];
                    if (IsOnImage((int)x, y) && buffer[(int)x, y] >= z)
                    {
                        buffer[(int)x, y] = z;
                        bitmap.SetPixel((int)x, y, color);
                    }
                });
            }
            else
            {
                if (dy < 0)
                {
                    MathHelp.Swap(ref start, ref end);
                }
                double[] xs = MathHelp.Interpolate(start.Y, start.X, end.Y, end.X);
                double[] zs = MathHelp.Interpolate(start.Y, start.Z, end.Y, end.Z);
                Parallel.For((int)start.Y, (int)end.Y, y =>
                {
                    int x = (int)xs[(int)(y - start.Y)];
                    double z = zs[(int)(y - start.Y)];
                    if (IsOnImage(x, (int)y) && buffer[x, (int)y] >= z)
                    {
                        buffer[x, (int)y] = z;
                        bitmap.SetPixel(x, (int)y, color);
                    }
                });
            }
        }
    }
}