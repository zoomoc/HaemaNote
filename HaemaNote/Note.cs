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
    //값이 변경될 경우 저장해달라고 요청할 책임을 진다.

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
                _noteData = value;
                OnChange();
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
                _noteData.text = value;
                OnChange();
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
                _noteData.isStickyNote = value;
                OnChange();
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
                _noteData.stickyNotePos = value;
                OnChange();
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
                _noteData.stickyNoteSize = value;
                OnChange();
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
                _noteData.lastModifiedDateTime = value;
                //OnChanged가 호출될때 항상 수정한 일시가 업데이트되니 여기서 또 호출하면 무한루프걸림
            }
        }

        public delegate void ChangeEventHandler();
        public event ChangeEventHandler RequestSave;
        
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

            RequestSave += changeEventHandler;
            this.id = id;

            _isInit = true;
        }
        public Note(NoteData noteData, ChangeEventHandler changeEventHandler) : this()
        {
            _isInit = false;

            RequestSave += changeEventHandler;
            this.NoteData = noteData;

            _isInit = true;
        }
        private void OnChange()
        {
            if (_isInit == false)
            {
                return;
            }

            LastModifiedDateTime = DateTime.Now;
            RequestSave();
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
    }
}
