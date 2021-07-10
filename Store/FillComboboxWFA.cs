using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;

namespace fllcbx
{
    class FillCombobox
    {
        public static void Fill(string connectionString, object combobox, string sql, int key, int value)
        {

//                                                                                                                                                                              //
//                                                                                                                                                                              //
//                                                                                                                                                                              //
//      FillCombobox.Fill(connectionString, ComboBox, "SELECT * FROM fullSpis;", номер столбца с ID(ключ) в бд(обфчно 0), номре столбца с значениями для отображения);          //
//                                                                                                                                                                              //
//                                                                                                                                                                              //                                                                                                                                                                              //

            try
            {
                OleDbConnection soed = new OleDbConnection(connectionString);
                soed.Open();
                string Query = sql;
                OleDbCommand oleDb = new OleDbCommand(Query, soed);
                OleDbDataReader dr = oleDb.ExecuteReader();
                Dictionary<int, string> comboBoxSource = new Dictionary<int, string>();
                while (dr.Read())
                {
                    int keyIN = Convert.ToInt32(dr.GetValue(key));
                    string valueIN = dr.GetValue(value).ToString();
                    comboBoxSource.Add(keyIN, valueIN);
                }      
                (combobox as ComboBox).DataSource = new BindingSource(comboBoxSource, null);
                (combobox as ComboBox).DisplayMember = "Value";
                (combobox as ComboBox).ValueMember = "Key";
                soed.Close();
            }

            catch (Exception e)
            {
                MessageBox.Show("Возникла ошибка " + e, "Oшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
