using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient
{
    public partial class FormPresets : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public delegate void OnPresetAdded(FormPresets form);
        public event OnPresetAdded EvtPresetAdded;

        public delegate void OnPresetRemoved(FormPresets form, Client.Model.PresetModel item);
        public event OnPresetRemoved EvtPresetRemoved;

        public FormPresets()
        {
            InitializeComponent();
        }

        private void buttonAddPreset_Click(object sender, EventArgs e)
        {
            if (EvtPresetAdded != null)
            {
                EvtPresetAdded.Invoke(this);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxPreset.SelectedItems)
            {
                if (EvtPresetRemoved != null)
                {
                    EvtPresetRemoved.BeginInvoke(this, (Client.Model.PresetModel)item, null, null);
                }
            }
        }

        private void FormPresets_Load(object sender, EventArgs e)
        {
            listBoxPreset.MouseDown += listBoxPreset_MouseDown;
        }

        void listBoxPreset_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxPreset.Items.Count == 0)
                return;

            int index = listBoxPreset.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                DragDropEffects dde1 = DoDragDrop(listBoxPreset.Items[index], DragDropEffects.All);
            }
        }

        public void SetPresetList(IList<Client.Model.PresetModel> presetList)
        {
            listBoxPreset.DataSource = new BindingSource(presetList, null);
            listBoxPreset.DisplayMember = "PresetName";
            listBoxPreset.ValueMember = "PresetId";
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EvtPresetAdded != null)
            {
                EvtPresetAdded.Invoke(this);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxPreset.SelectedItems)
            {
                if (EvtPresetRemoved != null)
                {
                    EvtPresetRemoved.BeginInvoke(this, (Client.Model.PresetModel)item, null, null);
                }
            }
        }
    }
}
