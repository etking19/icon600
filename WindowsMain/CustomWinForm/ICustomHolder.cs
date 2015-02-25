using System;
using System.Collections.Generic;
using System.Text;

namespace CustomWinForm
{
    public interface ICustomHolder
    {
        bool performSizeSnapCheck(int xRelativePos, int yRelativePos, int xRelativeWidth, int xRelativeHeight, 
            out int xRelativeSnapPos, out int yRelativeSnapPos, out int relativeSnapWidth, out int relativeSnapHeight);

        bool performLocationSnapCheck(int xRelativePos, int yRelativePos, out int snapX, out int snapY);
    }
}
