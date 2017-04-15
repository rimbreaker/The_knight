using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        public Form1 parennt;
        public Form2(Form1 parent)
        {
            parennt = parent;
            this.CenterToParent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if((string)comboBox1.SelectedItem == "8x8")
            {
                parennt.gameStarted = false;
                parennt.repaintt(8);                             
                this.Close();
            }
            else if ((string)comboBox1.SelectedItem == "10x10")
            {
                parennt.gameStarted = false;
                parennt.repaintt(10);               
                this.Close();
            }
            else
            {
                parennt.gameStarted = false;
                parennt.repaintt(12);                
                this.Close();
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();            
        }
    }
}
