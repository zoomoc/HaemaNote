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
        private List<Note> notes;
        private List<StickyNoteForm> stickyNoteForms;

        MainForm mainForm;

        Properties.Settings settings;

        public HaemaNote()
        {
            settings = Properties.Settings.Default;
            settings.Reset(); //설정 초기화하기

            ClientSize = new Size(120, 0);
            Icon = Properties.Resources.HaemaNote;
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Location = new Point(-2147483648, -2147483648);
            Name = "HaemaNote";
            Text = "HaemaNote";

            notes = new List<Note>();
            stickyNoteForms = new List<StickyNoteForm>();

            mainForm = new MainForm();

            mainForm.connect += ConnectWebDav;
            mainForm.showStickyNote += ShowNote;
            mainForm.VisibleChanged += MainForm_VisibleChanged;
            mainForm.RequestNotes += GetNotes;

            Shown += HaemaNote_Shown;

            LoadNotes();
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (mainForm.Visible == false)
            {
                if (stickyNoteForms.Count() == 0)
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

        private void SaveNotes()
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
        private void SaveByText()
        {
            try
            {
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
        private void SaveByFile()
        {
            string savePath = "";
            foreach (Note note in notes)
            {
                savePath = settings.LocalSaveAddress + "\\";
                savePath += note.lastModifiedTime.ToString("yyyy.MM.dd");
                savePath += " " + note.id.ToString() + ".txt";
                File.WriteAllText(savePath.ToString(), note.text, Encoding.UTF8);
            }
        }
        
        private void SaveByWebDAV()
        {

        }
        private void LoadNotes()
        {
            if (Properties.Settings.Default.noteManageType == "Text")
            {
                FileStream notesData = new FileStream("data.dat", FileMode.OpenOrCreate);
                if (notesData.Length != 0)
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    this.notes = (List<Note>)serializer.Deserialize(notesData);
                    notesData.Close();
                    
                    if (notes.Count != 0)
                    {
                        foreach (Note note in this.notes)
                        {
                            if (note.isStickyNote == true)
                            {
                                ShowNote(note);
                            }
                        }
                        if (stickyNoteForms.Count() == 0)
                        {
                            mainForm.Show();
                        }
                    }
                    else
                    {
                        AddNote();
                        ShowNote(notes.Last());
                    }
                }
                else
                {
                    notesData.Close();

                    AddNote();
                    ShowNote(notes.Last());
                }
                return;
            }
            
            if (Properties.Settings.Default.noteManageType == "File")
            {
                //나중에 구현
                throw new Exception("파일 타입 관리는 아직 구현되어 있지 않습니다");
                return;
            }

            //return하지 않을 경우 예외 발생
            throw new Exception("노트 불러오기에 실패했습니다");
        }
        
        private Note AddNote()
        {
            uint newNoteId = 0;
            foreach (Note note in notes)
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
            Note n = new Note(noteID);
            return AddNote(n);
        }
        private Note AddNote(Note note)
        {
            notes.Add(note);
            mainForm.RefreshNotes();
            return note;
        }
        private void AddStickyNote()
        {
            ShowNote(AddNote());
        }
        private Note FindNote(uint noteID)
        {
            foreach(Note note in notes)
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
            foreach (StickyNoteForm stickyNoteForm in stickyNoteForms)
            {
                if (stickyNoteForm.note == note)
                {
                    stickyNoteForm.Visible = true;
                    return;
                }
            }

            StickyNoteForm newStickyNoteForm = new StickyNoteForm(note);
            stickyNoteForms.Add(newStickyNoteForm);

            newStickyNoteForm.sendSaveEvent += SaveNotes;
            newStickyNoteForm.sendAddNoteEvent += AddStickyNote;
            newStickyNoteForm.sendDeleteEvent += DeleteNote;
            newStickyNoteForm.FormClosed += StickyNoteClosed;
            newStickyNoteForm.showMainForm += showMainForm;
            
            newStickyNoteForm.Show(this);
        }

        private void DeleteNote(StickyNoteForm sender)
        {
            notes.Remove(sender.note);
            stickyNoteForms.Remove(sender);
            SaveNotes();
            mainForm.RefreshNotes();
        }
        private void StickyNoteClosed(object sender, FormClosedEventArgs e)
        {
            stickyNoteForms.Remove((StickyNoteForm)sender);

            if(stickyNoteForms.Count == 0 & mainForm.Visible == false)
            {
                Application.Exit();
            }
        }
        private void showMainForm()
        {
            if(mainForm.Visible == true)
            {
                mainForm.Focus();
            }
            if(mainForm.Visible == false)
            {
                mainForm.Show(this);
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
            foreach (Note note in notes)
            {
                ShowNote(note);
            }
        }
        public List<Note> GetNotes()
        {
            return notes;
        }
    }
}
