namespace HaemaNote
{
    partial class NoteItemPanel
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Notch = new System.Windows.Forms.Panel();
            this.noteTextLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Notch
            // 
            this.Notch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(182)))));
            this.Notch.Location = new System.Drawing.Point(0, 0);
            this.Notch.Name = "Notch";
            this.Notch.Size = new System.Drawing.Size(300, 30);
            this.Notch.TabIndex = 0;
            // 
            // noteTextLabel
            // 
            this.noteTextLabel.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.noteTextLabel.Location = new System.Drawing.Point(7, 37);
            this.noteTextLabel.Name = "noteTextLabel";
            this.noteTextLabel.Size = new System.Drawing.Size(286, 156);
            this.noteTextLabel.TabIndex = 1;
            this.noteTextLabel.Text = "(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 " +
    "내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)" +
    "(메모 내용)(메모 내용)(메모 내용)(메모 내용)(메모 내용)";
            // 
            // NoteItemPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(201)))));
            this.Controls.Add(this.noteTextLabel);
            this.Controls.Add(this.Notch);
            this.Margin = new System.Windows.Forms.Padding(22, 13, 0, 0);
            this.Name = "NoteItemPanel";
            this.Size = new System.Drawing.Size(300, 200);
            this.DoubleClick += new System.EventHandler(this.NoteItemPanel_DoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Notch;
        private System.Windows.Forms.Label noteTextLabel;
    }
}
