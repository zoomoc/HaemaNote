using System;
using System.Drawing;
using System.Windows.Forms;

namespace HaemaNote
{
    public class StickyNoteForm : ShadowForm
    {
        private int titleHeight = 30; //상단바 두께
        private int borderThickness = 5; //가장자리 두께

        //상단바 구현에 필요한 멤버
        private Panel mover;
        private bool isMoverActive = false;

        //창크기 리사이즈 구현에 필요한 멤버
        private enum ResizingWidth : int { None = 0, Left = 1, Right = 2 }
        private ResizingWidth isResizingWidth;
        private enum ResizingHeight : int { None = 0, Top = 1, Bottom = 2 }
        private ResizingHeight isResizingHeight;

        private Point mouseDownPoint;
        private int mouseDownWidth;
        private int mouseDownHeight;

        //컨트롤들
        private TextBox textBox;
        private PictureBox closeButton;
        private PictureBox addButton;
        private ContextMenu contextMenu;
        private MenuItem item_ShowMainForm;
        private Label lastModifiedTimeLabel;

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
        private PictureBox pictureBox1;

        //초기화하는 도중 텍스트박스 값을 변경하면 이벤트 처리 함수가 동작하는 문제 해결을 위해
        //isInit 값이 false일때는 save, delete 등이 동작하지 않도록 함
        private bool isInit = false;

        #region 테두리 None일때 리사이즈 구현하는 코드
        protected override void WndProc(ref Message m)
        {
            const int RESIZE_HANDLE_SIZE = 10;

            switch (m.Msg)
            {
                case 0x0084/*NCHITTEST*/ :
                    base.WndProc(ref m);

                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)12/*HTTOP*/ ;
                            else
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                        }
                        else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)10/*HTLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                            else
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }
                        else
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // <--- use 0x20000
                return cp;
            }
        }
        #endregion

        private StickyNoteForm()
        {

            //창 일반 설정
            ClientSize = new System.Drawing.Size(300, 200);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = Color.FromArgb(253, 253, 201);
            ShowInTaskbar = false;

            //마우스 우클릭 메뉴
            contextMenu = new ContextMenu();

            item_ShowMainForm = new MenuItem("메모 목록 보기");
            item_ShowMainForm.Click += Item_ShowMainForm_Click;

            contextMenu.MenuItems.Add(item_ShowMainForm);

            //메모창 상단바(드래그해서 창 이동) 구현
            mover = new Panel
            {
                Location = new Point(0, 0),
                Width = Size.Width,
                Height = titleHeight,
                Anchor = (AnchorStyles.Top|AnchorStyles.Left | AnchorStyles.Right),
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
                Location = new Point(0 + borderThickness, titleHeight + borderThickness),
                Width = Size.Width - (borderThickness * 2),
                Height = Size.Height - titleHeight - (borderThickness * 2),
                Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(253, 253, 201),
                Multiline = true,
                Font = new Font(new FontFamily("맑은 고딕"), 12.0f)
            };
            textBox.TextChanged += TextBox_TextChanged;
            textBox.ContextMenu = contextMenu;
            Controls.Add(textBox);

            //닫기 버튼
            closeButton = new PictureBox
            {
                Image = Properties.Resources.closeButtonImage,
                Location = new Point(Size.Width - titleHeight, 0),
                Size = new Size(titleHeight, titleHeight),
                Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                BackColor = Color.Transparent
            };
            closeButton.Click += CloseButton_Click;
            closeButton.MouseEnter += CloseButton_MouseEnter;
            closeButton.MouseDown += CloseButton_MouseDown;
            closeButton.MouseUp += CloseButton_MouseUp;
            closeButton.MouseLeave += CloseButton_MouseLeave;
            mover.Controls.Add(closeButton);

            addButton = new PictureBox
            {
                Image = Properties.Resources.addButtonImage,
                Location = new Point(0, 0),
                Size = new Size(titleHeight, titleHeight),
                Anchor = (AnchorStyles.Top | AnchorStyles.Left),
                BackColor = Color.Transparent
            };
            addButton.Click += AddButton_Click;
            addButton.MouseEnter += AddButton_MouseEnter;
            addButton.MouseDown += AddButton_MouseDown;
            addButton.MouseUp += AddButton_MouseUp;
            addButton.MouseLeave += AddButton_MouseLeave;
            mover.Controls.Add(addButton);

            lastModifiedTimeLabel = new Label
            {
                Size = new Size(ClientSize.Width, 10),
                Dock = DockStyle.Bottom,
                //Text = note.lastModifiedTime.ToString()
            };
            Controls.Add(lastModifiedTimeLabel);

            //메모 추가 버튼
            /*
            addBtn = new Button
            {
                Location = new Point(0, 0),
                Size = new Size(titleHeight, titleHeight),
                Anchor = (AnchorStyles.Top | AnchorStyles.Left),
                Text = "+",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(248, 247, 182)
            };
            addBtn.FlatAppearance.BorderSize = 0;
            addBtn.Click += AddBtn_Click;
            Controls.Add(addBtn);
            */

            //리사이즈 
            MouseMove += StickyNoteForm_MouseMove;
            MouseDown += StickyNoteForm_MouseDown;
            MouseUp += StickyNoteForm_MouseUp;


            mover.SendToBack();

            isInit = true;

            Shown += StickyNoteForm_Shown;
            Load += StickyNoteForm_Load;
            MouseClick += StickyNoteForm_MouseClick;
        }



        private void AddButton_MouseUp(object sender, MouseEventArgs e)
        {
            addButton.BackColor = Color.FromArgb(30, 0, 0, 0);
        }
        private void AddButton_MouseDown(object sender, MouseEventArgs e)
        {
            addButton.BackColor = Color.FromArgb(70, 0, 0, 0);
        }
        private void AddButton_MouseLeave(object sender, EventArgs e)
        {
            addButton.BackColor = Color.Transparent;
        }
        private void AddButton_MouseEnter(object sender, EventArgs e)
        {
            addButton.BackColor = Color.FromArgb(30, 0, 0, 0);
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (isInit == false) return;
            sendAddNoteEvent();
        }

        private void CloseButton_MouseUp(object sender, MouseEventArgs e)
        {
            closeButton.BackColor = Color.FromArgb(30, 0, 0, 0);
        }
        private void CloseButton_MouseDown(object sender, MouseEventArgs e)
        {
            closeButton.BackColor = Color.FromArgb(70, 0, 0, 0);
        }
        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = Color.Transparent;
        }
        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.BackColor = Color.FromArgb(30, 0, 0, 0);
        }
        private void CloseButton_Click(object sender, EventArgs e)
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

        private void StickyNoteForm_MouseUp(object sender, MouseEventArgs e)
        {
            isResizingWidth = ResizingWidth.None;
            isResizingHeight = ResizingHeight.None;
        }

        private void StickyNoteForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < borderThickness)
            {
                isResizingWidth = ResizingWidth.Left;
            }
            else if(e.X > ClientSize.Width - borderThickness)
            {
                isResizingWidth = ResizingWidth.Right;
            }
            if (e.Y < borderThickness)
            {
                isResizingHeight = ResizingHeight.Top;
            }
            else if (e.Y > ClientSize.Height - borderThickness)
            {
                isResizingHeight = ResizingHeight.Bottom;
            }
            mouseDownPoint = PointToScreen(e.Location);
            mouseDownWidth = Width;
            mouseDownHeight = Height;
        }
        private void StickyNoteForm_MouseMove(object sender, MouseEventArgs e)
        {
            #region 마우스커서 바꾸는 코드
            //왼쪽위
            if (e.X < borderThickness &
                e.Y < borderThickness)
            {
                Cursor.Current = Cursors.SizeNWSE;
            }
            //왼쪽
            else if (e.X < borderThickness &
                (e.Y > borderThickness & e.Y < (ClientSize.Height - borderThickness)))
            {
                Cursor.Current = Cursors.SizeWE;
            }
            //왼쪽아래
            else if (e.X < borderThickness &
                e.Y > (ClientSize.Height - borderThickness))
            {
                Cursor.Current = Cursors.SizeNESW;
            }
            //아래
            else if ((e.X > borderThickness & e.X < (ClientSize.Width - borderThickness)) &
                e.Y > (ClientSize.Height - borderThickness))
            {
                Cursor.Current = Cursors.SizeNS;
            }
            //오른쪽아래
            else if (e.X > (ClientSize.Width - borderThickness) &
                e.Y > (ClientSize.Height - borderThickness))
            {
                Cursor.Current = Cursors.SizeNWSE;
            }
            //오른쪽
            else if (e.X > (ClientSize.Width - borderThickness) &
                (e.Y > borderThickness & e.Y < (ClientSize.Height - borderThickness)))
            {
                Cursor.Current = Cursors.SizeWE;
            }
            //오른쪽 위
            else if (e.X > (ClientSize.Width - borderThickness) &
                e.Y < borderThickness)
            {
                Cursor.Current = Cursors.SizeNESW;
            }
            //위
            else if ((e.X > borderThickness & e.X < (ClientSize.Width - borderThickness))
                & e.Y < borderThickness)
            {
                Cursor.Current = Cursors.SizeNS;
            }
            #endregion

            /*
            SetStyle(ControlStyles.ResizeRedraw, false);

            if(isResizingWidth == ResizingWidth.Left)
            {
                Left = PointToScreen(e.Location).X;
                Width = mouseDownWidth + mouseDownPoint.X - PointToScreen(e.Location).X;
            }
            else if(isResizingWidth == ResizingWidth.Right)
            {
                Width = mouseDownWidth + PointToScreen(e.Location).X - mouseDownPoint.X;
            }
            
            if(isResizingHeight == ResizingHeight.Top)
            {
                Top = PointToScreen(e.Location).Y;
                Height += mouseDownHeight+ mouseDownPoint.Y - PointToScreen(e.Location).Y;
            }
            else if(isResizingHeight == ResizingHeight.Bottom)
            {
                Height += mouseDownHeight - PointToScreen(e.Location).Y + mouseDownPoint.Y;
            }
            
            SetStyle(ControlStyles.ResizeRedraw, true);
            */
        }

        public StickyNoteForm(Note n) : this()
        {
            isInit = false;

            note = n;
            textBox.Text = note.text;
            lastModifiedTimeLabel.Text = note.lastModifiedTime.ToString();

            isInit = true;
        }

        private void Item_ShowMainForm_Click(object sender, EventArgs e)
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
                isMoverActive = false;
                Save();
            }
        }
        private void Mover_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoverActive == false) 
            {
                StickyNoteForm_MouseMove(sender, e);
                return;
            }
            int x = Location.X + (e.Location.X - mouseDownPoint.X);
            int y = Location.Y + (e.Location.Y - mouseDownPoint.Y);
            Location = new Point(x, y);
        }
        private void Mover_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < borderThickness)
            {
                StickyNoteForm_MouseDown(sender, e);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint = e.Location;
                isMoverActive = true;
            }
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Save();
        }
        public void Save()
        {
            if (isInit == false) return;
            note.text = textBox.Text;
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
