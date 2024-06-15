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
        public News()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsSuccess = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IsSuccess = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
