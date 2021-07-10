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
    public partial class Agents : Form
    {
        private string[] values;
        private string connectionString;

        public Agents(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void DataUpdate()
        {
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter("SELECT Agent FROM Agents GROUP BY Agent;", connectionString);
            DataTable data = new DataTable();
            oleDbDataAdapter.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.Columns[0].HeaderText = "Список контрагентов";
            values = new string[data.Rows.Count];
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    values[i] = (string)row[j];
                }
            }

            
        }
        private void Agents_Load(object sender, EventArgs e)
        {
            DataUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length <= 0)
            {
                MessageBox.Show("Пустое наименование! Введите корректное наименование", "Внимание");
                return;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (textBox3.Text == values[i])
                {
                    MessageBox.Show($"Наименование {textBox3.Text} уже добавлено", "Внимание!");
                    return;
                }
            }

            if (MessageBox.Show("Добавить " + textBox3.Text + "?", "Добавление", MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str = $"INSERT INTO Agents (Agent) VALUES (\'{textBox3.Text}\')";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
                textBox3.Clear();
            }

            DataUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Удалить " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "?", "Удаление", MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str =
                    "DELETE * FROM Agents WHERE Agent =" + $"'{dataGridView1.CurrentRow.Cells[0].Value.ToString()}'";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
            }
            DataUpdate();
        }
    }
}
