using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HaemaNote
{
    public partial class HaemaNote : Form
    {
        //각종 설정값
        enum NoteManageType : int { Text = 0, File = 1 };
        NoteManageType noteManageType = NoteManageType.Text;

        List<Note> notes;
        List<StickyNoteForm> stickyNoteForms;

        public HaemaNote()
        {
            InitializeComponent();

            notes = new List<Note>();
            stickyNoteForms = new List<StickyNoteForm>();
            LoadNotes();

            Shown += HaemaNote_Shown;
        }
        private void HaemaNote_Shown(object sender, EventArgs e)
        {
            Opacity = 0;
            Location = new Point(-2147483648, -2147483648);
            Size = new Size(0, 0);
        }

        private void SaveNotes()
        {
            if(noteManageType == NoteManageType.Text)
            {
                try
                {
                    notes = new List<Note>();
                    foreach(StickyNoteForm stickyNoteForm in stickyNoteForms)
                    {
                        notes.Add(stickyNoteForm.note);
                    }
                    BinaryFormatter serializer = new BinaryFormatter();
                    FileStream notesData = new FileStream("data.dat", FileMode.OpenOrCreate);
                    serializer.Serialize(notesData, notes);
                    notesData.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("노트 저장에 실패했습니다!\n에러 메시지: " + e.Message);
                }
            }
        }
        private void LoadNotes()
        {
            if (noteManageType == NoteManageType.Text)
            {
                FileStream notesData = new FileStream("data.dat", FileMode.OpenOrCreate);
                if (notesData.Length != 0)
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    this.notes = (List<Note>)serializer.Deserialize(notesData);

                    foreach(Note note in this.notes)
                    {
                        AddNote(note);
                    }
                }
                else
                {
                    AddNote();
                }
                notesData.Close();
                return;
            }
            
            if (noteManageType == NoteManageType.File)
            {
                //나중에 구현
                throw new Exception("파일 타입 관리는 아직 구현되어 있지 않습니다");
            }
            throw new Exception("노트 불러오기에 실패했습니다");
        }
        private void AddNote()
        {
            stickyNoteForms.Add(new StickyNoteForm());
            stickyNoteForms.Last<StickyNoteForm>().sendSaveEvent += SaveNotes;
            stickyNoteForms.Last<StickyNoteForm>().sendAddNoteEvent += AddNote;
            stickyNoteForms.Last<StickyNoteForm>().sendDeleteEvent += DeleteNote;
            stickyNoteForms.Last<StickyNoteForm>().FormClosed += StickyNoteClosed;
            stickyNoteForms.Last<StickyNoteForm>().Show(this);
        }


        private void AddNote(Note note)
        {
            stickyNoteForms.Add(new StickyNoteForm(note));
            stickyNoteForms.Last<StickyNoteForm>().sendSaveEvent += SaveNotes;
            stickyNoteForms.Last<StickyNoteForm>().sendAddNoteEvent += AddNote;
            stickyNoteForms.Last<StickyNoteForm>().sendDeleteEvent += DeleteNote;
            stickyNoteForms.Last<StickyNoteForm>().FormClosed += StickyNoteClosed;
            stickyNoteForms.Last<StickyNoteForm>().Show(this);
        }
        private void DeleteNote(StickyNoteForm sender)
        {
            stickyNoteForms.Remove(sender);
            SaveNotes();
        }
        private void StickyNoteClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show(stickyNoteForms.Count().ToString());
            MessageBox.Show(stickyNoteForms.Last<StickyNoteForm>().note.NoteText);
        }
    }
}
