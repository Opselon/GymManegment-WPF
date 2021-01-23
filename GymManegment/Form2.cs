using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GymManegment.window
{
    public partial class Form2: Form
    {
        public Form2()
        {
            InitializeComponent();
        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label14.Text = "22";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                label14.Text = "23";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                label14.Text = "24";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                label14.Text = "25";
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                label14.Text = "26";
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                label14.Text = "27";
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            double bmi;
            double vazn = double.Parse(numericUpDown2.Value.ToString());
            double ghad2 = Math.Pow(double.Parse(numericUpDown1.Value.ToString()), 2);
            bmi = vazn / ghad2;
            if (bmi.ToString().Length > 5)
            {
                textBox1.Text = (vazn / ghad2).ToString().Remove(5);
            }
            else
            {
                textBox1.Text = (vazn / ghad2).ToString();
            }
            if (bmi < 16.5 && bmi > 11)
            {
                label15.Visible = true;
                label16.Visible = false;
                label17.Visible = false;
                label18.Visible = false;
                label19.Visible = false;
            }
            else if (bmi < 18.5 && bmi > 16.5)
            {
                label15.Visible = false;
                label16.Visible = true;
                label17.Visible = false;
                label18.Visible = false;
                label19.Visible = false;
            }
            else if (bmi < 25 && bmi > 18.5)
            {
                label15.Visible = false;
                label16.Visible = false;
                label17.Visible = true;
                label18.Visible = false;
                label19.Visible = false;
            }
            else if (bmi < 30 && bmi > 25)
            {
                label15.Visible = false;
                label16.Visible = false;
                label17.Visible = false;
                label18.Visible = true;
                label19.Visible = false;
            }
            else if (bmi > 30)
            {
                label15.Visible = false;
                label16.Visible = false;
                label17.Visible = false;
                label18.Visible = true;
                label19.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label14.Text = "22";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                label14.Text = "23";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                label14.Text = "24";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                label14.Text = "25";
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                label14.Text = "26";
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                label14.Text = "27";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
       
        }
    }
}

