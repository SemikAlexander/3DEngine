using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class FastBitmap
{
    private int stride;
    private byte[] data;

    public int Width { get; }
    public int Height { get; }

    public FastBitmap(int width, int height, Color[,] colors)
    {
        data = ConvertToByte(colors);
        Width = width;
        Height = height;
        stride = data.Length / height;
    }

    private byte[] ConvertToByte(Color[,] colors)
    {
        byte[] result = new byte[colors.Length * 4];
        int position = 0;
        for (int y = 0; y < colors.GetLength(1); ++y)
            for (int x = 0; x < colors.GetLength(0); ++x)
            {
                result[position++] = colors[x, y].B;
                result[position++] = colors[x, y].G;
                result[position++] = colors[x, y].R;
                result[position++] = colors[x, y].A;
            }
        return result;
    }

    public FastBitmap(int width, int height, byte[] data)
    {
        this.data = data;
        Width = width;
        Height = height;
        stride = data.Length / height;
    }

    public byte[] GetData()
    {
        return data;
    }

    public FastBitmap(Bitmap image)
    {
        BitmapData bits = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        stride = bits.Stride;
        int bytes = stride * bits.Height;
        data = new byte[bytes];
        Marshal.Copy(bits.Scan0, data, 0, bytes);
        image.UnlockBits(bits);
        Width = image.Width;
        Height = image.Height;
    }

    public Bitmap GetBitmap()
    {
        Bitmap result = new Bitmap(Width, Height);
        BitmapData bits = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        Marshal.Copy(data, 0, bits.Scan0, data.Length);
        result.UnlockBits(bits);
        return result;
    }

    public Color GetPixel(int x, int y)
    {
        int position = y * stride + x * 4;
        int b = data[position++];
        int g = data[position++];
        int r = data[position++];
        int a = data[position++];
        return Color.FromArgb(a, r, g, b);
    }

    private void SetPixel(int x, int y, byte r, byte g, byte b, byte a = 255)
    {
        if (x > 0 && x < Width && y > 0 && y < Height)
        {
            int position = y * stride + x * 4;
            data[position] = (byte)(b * ((float)a / 255) + data[position] * ((float)(255 - a) / 255));
            ++position;
            data[position] = (byte)(g * ((float)a / 255) + data[position] * ((float)(255 - a) / 255));
            ++position;
            data[position] = (byte)(r * ((float)a / 255) + data[position] * ((float)(255 - a) / 255));
            ++position;
            data[position] = (byte)(a * ((float)a / 255) + data[position] * ((float)(255 - a) / 255));
        }
    }

    public void SetPixel(int x, int y, Color color)
    {
        SetPixel(x, y, color.R, color.G, color.B, color.A);
    }

    public void SwapPixels(int x, int y, int newX, int newY)
    {
        Color color = GetPixel(x, y);
        SetPixel(x, y, GetPixel(newX, newY));
        SetPixel(newX, newY, color);
    }

    public void DrawRectangle(Rectangle rectangle, Color color)
    {
        for (int y = rectangle.Y; y < rectangle.Y + rectangle.Height; ++y)
            for (int x = rectangle.X; x < rectangle.X + rectangle.Width; ++x)
                SetPixel(x, y, color.R, color.G, color.B, color.A);
    }

    public void Fill(Color color)
    {
        DrawRectangle(new Rectangle(0, 0, Width, Height), color);
    }

    public void DrawLine(int startX, int startY, int endX, int endY, Color color)
    {
        bool swap = false;
        if (Math.Abs(endY - startY) > Math.Abs(endX - startX))
        {
            Swap(ref startX, ref startY);
            Swap(ref endX, ref endY);
            swap = true;
        }
        if (endX < startX)
        {
            Swap(ref startX, ref endX);
            Swap(ref startY, ref endY);
        }
        int lengthX = endX - startX;
        int doubleLengthY = Math.Abs(endY - startY) * 2;
        int doubleLengthX = lengthX * 2;
        int step = startY < endY ? 1 : -1;
        int y = startY;
        int error = 0;
        for (int x = startX; x <= endX; ++x)
        {
            if (swap) SetPixel(y, x, color);
            else SetPixel(x, y, color);
            error += doubleLengthY;
            if (error > lengthX)
            {
                y += step;
                error -= doubleLengthX;
            }
        }
    }

    private void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
}