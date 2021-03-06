﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient
{
    public partial class FormMessageBox : Form
    {
        private int locationX;
        private int locationY;
        private int width;
        private int height;
        private int duration;

        public Color SelectedColor { get; set; }
        public Color BackgroundColor { get; set; }
        public Font SelectedFont { get; set; }

        public int LocationX { get { return locationX; } }
        public int LocationY { get { return locationY; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Duration { get { return duration; } }

        public bool AnimationEnabled { get; set; }
        public string Message 
        {
            get
            {
                return textBoxMessage.Text;
            }
        }

        private FontDialog fontDialog;
        private ColorDialog bgndColorDialog;

        public FormMessageBox()
        {
            InitializeComponent();
        }

        private void FormMessageBox_Load(object sender, EventArgs e)
        {
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            SelectedFont = textBoxMessage.Font;
            SelectedColor = textBoxMessage.ForeColor;

            BackgroundColor = textBoxMessage.BackColor;

            fontDialog = new FontDialog();
            fontDialog.ShowColor = true;
            fontDialog.ShowEffects = true;
            fontDialog.ShowApply = true;
            fontDialog.FontMustExist = true;
            fontDialog.Apply += fontDialog_Apply;

            bgndColorDialog = new ColorDialog();
            bgndColorDialog.AnyColor = true;

            radioButtonInfinite.Checked = true;
            numericUpDownDuration.Enabled = false;
            radioButtonInfinite.CheckedChanged += radioButtonInfinite_CheckedChanged;
        }

        void radioButtonInfinite_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            numericUpDownDuration.Enabled = !radioBtn.Checked;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // validate all data
            if(textBoxLocationX.Text.Length == 0 ||
                int.TryParse(textBoxLocationX.Text, out locationX) == false)
            {
                MessageBox.Show("Invalid Data");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (textBoxLocationY.Text.Length == 0 ||
                int.TryParse(textBoxLocationY.Text, out locationY) == false)
            {
                MessageBox.Show("Invalid Data");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if(radioButtonInfinite.Checked)
            {
                duration = -1;
            }
            else
            {
                duration = Convert.ToInt32(numericUpDownDuration.Value);
            }
            
            AnimationEnabled = checkBoxAnimation.Checked;
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            // show a font dialog
            if(fontDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // get the font dialog setting
                SelectedColor = fontDialog.Color;
                SelectedFont = fontDialog.Font;

                textBoxMessage.Font = SelectedFont;
                textBoxMessage.ForeColor = SelectedColor;
            }
        }

        void fontDialog_Apply(object sender, EventArgs e)
        {
            FontDialog fontDialog = (FontDialog)sender;
            textBoxMessage.Font = fontDialog.Font;
            textBoxMessage.ForeColor = fontDialog.Color;
        }

        private void buttonBgnd_Click(object sender, EventArgs e)
        {
            if (bgndColorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                BackgroundColor = bgndColorDialog.Color;
                textBoxMessage.BackColor = BackgroundColor;
            }
        }

    }
}
