using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HaemaNote
{
    public partial class MainForm : Form
    {
        public delegate void ConnectEventHandler();
        public event ConnectEventHandler connect;

        public delegate void ShowNoteEventHandler();
        public event ShowNoteEventHandler showNote;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void showNoteBtn_Click(object sender, EventArgs e)
        {
            showNote();
        }
    }
}
