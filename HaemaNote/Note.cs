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
        public readonly uint id;
        public bool isStickyNote = false;
        public string NoteText = "";
        public Point StickyNotePos;
        
        public Note(uint id)
        {
            this.id = id;
        }
        public Note(uint id, string noteText)
        {
            this.NoteText = noteText;
        }
    }
}
