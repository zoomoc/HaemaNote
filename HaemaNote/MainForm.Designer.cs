namespace HaemaNote
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.webdavTestBtn = new System.Windows.Forms.Button();
            this.showNoteBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webdavTestBtn
            // 
            this.webdavTestBtn.Location = new System.Drawing.Point(31, 24);
            this.webdavTestBtn.Name = "webdavTestBtn";
            this.webdavTestBtn.Size = new System.Drawing.Size(143, 78);
            this.webdavTestBtn.TabIndex = 0;
            this.webdavTestBtn.Text = "webDav 연결 테스트";
            this.webdavTestBtn.UseVisualStyleBackColor = true;
            this.webdavTestBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // showNoteBtn
            // 
            this.showNoteBtn.Location = new System.Drawing.Point(31, 130);
            this.showNoteBtn.Name = "showNoteBtn";
            this.showNoteBtn.Size = new System.Drawing.Size(143, 82);
            this.showNoteBtn.TabIndex = 1;
            this.showNoteBtn.Text = "노트 모두 보이게 하기";
            this.showNoteBtn.UseVisualStyleBackColor = true;
            this.showNoteBtn.Click += new System.EventHandler(this.showNoteBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.showNoteBtn);
            this.Controls.Add(this.webdavTestBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "HaemaNote";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button webdavTestBtn;
        private System.Windows.Forms.Button showNoteBtn;
    }
}