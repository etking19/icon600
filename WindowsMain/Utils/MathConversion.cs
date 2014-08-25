using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class MathConversion
    {
        public static IntPtr MakeLParam(Int32 LoWord, Int32 HiWord)
        {
            Int32 i = (HiWord << 16) | (LoWord & 0xffff);
            return new IntPtr(i);
        }

        public static void SplitParam(IntPtr input, out Int16 LoWord, out Int16 HiWord)
        {
            LoWord = BitConverter.ToInt16(BitConverter.GetBytes(input.ToInt32()), 0);
            HiWord = BitConverter.ToInt16(BitConverter.GetBytes(input.ToInt32()), 2);
        }
    }
}
