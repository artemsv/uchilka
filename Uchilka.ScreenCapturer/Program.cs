using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uchilka.ScreenCapturer
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Uchilka Screen Capturer: started");

            if (args.Length > 1)
            {
                var targetDir = args[0];

                Debug.WriteLine($"Uchilka Screen Capturer: target dir = {targetDir}");

                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                    Debug.WriteLine($"Uchilka Screen Capturer: target dir created");
                }

                var bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);
                Graphics graphics = Graphics.FromImage(bitmap as Image);
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

                var fileName = args[1];
                Debug.WriteLine($"Uchilka Screen Capturer: file name = {fileName}");

                var filePath = Path.Combine(targetDir, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                bitmap.Save(filePath, ImageFormat.Png);
            }else
            {
                Debug.WriteLine("Uchilka Screen Capturer: no arguments");
            }

            Debug.WriteLine("Uchilka Screen Capturer: finished");
        }
    }
}
