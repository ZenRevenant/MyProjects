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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ZOO Zoo = ZOO.Instance;
        int daynumber = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem?.ToString();
            if (selectedItem != null)
            {
                Zoo.Add_Animals(this, selectedItem);
            }
            else
            {
                MessageBox.Show("Выберите, пожалуйста!");
            }
        }
        public delegate void Display(Form1 form);
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();
            Animal animal = AnimalFactory.CreateAnimal(selectedItem);
            Display Disp = animal.GetInfo;
            Disp += animal.SetPic;
            Disp(this);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "Мясо: " + Zoo.Meat.ToString();
            label3.Text = "Растительная пища: " + Zoo.Plant_Food.ToString();
            label4.Text = "Морепродукты: " + Zoo.Fish.ToString();
            label5.Text = "День: " + daynumber.ToString();
            label7.Text = "Баланс: " + Zoo.money.ToString($"F{2}");
            dataGridView1.RowHeadersVisible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Zoo.Feed_All(this);
            label2.Text = "Мясо: " + Zoo.Meat.ToString();
            label3.Text = "Растительная пища: " + Zoo.Plant_Food.ToString();
            label4.Text = "Морепродукты: " + Zoo.Fish.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                checked
                {
                    // Попытка увеличить значение y на 1
                    daynumber++;
                    Zoo.Next_Day(this);
                    label5.Text = "День: " + daynumber.ToString();
                    label7.Text = "Баланс: " + Zoo.money.ToString($"F{2}");
                }
            }
            catch (OverflowException ex)
            {
                // Обработка переполнения
                MessageBox.Show("Произошло переполнение","Ошибка!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox3.SelectedItem.ToString();
            if (selectedItem != null)
            {
                MessageBox.Show(Zoo.BuyFood(selectedItem), "Покупка");
                label2.Text = "Мясо: " + Zoo.Meat.ToString();
                label3.Text = "Растительная пища: " + Zoo.Plant_Food.ToString();
                label4.Text = "Морепродукты: " + Zoo.Fish.ToString();
                label7.Text = "Баланс: " + Zoo.money.ToString($"F{2}");
            }
            else
            {
                MessageBox.Show("Выберите, пожалуйста!");
            }
        }
    }
}
