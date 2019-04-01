using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSecurityProject.Services
{
    public class FileBytesConverter
    {
        public static byte[] fileToByteArray(string pathWhereToLoadFile)
        {
            FileStream stream = File.OpenRead(pathWhereToLoadFile);
            byte[] fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            stream.Close();
            return fileBytes;
        }
        
        public static void byteArrayToFile(byte[] fileBytes, string pathWhereToCreateFile)
        {
            //@"c:\path\to\your\file\here.txt"
            using (Stream file = File.OpenWrite(pathWhereToCreateFile))
            {
                file.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}
