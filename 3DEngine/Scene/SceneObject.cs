using System;
using System.Collections.Generic;

namespace _3DEngine
{
    public class SceneObject
    {
        public string Name { get; private set; }
        public Point3D BasePoint { get; set; }

        public List<ScenePrimitive> ScenePrimitives { get; private set; } = new List<ScenePrimitive>();

        public SceneObject(string name)
        {
            Name = name;
        }

        public void AddScenePrimitive(ScenePrimitive scenePrimitive)
        {
            ScenePrimitives.Add(scenePrimitive);
        }

        public ScenePrimitive GetScenePrimitiveByName(string name)
        {
            foreach (ScenePrimitive scenePrimitive in ScenePrimitives)
            {
                if (name.Equals(scenePrimitive.Name))
                {
                    return scenePrimitive;
                }
            }
            return null;
        }

        public double ScaleX { get; private set; } = 1.0;
        public double ScaleY { get; private set; } = 1.0;
        public double ScaleZ { get; private set; } = 1.0;
        public int AngleX { get; set; } = 0;
        public int AngleY { get; set; } = 0;
        public int AngleZ { get; set; } = 0;
        private double maxLength = 0;
        public double MaxLength
        {
            get
            {
                if (maxLength == 0)
                {
                    double result = 0;
                    foreach (ScenePrimitive primitive in ScenePrimitives)
                    {
                        result = Math.Max(primitive.MaxLength, result);
                    }
                    maxLength = result * Math.Max(Math.Max(ScaleX, ScaleY), ScaleZ);
                }
                return maxLength;
            }
        }

        public void SetScale(double scaleX, double scaleY, double scaleZ)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleZ = scaleZ;
            maxLength = 0;
        }

        public Matrix GetModificationMatrix()
        {
            Matrix matrix = ModificationMatrix.GetIdentityMatrix(4);
            matrix = matrix.Multiply(ModificationMatrix.GetScaleMatrix(ScaleX, ScaleY, ScaleZ));
            matrix = matrix.Multiply(ModificationMatrix.GetRotateXMatrix(MathHelp.DegreesToRadians(AngleX)));
            matrix = matrix.Multiply(ModificationMatrix.GetRotateYMatrix(MathHelp.DegreesToRadians(AngleY)));
            matrix = matrix.Multiply(ModificationMatrix.GetRotateZMatrix(MathHelp.DegreesToRadians(AngleZ)));
            return matrix;
        }

        public Matrix GetGizmoModificationMatrix()
        {
            Matrix matrix = ModificationMatrix.GetIdentityMatrix(4);
            matrix = matrix.Multiply(ModificationMatrix.GetRotateXMatrix(MathHelp.DegreesToRadians(AngleX)));
            matrix = matrix.Multiply(ModificationMatrix.GetRotateYMatrix(MathHelp.DegreesToRadians(AngleY)));
            matrix = matrix.Multiply(ModificationMatrix.GetRotateZMatrix(MathHelp.DegreesToRadians(AngleZ)));
            return matrix;
        }
    }
}
