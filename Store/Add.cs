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
using fllcbx;

namespace Store
{
    public partial class Add : Form
    {
        private string[,] values;
        private string connectionString;
        public Add(string connectString)
        {
            InitializeComponent();
            connectionString = connectString;
        }

        private void FillAllBox()
        {
            FillCombobox.Fill(connectionString, comboBox1, "SELECT * FROM Types", 0, 1);
            FillCombobox.Fill(connectionString, comboBox3, "SELECT * FROM Types", 0, 1);
            FillCombobox.Fill(connectionString, comboBox4, "SELECT * FROM Items", 0, 1);
        }

        private void DataUpdate()
        {
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter("SELECT DISTINCT Items.NameItem, Types.Type FROM Types INNER JOIN Items ON Types.idType = Items.idType;", connectionString);
            DataTable data = new DataTable();
            oleDbDataAdapter.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.Columns[0].HeaderText = "Список зарегистрированных наименований";
            dataGridView1.Columns[1].HeaderText = "Тип";

            values = new string[data.Rows.Count, data.Columns.Count];
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    values[i, j] = (string)row[j];
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= 0)
            {
                MessageBox.Show("Вы пытаетесь добавить тип без названия", "Внимание");
                return;
            }
            for (int i = 0; i < values.Length / 2; i++)
            {
                if (textBox1.Text == values[i, 1])
                {
                    MessageBox.Show($"Тип  {textBox1.Text} уже добавлен", "Внимание!");
                    return;
                }
            }


            if (MessageBox.Show($"Добавить тип  {textBox1.Text}?", "Добавление", MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str = $"INSERT INTO Types (Type) VALUES (\'{textBox1.Text}\')";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
                textBox1.Clear();
            }
            DataUpdate();
            FillAllBox();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Удалить тип  " + comboBox1.Text.ToString() + "?\nУдаление типа  " + comboBox1.Text.ToString() + " приведет к удалению всех наименований данного типа. Это действие НЕОБРАТИМО!\nУбедитесь, что все наименования данного типа не используются!", "Удаление",
                    MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str =
                    "DELETE * FROM Types WHERE idType =" +
                    $"{comboBox1.SelectedValue}";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
            }
            DataUpdate();
            FillAllBox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length <= 0)
            {
                MessageBox.Show("Вы пытаетесь добавить наимeнование  без названия", "Внимание");
                return;
            }
            for (int i = 0; i < values.Length / 2; i++)
            {
                if (textBox3.Text == values[i, 0])
                {
                    MessageBox.Show($"Наименование {textBox3.Text} уже добавлен", "Внимание!");
                    return;
                }
            }

            if (MessageBox.Show($"Добавить наименование {textBox3.Text}?", "Добавление", MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str = $"INSERT INTO Items (NameItem, idType) VALUES (\'{textBox3.Text}\', \'{comboBox3.SelectedValue}\' )";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
                textBox3.Clear();
            }
            DataUpdate();
            FillAllBox();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Удалить тип " + comboBox4.Text.ToString() + "?\nУдаление наименования  " + comboBox4.Text.ToString() + " приведет к удалению всех наименований данного типа. Это действие НЕОБРАТИМО!\nУбедитесь, что все наименования мебели данного типа не используются!", "Удаление",
                    MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                var str =
                    "DELETE * FROM Items WHERE idName =" +
                    $"{comboBox4.SelectedValue}";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand oleDb = new OleDbCommand(str, connection);
                oleDb.ExecuteNonQuery();
                connection.Close();
            }
            DataUpdate();
            FillAllBox();
        }

        private void Add_Load(object sender, EventArgs e)
        {
            DataUpdate();
            FillAllBox();
        }
    }
}
