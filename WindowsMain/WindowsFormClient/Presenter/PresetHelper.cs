using CustomWinForm;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Client.Model;

namespace WindowsFormClient.Presenter
{
    class PresetHelper
    {
        private CustomControlHolder mimicWndHolder;

        private Dictionary<Client.Model.ApplicationModel, CustomWinForm.CustomControlHolder.ControlAttributes> triggeredAppList = new Dictionary<ApplicationModel, CustomWinForm.CustomControlHolder.ControlAttributes>();
        private Dictionary<Client.Model.VncModel, CustomWinForm.CustomControlHolder.ControlAttributes> triggeredVncList = new Dictionary<VncModel, CustomWinForm.CustomControlHolder.ControlAttributes>();
        private Dictionary<InputAttributes, CustomWinForm.CustomControlHolder.ControlAttributes> triggeredInputList = new Dictionary<InputAttributes, CustomWinForm.CustomControlHolder.ControlAttributes>();

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
        }

        public void AddTriggeredVNC(Client.Model.VncModel vncModel)
        {
            pendingVncList.Add(vncModel);
        }

        public void AddTriggeredVisionInput(InputAttributes visionInput)
        {
            pendingVisionList.Add(visionInput);
        }

        public void AddWindow(int windowId)
        {
            if (pendingAppList.Count > 0)
            {
                triggeredAppList.Add(pendingAppList.ElementAt(0), new CustomWinForm.CustomControlHolder.ControlAttributes() { Id = windowId });
                pendingAppList.RemoveAt(0);
                return;
            }

            if (pendingVncList.Count > 0)
            {
                triggeredVncList.Add(pendingVncList.ElementAt(0), new CustomWinForm.CustomControlHolder.ControlAttributes() { Id = windowId });
                pendingVncList.RemoveAt(0);
                return;
            }

            if (pendingVisionList.Count > 0)
            {
                triggeredInputList.Add(pendingVisionList.ElementAt(0), new CustomWinForm.CustomControlHolder.ControlAttributes() { Id = windowId });
                pendingVisionList.RemoveAt(0);
                return;
            }
        }

        public void RemoveWindow(int windowId)
        {
            // TODO: find and remove first occurance of the window id
            var controlAttr = triggeredAppList.Values.First(x => x.Id == windowId);
            
        }

        public void UpdateListContents()
        {
            // TODO: get all position form mimic holder
            
        }
    }
}
