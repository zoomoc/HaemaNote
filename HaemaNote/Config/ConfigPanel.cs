using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HaemaNote
{
    public partial class ConfigPanel : UserControl
    {
        Config config;
        public ConfigPanel(Config cfg)
        {
            InitializeComponent();

            config = cfg;

            switch (config.noteManageType)
            {
                case Config.NoteManageType.Text:
                    radio_text.Checked = true;
                    break;
                case Config.NoteManageType.File:
                    radio_file.Checked = true;
                    break;
            }
        }
    }
}
