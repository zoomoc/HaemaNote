using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HaemaNote.SettingPanels;

namespace HaemaNote
{
    public partial class MainForm : Form
    {
        public delegate void ConnectEventHandler();
        public event ConnectEventHandler connect;

        public delegate void ShowStickyNoteEventHandler(Note n);
        public event ShowStickyNoteEventHandler showStickyNote;

        public delegate List<Note> RequestNotesEventHandler();
        public event RequestNotesEventHandler RequestNotes;

        private List<Note> notes;
        private List<NoteItem> noteItems;

        private NoteManageTypePanel configPanel;

        public MainForm()
        {
            InitializeComponent();

            

            notes = new List<Note>();
            configPanel = new NoteManageTypePanel();
            configPanel.Location = new Point(0, 60);

            Controls.Add(configPanel);
            configPanel.SendToBack();

            Shown += MainForm_Shown;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            RefreshNotes();
        }

        public void RefreshNotes()
        {
            notes = RequestNotes();
            
            noteItems = new List<NoteItem>();
            noteItemContainer.Controls.Clear();
            foreach (Note note in notes)
            {
                NoteItem newNoteItem = new NoteItem(note);
                newNoteItem.showStickyNote += NewNoteItem_showStickyNote;
                noteItems.Add(newNoteItem);

                noteItemContainer.Controls.Add(newNoteItem);
            }
        }

        private void NewNoteItem_showStickyNote(Note n)
        {
            showStickyNote(n);
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

        private void button2_Click(object sender, EventArgs e)
        {
            listPanel.Visible = false;
        }
        private void Button3_Click(object sender, System.EventArgs e)
        {
            listPanel.Visible = true;
        }

    }

    public class NoteItem : Panel
    {
        public delegate void ShowStickyNoteEventHandler(Note n);
        public event ShowStickyNoteEventHandler showStickyNote;

        Note note;
        Label text;
        public NoteItem(Note n)
        {
            note = n;
            Size = new Size(300, 100);
            BackColor = Color.FromArgb(253, 253, 201);

            text = new Label();
            text.Text = note.text;
            text.Location = new Point(0, 0);
            text.Size = this.Size;
            text.BackColor = Color.FromArgb(253, 253, 201);

            text.MouseDoubleClick += Text_MouseDoubleClick;

            this.Controls.Add(text);

            //MessageBox.Show("NoteItem 생성자 실행!\n텍스트 받음? : " + text.Text);
        }

        private void Text_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showStickyNote(note);
        }
    }
}
