using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace LicenseChecker
{
    public class Utils
    {
        public static string ReadFile(string filePath)
        {
            string content = String.Empty;
            if (System.IO.File.Exists(filePath))
            {
                content = System.IO.File.ReadAllText(filePath, Encoding.ASCII);
            }

            return content;
        }

        public static byte[] ReadFileByte(string filePath)
        {
            return ConvertToBinary(ReadFile(filePath));
        }

        private static byte[] ConvertToBinary(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        public static bool WriteFile(string filePath, byte[] content)
        {
            bool result = false;

            try
            {
                // create the file
                System.IO.FileStream stream = System.IO.File.Create(filePath);
                stream.Write(content, 0, content.Length);
                stream.Close();

                result = true;
            }
            catch
            {
            }
            

            return result;
        }

        public static string GetMachineIdentifier()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
    }
}
