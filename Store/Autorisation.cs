using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Store
{
    public partial class Autorisation : Form
    {
        private bool isEnter = false;
        private string connectionString;
        public Autorisation(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void Autorisation_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isEnter = true;

                OleDbDataAdapter adapter = new OleDbDataAdapter(
                    "SELECT * FROM Users WHERE UserName= \'" + textBox1.Text + "\' and UserPass= \'" + textBox2.Text +
                    "\'", connectionString);
                DataTable data = new DataTable();
                adapter.Fill(data);
                try
                {
                    string access = data.Rows[0][1].ToString();
                    if (textBox1.Text.Length > 0) // проверяем введён ли логин     
                    {
                        if (textBox2.Text.Length > 0) // проверяем введён ли пароль         
                        {
                            // ищем в базе данных пользователя с такими данными         
                            if (data.Rows.Count > 0) // если такая запись существует       
                            {
                                Form1 form1 = (Form1)this.Owner;
                                if (access == "admin")
                                {
                                    form1.Show();
                                    form1.SighAdmin(access);
                                    Close();
                                }
                                else
                                {
                                    form1.Show();
                                    form1.SighUser(access);
                                    Close();
                                }

                            }
                            else
                            {
                                MessageBox.Show("Логин или пароль введены неверно!", "Ошибка", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error); // выводим ошибку  
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Логин или пароль введены неверно!", "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error); // выводим ошибку  

                }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void Autorisation_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isEnter)
            {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
                
            }
        }
    }
}
