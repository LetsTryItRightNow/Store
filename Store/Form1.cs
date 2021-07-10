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
    public partial class Form1 : Form
    {
        private string nowDate = DateTime.Now.ToShortDateString();
        private string UserLogin;
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DataBaseStore.accdb;";
        private string stringD = "SELECT * FROM FullList;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            Autorisation au = new Autorisation(connectString);
            au.Owner = this;
            au.ShowDialog();
            
            comboBox3.Text = "";
            comboBox2.Text = "";
            comboBox1.Text = "";

            FillCombobox.Fill(connectString, comboBox4, "SELECT * FROM Items", 0, 1);
            FillCombobox.Fill(connectString, comboBox3, "SELECT * FROM Items", 0, 1);
            FillCombobox.Fill(connectString, comboBox2, "SELECT * FROM Types", 0, 1);
            FillCombobox.Fill(connectString, comboBox1, "SELECT * FROM Agents", 0, 1);

            FillDataGrid(stringD);
            
        }

        public void FillDataGrid(string StringD)
        {


            var stringData = StringD;
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(stringData, connectString);
            DataTable data = new DataTable();
            oleDbDataAdapter.Fill(data);

            dataGridView1.DataSource = data;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Тип";
            dataGridView1.Columns[2].HeaderText = "Наименование";
            dataGridView1.Columns[3].HeaderText = "Источник поставки";
            dataGridView1.Columns[4].HeaderText = "Дата учета в магазине";
            dataGridView1.Columns[6].HeaderText = "Реализовавший сотрудник";
            dataGridView1.Columns[5].HeaderText = "Кому реализовано";


        }

        private void контрагентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Agents addForm = new Agents(connectString);

            addForm.ShowDialog();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users addUsers = new Users(connectString);
            addUsers.ShowDialog();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About addUsers = new About();
            addUsers.ShowDialog();
        }

        private void рУководствоПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UsersManual usersManual = new UsersManual();
            usersManual.ShowDialog();
        }

        private void изделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add addF = new Add(connectString);
            addF.Owner = this;
            addF.ShowDialog();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDataGrid($"SELECT * FROM FullList WHERE NameItem = '{comboBox3.Text}'");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddData("От поставщика");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddData("Из производства");
        }

        private void AddData(string from)
        {
            int number;
            var value = int.TryParse(textBox2.Text, result: out number);

            if (textBox2.Text == "" || (value == false || number <= 0))
            {
                MessageBox.Show("Введите корректное количество", "Ошибка!");
                return;
            }

            if (MessageBox.Show($"Указанное Вами наименование в количестве {textBox2.Text} будет добавлено в магазин", "Добавление", MessageBoxButtons.OKCancel) ==
                DialogResult.OK)
            {
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter($"SELECT DISTINCT Types.Type FROM Types INNER JOIN Items ON Types.idType = Items.idType Where NameItem = '{comboBox4.Text}'", connectString);
                DataTable data = new DataTable();
                oleDbDataAdapter.Fill(data);

                string Typedata = data.Rows[0][0].ToString();

                var str = $"INSERT INTO [Full] (Type, NameItem, Source, DateIn) VALUES ('{Typedata}', '{comboBox4.Text}', '{from}', '{nowDate}');";
                OleDbConnection connection = new OleDbConnection(connectString);
                connection.Open();
                for (int i = 0; i < Int32.Parse(textBox2.Text); i++)
                {
                    OleDbCommand oleDb = new OleDbCommand(str, connection);
                    oleDb.ExecuteNonQuery();
                }
                connection.Close();
                textBox2.Clear();
            }
            FillDataGrid(stringD);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                FillDataGrid($"SELECT * FROM FullList WHERE Type = '{comboBox2.Text}'");
            
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            FillDataGrid(stringD);
        }


        public void SighAdmin(string User)
        {
            this.Text = "Учет продукции в магазине [" + User + "]";
            UserLogin = User;
            groupBox2.Enabled = true;
            groupBox2.Visible = true;

        }

        public void SighUser(string User)
        {
            UserLogin = User;
            groupBox1.Enabled = true;
            groupBox1.Visible = true;
            изделияToolStripMenuItem.Enabled = false;
            сотрудникиToolStripMenuItem.Enabled = false;
            this.Text = "Учет продукции в магазине [" + User + "]";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Выберите контрагента", "Внимание");
                return;
            }

            string aeasd = comboBox1.Text.ToString();
            //var str = $"UPDATE Full SET Employee = '{UserLogin}', Agent = '{comboBox1.Text.ToString()}' WHERE idItem = {dataGridView1.SelectedRows[0].Cells[0].Value}";
            OleDbConnection connection = new OleDbConnection(connectString);
       
            connection.Open();
            OleDbCommand oleDb = new OleDbCommand($"UPDATE [Full] SET Agent = \'{comboBox1.Text.ToString()}\', Employee = \'{UserLogin}\' WHERE idItem = {dataGridView1.SelectedRows[0].Cells[0].Value}", connection);
            oleDb.ExecuteNonQuery();
            connection.Close();
            FillDataGrid(stringD);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void поступленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report addF = new Report(connectString);
           
            addF.ShowDialog();
        }
    }
    

}
