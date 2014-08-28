using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils
{
    public class Files
    {
        public static bool IsFileExists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }

        public static List<string> DirSearch(string sDir, string fileNameWithExtension)
        {
            List<string> fileList = new List<string>();
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    try
                    {
                        foreach (string f in Directory.GetFiles(d, String.Format("{0}", fileNameWithExtension)))
                        {
                            fileList.Add(f);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        fileList.AddRange(DirSearch(d, fileNameWithExtension));
                    }
                    catch (Exception)
                    {
                    }
                    
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return fileList;
        }
    }
}
