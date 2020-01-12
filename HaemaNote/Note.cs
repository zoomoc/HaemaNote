using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaemaNote
{
    [Serializable]
    class Note
    {
        public string NoteText;
        public Point StickyNotePos;
        public Note()
        {
            NoteText = "";
        }
        public Note(string noteText) : this()
        {
            NoteText = noteText;
        }
    }
}
