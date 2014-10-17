using Session;
using Session.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using WindowsFormClient.RgbInput;
using WindowsFormClient.Server;

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

            foreach (Tuple<int, Window, Input, OnScreenDisplay> data in Server.ServerVisionHelper.getInstance().GetAllVisionInputs())
            {
                System.Xml.Serialization.XmlSerializer wndSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Window));
                StringWriter wndTextWriter = new StringWriter();
                wndSerializer.Serialize(wndTextWriter, data.Item2);

                System.Xml.Serialization.XmlSerializer inputSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Input));
                StringWriter inputTextWriter = new StringWriter();
                inputSerializer.Serialize(inputTextWriter, data.Item3);

                System.Xml.Serialization.XmlSerializer osdSerializer = new System.Xml.Serialization.XmlSerializer(typeof(OnScreenDisplay));
                StringWriter osdTextWriter = new StringWriter();
                osdSerializer.Serialize(osdTextWriter, data.Item4);

                table.Rows.Add(
                    data.Item1, 
                    wndTextWriter.ToString(),
                    inputTextWriter.ToString(),
                    osdTextWriter.ToString(),
                    data.Item3.InputNumber, 
                    data.Item3.LabelName,
                    data.Item3.InputCropping,
                    data.Item3.InputCropWidth,
                    data.Item3.InputCropHeight);
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
