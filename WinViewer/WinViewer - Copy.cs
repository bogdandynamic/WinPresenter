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
            //Form2 secondForm = new Form2();
            InitializeComponent();
        }
        //public virtual int exitCode { get; set; }
        //public virtual string peerAddress { get; set; }
        //public virtual string peerMessage { get; set; }
        //public virtual bool sendMessage { get; set; }

        public string peerMessage
        {
            get { return peerMessage; }
            set { peerMessage = value; }
        }

        //public string peerAddress
        //{
        //    get { return peerAddress; }
        //    set { peerAddress = value; }
        //}

        public bool sendMessage
        {
            get { return sendMessage; }
            set { sendMessage = value; }
        }

        public int exitCode
        {
            get { return exitCode; }
            set { exitCode = value; }
        }

        public bool _monitorSwitch = false;
        public bool _controlSwitch = false;
        public int monitor = 0;
        //MessagesClass messagesClass = new MessagesClass();
        //private BackgroundWorker bw = new BackgroundWorker();
        public static void MonitorCounter(out int count)
        {
            int counter = System.Windows.Forms.SystemInformation.MonitorCount;
            //int count;
            count = counter;
            //return count.ToString;
            //LogTextBox.Text += (count + Environment.NewLine);
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
            LogTextBox.AppendText("Window size: " + pRdpViewer.Width + "x" + pRdpViewer.Height + Environment.NewLine);
            pRdpViewer.Width = viewerwidth * count;
            int locationx = Width / 110;
            pRdpViewer.Location = new Point(locationx, pRdpViewer.Location.Y);
            //pRdpViewer.Height = viewerheight - viewerheight/1;
            pRdpViewer.SmartSizing = true;
            //bool monitorSwitch = true;
            //pRdpViewer.OcxState;
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


            string peerIP = serverInfo.IPv4Address_Box.Text;
            string peerPort = serverInfo.textBox1.Text;

            string peerAddress = peerIP + ":" + peerPort;

            LogTextBox.AppendText("Address: " + peerAddress + Environment.NewLine);

            //SSLExample serverConnect = new SSLExample();

            LogTextBox.AppendText("Attempting to establish a connection." + Environment.NewLine);

            //peerMessage("This is a test", peerAddress);

            //var netManager = new SSLExample();
            var netManager = new NetManager();
            //netManager.peerMessages();


            IPAddressSplitter(peerIPAddressAndPort);


            exitCode = 1;
            peerMessage = "Connection test 1";
            sendMessage = true;

            netManager.RunExample();

            //Thread.Sleep(500);

            //var messageClass = new MessagesClass();


            //messageClass.peerMessages();

            //Thread networkManager = new Thread(netManager.RunExample);
            //Thread networkManager = new Thread(new ThreadStart(netManager.RunExample));
            //netManager.SetApartmentState(ApartmentState.STA);
            //networkManager.Start();
            //Task.Factory.StartNew(netManager.RunExample);

            //new Thread(new ThreadStart(netManager.RunExample)).Start();

            //BeginInvoke(netManager.RunExample());
            //BackgroundWorker worker = sender as BackgroundWorker;


            //NetPeerConfiguration config = new NetPeerConfiguration("WJB_RemoteDesktop_Application");
            //NetClient myClient = new NetClient(config);
            //myClient.DiscoverLocalPeers(14242);
            //myClient.Connect("WJB_RemoteDesktop_Application", 14242);

            //NetIncomingMessage inc; while ((inc = myClient.ReadMessage()) != null)
            //{
            //    switch (inc.MessageType)
            //    {
            //        case NetIncomingMessageType.DiscoveryResponse:
            //            LogTextBox.AppendText("Found server at " + inc.SenderEndPoint + " name: " + inc.ReadString() + Environment.NewLine);
            //            //Console.WriteLine("Found server at " + inc.SenderEndPoint + " name: " + inc.ReadString());
            //            break;
            //    }
            //}

            Password pass = new Password();
            pass.ShowDialog();
            string password = pass.textBox1.Text;
            //LogTextBox.Text += ("This is working" + Environment.NewLine);
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
                }
            }
        }

        
        //public void peerMessage(string peerMessage, string peerAddress)
        //{

            
            //SSLExample serverConnect = new SSLExample();


            //var netManager = new SSLExample();

            //string passAddress = peerAddress;
            //string passMessage = peerMessage;
            //bool sendMessage = true;

            //string peerAddress1 = peerAddress

            //netManager.RunExample(passAddress, passMessage, sendMessage, sendExit);

            //netManager.RunExample

            //string peerExit = "exit";
            
            //netManager.RunExample(passAddress, peerExit, sendMessage, sendExit);

        //}


        public void IPAddressSplitter(string address)
        {

            string ip = address;
            string[] values = ip.Split('.', ':');


            peerAddress = values;




        }

        public void IPAddressCombiner(string[] address, out string[] peerIP, out string peerPort)
        {


            string test = address.Last();
            string[] test2;
            test2 = address.Take(address.Count() - 1).ToArray();

            //string[] ip = address;

            //string value = string.Join(".", test2);


            peerIP = test2;

            peerPort = test;



            //peerAddress = value;




        }




        public void peerUpdate(string newMessage, string[] newAddress, bool newSend, int newExitCode)
        {
            
            peerMessage = newMessage;
            peerAddress = newAddress;
            sendMessage = newSend;
            exitCode = newExitCode;


        }


        public void peerReturn(out string passMessage, out string[] passAddress, out bool passSend, out int peerExitCode)
        {
            //var netManager = new SSLExample();



            passMessage = peerMessage;
            passAddress = peerAddress;
            passSend = sendMessage;


            peerExitCode = exitCode;




        }
        








        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            //var netprogram = new NetProgram();
            //netprogram.NetPeerShutdown();
            pRdpViewer.Disconnect();
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
            ControlButton.Enabled = true;
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
            ControlButton.Enabled = false;
            //Close();
        }


        private void ControlButton_Click(object sender, EventArgs e)
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

        private void OnConnectionFailed(object sender, EventArgs e)
        {
            //LogTextBox.Text += "Connection Failed." + Environment.NewLine;
            LogTextBox.AppendText("Connection Failed." + Environment.NewLine);
        }

        private void monitorSwitch_Click(object sender, EventArgs e)
        {
            int count;
            MonitorCounter(out count);
            //count = 5;
            //int monitor;
            int monitor1 = monitor + 1;
            monitor = monitor1;



            if (monitor == count)
            {
                //int x = Width / 110;
                //pRdpViewer.Location = new Point(x, pRdpViewer.Location.Y);
                //monitor = 0;

                pRdpViewer.Location = new Point(pRdpViewer.Location.X + ((1920 / 3)*(count-1)) + ((1920 / 3)*(count-1)), pRdpViewer.Location.Y);
                monitor = 0;
                //LogTextBox.Text += monitor + Environment.NewLine;

            }
            else
            {
                pRdpViewer.Location = new Point(pRdpViewer.Location.X - 1920 / 3 - 1920 / 3, pRdpViewer.Location.Y);
            }

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
        



    }
}