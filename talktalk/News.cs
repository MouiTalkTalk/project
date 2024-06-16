using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace talktalk
{
    public partial class News : Form
    {
        public bool IsSuccess { get; set; }

        private int HorT = 1;

        public News()
        {
            InitializeComponent();
            MessageBox.Show("방금 동전을 던졌습니다. 그림일까요, 숫자일까요?");
        }

        private void button1_Click(object sender, EventArgs e)
        { // 그림
            if(HorT == 0)
            {
                IsSuccess = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                IsSuccess = false;
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        { // 숫자
            if (HorT == 1)
            {
                IsSuccess = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                IsSuccess = false;
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }

        private void News_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            DateTime currentTime = DateTime.Now;
            Random random = new Random(currentTime.Millisecond);
            int answer = random.Next(2);
            HorT = answer;
        }
    }
}
