using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace License
{
    public class Encryptor
    {
        private const int BYTE_DATA_START = 363;
        private const int FILE_SIZE = 100000;

        private static Encryptor sInstance = null;

        private Encryptor()
        {

        }

        public static Encryptor GetInstance()
        {
            if (sInstance == null)
            {
                sInstance = new Encryptor();
            }

            return sInstance;
        }

        /// <summary>
        /// Decode the input to output the desire string
        /// </summary>
        /// <param name="encodedString"></param>
        /// <returns>mac address of machine</returns>
        public string DecodeContent(byte[] encodedData)
        {
            if (encodedData == null || encodedData.Length == 0)
            {
                return String.Empty;
            }

            // read the bytes data length
            int contentLength = encodedData[encodedData.Length -1];

            // get the actual data
            var xorByte = new byte[contentLength];
            for (int i = BYTE_DATA_START, k = 0; k < contentLength; i++, k++)
            {
                xorByte[k] = encodedData[i];
            }

            return ConvertToString(xorByte);
        }

        /// <summary>
        /// Encode the mac address and output the encoded string
        /// </summary>
        /// <param name="macAdd"></param>
        /// <returns>encoded string</returns>
        public byte[] EncodeContent(string macAdd)
        {
            byte[] finalByte = ConvertToBinary(macAdd);
            var xorByte = new byte[FILE_SIZE];

            // randomly generate dummy data first
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < xorByte.Length; i++)
            {
                xorByte[i] = (byte)(random.Next(-127, 127));
            }

            // put the actual data in between
            for (int i = BYTE_DATA_START, k = 0; k < finalByte.Length; i++, k++)
            {
                xorByte[i] = finalByte[k];
            }

            // put the number of byte needs to be read as last input
            xorByte[xorByte.Length - 1] = (byte)(finalByte.Length);

            string test = DecodeContent(xorByte);
            return xorByte;
        }

        private byte[] ConvertToBinary(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        private string ConvertToString(byte[] data)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetString(data);
        }
    }
}
