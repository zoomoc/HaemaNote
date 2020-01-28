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
    public partial class NoteItemPanel : UserControl
    {

        public delegate void ShowNoteEventHandler(Note note);
        public event ShowNoteEventHandler ShowNote;

        private Note _note;

        private NoteItemPanel()
        {
            InitializeComponent();
        }
            
        public NoteItemPanel(Note note, ShowNoteEventHandler showNoteEventHandler) : this()
        {
            _note = note;
            _note.DataChanged += _note_DataChanged;
            noteTextLabel.Text = _note.Text;

            foreach (Control control in this.Controls)
            {
                control.DoubleClick += NoteItemPanel_DoubleClick;
            }

            ShowNote += showNoteEventHandler;
        }

        private void _note_DataChanged()
        {
            noteTextLabel.Text = _note.Text;
        }

        private void NoteItemPanel_DoubleClick(object sender, EventArgs e)
        {
            ShowNote(_note);
        }
    }
}
