using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class StringEncoding
    {
        public static string RandomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string ConvertBytesToString(byte[] bytes)
        {
            return ASCIIEncoding.Unicode.GetString(bytes);
        }

        public static byte[] ConvertStringToBytes(string str)
        {
            return ASCIIEncoding.Unicode.GetBytes(str);
        }
    }
}
