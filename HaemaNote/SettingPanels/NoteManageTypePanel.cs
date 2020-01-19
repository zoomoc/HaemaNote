using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HaemaNote.SettingPanels;

namespace HaemaNote.SettingPanels
{
    public partial class NoteManageTypePanel : UserControl
    {
        private bool isInit = false;

        FileSaveSettingsPanel fileSaveSettingsPanel;
        public NoteManageTypePanel()
        {
            InitializeComponent();

            fileSaveSettingsPanel = new FileSaveSettingsPanel();



            switch (Properties.Settings.Default.noteManageType)
            {
                case "Text":
                    radio_text.Checked = true;
                    break;
                case "File":
                    radio_file.Checked = true;
                    break;
            }

            isInit = true;
        }

        private void radio_text_CheckedChanged(object sender, EventArgs e)
        {
            if (isInit == false) return;
            if(radio_text.Checked)
            {
                Properties.Settings.Default.noteManageType = "Text";
                Properties.Settings.Default.Save();
            }
        }

        private void radio_file_CheckedChanged(object sender, EventArgs e)
        {
            if (isInit == false) return;
            if (radio_file.Checked)
            {
                Properties.Settings.Default.noteManageType = "File";
                fileSaveSettingsPanel.Location = new Point(Left, 200);
                Controls.Add(fileSaveSettingsPanel);

                Properties.Settings.Default.Save();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
