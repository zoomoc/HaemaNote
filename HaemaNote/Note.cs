using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaemaNote
{
    [Serializable]
    public class Note
    {
        public readonly uint id;

        public string title = "";
        public string text = "";

        public bool isStickyNote = false;
        public Point StickyNotePos;
        public Size StickyNoteSize;
        public DateTime lastModifiedTime;
        
        public Note(uint id)
        {
            this.id = id;
            lastModifiedTime = DateTime.Now;
        }
        public Note(uint id, string noteText)
        {
            this.id = id;
            this.text = noteText;
            lastModifiedTime = DateTime.Now;
        }

    }
}
