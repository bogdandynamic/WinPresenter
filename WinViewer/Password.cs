using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinViewer
{
    public partial class Password : Form
    {
        //private TextBox pass = new TextBox();
        public bool cancelButton = false;

        public Password()
        {
            InitializeComponent();
        
        }
        protected override void OnLoad(EventArgs e)
        {
            this.ActiveControl = textBox1;
        }

        public void Connect_Click(object sender, EventArgs e)
        {
            string password = textBox1.Text;
            if(password == "")
            {return;}

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            //var netprogram = new NetProgram();
            //netprogram.NetPeerShutdown();

            cancelButton = true;

            Close();
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
