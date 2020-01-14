using System;
using System.Drawing;
using System.Windows.Forms;

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

        /*
        [Flags]
        public enum WindowStyle
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = -2147483648, //0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
            WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU |
                        WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
            WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),
            WS_CHILDWINDOW = (WS_CHILD)
        }

        private const int GWL_STYLE = -16;

        [DllImport("user32.dll")]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 Offset);
        [DllImport("user32.dll")]
        public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 Offset, Int32 newLong);
        */

        const int WM_NCHITTEST = 0x0084;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    if (m.Result == (IntPtr)HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    break;
            }
        }


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
            closeBtn.MouseHover += CloseBtn_MouseHover;
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

            //리사이즈 
            MouseHover += StickyNoteForm_MouseHover;


            mover.SendToBack();

            isInit = true;

            Shown += StickyNoteForm_Shown;
            Load += StickyNoteForm_Load;
            MouseClick += StickyNoteForm_MouseClick;
        }

        private void CloseBtn_MouseHover(object sender, EventArgs e)
        {
            //MessageBox.Show("Mousehover! btn");
        }

        private void StickyNoteForm_MouseHover(object sender, EventArgs e)
        {
            //MessageBox.Show("Mousehover!");
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
            /*
            int style = GetWindowLong(this.Handle, GWL_STYLE);
            WindowStyle myStyle = (WindowStyle)style;
            myStyle = myStyle & ~WindowStyle.WS_CAPTION;
            myStyle = myStyle | WindowStyle.WS_BORDER;

            style = SetWindowLong(this.Handle, GWL_STYLE, (int)myStyle);
            */
            //textBox.Visible = false;

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
