using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game2
{
    public partial class Form2 : Form
    {
        public bool IsSuccess { get; set; }

        Random random = new Random();
        int[] horse = new int[5];
        int selected = -1;

        PictureBox[] horses = new PictureBox[5];
        public Form2()
        {
            InitializeComponent();
            MessageBox.Show("원하는 말을 신중하게 선택해주세요!");
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < 5; i++)
            {
                horse[i] = random.Next(5, 10);
                horses[i] = (PictureBox)this.Controls["Horse" + (i + 1).ToString()];
            }
        }

        int time = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time > 10)
            {
                for (int i = 0; i < 5; i++)
                {
                    horse[i] = random.Next(0, 20);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                horses[i].Left += horse[i];
                if (horses[i].Left > 900)
                {
                    timer1.Stop();
                    MessageBox.Show("winner is Line " + (i + 1).ToString() + "!!");
                    if (i == selected)
                    {
                        MessageBox.Show("당신의 말이 우승하였습니다. 2000만원 지급!!");
                        IsSuccess = true;
                        this.DialogResult = DialogResult.OK;
                    }
                    else {
                        MessageBox.Show("경마에 재능이 없으시군요.. 500만원을 잃었습니다..");
                        IsSuccess = false;
                        this.DialogResult = DialogResult.Cancel;
                    }
                    Close();
                }
            }

            time++;
        }

        private void Horse1_Click(object sender, EventArgs e)
        {
            if (selected == -1)
            {
                MessageBox.Show("1번말 선택");
                label1.Text = "선택한 말은 1번 말입니다.";
                selected = 0;
                timer1.Start();
            }
        }

        private void Horse2_Click(object sender, EventArgs e)
        {
            if (selected == -1)
            {
                MessageBox.Show("2번말 선택");
                label1.Text = "선택한 말은 2번 말입니다.";
                selected = 1;
                timer1.Start();
            }
        }

        private void Horse3_Click(object sender, EventArgs e)
        {
            if (selected == -1)
            {
                MessageBox.Show("3번말 선택");
                label1.Text = "선택한 말은 3번 말입니다.";
                selected = 2;
                timer1.Start();
            }
        }

        private void Horse4_Click(object sender, EventArgs e)
        {
            if (selected == -1)
            {
                MessageBox.Show("4번말 선택");
                label1.Text = "선택한 말은 4번 말입니다.";
                selected = 3;
                timer1.Start();
            }
        }

        private void Horse5_Click(object sender, EventArgs e)
        {
            if (selected == -1)
            {
                MessageBox.Show("5번말 선택");
                label1.Text = "선택한 말은 5번 말입니다.";
                selected = 4;
                timer1.Start();
            }
        }
    }

}
