using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game1
{
    public partial class Form1 : Form
    {
        int present = 0;
        public bool IsSuccess { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (present == 2)
            {
                MessageBox.Show("당첨!! 10만원 지급!!");
                IsSuccess = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("실패.. 다음 기회에..");
                IsSuccess = false;
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (present == 3)
            {
                MessageBox.Show("당첨!! 10만원 지급!!");
                IsSuccess = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("실패.. 다음 기회에..");
                IsSuccess = false;
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (present == 1)
            {
                MessageBox.Show("당첨!! 10만원 지급!!");
                IsSuccess = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("실패.. 다음 기회에..");
                IsSuccess = false;
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (present == 4)
            {
                MessageBox.Show("당첨!! 10만원 지급!!");
                IsSuccess = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("실패.. 다음 기회에..");
                IsSuccess = false;
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (present == 5)
            {
                MessageBox.Show("당첨!! 10만원 지급!!");
                IsSuccess = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("실패.. 다음 기회에..");
                IsSuccess = false;
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            present = random.Next(0, 5);

            if (present < 0) { present = 0; }
            else if (present > 4) { present = 5; }
        }
    }
}
