namespace WinViewer
{
    partial class PickServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IPv4Address = new System.Windows.Forms.Label();
            this.IPv4Address_Box = new IPAddressControlLib.IPAddressControl();
            this.IPAddress_Port = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.Connect = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // IPv4Address
            // 
            this.IPv4Address.AutoSize = true;
            this.IPv4Address.Location = new System.Drawing.Point(12, 16);
            this.IPv4Address.Name = "IPv4Address";
            this.IPv4Address.Size = new System.Drawing.Size(73, 13);
            this.IPv4Address.TabIndex = 0;
            this.IPv4Address.Text = "IPv4 Address:";
            // 
            // IPv4Address_Box
            // 
            this.IPv4Address_Box.AllowInternalTab = false;
            this.IPv4Address_Box.AutoHeight = true;
            this.IPv4Address_Box.BackColor = System.Drawing.SystemColors.Window;
            this.IPv4Address_Box.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.IPv4Address_Box.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.IPv4Address_Box.Location = new System.Drawing.Point(91, 13);
            this.IPv4Address_Box.MinimumSize = new System.Drawing.Size(87, 20);
            this.IPv4Address_Box.Name = "IPv4Address_Box";
            this.IPv4Address_Box.ReadOnly = false;
            this.IPv4Address_Box.Size = new System.Drawing.Size(481, 20);
            this.IPv4Address_Box.TabIndex = 1;
            this.IPv4Address_Box.Text = "...";
            this.IPv4Address_Box.TextChanged += new System.EventHandler(this.IPv4Address_Box_TextChanged);
            // 
            // IPAddress_Port
            // 
            this.IPAddress_Port.AutoSize = true;
            this.IPAddress_Port.Location = new System.Drawing.Point(12, 43);
            this.IPAddress_Port.Name = "IPAddress_Port";
            this.IPAddress_Port.Size = new System.Drawing.Size(72, 13);
            this.IPAddress_Port.TabIndex = 0;
            this.IPAddress_Port.Text = "Port Number :";
            this.IPAddress_Port.Click += new System.EventHandler(this.IPAddress_Port_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(91, 40);
            this.textBox1.MaxLength = 5;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(481, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "14242";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(451, 66);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(121, 23);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(12, 66);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(121, 23);
            this.Connect.TabIndex = 3;
            this.Connect.Text = "Attempt to Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(139, 68);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.Size = new System.Drawing.Size(306, 20);
            this.Output.TabIndex = 0;
            this.Output.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Output.WordWrap = false;
            // 
            // PickServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 102);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.IPAddress_Port);
            this.Controls.Add(this.IPv4Address_Box);
            this.Controls.Add(this.IPv4Address);
            this.Name = "PickServer";
            this.Text = "Please enter the host\'s network information.";
            this.Load += new System.EventHandler(this.PickServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IPv4Address;
        private System.Windows.Forms.Label IPAddress_Port;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.TextBox Output;
        public IPAddressControlLib.IPAddressControl IPv4Address_Box;
        public System.Windows.Forms.TextBox textBox1;
    }
}