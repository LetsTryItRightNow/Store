using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Store
{
    public partial class UsersManual : Form
    {
        public UsersManual()
        {
            InitializeComponent();
        }

        private void UsersManual_Load(object sender, EventArgs e)
        {
            using (var sr = new StreamReader("UsersManual.txt"))
            {
                richTextBox1.Text = sr.ReadToEnd();
            }
           }
    }
}
