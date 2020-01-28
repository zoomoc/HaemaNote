using System;
using System.Drawing;
using System.Windows.Forms;

namespace HaemaNote
{
    //스티커노트 창 클래스

    //자기 창에 표시되는 내용을 항상 올바르게 유지할 책임을 진다
    //입력을 받거나 창 크기, 위치가 변경되면 note에 기록한다.
    //note의 값이 변경될 경우 메시지를 보내주므로 메시지 받을때마다 새로고침
    //직접 저장요청하거나 다른작업을 하지 않는다

    //노트 추가 요청, 리스트 창 열기 요청을 보낸다

    public class StickyNoteForm : ShadowForm
    {
        private int TITLE_HEIGHT = 30; //상단바 두께
        private int BORDER_THICKNESS = 7; //가장자리 두께

        //초기화하는 도중 텍스트박스 값을 변경하면 이벤트 처리 함수가 동작하는 문제 해결을 위해
        //isInit 값이 false일때는 save, delete 등이 동작하지 않도록 함
        private bool _isInit = false;

        //상단바 구현에 필요한 멤버
        private bool _isMoverActive = false;

        //창크기 리사이즈 구현에 필요한 멤버
        private enum ResizingWidth : int { None = 0, Left = 1, Right = 2 }
        private ResizingWidth isResizingWidth;
        private enum ResizingHeight : int { None = 0, Top = 1, Bottom = 2 }
        private ResizingHeight isResizingHeight;

        private Point mouseDownPoint;
        private int mouseDownWidth;
        private int mouseDownHeight;

        //컨트롤들
        private Panel mover;
        private TextBox textBox;
        private PictureBox closeButton;
        private PictureBox addButton;
        private ContextMenu contextMenu;
        private MenuItem item_ShowMainForm;
        private Label lastModifiedTimeLabel;

        //노트추가, 저장, 삭제 이벤트 핸들러
        public delegate void AddNoteEventHandler();
        public event AddNoteEventHandler sendAddNoteEvent;

        public delegate void ShowMainFormEventHandler();
        public event ShowMainFormEventHandler showMainForm;

        //노트
        private Note _note;
        public Note note
        {
            get
            {
                return _note;
            }
        }

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
                Height = TITLE_HEIGHT,
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
                Location = new Point(0 + BORDER_THICKNESS, TITLE_HEIGHT + BORDER_THICKNESS),
                Width = Size.Width - (BORDER_THICKNESS * 2),
                Height = Size.Height - TITLE_HEIGHT - (BORDER_THICKNESS * 2),
                Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(253, 253, 201),
                Multiline = true,
                Font = new Font(new FontFamily("맑은 고딕"), 12.0f),
                ScrollBars = ScrollBars.Vertical

            };
            textBox.TextChanged += TextBox_TextChanged;
            textBox.ContextMenu = contextMenu;
            Controls.Add(textBox);

            //닫기 버튼
            closeButton = new PictureBox
            {
                Image = Properties.Resources.closeButtonImage,
                Location = new Point(Size.Width - TITLE_HEIGHT, 0),
                Size = new Size(TITLE_HEIGHT, TITLE_HEIGHT),
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
                Size = new Size(TITLE_HEIGHT, TITLE_HEIGHT),
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
                Location = new Point(BORDER_THICKNESS, ClientSize.Height - BORDER_THICKNESS - 15),
                Size = new Size(ClientSize.Width - (BORDER_THICKNESS * 2), 15),
                Font = new Font("맑은 고딕", 9.0f),
                BackColor = Color.Transparent,
                Anchor = (AnchorStyles.Bottom | AnchorStyles.Left)
            };
            lastModifiedTimeLabel.MouseMove += LastModifiedTimeLabel_MouseMove;
            //lastModifiedTimeLabel.MouseUp += StickyNoteForm_MouseUp;
            //lastModifiedTimeLabel.MouseDown += StickyNoteForm_MouseDown;
            //lastModifiedTimeLabel.MouseMove += StickyNoteForm_MouseMove;
            Controls.Add(lastModifiedTimeLabel);
            lastModifiedTimeLabel.Hide();

            Load += StickyNoteForm_Load;
            Shown += StickyNoteForm_Shown;
            Activated += StickyNoteForm_Activated;
            Deactivate += StickyNoteForm_Deactivate;
            //ResizeEnd += StickyNoteForm_ResizeEnd;

            MouseUp += StickyNoteForm_MouseUp;
            MouseDown += StickyNoteForm_MouseDown;
            MouseMove += StickyNoteForm_MouseMove;
            MouseClick += StickyNoteForm_MouseClick;

            mover.SendToBack();
            

            _isInit = true;
        }

        private void StickyNoteForm_ResizeEnd(object sender, EventArgs e)
        {
            _note.StickyNoteSize = Size;
        }

        public StickyNoteForm(Note note) : this()
        {
            _isInit = false;

            _note = note;
            _note.DataChanged += LoadNoteData;
            /*
            if (_note.StickyNotePos != null)
            {
                Location = _note.StickyNotePos;
                Size = _note.StickyNoteSize;
            }
            else
            {
                _note.StickyNotePos = Location;
                _note.StickyNoteSize = Size;
            }

            textBox.Text = _note.Text;
            lastModifiedTimeLabel.Text = _note.LastModifiedDateTime.ToString();
            */
            _isInit = true;
        }

        private void LoadNoteData()
        {
            textBox.Text = _note.Text;
            lastModifiedTimeLabel.Text = _note.LastModifiedDateTime.ToString("yyyy. MM. dd.(ddd) HH:mm:ss");
        }

        private void StickyNoteForm_Load(object sender, EventArgs e)
        {
            if (_note.StickyNotePos != null)
            {
                Location = _note.StickyNotePos;
                Size = _note.StickyNoteSize;
            }
            else
            {
                _note.StickyNotePos = Location;
                _note.StickyNoteSize = Size;
            }

            textBox.Text = _note.Text;
            lastModifiedTimeLabel.Text = _note.LastModifiedDateTime.ToString();
        }
        private void StickyNoteForm_Shown(object sender, EventArgs e)
        {
            _note.IsStickyNote = true;
        }
        private void StickyNoteForm_Activated(object sender, EventArgs e)
        {
            if (_isInit == false) return;

            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.DeselectAll();
            addButton.Show();
            closeButton.Show();
            lastModifiedTimeLabel.Hide();
        }
        private void StickyNoteForm_Deactivate(object sender, EventArgs e)
        {
            if (_isInit == false) return;

            textBox.ScrollBars = ScrollBars.None;
            textBox.DeselectAll();
            addButton.Hide();
            closeButton.Hide();

            lastModifiedTimeLabel.Show();
            lastModifiedTimeLabel.BringToFront();
        }

        private void StickyNoteForm_MouseUp(object sender, MouseEventArgs e)
        {
            if(isResizingWidth != ResizingWidth.None || isResizingHeight != ResizingHeight.None)
            {
                _note.StickyNoteSize = Size;
                isResizingWidth = ResizingWidth.None;
                isResizingHeight = ResizingHeight.None;
            }
        }
        private void StickyNoteForm_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.X < BORDER_THICKNESS)
            {
                isResizingWidth = ResizingWidth.Left;
            }
            else if (e.X > ClientSize.Width - BORDER_THICKNESS)
            {
                isResizingWidth = ResizingWidth.Right;
            }
            if (e.Y < BORDER_THICKNESS)
            {
                isResizingHeight = ResizingHeight.Top;
            }
            else if (e.Y > ClientSize.Height - BORDER_THICKNESS)
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
            if (e.X < BORDER_THICKNESS &
                e.Y < BORDER_THICKNESS)
            {
                Cursor.Current = Cursors.SizeNWSE;
            }
            //왼쪽
            else if (e.X < BORDER_THICKNESS &
                (e.Y > BORDER_THICKNESS & e.Y < (ClientSize.Height - BORDER_THICKNESS)))
            {
                Cursor.Current = Cursors.SizeWE;
            }
            //왼쪽아래
            else if (e.X < BORDER_THICKNESS &
                e.Y > (ClientSize.Height - BORDER_THICKNESS))
            {
                Cursor.Current = Cursors.SizeNESW;
            }
            //아래
            else if ((e.X > BORDER_THICKNESS & e.X < (ClientSize.Width - BORDER_THICKNESS)) &
                e.Y > (ClientSize.Height - BORDER_THICKNESS))
            {
                Cursor.Current = Cursors.SizeNS;
            }
            //오른쪽아래
            else if (e.X > (ClientSize.Width - BORDER_THICKNESS) &
                e.Y > (ClientSize.Height - BORDER_THICKNESS))
            {
                Cursor.Current = Cursors.SizeNWSE;
            }
            //오른쪽
            else if (e.X > (ClientSize.Width - BORDER_THICKNESS) &
                (e.Y > BORDER_THICKNESS & e.Y < (ClientSize.Height - BORDER_THICKNESS)))
            {
                Cursor.Current = Cursors.SizeWE;
            }
            //오른쪽 위
            else if (e.X > (ClientSize.Width - BORDER_THICKNESS) &
                e.Y < BORDER_THICKNESS)
            {
                Cursor.Current = Cursors.SizeNESW;
            }
            //위
            else if ((e.X > BORDER_THICKNESS & e.X < (ClientSize.Width - BORDER_THICKNESS))
                & e.Y < BORDER_THICKNESS)
            {
                Cursor.Current = Cursors.SizeNS;
            }
            #endregion

            #region 창크기 조절하는 코드
            SetStyle(ControlStyles.ResizeRedraw, false);

            if (isResizingWidth == ResizingWidth.Left)
            {
                Left = PointToScreen(e.Location).X;
                Width = mouseDownWidth + mouseDownPoint.X - PointToScreen(e.Location).X;
            }
            else if (isResizingWidth == ResizingWidth.Right)
            {
                Width = mouseDownWidth + PointToScreen(e.Location).X - mouseDownPoint.X;
            }

            if (isResizingHeight == ResizingHeight.Top)
            {
                Top = PointToScreen(e.Location).Y;
                Height = mouseDownHeight + mouseDownPoint.Y - PointToScreen(e.Location).Y;
            }
            else if (isResizingHeight == ResizingHeight.Bottom)
            {
                Height = mouseDownHeight + PointToScreen(e.Location).Y - mouseDownPoint.Y;
            }

            SetStyle(ControlStyles.ResizeRedraw, true);
            #endregion
        }
        private void StickyNoteForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location);
            }
        }

        private void Mover_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Y < BORDER_THICKNESS)
            {
                StickyNoteForm_MouseUp(sender, e);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                _isMoverActive = false;
                _note.StickyNotePos = Location;
            }
        }
        private void Mover_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoverActive == false)
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
            if (e.Y < BORDER_THICKNESS)
            {
                StickyNoteForm_MouseDown(sender, e);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint = e.Location;
                _isMoverActive = true;
            }
        }
        private void Mover_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location);
            }
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
            if (_isInit == false) return;
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
            _note.IsStickyNote = false;
            Close();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (_isInit == false) return;
            _note.Text = textBox.Text;
        }

        private void LastModifiedTimeLabel_MouseMove(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.IBeam;
        }

        private void Item_ShowMainForm_Click(object sender, EventArgs e)
        {
            showMainForm();
        }
    }
}
