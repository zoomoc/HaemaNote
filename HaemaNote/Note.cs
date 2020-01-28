using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaemaNote
{
    //Note : 메모를 추상화한 클래스
    //메모할 때 입력되는 텍스트나 마지막으로 수정한 날짜를 관리하고,
    //올바른 값이 입력되었는지도 확인해야 하고,
    //값이 변경될 경우 저장 및 새로고침 해달라고 요청할 책임을 진다.
    //저장요청에 대한 책임은 Note에 있으니 다른 객체에서 저장요청을 하지 않는다.

    public class Note
    {
        //생성자 호출 중에 OnChange가 실행되는 것을 방지하는 역할
        private readonly bool _isInit;
        private NoteData _noteData;

        public readonly uint id;
        public NoteData NoteData
        {
            get
            {
                return _noteData;
            }
            set
            {
                if (_noteData != value)
                {
                    _noteData = value;
                    OnChange();
                }
            }
        }
        public string Text
        {
            get
            {
                return _noteData.text;
            }
            set
            {
                if (_noteData.text != value)
                {
                    _noteData.text = value;
                    OnChange();
                }
            }
        }
        public bool IsStickyNote
        {
            get
            {
                return _noteData.isStickyNote;
            }
            set
            {
                if (_noteData.isStickyNote != value)
                {
                    _noteData.isStickyNote = value;
                    OnChange();
                }
            }
        }
        public Point StickyNotePos
        {
            get
            {
                return _noteData.stickyNotePos;
            }
            set
            {
                if (_noteData.stickyNotePos != value)
                {
                    _noteData.stickyNotePos = value;
                    OnChange(false);
                }
            }
        }
        public Size StickyNoteSize
        {
            get
            {
                return _noteData.stickyNoteSize;
            }
            set
            {
                if (_noteData.stickyNoteSize != value)
                {
                    _noteData.stickyNoteSize = value;
                    OnChange(false);
                }
            }
        }
        public DateTime LastModifiedDateTime
        {
            get
            {
                return _noteData.lastModifiedDateTime;
            }
            private set
            {
                if (_noteData.lastModifiedDateTime != value)
                {
                    _noteData.lastModifiedDateTime = value;
                    OnChange(false);
                }
            }
        }

        public delegate void ChangeEventHandler();
        public event ChangeEventHandler DataChanged;

        private Note()
        {
            _isInit = false;

            Text = "";
            IsStickyNote = false;
            LastModifiedDateTime = DateTime.Now;

            _isInit = true;
        }
        public Note(uint id, ChangeEventHandler changeEventHandler) : this()
        {
            _isInit = false;

            DataChanged += changeEventHandler;
            this.id = id;

            _isInit = true;
        }
        public Note(NoteData noteData, ChangeEventHandler changeEventHandler) : this()
        {
            _isInit = false;

            DataChanged += changeEventHandler;
            this.NoteData = noteData;

            _isInit = true;
        }
        private void OnChange(bool isChangeModifiedTime = true)
        {
            if (_isInit == false)
            {
                return;
            }
            if (isChangeModifiedTime == true)
            {
                LastModifiedDateTime = DateTime.Now;
            }
            DataChanged();
        }
        
    }

    [Serializable]
    public struct NoteData
    {
        public string text;
        public bool isStickyNote;
        public Point stickyNotePos;
        public Size stickyNoteSize;
        public DateTime lastModifiedDateTime;
        public static bool operator ==(NoteData A, NoteData B)
        {
            if(A.text == B.text &&
                A.isStickyNote == B.isStickyNote &&
                A.stickyNotePos == B.stickyNotePos &&
                A.stickyNoteSize == B.stickyNoteSize &&
                A.lastModifiedDateTime == B.lastModifiedDateTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(NoteData A, NoteData B)
        {
            if (A.text == B.text &&
                A.isStickyNote == B.isStickyNote &&
                A.stickyNotePos == B.stickyNotePos &&
                A.stickyNoteSize == B.stickyNoteSize &&
                A.lastModifiedDateTime == B.lastModifiedDateTime)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    
}
