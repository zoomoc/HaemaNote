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

        public delegate List<Note> RequestNotesEventHandler();
        public event RequestNotesEventHandler RequestNotes;

        private List<Note> notes;
        private List<NoteItem> noteItems;
        public MainForm()
        {
            InitializeComponent();

            notes = new List<Note>();

            Shown += MainForm_Shown;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            notes = RequestNotes();

            noteItems = new List<NoteItem>();
            foreach (Note note in notes)
            {
                NoteItem newNoteItem = new NoteItem(note);
                noteItems.Add(newNoteItem);

                noteItemContainer.Controls.Add(newNoteItem);
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            listPanel.Visible = false;
        }
        private void Button3_Click(object sender, System.EventArgs e)
        {
            listPanel.Visible = true;
        }

    }

    public class NoteItem : Control
    {
        Note note;
        Label text;
        public NoteItem(Note n)
        {
            note = n;
            Size = new Size(80, 20);

            text = new Label();
            text.Text = note.NoteText;
            text.Location = new Point(0, 0);
            text.Size = new Size(80, 20);

            this.Controls.Add(text);

            MessageBox.Show("NoteItem 생성자 실행!\n텍스트 받음? : " + text.Text);
        }
    }
}
