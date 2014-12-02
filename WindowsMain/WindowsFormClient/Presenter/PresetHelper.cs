using CustomWinForm;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using WindowsFormClient.Client.Model;

namespace WindowsFormClient.Presenter
{
    class PresetHelper
    {
        public Dictionary<ControlAttributes, Client.Model.ApplicationModel> TriggeredAppList
        {
            get { return triggeredAppList; }
        }

        public Dictionary<ControlAttributes, Client.Model.VncModel> TriggeredVncList
        {
            get { return triggeredVncList; }
        }

        public Dictionary<ControlAttributes, InputAttributes> TriggeredVisionList
        {
            get { return triggeredInputList; }
        }

        private CustomControlHolder mimicWndHolder;

        private Dictionary<ControlAttributes, Client.Model.ApplicationModel> triggeredAppList = new Dictionary<ControlAttributes, ApplicationModel>();
        private Dictionary<ControlAttributes, Client.Model.VncModel> triggeredVncList = new Dictionary<ControlAttributes, VncModel>();
        private Dictionary<ControlAttributes, InputAttributes> triggeredInputList = new Dictionary<ControlAttributes, InputAttributes>();

        private List<Client.Model.ApplicationModel> pendingAppList = new List<ApplicationModel>();
        private List<Client.Model.VncModel> pendingVncList = new List<VncModel>();
        private List<InputAttributes> pendingVisionList = new List<InputAttributes>();

        public PresetHelper(CustomControlHolder holder)
        {
            this.mimicWndHolder = holder;
        }

        public void AddTriggeredApplication(Client.Model.ApplicationModel appModel)
        {
            pendingAppList.Add(appModel);

            ThreadPool.QueueUserWorkItem(s =>
            {
                Thread.Sleep(1500);
                // not a safe way if user trigger 2 similar application within 1.5 second
                if (pendingAppList.Remove(appModel))
                {
                    ControlAttributes attr = mimicWndHolder.GetTopMostControl();
                    Trace.WriteLine(String.Format("Add triggered app in queue: {0}, pos: {1},{2},{3},{4}",
                        attr.WindowName,
                        attr.Xpos,
                        attr.Ypos,
                        attr.Width,
                        attr.Height));
                    triggeredAppList.Add(attr, appModel);
                }
                
            });
        }

        public void AddTriggeredVNC(Client.Model.VncModel vncModel)
        {
            pendingVncList.Add(vncModel);

            ThreadPool.QueueUserWorkItem(s =>
            {
                Thread.Sleep(1500);
                
                // not a safe way if user trigger 2 similar application within 1.5 second
                if (pendingVncList.Remove(vncModel))
                {
                    ControlAttributes attr = mimicWndHolder.GetTopMostControl();
                    Trace.WriteLine(String.Format("Add triggered vnc in queue: {0}", attr.WindowName));
                    triggeredVncList.Add(mimicWndHolder.GetTopMostControl(), vncModel);
                }
            });
        }

        public void AddTriggeredVisionInput(InputAttributes visionInput)
        {
            pendingVisionList.Add(visionInput);

            ThreadPool.QueueUserWorkItem(s =>
            {
                Thread.Sleep(1500);
                pendingVisionList.Remove(visionInput);        // not a safe way if user trigger 2 similar application within 1.5 second
            });
        }

        public void AddWindow(int windowId)
        {
            try
            {
                if (pendingAppList.Count > 0)
                {
                    Trace.WriteLine(String.Format("Add window app with window Id: {0}",
                        windowId));

                    triggeredAppList.Add(new ControlAttributes() { Id = windowId }, pendingAppList.ElementAt(0));
                    pendingAppList.RemoveAt(0);
                    return;
                }

                if (pendingVncList.Count > 0)
                {
                    Trace.WriteLine(String.Format("Add triggered vnc with windows Id: {0}", windowId));
                    triggeredVncList.Add(new ControlAttributes() { Id = windowId }, pendingVncList.ElementAt(0));
                    pendingVncList.RemoveAt(0);
                    return;
                }

                if (pendingVisionList.Count > 0)
                {
                    triggeredInputList.Add(new ControlAttributes() { Id = windowId }, pendingVisionList.ElementAt(0));
                    pendingVisionList.RemoveAt(0);
                    return;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
        }

        public void RemoveWindow(int windowId)
        {
            // find and remove occurance of the window id
            try
            {
                var dicAppItem = triggeredAppList.Where(x => x.Key.Id == windowId);
                foreach(KeyValuePair<ControlAttributes, Client.Model.ApplicationModel> pair in dicAppItem)
                {
                    triggeredAppList.Remove(pair.Key);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var dicVncItem = triggeredVncList.Where(x => x.Key.Id == windowId);
                foreach (KeyValuePair<ControlAttributes, Client.Model.VncModel> pair in dicVncItem)
                {
                    triggeredVncList.Remove(pair.Key);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var dicVisionItem = triggeredInputList.Where(x => x.Key.Id == windowId);
                foreach (KeyValuePair<ControlAttributes, InputAttributes> pair in dicVisionItem)
                {
                    triggeredInputList.Remove(pair.Key);
                }
            }
            catch (Exception)
            {
            }
            
        }

        public void UpdateListContents()
        {
            // get all position form mimic holder
            Dictionary<ControlAttributes, Client.Model.ApplicationModel> tempAppDic = new Dictionary<ControlAttributes, ApplicationModel>();
            for (int count = 0; count < triggeredAppList.Count; count++ )
            {
                KeyValuePair<ControlAttributes, Client.Model.ApplicationModel> content = triggeredAppList.ElementAt(count);
                ControlAttributes latestAttr = mimicWndHolder.GetControl(content.Key.Id);
                tempAppDic.Add(latestAttr, content.Value);

                Trace.WriteLine(String.Format("Application: {0}, pos: {1},{2}, width:{3}, height:{4}", latestAttr.WindowName, latestAttr.Xpos, latestAttr.Ypos, latestAttr.Width, latestAttr.Height));
            }
            triggeredAppList = tempAppDic;


            Dictionary<ControlAttributes, Client.Model.VncModel> tempVncList = new Dictionary<ControlAttributes, VncModel>();
            for (int count = 0; count < triggeredVncList.Count; count++)
            {
                KeyValuePair<ControlAttributes, Client.Model.VncModel> content = triggeredVncList.ElementAt(count);
                ControlAttributes latestAttr = mimicWndHolder.GetControl(content.Key.Id);
                tempVncList.Add(latestAttr, content.Value);

                Trace.WriteLine(String.Format("VNC: {0}, pos: {1},{2}, width:{3}, height:{4}", latestAttr.WindowName, latestAttr.Xpos, latestAttr.Ypos, latestAttr.Width, latestAttr.Height));
            }
            triggeredVncList = tempVncList;


            Dictionary<ControlAttributes, InputAttributes> tempInputList = new Dictionary<ControlAttributes, InputAttributes>();
            for (int count = 0; count < triggeredInputList.Count; count++)
            {
                KeyValuePair<ControlAttributes, InputAttributes> content = triggeredInputList.ElementAt(count);
                ControlAttributes latestAttr = mimicWndHolder.GetControl(content.Key.Id);
                tempInputList.Add(latestAttr, content.Value);

                Trace.WriteLine(String.Format("Input source: {0}, pos: {1},{2}, width:{3}, height:{4}", latestAttr.WindowName, latestAttr.Xpos, latestAttr.Ypos, latestAttr.Width, latestAttr.Height));
            }
            triggeredInputList = tempInputList;
        }

        public void Reset()
        {
            pendingAppList.Clear();
            pendingVisionList.Clear();
            pendingVncList.Clear();

            triggeredAppList.Clear();
            triggeredVncList.Clear();
            triggeredInputList.Clear();
        }
    }
}
