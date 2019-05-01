using BasicSecurityProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Encryption
{
    public class Steganography : ISteganography
    {
        //encryption
        public void Encryption(Bitmap imageToEncryptWith, String messageToEncrypt, String SaveFilePath, String SaveFileName)
        {
            Bitmap image = imageToEncryptWith;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    if (i < 1 && j < messageToEncrypt.Length)
                    {
                        char letter = Convert.ToChar(messageToEncrypt.Substring(j, 1));
                        int value = Convert.ToInt32(letter);
                        image.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, value));
                    }

                    if (i == image.Width - 1 && j == image.Height - 1)
                    {
                        image.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, messageToEncrypt.Length));
                    }
                }
            }
            image.Save(SaveFilePath + "/" + SaveFileName + ".png");
        }


        //decryption
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
    }
}
