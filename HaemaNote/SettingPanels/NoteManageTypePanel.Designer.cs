namespace HaemaNote.SettingPanels
{
    partial class NoteManageTypePanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.radio_text = new System.Windows.Forms.RadioButton();
            this.radio_file = new System.Windows.Forms.RadioButton();
            this.radio_cloud = new System.Windows.Forms.RadioButton();
            this.noteManageType = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.noteManageType.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "설정";
            // 
            // radio_text
            // 
            this.radio_text.AutoSize = true;
            this.radio_text.Location = new System.Drawing.Point(6, 20);
            this.radio_text.Name = "radio_text";
            this.radio_text.Size = new System.Drawing.Size(59, 16);
            this.radio_text.TabIndex = 1;
            this.radio_text.TabStop = true;
            this.radio_text.Text = "텍스트";
            this.radio_text.UseVisualStyleBackColor = true;
            this.radio_text.CheckedChanged += new System.EventHandler(this.radio_text_CheckedChanged);
            // 
            // radio_file
            // 
            this.radio_file.AutoSize = true;
            this.radio_file.Location = new System.Drawing.Point(6, 42);
            this.radio_file.Name = "radio_file";
            this.radio_file.Size = new System.Drawing.Size(47, 16);
            this.radio_file.TabIndex = 2;
            this.radio_file.TabStop = true;
            this.radio_file.Text = "파일";
            this.radio_file.UseVisualStyleBackColor = true;
            this.radio_file.CheckedChanged += new System.EventHandler(this.radio_file_CheckedChanged);
            // 
            // radio_cloud
            // 
            this.radio_cloud.AutoSize = true;
            this.radio_cloud.Location = new System.Drawing.Point(6, 64);
            this.radio_cloud.Name = "radio_cloud";
            this.radio_cloud.Size = new System.Drawing.Size(123, 16);
            this.radio_cloud.TabIndex = 3;
            this.radio_cloud.TabStop = true;
            this.radio_cloud.Text = "해마노트 클라우드";
            this.radio_cloud.UseVisualStyleBackColor = true;
            // 
            // noteManageType
            // 
            this.noteManageType.Controls.Add(this.radio_text);
            this.noteManageType.Controls.Add(this.radio_cloud);
            this.noteManageType.Controls.Add(this.radio_file);
            this.noteManageType.Location = new System.Drawing.Point(21, 50);
            this.noteManageType.Name = "noteManageType";
            this.noteManageType.Size = new System.Drawing.Size(150, 92);
            this.noteManageType.TabIndex = 4;
            this.noteManageType.TabStop = false;
            this.noteManageType.Text = "메모 데이터 저장 방식";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(327, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "닫기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NoteManageTypePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.noteManageType);
            this.Controls.Add(this.label1);
            this.Name = "NoteManageTypePanel";
            this.Size = new System.Drawing.Size(420, 608);
            this.noteManageType.ResumeLayout(false);
            this.noteManageType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radio_text;
        private System.Windows.Forms.RadioButton radio_file;
        private System.Windows.Forms.RadioButton radio_cloud;
        private System.Windows.Forms.GroupBox noteManageType;
        private System.Windows.Forms.Button button1;
    }
}
