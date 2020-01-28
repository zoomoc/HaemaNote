using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Independentsoft.Webdav;
using System.Runtime.InteropServices;
using HaemaNote.Properties;

namespace HaemaNote
{
    public partial class HaemaNote : Form
    {
        private List<Note> noteList;
        private List<StickyNoteForm> stickyNoteFormList;

        NoteListForm noteListForm;

        Settings settings;

        public HaemaNote()
        {
            settings = Settings.Default;
            settings.Reset(); //설정 초기화하기

            ClientSize = new Size(120, 0);
            Icon = Properties.Resources.HaemaNote;
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Location = new Point(-2147483648, -2147483648);
            Name = "HaemaNote";
            Text = "HaemaNote";

            noteList = new List<Note>();
            stickyNoteFormList = new List<StickyNoteForm>();

            noteListForm = new NoteListForm();

            noteListForm.connect += ConnectWebDav;
            noteListForm.showStickyNote += ShowNote;
            noteListForm.VisibleChanged += MainForm_VisibleChanged;
            noteListForm.RequestNotes += GetNotes;

            Shown += HaemaNote_Shown;

            try
            {
                NoteList_Load();
            }
            catch (Exception e)
            {
                MessageBox.Show("노트 로드 중 오류가 발생했습니다. 저장된 노트를 초기화합니다.\n오류 메시지: " + e.Message);
                NoteList_Reset();
            }

            if(IsVisibleNoteExist() == false)
            {
                noteListForm.Show(this);
            }
        }

        private bool IsVisibleNoteExist()
        {
            foreach (Note note in noteList)
            {
                if (note.IsStickyNote == true)
                {
                    return true;
                }
            }
            return false;
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (noteListForm.Visible == false)
            {
                if (stickyNoteFormList.Count() == 0)
                {
                    Application.Exit();
                }
            }
        }

        private void HaemaNote_Shown(object sender, EventArgs e)
        {
            //메인창 숨기기
            Opacity = 0;
            Location = new Point(-2147483648, -2147483648);
            Size = new Size(0, 0);
        }

        private void NoteList_Save()
        {
            switch(settings.noteManageType)
            {
                case "Text":
                    SaveByText();
                    break;
                case "File":
                    SaveByFile();
                    break;
                case "WebDAV":
                    SaveByWebDAV();
                    break;
            }
        }
        private void NoteList_Load()
        {
            if (Properties.Settings.Default.noteManageType == "Text")
            {
                FileStream noteDataStream = new FileStream("data.dat", FileMode.OpenOrCreate);
                if (noteDataStream.Length != 0)
                {
                    BinaryFormatter serializer = new BinaryFormatter();

                    List<NoteData> noteDataList = (List<NoteData>)serializer.Deserialize(noteDataStream);
                    noteDataStream.Close();

                    noteList = new List<Note>();

                    foreach (NoteData noteData in noteDataList)
                    {
                        noteList.Add(new Note(noteData, NoteList_Save));
                        if(noteList.Last().IsStickyNote == true)
                        {
                            ShowNote(noteList.Last());
                        }
                    }
                }
                else
                {
                    noteDataStream.Close();
                    NoteList_Reset();
                }
            }
            if (Properties.Settings.Default.noteManageType == "File")
            {
                //나중에 구현
                throw new Exception("파일 타입 관리는 아직 구현되어 있지 않습니다");
            }
        }
        private void NoteList_Reset()
        {
            noteList = new List<Note>();
            noteList.Add(new Note(0, NoteList_Save));
            ShowNote(noteList.Last());

            NoteList_Save();
        }
        
        private void SaveByText()
        {
            try
            {
                List<NoteData> noteDataList = new List<NoteData>();
                foreach(Note note in noteList)
                {
                    noteDataList.Add(note.NoteData);
                }

                BinaryFormatter serializer = new BinaryFormatter();
                FileStream noteDataStream = new FileStream("data.dat", FileMode.OpenOrCreate);
                serializer.Serialize(noteDataStream, noteDataList);
                noteDataStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("노트 저장에 실패했습니다!\n에러 메시지: " + e.Message);
            }
        }
        private void SaveByFile()
        {
            string savePath = "";
            foreach (Note note in noteList)
            {
                savePath = settings.LocalSaveAddress + "\\";
                savePath += note.LastModifiedDateTime.ToString("yyyy.MM.dd");
                savePath += " " + note.id.ToString() + ".txt";
                File.WriteAllText(savePath.ToString(), note.Text, Encoding.UTF8);
            }
        }
        
        private void SaveByWebDAV()
        {

        }


        private Note AddNote()
        {
            uint newNoteId = 0;
            foreach (Note note in noteList)
            {
                if(note.id >= newNoteId)
                {
                    newNoteId = note.id+1;
                }
            }

            return AddNote(newNoteId);
        }
        private Note AddNote(uint noteID)
        {
            Note n = new Note(noteID, NoteList_Save);
            return AddNote(n);
        }
        private Note AddNote(Note note)
        {
            noteList.Add(note);
            noteListForm.RefreshNotes();
            return note;
        }
        private void AddStickyNote()
        {
            ShowNote(AddNote());
        }
        private Note FindNote(uint noteID)
        {
            foreach(Note note in noteList)
            {
                if(note.id == noteID)
                {
                    return note;
                }
            }
            return null;
        }
        private void ShowNote(Note note)
        {
            foreach (StickyNoteForm stickyNoteForm in stickyNoteFormList)
            {
                if (stickyNoteForm.note == note)
                {
                    stickyNoteForm.Visible = true;
                    note.IsStickyNote = true;
                    return;
                }
            }

            StickyNoteForm newStickyNoteForm = new StickyNoteForm(note);
            stickyNoteFormList.Add(newStickyNoteForm);

            newStickyNoteForm.sendAddNoteEvent += AddStickyNote;
            newStickyNoteForm.FormClosed += StickyNoteClosed;
            newStickyNoteForm.showMainForm += showMainForm;
            
            newStickyNoteForm.Show(this);
        }

        private void DeleteNote(StickyNoteForm sender)
        {
            noteList.Remove(sender.note);
            stickyNoteFormList.Remove(sender);
            NoteList_Save();
            noteListForm.RefreshNotes();
        }
        private void StickyNoteClosed(object sender, FormClosedEventArgs e)
        {
            stickyNoteFormList.Remove((StickyNoteForm)sender);

            if(stickyNoteFormList.Count == 0 & noteListForm.Visible == false)
            {
                Application.Exit();
            }
        }
        private void showMainForm()
        {
            if(noteListForm.Visible == true)
            {
                noteListForm.Focus();
            }
            if(noteListForm.Visible == false)
            {
                noteListForm.Show(this);
            }
        }
        private void ConnectWebDav()
        {
            NetworkCredential credential = new NetworkCredential("tmddlf372", "suprem@73");
            WebdavSession session = new WebdavSession(credential);
            Resource resource = new Resource(session);

            string[] list = resource.List("http://tmddlf37.iptime.org:5005/");

            string msg = "";
            foreach(string res in list) {
                msg += res + "\n";
            }
            MessageBox.Show(msg);
        }
        private void ShowAllNote()
        {
            foreach (Note note in noteList)
            {
                ShowNote(note);
            }
        }
        public List<Note> GetNotes()
        {
            return noteList;
        }
    }
}
