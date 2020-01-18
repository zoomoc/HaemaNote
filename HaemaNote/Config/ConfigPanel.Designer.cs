namespace HaemaNote
{
    partial class ConfigPanel
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
            this.radio_text.Dock = System.Windows.Forms.DockStyle.Top;
            this.radio_text.Location = new System.Drawing.Point(3, 17);
            this.radio_text.Name = "radio_text";
            this.radio_text.Size = new System.Drawing.Size(145, 16);
            this.radio_text.TabIndex = 1;
            this.radio_text.TabStop = true;
            this.radio_text.Text = "텍스트";
            this.radio_text.UseVisualStyleBackColor = true;
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
            this.noteManageType.Location = new System.Drawing.Point(21, 65);
            this.noteManageType.Name = "noteManageType";
            this.noteManageType.Size = new System.Drawing.Size(151, 102);
            this.noteManageType.TabIndex = 4;
            this.noteManageType.TabStop = false;
            this.noteManageType.Text = "메모 데이터 저장 방식";
            // 
            // ConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.noteManageType);
            this.Controls.Add(this.label1);
            this.Name = "ConfigPanel";
            this.Size = new System.Drawing.Size(193, 191);
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
    }
}
