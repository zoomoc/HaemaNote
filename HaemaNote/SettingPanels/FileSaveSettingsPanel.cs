using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HaemaNote.SettingPanels
{
    public partial class FileSaveSettingsPanel : UserControl
    {
        Properties.Settings settings;

        public FileSaveSettingsPanel()
        {
            InitializeComponent();

            settings = Properties.Settings.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox_SavePath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void textBox_SavePath_TextChanged(object sender, EventArgs e)
        {
            settings.LocalSaveAddress = textBox_SavePath.Text;
        }

        private void textBox_SavePath_Leave(object sender, EventArgs e)
        {
            CheckSavePath();
        }

        private void CheckSavePath()
        {
            if (settings.LocalSaveAddress == "")
            {
                return;
            }

            if (Directory.Exists(settings.LocalSaveAddress) == false)
            {
                MessageBox.Show("파일 저장 경로가 올바르지 않습니다. 올바른 경로를 입력하세요.");
                textBox_SavePath.Text = "";
            }
        }
    }
}
