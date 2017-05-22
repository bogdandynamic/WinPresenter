using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinViewer
{
    public partial class PickServer : Form
    {

        public bool cancelButton = false;

        public PickServer()
        {
            InitializeComponent();
        }

        private void PickServer_Load(object sender, EventArgs e)
        {

        }

        private void IPv4Address_Box_TextChanged(object sender, EventArgs e)
        {

        }

        private void IPAddress_Port_Click(object sender, EventArgs e)
        {

        }

        private void Connect_Click(object sender, EventArgs e)
        {
            string IPv4Address = IPv4Address_Box.Text;
            if (IPv4Address == "")
            { return; }

            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            cancelButton = true;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //int x = textBox1.Text;
            //int x = Int32.Parse(textBox1.Text);
            string inputString = textBox1.Text;
            int numValue;
            bool parsed = Int32.TryParse(inputString, out numValue);

            if (!parsed)
            {
                //Output.AppendText("Error with port: " + "Int32.TryParse could not parse '" + inputString + "' to an int.\n" + Environment.NewLine);
                Output.AppendText(Environment.NewLine + "Error with port: " + "Int32.TryParse could not parse '" + inputString + "' to an int.\n");
                //Console.WriteLine("Int32.TryParse could not parse '{0}' to an int.\n", inputString);
                return;
            }
            else
            {
                Output.AppendText(Environment.NewLine);
            }
            
            if (Enumerable.Range(1, 65535).Contains(numValue))
            { return; }


            // Output: Int32.TryParse could not parse 'abc' to an int.
        }
    }
}
