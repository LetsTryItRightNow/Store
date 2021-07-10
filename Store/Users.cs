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
    public partial class Users : Form
    {
        private readonly string connectString;

        public Users(string connectString)
        {
            InitializeComponent();
            this.connectString = connectString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= 0 & textBox2.Text.Length <= 0)
            {
                MessageBox.Show("Вы пытаетесь добавить сотрудника без имени", "Внимание");
                return;
            }

            if (MessageBox.Show("Добавить сотрудника " + textBox1.Text + "?", "Добавление",
                    MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str = $"INSERT INTO Users (UserName, UserPass)  VALUES (\'{textBox1.Text}\', \'{textBox2.Text}\')";
                OleDbConnection connection = new OleDbConnection(connectString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
                textBox1.Clear();
                textBox2.Clear();
                FillDataGrid();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Users_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            var stringData = "SELECT UserName, UserPass FROM Users WHERE UserName <> 'admin';";
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(stringData, connectString);
            DataTable data = new DataTable();
            oleDbDataAdapter.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.Columns[0].HeaderText = "Идентификатор";
            dataGridView1.Columns[1].HeaderText = "Пароль";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Удалить " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "?", "Удаление", MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str =
                    "DELETE * FROM Users WHERE UserName=" + $"'{dataGridView1.CurrentRow.Cells[0].Value.ToString()}'";
                OleDbConnection connection = new OleDbConnection(connectString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
                FillDataGrid();
            }
        }
    }
}
