namespace game2
{
    partial class Form2
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Horse1 = new System.Windows.Forms.PictureBox();
            this.Horse2 = new System.Windows.Forms.PictureBox();
            this.Horse3 = new System.Windows.Forms.PictureBox();
            this.Horse4 = new System.Windows.Forms.PictureBox();
            this.Horse5 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse5)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::game2.Properties.Resources.HorseLine;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(900, 600);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Horse1
            // 
            this.Horse1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(165)))), ((int)(((byte)(38)))));
            this.Horse1.Image = global::game2.Properties.Resources.Horse1;
            this.Horse1.Location = new System.Drawing.Point(0, 222);
            this.Horse1.Name = "Horse1";
            this.Horse1.Size = new System.Drawing.Size(130, 90);
            this.Horse1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Horse1.TabIndex = 1;
            this.Horse1.TabStop = false;
            this.Horse1.Click += new System.EventHandler(this.Horse1_Click);
            // 
            // Horse2
            // 
            this.Horse2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(165)))), ((int)(((byte)(38)))));
            this.Horse2.Image = global::game2.Properties.Resources.Horse1;
            this.Horse2.Location = new System.Drawing.Point(0, 292);
            this.Horse2.Name = "Horse2";
            this.Horse2.Size = new System.Drawing.Size(130, 90);
            this.Horse2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Horse2.TabIndex = 2;
            this.Horse2.TabStop = false;
            this.Horse2.Click += new System.EventHandler(this.Horse2_Click);
            // 
            // Horse3
            // 
            this.Horse3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(165)))), ((int)(((byte)(38)))));
            this.Horse3.Image = global::game2.Properties.Resources.Horse1;
            this.Horse3.Location = new System.Drawing.Point(0, 363);
            this.Horse3.Name = "Horse3";
            this.Horse3.Size = new System.Drawing.Size(130, 90);
            this.Horse3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Horse3.TabIndex = 3;
            this.Horse3.TabStop = false;
            this.Horse3.Click += new System.EventHandler(this.Horse3_Click);
            // 
            // Horse4
            // 
            this.Horse4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(165)))), ((int)(((byte)(38)))));
            this.Horse4.Image = global::game2.Properties.Resources.Horse1;
            this.Horse4.Location = new System.Drawing.Point(0, 440);
            this.Horse4.Name = "Horse4";
            this.Horse4.Size = new System.Drawing.Size(130, 90);
            this.Horse4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Horse4.TabIndex = 4;
            this.Horse4.TabStop = false;
            this.Horse4.Click += new System.EventHandler(this.Horse4_Click);
            // 
            // Horse5
            // 
            this.Horse5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(165)))), ((int)(((byte)(38)))));
            this.Horse5.Image = global::game2.Properties.Resources.Horse1;
            this.Horse5.Location = new System.Drawing.Point(0, 510);
            this.Horse5.Name = "Horse5";
            this.Horse5.Size = new System.Drawing.Size(130, 90);
            this.Horse5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Horse5.TabIndex = 5;
            this.Horse5.TabStop = false;
            this.Horse5.Click += new System.EventHandler(this.Horse5_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(230)))), ((int)(((byte)(29)))));
            this.label1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(283, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "말을 선택하면 게임이 시작됩니다.";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(901, 601);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Horse5);
            this.Controls.Add(this.Horse4);
            this.Controls.Add(this.Horse3);
            this.Controls.Add(this.Horse2);
            this.Controls.Add(this.Horse1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form2";
            this.Text = "경마 게임";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Horse5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox Horse1;
        private System.Windows.Forms.PictureBox Horse2;
        private System.Windows.Forms.PictureBox Horse3;
        private System.Windows.Forms.PictureBox Horse4;
        private System.Windows.Forms.PictureBox Horse5;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
    }
}

