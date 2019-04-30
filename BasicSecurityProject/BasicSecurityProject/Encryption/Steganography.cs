using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Encryption
{
    public class Steganography
    {

        public void Encryption(Bitmap imageToEncryptWith, String messageToEncrypt, String SaveFilePath, String SaveFileName)
        {
            Bitmap image = imageToEncryptWith;

            for(int i = 0; i < image.Width; i++)
            {
                for(int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    if(i < 1 && j < messageToEncrypt.Length)
                    {
                        char letter = Convert.ToChar(messageToEncrypt.Substring(j, 1));
                        int value = Convert.ToInt32(letter);
                        image.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, value));
                    }

                    if(i == image.Width - 1 && j == image.Height - 1)
                    {
                        image.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, messageToEncrypt.Length));
                    }
                }
            }
            image.Save(SaveFilePath + "/" + SaveFileName + ".png");
        }

        public void Decryption(Bitmap imageToDecrypt, String SaveFilePath, String SaveFileName)
        {
            Bitmap image = imageToDecrypt;
            string message = "";
            Color lpixel = image.GetPixel(image.Width - 1, image.Height - 1);
            int messageLength = lpixel.B;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    if (i < 1 && j < messageLength)
                    {
                        int value = pixel.B;
                        char character = Convert.ToChar(value);
                        string letter = character.ToString();
                        message = message + letter;
                    }
                }
            }
            File.WriteAllText(SaveFilePath + "/" + SaveFileName + ".txt", message);
        }

        public Bitmap ConvertByteArrayToBitmap(byte[] imageData)
        {
            Bitmap imageAsBitmap;
            using (var ms = new MemoryStream(imageData))
            {
                imageAsBitmap = new Bitmap(ms);
            }
            return imageAsBitmap;
        }

        public byte[] ConvertBitMapToByteArray(Bitmap bitmap)
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
