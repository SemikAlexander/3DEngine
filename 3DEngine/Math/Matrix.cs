namespace _3DEngine
{
    public class Matrix
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private double[,] values;

        public double this[int j, int i]
        {
            get
            {
                return values[j, i];
            }
            set
            {
                values[j, i] = value;
            }
        }

        public Matrix(int height, int width)
        {
            Height = height;
            Width = width;
            values = new double[Height, Width];
        }

        public Matrix(double[] values)
        {
            Height = values.GetLength(0);
            Width = 1;

            this.values = new double[Height, Width];

            for (int j = 0; j < Height; ++j)
            {
                this.values[j, 0] = values[j];
            }
        }

        public Matrix(double[,] values)
        {
            Height = values.GetLength(0);
            Width = values.GetLength(1);

            this.values = new double[Height, Width];

            for (int j = 0; j < Height; ++j)
            {
                for (int i = 0; i < Width ; ++i)
                {
                    this.values[j, i] = values[j, i];
                }
            }
        }

        public Matrix Multiply(Matrix matrix)
        {
            if (Width == matrix.Height)
            {
                Matrix result = new Matrix(Height, matrix.Width);
                for (int i = 0; i < result.Width; ++i)
                {
                    for (int j = 0; j < result.Height; ++j)
                    {
                        for (int k = 0; k < matrix.Height; ++k)
                        {
                            result[j, i] += this[j, k] * matrix[k, i];
                        }
                    }
                }
                return result;
            }
            else
            {
                return this;
            }
        }
    }
}
