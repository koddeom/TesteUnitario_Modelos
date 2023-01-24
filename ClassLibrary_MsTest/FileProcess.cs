using System;
using System.IO;

namespace ClassLibrary_MsTest
{
    public class FileProcess
    {
        public bool FileExists(String filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("file name");
            }

            return File.Exists(filename);
        }
    }
}