using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace markov_chains
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private String TextLabel2 = "2. Заполните таблицу вероятностей переходов";
        int n; //колво состояний
        int[] kolvo;
        Random r = new Random();
        int constK = 10000;
        int time;
        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = TextLabel2;
            n = Int32.Parse(comboBox1.Text);
            dataGridView1.ColumnCount = n;
            dataGridView1.RowCount = n;
            dataGridView3.ColumnCount = n;
            dataGridView3.RowCount = 1;
            dataGridView3.Rows[0].HeaderCell.Value = "P0";
            for (int i = 0; i<n;i++)
            {
                dataGridView1.Columns[i].Width = 30;
                dataGridView3.Columns[i].Width = 30;
                dataGridView1.Columns[i].HeaderText = i.ToString();                
                dataGridView3.Columns[i].HeaderText = "S" + i.ToString();
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                dataGridView3.Rows[0].Cells[i].Value = 0;
                for (int j = 0; j < n; j++)
                { dataGridView1.Rows[i].Cells[j].Value = 0; }

                dataGridView1.Rows[i].Cells[i].Value = 0;
            }
            dataGridView3.Rows[0].Cells[0].Value = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.ColumnCount = n;
            dataGridView2.RowCount = 1;
            dataGridView2.Rows[0].HeaderCell.Value = "P";
            dataGridView4.ColumnCount = n;
            dataGridView4.RowCount = 1;
            dataGridView4.Rows[0].HeaderCell.Value = "P";
            for (int i = 0; i < n; i++)
            {
                dataGridView2.Columns[i].Width = 30;
                dataGridView2.Columns[i].HeaderText = "S"+i.ToString();
                dataGridView4.Columns[i].Width = 30;
                dataGridView4.Columns[i].HeaderText = "S" + i.ToString();
            }
            time = Int32.Parse(textBox1.Text);

            /////////////////////
            //метод монте-карло//
            /////////////////////
            kolvo = new int[n];           
            for (int k = 0; k < constK; k++)
            {
                int tekushee = -1;
                //выбираем текущее из начальных
                int rand = r.Next(1,100);
                int now = 0;
                for (int i = 0; i < n; i++)
                {
                    int a = Int32.Parse(dataGridView3.Rows[0].Cells[i].Value.ToString()); //текущая ячейка
                    if ((rand > now)&&(rand <= now+a))
                            { tekushee = i; break; }
                    now += a;
                }
                if (tekushee == -1) throw new Exception();
                now = 0;
                for (int i = 0; i < time; i++)
                {
                    rand = r.Next(1,100);
                    for (int j = 0; j < n; j++)
                    {
                        int a = Int32.Parse(dataGridView1.Rows[tekushee].Cells[j].Value.ToString()); //текущая ячейка
                        if ((rand > now) && (rand < now + a))
                        { tekushee = j; break; }
                        now += a;
                    }
                    //выбираем куда идти из текущего положения
                    now = 0;
                }
                kolvo[tekushee]+= 1;
                tekushee = -1;
            }

            for (int i = 0; i < n; i++)
            { dataGridView2.Rows[0].Cells[i].Value = (double)kolvo[i]*100/constK;
                dataGridView2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            }

           
        }

        private double P(int i, int time)
        {
           
            if (time == 0) return double.Parse(dataGridView3.Rows[0].Cells[i].Value.ToString())/100;
            else
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                { sum += P(j, time - 1) * double.Parse(dataGridView1.Rows[j].Cells[i].Value.ToString())/100; }
                return sum;
            } 
            



        }

        private void button3_Click(object sender, EventArgs e)
        {
            ///////////////////////
            //аналитический метод//
            ///////////////////////
            
            for (int i = 0; i < n; i++)
            {
                dataGridView4.Rows[0].Cells[i].Value = (100* P(i, time)).ToString("##.##");
                dataGridView4.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
    }
}
