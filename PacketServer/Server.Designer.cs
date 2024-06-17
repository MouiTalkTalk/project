namespace PacketServer
{
    partial class Server
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
            this.richUsrText = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ServerState = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ConnClientNum = new System.Windows.Forms.Label();
            this.UsrList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // richUsrText
            // 
            this.richUsrText.Location = new System.Drawing.Point(17, 96);
            this.richUsrText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richUsrText.Name = "richUsrText";
            this.richUsrText.ReadOnly = true;
            this.richUsrText.Size = new System.Drawing.Size(752, 508);
            this.richUsrText.TabIndex = 1;
            this.richUsrText.Text = "";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(19, 616);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMessage.MaxLength = 300;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1005, 28);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(1034, 616);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(91, 34);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "전송";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "서버 상태 :";
            // 
            // ServerState
            // 
            this.ServerState.AutoSize = true;
            this.ServerState.Location = new System.Drawing.Point(120, 34);
            this.ServerState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ServerState.Name = "ServerState";
            this.ServerState.Size = new System.Drawing.Size(45, 18);
            this.ServerState.TabIndex = 5;
            this.ServerState.Text = "Stop";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "접속한 Client 숫자 : ";
            // 
            // ConnClientNum
            // 
            this.ConnClientNum.AutoSize = true;
            this.ConnClientNum.Location = new System.Drawing.Point(401, 34);
            this.ConnClientNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ConnClientNum.Name = "ConnClientNum";
            this.ConnClientNum.Size = new System.Drawing.Size(18, 18);
            this.ConnClientNum.TabIndex = 8;
            this.ConnClientNum.Text = "0";
            // 
            // UsrList
            // 
            this.UsrList.HideSelection = false;
            this.UsrList.Location = new System.Drawing.Point(777, 96);
            this.UsrList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.UsrList.Name = "UsrList";
            this.UsrList.Size = new System.Drawing.Size(347, 508);
            this.UsrList.TabIndex = 9;
            this.UsrList.UseCompatibleStateImageBehavior = false;
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 675);
            this.Controls.Add(this.UsrList);
            this.Controls.Add(this.ConnClientNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ServerState);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.richUsrText);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Server";
            this.Text = "Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Server_FormClosed);
            this.Load += new System.EventHandler(this.Server_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richUsrText;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ServerState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ConnClientNum;
        private System.Windows.Forms.ListView UsrList;
    }
}

