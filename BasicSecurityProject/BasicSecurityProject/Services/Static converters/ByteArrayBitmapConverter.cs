using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Services.Static_converters
{
    public class ByteArrayBitmapConverter
    {
        public static Bitmap ConvertByteArrayToBitmap(byte[] imageData)
        {
            Bitmap imageAsBitmap;
            using (var ms = new MemoryStream(imageData))
            {
                imageAsBitmap = new Bitmap(ms);
            }
            return imageAsBitmap;
        }

        public static byte[] ConvertBitMapToByteArray(Bitmap bitmap)
        {
            byte[] result = null;
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, bitmap.RawFormat);
                result = stream.ToArray();
            }
            return result;
        }
    }
}
