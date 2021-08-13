#r "nuget: System.Drawing.Common, 5.0.0"

using System.Drawing;

void TestBitmap(){
    Bitmap bmp = new Bitmap(400,400);
    Console.WriteLine($"Create Bitmap with size({bmp.Width}, {bmp.Height})");
}

TestBitmap();

Console.WriteLine("Hello World!");