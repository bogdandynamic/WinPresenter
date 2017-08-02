/* All content in this sample is ”AS IS” with with no warranties, and confer no rights. 
 * Any code on this blog is subject to the terms specified at http://www.microsoft.com/info/cpyright.mspx. 
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxRDPCOMAPILib;
using System.Threading;
using System.Runtime.InteropServices;
using RDPCOMAPILib;
using System.Net.Sockets;
using System.Net;
using Examples.ExamplesConsole;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using System.Linq;

namespace WinViewer
{
    public partial class WinViewer : Form
    {
        //private BackgroundWorker bw = new BackgroundWorker();
        public WinViewer()
        {

            /*if (File.Exists("inv.xml")) { File.Delete("inv.xml"); }
            if (File.Exists("peerAddress.txt")) { File.Delete("peerAddress.txt"); }
            if (File.Exists("monCount.xml")) { File.Delete("monCount.xml"); }
            if (File.Exists("monPosition.xml")) { File.Delete("monPosition.xml"); }
            if (File.Exists("monSelCheck.xml")) { File.Delete("monSelCheck.xml"); }
            if (File.Exists("test11.xml")) { File.Delete("test11.xml"); }*/


            //FileDelete("inv.xml");
            FileDelete("peerAddress.txt");
            FileDelete("monCount.xml");
            FileDelete("monPosition.xml");
            FileDelete("monSelCheck.xml");
            FileDelete("test11.xml");

            //File.Delete("inv.txt");
            //File.Delete("peerAddress.txt");


            //Form2 secondForm = new Form2();
            InitializeComponent();

            TextWriter tw = File.CreateText("monCount.xml");
            //tw.WriteLine(System.Windows.Forms.SystemInformation.MonitorCount);
            tw.WriteLine("1");
            tw.Close();


        }
        //public int exitCode { get; set; }
        //public string peerAddress { get; set; }
        //public string peerMessage { get; set; }
        //public bool sendMessage { get; set; }
        //private string netPeerAddress;

        public void FileDelete(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }


        public bool _controlAllowed = true;
        public bool _monitorSwitch = false;
        public bool _controlSwitch = false;
        public int monitor = 0;
        public int monTotal = 1;
        //MessagesClass messagesClass = new MessagesClass();
        //private BackgroundWorker bw = new BackgroundWorker();
        public void MonitorCounter(out int count)
        {
            //if (!File.Exists("monCount.xml"))
            //{
            //    TextWriter tw = File.CreateText("monCount.xml");
            //    //tw.WriteLine(System.Windows.Forms.SystemInformation.MonitorCount);
            //    tw.WriteLine("1");
            //    tw.Close();
            //}


            /*TextWriter mp = File.CreateText("monPosition.xml");
            mp.WriteLine("{X = 0,Y = 0,Width = 1920,Height = 1080}");
            mp.Close();*/

            if (!File.Exists("monPosition.xml"))
            {
                TextWriter mp = File.CreateText("monPosition.xml");
                mp.WriteLine("{X = 0,Y = 0,Width = 1920,Height = 1080}");
                mp.Close();
            }
       
            //int counter = Int32.Parse(File.ReadLines("monCount.xml").First());
            int counter = Int32.Parse(File.ReadLines("monPosition.xml").Count().ToString());

            monTotal = counter;
            //int counter = System.Windows.Forms.SystemInformation.MonitorCount;
            //int count;=
            count = counter;
            //return count.ToString;
            //LogTextBox.Text += (count + Environment.NewLine);
        }

        public void MonitorSelectionCheck()
        {
            
            if (monitor == monTotal)
            {
                TextWriter tw1 = File.CreateText("monSelCheck.xml");
                tw1.WriteLine(monitor.ToString() + "=" + monTotal.ToString());
                tw1.Close();

                monitor = 0;
            }

        }

        public void viewerWidth()
        {



            int count;
            MonitorCounter(out count);

            int viewerwidth = Int32.Parse(File.ReadLines("defaultViewerWidth.xml").First());



            pRdpViewer.Width = viewerwidth * count;


        }

        public void viewerHeight()
        {



            int count;
            MonitorCounter(out count);

            int viewerheight = Int32.Parse(File.ReadLines("defaultViewerHeight.xml").First());



            pRdpViewer.Height = viewerheight * count;


        }












        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int count;
            MonitorCounter(out count);
            ControlButton.Enabled = false;
            LogTextBox.AppendText("Monitor Count: " + count + Environment.NewLine);
            //FormBorderStyle = FormBorderStyle.None;
            //pRdpViewer.Size = new Size(1280, 720);
            //pRdpViewer.ClientSize = new Size(Width, Height);
            //pRdpViewer.AutoSize = false;
            //AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //LogTextBox.Text += ("This is working" + Environment.NewLine);
            int viewerheight = pRdpViewer.Height;
            int viewerwidth = pRdpViewer.Width;

            TextWriter tw = File.CreateText("defaultViewerWidth.xml");
            TextWriter tw1 = File.CreateText("defaultViewerHeight.xml");
            //tw.WriteLine(System.Windows.Forms.SystemInformation.MonitorCount);
            tw.WriteLine(viewerwidth);
            tw1.WriteLine(viewerheight);
            tw.Close();
            tw1.Close();

            string test11 = System.Windows.Forms.Screen.AllScreens.ToString();

            TextWriter tw2 = File.CreateText("test11.xml");
            tw2.WriteLine(test11);

            foreach (var screen in Screen.AllScreens)
            {
                // For each screen, add the screen properties to a list box.
                //tw2.WriteLine("Device Name: " + screen.DeviceName);
                //tw2.WriteLine("Bounds: " + screen.Bounds.ToString());
                tw2.WriteLine(screen.Bounds.ToString());
                //tw2.WriteLine("Type: " + screen.GetType().ToString());
                //tw2.WriteLine("Working Area: " + screen.WorkingArea.ToString());
                //tw2.WriteLine("Primary Screen: " + screen.Primary.ToString());
            }

            tw2.Close();

            //LogTextBox.AppendText(test11 + Environment.NewLine);


            Screen scrn = Screen.FromControl(this);




            string scrnbound = scrn.Bounds.ToString();

            LogTextBox.AppendText(scrnbound + Environment.NewLine);


            LogTextBox.AppendText("Window size: " + pRdpViewer.Width + "x" + pRdpViewer.Height + Environment.NewLine);
            pRdpViewer.Width = viewerwidth * count;
            int locationx = Width / 110;
            pRdpViewer.Location = new Point(locationx, pRdpViewer.Location.Y);
            //pRdpViewer.Height = viewerheight - viewerheight/1;
            pRdpViewer.SmartSizing = true;
            //bool monitorSwitch = true;
            //pRdpViewer.OcxState;

            UpdateMonitorPosition();


        }

        public void UpdateMonitorPosition()
        {
            MonitorSelectionCheck();

            int count;
            MonitorCounter(out count);
            //count = 5;
            //int monitor;
            int monitor1 = monitor + 1;
            monitor = monitor1;

            //int count;
            //MonitorCounter(out count);
            string mon = File.ReadLines("monPosition.xml").Skip(monitor - 1).Take(monitor).First();
            string trimmed = mon.Trim('{', '}');
            char[] delimiterChars = { ',' };

            string[] newmon = trimmed.Split(delimiterChars);

            LogTextBox.AppendText("Monitor: " + monitor + Environment.NewLine);
            LogTextBox.AppendText(newmon[0] + Environment.NewLine);
            LogTextBox.AppendText(newmon[1] + Environment.NewLine);
            LogTextBox.AppendText(newmon[2] + Environment.NewLine);
            LogTextBox.AppendText(newmon[3] + Environment.NewLine);

            /*
            TextWriter tw11 = File.CreateText("bounds.xml");
            tw11.WriteLine(newmon[0]);
            tw11.WriteLine(newmon[1]);
            tw11.WriteLine(newmon[2]);
            tw11.WriteLine(newmon[3]);
            tw11.Close();
            */

            //TextWriter tw12 = File.CreateText("bounds.xml");
            string x = newmon[0].Substring(newmon[0].IndexOf('=') + 1);
            string y = newmon[1].Substring(newmon[1].IndexOf('=') + 1);
            string w = newmon[2].Substring(newmon[2].IndexOf('=') + 1);
            string h = newmon[3].Substring(newmon[3].IndexOf('=') + 1);
            //tw12.Close();

            //int viewerwidth;
            int viewerwidth = Convert.ToInt32(w);
            int viewerheight = Convert.ToInt32(h);
            pRdpViewer.Location = new Point(-Convert.ToInt32(x), -Convert.ToInt32(y));
            //pRdpViewer.Location = new Point(pRdpViewer.Location.X, Convert.ToInt32(y));
            pRdpViewer.Width = viewerwidth * count;
            pRdpViewer.Height = viewerheight;






            

        }

        //ServerName = data["Server"];
        //ChannelName = data["Channel"];

        //double zoom = Math.Sqrt((squared / 127204) + ((width + height) / 880));

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            //LogTextBox.AppendText("Searching for host and attempting to establish a connection. This window may lock up for a few seconds." + Environment.NewLine);

            LogTextBox.AppendText("Please enter the host's network address and port." + Environment.NewLine);

            //NetManager.RunExample();


            //LogTextBox.AppendText("Found host. Attempting to establish a connect" + Environment.NewLine);

            //NetManager.EstablishConnection();


            //NetManager.ReceiveConnection();

            //WinViewer netmanager = new WinViewer();
            //NetManager netmanager = new NetManager();
            //NetProgram netprogram = new NetProgram();


            //var netprogram = new NetProgram();
            //netprogram.NetPeerShutdown();
            //netprogram.NetPeerProgram();

            //netprogram.NetPeerShutdown();

            PickServer serverInfo = new PickServer();
            serverInfo.ShowDialog();

            if (serverInfo.IPv4Address_Box.Text == "")
            { return; }
            if (serverInfo.textBox1.Text == "")
            { return; }
            if (serverInfo.cancelButton == true)
            { return; }




            //TextWriter tw = File.CreateText("ip.config");

            string peerIP = serverInfo.IPv4Address_Box.Text;
            string peerPort = serverInfo.textBox1.Text;


            //WinViewer propTest = new WinViewer();
            //MessagesClass propTest = new MessagesClass();
            //NetManager tws = new NetManager(
            //"This report displays the number {0}.", 42);

            //NetManager propTest = new NetManager();


            //Console.WriteLine(peerAddress + Environment.NewLine);
            string peerAddress = peerIP + ":" + peerPort;

            if (serverInfo.cancelButton == true)
            {
                return;
            }
            else
            {
                Console.WriteLine(peerAddress + Environment.NewLine);

                TextWriter tw = File.CreateText("peerAddress.txt");
                tw.WriteLine(peerAddress);
                tw.Close();


                LogTextBox.AppendText("Address: " + peerAddress + Environment.NewLine);


                StartNetManager();


            }

            //Console.WriteLine(peerAddress + Environment.NewLine);

            //TextWriter tw = File.CreateText("peerAddress.txt");
            //tw.WriteLine(peerAddress);
            //tw.Close();



            //tw.WriteLine(peerAddress);

            //tw.Close();

            //netPeerAddress = peerAddress;


            // -- LogTextBox.AppendText("Address: " + peerAddress + Environment.NewLine);

            //SSLExample serverConnect = new SSLExample();

            // -- LogTextBox.AppendText("Attempting to establish a connection." + Environment.NewLine);

            //peerMessage("This is a test", peerAddress);

            //var netManager = new SSLExample();
            //var netManager = new NetManager();
            //netManager.peerMessages();
            //Console.WriteLine(propTest.peerAddress + Environment.NewLine);
            //Console.WriteLine(propTest.peerMessage + Environment.NewLine);
            //Console.WriteLine(propTest.sendMessage + Environment.NewLine);
            //Console.WriteLine(propTest.exitCode + Environment.NewLine);


            // Request host's XML connection file
            //int exitCode = 0;
            //string peerMessage = "decodeAddress";
            //bool sendMessage = true;

            //Console.WriteLine("New properties" + Environment.NewLine);

            //Console.WriteLine(peerAddress + Environment.NewLine);
            //Console.WriteLine(peerMessage + Environment.NewLine);
            //Console.WriteLine(sendMessage + Environment.NewLine);
            //Console.WriteLine(exitCode + Environment.NewLine);

            //NetManager propTest = new NetManager(peerMessage, peerAddress, sendMessage, exitCode);

            // -- NetManager propTest = new NetManager();

            // -- Thread netManager = new Thread(new ThreadStart(propTest.RunExample));

            // -- netManager.Start();


            //LogTextBox.AppendText("Network thread started. Sleeping for 5 seconds while data is bing received" + Environment.NewLine);
            // -- LogTextBox.AppendText("Network thread started." + Environment.NewLine);

            //Thread.Sleep(5000);


            //Thread.Sleep(2500);



            //monitor = File.ReadLines("inv.xml").First();


            // -- LogTextBox.AppendText("Opening password dialog box" + Environment.NewLine);



            //netManager.




            // -- Password pass = new Password();
            // -- pass.ShowDialog();
            // -- string password = pass.textBox1.Text;
            //LogTextBox.Text += ("This is working" + Environment.NewLine);
            // -- string ConnectionString = ReadFromFile();
            // -- if (ConnectionString != null)
            // -- {
            // --     try
            // --     {
                    //pRdpViewer.OnAttendeeConnected += new _IRDPSessionEvents_OnAttendeeConnectedEventHandler(OnAttendeeConnected);
                    //pRdpViewer.OnAttendeeDisconnected += new _IRDPSessionEvents_OnAttendeeDisconnectedEventHandler(OnAttendeeDisconnected);
                    //pRdpViewer.OnAttendeeUpdate += new _IRDPSessionEvents_OnAttendeeUpdateEvent(OnAttendeeUpdate);
                    //pRdpViewer.Connect(ConnectionString, "Viewer1", "");
            // --         pRdpViewer.Connect(ConnectionString, Environment.MachineName, password);
            // --     }
            // --     catch (Exception ex)
            // --     {
                    //LogTextBox.Text += ("Error in Connecting. Error Info: " + ex.ToString() + Environment.NewLine);
            // --         LogTextBox.AppendText("Error in Connecting. Error Info: " + ex.ToString() + Environment.NewLine);
            // --     }
            // -- }
        }


        public void StartNetManager()
        {
            LogTextBox.AppendText("Attempting to establish a connection." + Environment.NewLine);


            NetManager propTest = new NetManager();

            Thread netManager = new Thread(new ThreadStart(propTest.RunExample));


            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("CtrlLvl", HandleIncomingCtrlLvlPacket);


            netManager.Start();


            LogTextBox.AppendText("Network thread started." + Environment.NewLine);


            EnterPassword();



        }




        public void EnterPassword()
        {
            //LogTextBox.AppendText("Opening password dialog box" + Environment.NewLine);
            string password = "2314";

            /*Password pass = new Password();
            pass.ShowDialog();
            string password = pass.textBox1.Text;

            if (pass.cancelButton == true)
            {
                NetManager propTest = new NetManager();
                propTest.netShutdown();

                return;
            }
            else
            {
                StartConnect(password);
            }*/
            //Thread.Sleep(2500);

            while (!File.Exists("inv.xml")) 
            {
                //LogTextBox.AppendText("Waiting for inv.xml to be received and written to file." + Environment.NewLine);
            }

            StartConnect(password);

        }


        public void StartConnect(string password)
        {
            string ConnectionString = ReadFromFile();
            if (ConnectionString != null)
            {
                try
                {
                    //pRdpViewer.OnAttendeeConnected += new _IRDPSessionEvents_OnAttendeeConnectedEventHandler(OnAttendeeConnected);
                    //pRdpViewer.OnAttendeeDisconnected += new _IRDPSessionEvents_OnAttendeeDisconnectedEventHandler(OnAttendeeDisconnected);
                    //pRdpViewer.OnAttendeeUpdate += new _IRDPSessionEvents_OnAttendeeUpdateEvent(OnAttendeeUpdate);
                    //pRdpViewer.Connect(ConnectionString, "Viewer1", "");
                    pRdpViewer.Connect(ConnectionString, Environment.MachineName, password);
                }
                catch (Exception ex)
                {
                    //LogTextBox.Text += ("Error in Connecting. Error Info: " + ex.ToString() + Environment.NewLine);
                    LogTextBox.AppendText("Error in Connecting. Error Info: " + ex.ToString() + Environment.NewLine);

                    NetManager propTest = new NetManager();
                    propTest.netShutdown();


                }
            }


        }





        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            //var netprogram = new NetProgram();
            //netprogram.NetPeerShutdown();
            pRdpViewer.Disconnect();
            //

            //ConnectButton.Enabled = true;

            //ControlButton.Enabled = false;


            //MessagesClass propTest = new MessagesClass();
            //propTest.netShutdown();
            //Close();
        }

        private string ReadFromFile()
        {
            string ReadText = null;
            string FileName = null;
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length == 2)
            {
                if (!args[1].EndsWith("inv.xml"))
                {
                    FileName = args[1] + @"\" + "inv.xml";
                }
                else
                {
                    FileName = args[1];
                }
            }
            else
            {
                FileName = "inv.xml";
            }

            //LogTextBox.Text += ("Reading the connection string from the file name " +
            //    FileName + Environment.NewLine);
            LogTextBox.AppendText("Reading the connection string from the file name " +
                FileName + Environment.NewLine);
            try
            {
                using (StreamReader sr = File.OpenText(FileName))
                {
                    ReadText = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                //LogTextBox.Text += ("Error in Reading input file. Error Info: " + ex.ToString() + Environment.NewLine);
                LogTextBox.AppendText("Error in Reading input file. Error Info: " + ex.ToString() + Environment.NewLine);
            }
            return ReadText;
        }

        private void OnConnectionEstablished(object sender, EventArgs e)
        {
            //LogTextBox.Text += "Connection Established" + Environment.NewLine;
            LogTextBox.AppendText("Connection Established" + Environment.NewLine);

            //ControlButton is kinda glitchy
            ControlButton.Enabled = true;
            ConnectButton.Enabled = false;

            viewerWidth();

            //pRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_VIEW);



            NetworkComms.AppendGlobalIncomingPacketHandler<string>("CtrlLvl", HandleIncomingCtrlLvlPacket);

            GetCtrlLvl();

            //pRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_VIEW);

            //Thread.Sleep(10000);
            //pRdpViewer.Size = new Size(1280, 720);
            //Form2 newMDIChild = new Form2();
            // Set the Parent Form of the Child window.
            //newMDIChild.MdiParent = this;
            // Display the new form.
            //newMDIChild.Show();
        }
        //_IRDPSessionEvents.OnControlLevelChangeResponse(object, RDPCOMAPILib.CTRL_LEVEL, int)
        //_IRDPSessionEvents_OnSharedDesktopSettingsChangedEventHandler(int width, int height, int colordepth)
        void OnControlLevelChangeResponse(object pAttendee, CTRL_LEVEL RequestedLevel, int ReasonCode)
        {



            LogTextBox.AppendText("Level Changed " + ReasonCode + Environment.NewLine);
            //   _IRDPSessionEvents_Event_OnControlLevelChangeResponse
        }


        private void OnError(object sender, _IRDPSessionEvents_OnErrorEvent e)
        {
            int ErrorCode = (int)e.errorInfo;
            //LogTextBox.Text += ("Error 0x" + ErrorCode.ToString("X") + Environment.NewLine);
            LogTextBox.AppendText("Error 0x" + ErrorCode.ToString("X") + Environment.NewLine);
        }

        private void OnConnectionTerminated(object sender, _IRDPSessionEvents_OnConnectionTerminatedEvent e)
        {//2308 == Presenter Ended Session
            //LogTextBox.Text += "Connection Terminated. Reason: " + e.discReason + Environment.NewLine;
            LogTextBox.AppendText("Connection Terminated. Reason: " + e.discReason + Environment.NewLine);
            //ControlButton.Enabled = false;
            //Close();

            
            ConnectButton.Enabled = true;
            DisconnectButton.Enabled = false;
            ControlButton.Enabled = false;


            NetManager propTest = new NetManager();

            //Thread netManager = new Thread(new ThreadStart(propTest.RunExample));

            //netManager.Start();


            propTest.netShutdown();


            //netManager.

            



        }


        private void ControlButton_Click(object sender, EventArgs e)
        {

            if (_controlAllowed == true)
            {

                _controlSwitch = !_controlSwitch;



                if (_controlSwitch == true)
                {
                    pRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE);
                }
                else
                {
                    pRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_VIEW);
                }
            }
        }


        private void OnConnectionFailed(object sender, EventArgs e)
        {
            //LogTextBox.Text += "Connection Failed." + Environment.NewLine;
            LogTextBox.AppendText("Connection Failed." + Environment.NewLine);
        }

        private void monitorSwitch_Click(object sender, EventArgs e)
        {


            UpdateMonitorPosition();

            /*int count;
            MonitorCounter(out count);
            //count = 5;
            //int monitor;
            int monitor1 = monitor + 1;
            monitor = monitor1;

            string mon = File.ReadLines("test11.xml").Skip(monitor-1).Take(monitor).First();
            string trimmed = mon.Trim('{', '}');


            //string newmon = mon.Substring(2, str.Length - 4);
            char[] delimiterChars = {','};

            string[] newmon = trimmed.Split(delimiterChars);

            LogTextBox.AppendText(newmon[0] + Environment.NewLine);
            LogTextBox.AppendText(newmon[1] + Environment.NewLine);
            LogTextBox.AppendText(newmon[2] + Environment.NewLine);
            LogTextBox.AppendText(newmon[3] + Environment.NewLine);

            TextWriter tw11 = File.CreateText("bounds.xml");
            tw11.WriteLine(newmon[0]);
            tw11.WriteLine(newmon[1]);
            tw11.WriteLine(newmon[2]);
            tw11.WriteLine(newmon[3]);
            tw11.Close();

            //TextWriter tw12 = File.CreateText("bounds.xml");
            string x = newmon[0].Substring(newmon[0].IndexOf('=') + 1);
            string y = newmon[1].Substring(newmon[1].IndexOf('=') + 1);
            string w = newmon[2].Substring(newmon[2].IndexOf('=') + 1);
            string h = newmon[3].Substring(newmon[3].IndexOf('=') + 1);
            //tw12.Close();
            
            if (monitor == count)
            {
                //int x = Width / 110;
                //pRdpViewer.Location = new Point(x, pRdpViewer.Location.Y);
                //monitor = 0;

                pRdpViewer.Location = new Point(Convert.ToInt32(x), Convert.ToInt32(y));

                //pRdpViewer.Location = new Point(pRdpViewer.Location.X + ((1920 / 3) * (count - 1)) + ((1920 / 3) * (count - 1)), pRdpViewer.Location.Y);
                monitor = 0;
                //LogTextBox.Text += monitor + Environment.NewLine;

            }
            else
            {
                pRdpViewer.Location = new Point(pRdpViewer.Location.X - 1920 / 3 - 1920 / 3, pRdpViewer.Location.Y);
            }
            */
            //_monitorSwitch = !_monitorSwitch;
            //if (_monitorSwitch == true)
            //{
            //    pRdpViewer.Location = new Point(pRdpViewer.Location.X - 1920/3 - 1920/3, pRdpViewer.Location.Y);
            //    
            //}
            //else
            //{
            //    pRdpViewer.Location = new Point(pRdpViewer.Location.X + 1920/3 + 1920/3, pRdpViewer.Location.Y);
            //}
            //int viewerheight = pRdpViewer.Height;
            //int viewerwidth = pRdpViewer.Width;
            //pRdpViewer.Width = viewerwidth * 2;
            //pRdpViewer.SmartSizing = false;
        }
        //private void Form_KeyDown(object sender, KeyEventArgs e)
        //{
        //   if (e.KeyCode == Keys.Escape)
        //   {
        //       //this.Close();
        //       ControlButton.PerformClick();
        //   }
        //}


        public void GetCtrlLvl()
        {

            MessagesClass propTest = new MessagesClass();

            propTest.sendMessages("GET_CTRL_LEVEL", "CtrlLvl");


        }
        



        private void HandleIncomingCtrlLvlPacket(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            //MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming CtrlLvl packet from " + connection.ToString() + " saying '" + incomingString + "'.");


            if (incomingString == "CTRL_LEVEL_NORMAL")
            {
                _controlSwitch = false;
                _controlAllowed = true;
                ControlButton.Enabled = true;
                pRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_VIEW);
            }
            else if (incomingString == "CTRL_LEVEL_VIEWONLY")
            {

                //ControlButton.Enabled = false;

                _controlSwitch = false;
                _controlAllowed = false;
                ControlButton.Enabled = false;
                pRdpViewer.RequestControl(RDPCOMAPILib.CTRL_LEVEL.CTRL_LEVEL_VIEW);
            }

        }

    }
}