using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.RgbInput
{
    public class Input
    {
        public enum EInputType
        {
            Unknown = 0,
            VGA,
            DVI,
            Component,
            Composite,
            SVideo,
            OutOfRange,
        }

        private uint inputNumber = 1;

        public uint InputNumber 
        {
            get
            {
                return inputNumber;
            }
            set
            {
                inputNumber = value;
            }
        }

        public string LabelName { get; set; }
        public uint InputCaptureWidth { get; set; }
        public uint InputCaptureHeight { get; set; }
        public bool InputCropping { get; set; }
        public int InputCropLeft { get; set; }
        public int InputCropTop { get; set; }
        public uint InputCropWidth { get; set; }
        public uint InputCropHeight { get; set; }
    }
}
