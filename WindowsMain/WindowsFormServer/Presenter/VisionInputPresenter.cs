using System;
using System.Data;
using System.IO;
using WindowsFormClient.RgbInput;

namespace WindowsFormClient.Presenter
{
    public class VisionInputPresenter
    {
        public VisionInputPresenter()
        {
        }

        public DataTable GetVisionInputTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Input DB Id", typeof(uint)).ReadOnly = true;

            table.Columns.Add("Windows Obj", typeof(string)).ReadOnly = true;
            table.Columns.Add("Input Obj", typeof(string)).ReadOnly = true;
            table.Columns.Add("OSD Obj", typeof(string)).ReadOnly = true;

            table.Columns.Add("Input", typeof(int)).ReadOnly = true;
            table.Columns.Add("Display Name", typeof(string)).ReadOnly = true;
            table.Columns.Add("Cropping", typeof(bool)).ReadOnly = true;
            table.Columns.Add("Cropping Width", typeof(uint)).ReadOnly = true;
            table.Columns.Add("Cropping Height", typeof(uint)).ReadOnly = true;

            foreach (Tuple<int, string, string, string> data in Server.ServerDbHelper.GetInstance().GetAllVisionInputs())
            {
                System.Xml.Serialization.XmlSerializer inputSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Input));
                TextReader inputReader = new StringReader(data.Item3);
                Input input = (Input)inputSerializer.Deserialize(inputReader);

                table.Rows.Add(
                    data.Item1, 
                    data.Item2,
                    data.Item3,
                    data.Item4,
                    input.InputNumber,
                    input.LabelName,
                    input.InputCropping,
                    input.InputCropWidth,
                    input.InputCropHeight);
            }

            return table;
        }

        public Window GetWindowFromString(string cmd)
        {
            System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Window));
            TextReader wndReader = new StringReader(cmd);
            return (Window)wndSerializer.Deserialize(wndReader);
        }

        public Input GetInputFromString(string cmd)
        {
            System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Input));
            TextReader wndReader = new StringReader(cmd);
            return (Input)wndSerializer.Deserialize(wndReader);
        }

        public OnScreenDisplay GetOSDFromString(string cmd)
        {
            System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(typeof(OnScreenDisplay));
            TextReader wndReader = new StringReader(cmd);
            return (OnScreenDisplay)wndSerializer.Deserialize(wndReader);
        }

        public void AddVisionInput(Window window, Input input, OnScreenDisplay osd)
        {
            Server.ServerVisionHelper.getInstance().AddVisionInput(window, input, osd);
        }

        public void RemoveVisionInput(uint id)
        {
            Server.ServerVisionHelper.getInstance().RemoveVisionInput(id);
        }

        public void EditVisionInput(uint id, Window window, Input input, OnScreenDisplay osd)
        {
            Server.ServerVisionHelper.getInstance().EditVisionInput(id, window, input, osd);
        }

        public uint GetNumberOfInputs()
        {
            return Server.ServerVisionHelper.getInstance().GetNumberOfInputs();
        }
    }
}
