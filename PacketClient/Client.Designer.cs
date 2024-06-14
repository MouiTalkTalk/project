namespace PacketClient
{
    partial class Client
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
            this.label1 = new System.Windows.Forms.Label();
            this.clientState = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.richUserText = new System.Windows.Forms.RichTextBox();
            this.UsrList = new System.Windows.Forms.ListBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "연결 상태 :";
            // 
            // clientState
            // 
            this.clientState.AutoSize = true;
            this.clientState.Location = new System.Drawing.Point(84, 24);
            this.clientState.Name = "clientState";
            this.clientState.Size = new System.Drawing.Size(69, 12);
            this.clientState.TabIndex = 1;
            this.clientState.Text = "연결 없음...";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(713, 415);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "전송";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(15, 417);
            this.txtMessage.MaxLength = 300;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(692, 21);
            this.txtMessage.TabIndex = 4;
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // richUserText
            // 
            this.richUserText.Location = new System.Drawing.Point(15, 69);
            this.richUserText.Name = "richUserText";
            this.richUserText.ReadOnly = true;
            this.richUserText.Size = new System.Drawing.Size(581, 342);
            this.richUserText.TabIndex = 5;
            this.richUserText.Text = "";
            // 
            // UsrList
            // 
            this.UsrList.FormattingEnabled = true;
            this.UsrList.ItemHeight = 12;
            this.UsrList.Location = new System.Drawing.Point(602, 69);
            this.UsrList.Name = "UsrList";
            this.UsrList.Size = new System.Drawing.Size(186, 340);
            this.UsrList.TabIndex = 6;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(496, 24);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(100, 21);
            this.txtUserName.TabIndex = 7;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(425, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "유저 이름 :";
            // 
            // btnConnect
            // 
            this.btnConnect.Enabled = false;
            this.btnConnect.Location = new System.Drawing.Point(693, 24);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(95, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "서버와 연결";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.UsrList);
            this.Controls.Add(this.richUserText);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.clientState);
            this.Controls.Add(this.label1);
            this.Name = "Client";
            this.Text = "Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_FormClosed);
            this.Load += new System.EventHandler(this.Client_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label clientState;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.RichTextBox richUserText;
        private System.Windows.Forms.ListBox UsrList;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Timer timer1;
    }
}

