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
    public partial class Report : Form
    {
        private string stringD = "SELECT * FROM FullList;";
        public string connectString;

        public Report(string connectString)
        {
            this.connectString = connectString;
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
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

        private void button5_Click(object sender, EventArgs e)
        {
            FillDataGrid($"SELECT * FROM FullList WHERE FullList.DateIn BETWEEN #{dateTimePicker1.Text}# AND #{dateTimePicker2.Text}# AND ((FullList.Source)=\"От поставщика\")");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillDataGrid($"SELECT * FROM FullList WHERE FullList.DateIn BETWEEN #{dateTimePicker1.Text}# AND #{dateTimePicker2.Text}# AND ((FullList.Source)=\"Из производства\")");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FillDataGrid($"SELECT * FROM FullList WHERE FullList.DateIn BETWEEN #{dateTimePicker1.Text}# AND #{dateTimePicker2.Text}# AND ((FullList.Agent) <> '')");
        }
    }
}