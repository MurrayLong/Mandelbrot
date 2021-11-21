using System;
using System.Drawing;
using System.Linq;

namespace Mandelbrot
{

    public record Complex(double real, double imaginary) {
        public double Mag2 => (real * real) + (imaginary * imaginary);

        public static Complex Zero = new Complex(0, 0);

        public static Complex operator * (Complex a, Complex b) =>
            new Complex(a.real * b.real - (a.imaginary * b.imaginary), (a.real * b.imaginary) + (b.real * a.imaginary));

        public static Complex operator + (Complex a, Complex b) =>
            new Complex(a.real + b.real, a.imaginary + b.imaginary);

        public override string ToString()
            => $"{real} + {imaginary}i";
    }

    class Program
    {
        const int ITERATIONS = 256;
        const double MAX2 = 4;
        static int Bounded(Complex c) {
            var n = Complex.Zero;

            for (int i =0; i< ITERATIONS; i++) {
                n = (n * n) + c;

                if (n.Mag2 > MAX2) return i;
            }
            return ITERATIONS;
        }

        static void Main(string[] args)
        {
            var resolution = (x:4096, y:4096);

            var bmp = new Bitmap(resolution.x, resolution.y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            foreach (var x in Enumerable.Range(0, resolution.x))
            {
                foreach (var y in Enumerable.Range(0, resolution.y))
                {
                    var r = (x / (double)resolution.x) * 3 -2;
                    var i = (y / (double)resolution.y) * 3 -1.5;
                    var iterations = 255-(Bounded(new Complex(r, i))*255/ITERATIONS);
                    bmp.SetPixel(x, y, Color.FromArgb(iterations, iterations, iterations));
                }
            }
            bmp.Save(@"output.bmp");
            return;
        }
    }
}
