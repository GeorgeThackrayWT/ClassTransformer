 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DtoParser;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            var ct = new ClassTransformer();

            ct.Source = this.textBox1.Text;

            ct.Transform();

            var newText = "";
            foreach (var t in ct.TargetLines)
            {
                newText += t + Environment.NewLine;
            }


            this.textBox2.Text = newText;
          
        }
    }
}
