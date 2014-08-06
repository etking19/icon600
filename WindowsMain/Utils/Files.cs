using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class Files
    {
        public static bool IsFileExists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }
    }
}
