using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace QuickMaths
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        DataTable RaspCorecte = new DataTable();
        private void button1_Click(object sender, EventArgs e)  // incarca raspunsuri corecte 
        {
            var FD = new System.Windows.Forms.OpenFileDialog() { Filter = "CSV files (*.csv)|*.csv|Excel Files|*.xls;*.xlsx", ValidateNames = true };
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(FD.FileName)), true))
                {
                    RaspCorecte.Load(csvReader);
                }
            }
        }

       DataTable RaspElevi = new DataTable();
        private void button2_Click(object sender, EventArgs e)  // incarca raspunsurile elevilor
        {

            var FD = new System.Windows.Forms.OpenFileDialog() { Filter = "CSV files (*.csv)|*.csv|Excel Files|*.xls;*.xlsx", ValidateNames = true };
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(FD.FileName)), true))
                {
                    RaspElevi.Load(csvReader);
                }
            }
        }


        public static void WriteDataToFile(DataTable submittedDataTable, string submittedFilePath)
        {
            int i = 0;
            StreamWriter sw = null;

            sw = new StreamWriter(submittedFilePath, false);

            for (i = 0; i < submittedDataTable.Columns.Count - 1; i++)
            {

                sw.Write(submittedDataTable.Columns[i].ColumnName + " ");

            }
            sw.Write(submittedDataTable.Columns[i].ColumnName);
            sw.WriteLine();

            foreach (DataRow row in submittedDataTable.Rows)
            {
                object[] array = row.ItemArray;

                for (i = 0; i < array.Length - 1; i++)
                {
                    sw.Write(array[i].ToString() + " ");
                }
                sw.Write(array[i].ToString());
                sw.WriteLine();

            }

            sw.Close();
        }

        private void button3_Click(object sender, EventArgs e) //genereaza notele 
        {
            string Nume;
            DataTable RaspFinale = new DataTable();

            RaspFinale.Columns.Add("Nume");
            RaspFinale.Columns.Add("Punctaj" ,typeof(double));
            RaspFinale.Columns.Add("Exercitii Corecte/Exercitii Totale");

            for (int i=0; i < RaspElevi.Rows.Count; i++)
            {
                Nume = RaspElevi.Rows[i][1].ToString();

                int IntrebariCorecte=0;
                int Total = RaspCorecte.Columns.Count;

                for(int j = 0; j < RaspCorecte.Columns.Count; j++)
                {
                    if(RaspElevi.Rows[i][j+2].ToString() == RaspCorecte.Rows[0][j].ToString())
                    {
                        IntrebariCorecte++;
                    }

                }

                int Punctaj = (IntrebariCorecte * 100) / Total;

                DataRow RaspFinaleRow = RaspFinale.NewRow();
                RaspFinaleRow[0] = Nume;
                RaspFinaleRow[1] = Punctaj;
                RaspFinaleRow[2] = IntrebariCorecte.ToString() + "/" + Total.ToString();

                RaspFinale.Rows.Add(RaspFinaleRow);
            }


            DataView dv = RaspFinale.DefaultView;
            dv.Sort = "Punctaj desc";
            DataTable sortedDT = dv.ToTable();
            WriteDataToFile(sortedDT, "Raspunsuri " + DateTime.Now.ToString("dd,MM,yyyy") + ".txt");

            

        }
    }
}
