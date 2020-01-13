using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HaemaNote
{
    class StickyNoteForm : Form
    {
        int titleHeight = 30;

        //상단바 구현에 필요한 멤버
        private Panel mover;
        private bool isMouseDown = false;
        private Point mouseDownPoint;

        //컨트롤들
        private TextBox textBox;
        private Button closeBtn;
        private Button addBtn;
        private ContextMenu contextMenu;
        private MenuItem showMainFormItem;

        //노트추가, 저장, 삭제 이벤트 핸들러
        public delegate void AddNoteEventHandler();
        public event AddNoteEventHandler sendAddNoteEvent;

        public delegate void SaveEventHandler();
        public event SaveEventHandler sendSaveEvent;

        public delegate void DeleteEventHandler(StickyNoteForm sender);
        public event DeleteEventHandler sendDeleteEvent;

        public delegate void ShowMainFormEventHandler();
        public event ShowMainFormEventHandler showMainForm;

        //노트 데이터
        public Note note;

        //초기화하는 도중 텍스트박스 값을 변경하면 이벤트 처리 함수가 동작하는 문제 해결을 위해
        //isInit 값이 false일때는 save, delete 등이 동작하지 않도록 함
        private bool isInit = false;

        private StickyNoteForm()
        {
            //창 일반 설정
            ClientSize = new System.Drawing.Size(224, 181);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = Color.FromArgb(253, 253, 201);
            ShowInTaskbar = false;

            //마우스 우클릭 메뉴
            contextMenu = new ContextMenu();

            showMainFormItem = new MenuItem("메모 목록 보기");
            showMainFormItem.Click += ShowMainFormItem_Click;

            contextMenu.MenuItems.Add(showMainFormItem);

            //메모창 상단바(드래그해서 창 이동) 구현
            mover = new Panel
            {
                Location = new Point(0, 0),
                Width = Size.Width,
                Height = titleHeight,
                BackColor = Color.FromArgb(248, 247, 182)
            };
            mover.MouseDown += Mover_MouseDown;
            mover.MouseMove += Mover_MouseMove;
            mover.MouseUp += Mover_MouseUp;
            mover.MouseClick += Mover_MouseClick;
            Controls.Add(mover);

            //메모 영역 텍스트박스
            textBox = new TextBox
            {
                Location = new Point(0, titleHeight),
                Width = Size.Width,
                Height = Size.Height,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(253, 253, 201),
                Multiline = true,
                Font = new Font(new FontFamily("맑은 고딕"), 12.0f),
                
            };
            textBox.TextChanged += TextBox_TextChanged;
            textBox.ContextMenu = contextMenu;
            Controls.Add(textBox);

            //닫기 버튼
            closeBtn = new Button
            {
                Location = new Point(Size.Width - titleHeight, 0),
                Size = new Size(titleHeight, titleHeight),
                Text = "X",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(248, 247, 182)
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += CloseBtn_Click;
            Controls.Add(closeBtn);

            //메모 추가 버튼
            addBtn = new Button
            {
                Location = new Point(0, 0),
                Size = new Size(titleHeight, titleHeight),
                Text = "+",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(248, 247, 182)
            };
            addBtn.FlatAppearance.BorderSize = 0;
            addBtn.Click += AddBtn_Click;
            Controls.Add(addBtn);

            mover.SendToBack();

            isInit = true;

            Shown += StickyNoteForm_Shown;
            Load += StickyNoteForm_Load;
            MouseClick += StickyNoteForm_MouseClick;
        }
        public StickyNoteForm(Note n) : this()
        {
            isInit = false;

            note = n;
            textBox.Text = note.NoteText;

            isInit = true;
        }

        private void ShowMainFormItem_Click(object sender, EventArgs e)
        {
            showMainForm();
        }

        private void Mover_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location);
            }
        }

        private void StickyNoteForm_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location);
            }
        }

        private void StickyNoteForm_Load(object sender, EventArgs e)
        {
            if (note.StickyNotePos != null)
            {
                Location = note.StickyNotePos;
            }
            note.StickyNotePos = Location;
        }

        private void StickyNoteForm_Shown(object sender, EventArgs e)
        {
            note.isStickyNote = true;
        }


        private void Mover_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
                Save();
            }
        }
        private void Mover_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown == false) {
                return;
            }
            int x = Location.X + (e.Location.X - mouseDownPoint.X);
            int y = Location.Y + (e.Location.Y - mouseDownPoint.Y);
            Location = new Point(x, y);
        }
        private void Mover_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                mouseDownPoint = e.Location;
                isMouseDown = true;
            }
        }
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "이 노트를 삭제할까요?", 
                "노트 삭제 확인", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question
                );

            if (dr == DialogResult.Yes)
            {
                Delete();
            }
            note.isStickyNote = false;
            Close();
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Save();
        }
        public void Save()
        {
            if (isInit == false) return;
            note.NoteText = textBox.Text;
            note.StickyNotePos = Location;
            sendSaveEvent();
        }
        private void Delete()
        {
            if (isInit == false) return;
            sendDeleteEvent(this);
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (isInit == false) return;
            sendAddNoteEvent();
        }
    }
}
