/* All content in this sample is ”AS IS” with with no warranties, and confer no rights. 
 * Any code on this blog is subject to the terms specified at http://www.microsoft.com/info/cpyright.mspx. 
 */

using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RDPCOMAPILib;
using System.Net.Sockets;
using System.Threading;
using Examples.ExamplesConsole;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet;

namespace WinSharer
{
    public partial class WinSharer : Form
    {
        public WinSharer()
        {
            InitializeComponent();
        }
        //public bool _checkSwitch = true;
        public bool _networkConnection = false;
        public bool _netmanagerstarted = false;
        const int ERROR_SHARING_VIOLATION = 32;
        const int ERROR_LOCK_VIOLATION = 33;
        private bool IsFileLocked(string file)
        {
            //check that problem is not in destination file
            if (File.Exists(file) == true)
            {
                FileStream stream = null;
                try
                {
                    stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (Exception ex2)
                {
                    //_log.WriteLog(ex2, "Error in checking whether file is locked " + file);
                    int errorCode = Marshal.GetHRForException(ex2) & ((1 << 16) - 1);
                    if ((ex2 is IOException) && (errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION))
                    {
                        return true;
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            return false;
        }


        private bool IsFileEmpty(string file)
        {
            if (new FileInfo(file).Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        void OnAttendeeDisconnected(object pDisconnectInfo)
        {
            IRDPSRAPIAttendeeDisconnectInfo pDiscInfo = pDisconnectInfo as IRDPSRAPIAttendeeDisconnectInfo;
            textBox2.AppendText(Environment.NewLine);
            textBox1.AppendText(Environment.NewLine + "Disconnected");
            //LogTextBox.Text += ("Attendee Disconnected: " + pDiscInfo.Attendee.RemoteName + Environment.NewLine);
            LogTextBox.AppendText("Attendee Disconnected: " + pDiscInfo.Attendee.RemoteName + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            StartButton.Enabled = false;

            try
            {
                m_pRdpSession = new RDPSession();

                m_pRdpSession.OnAttendeeConnected += new _IRDPSessionEvents_OnAttendeeConnectedEventHandler(OnAttendeeConnected);
                m_pRdpSession.OnAttendeeDisconnected += new _IRDPSessionEvents_OnAttendeeDisconnectedEventHandler(OnAttendeeDisconnected);
                m_pRdpSession.OnControlLevelChangeRequest += new _IRDPSessionEvents_OnControlLevelChangeRequestEventHandler(OnControlLevelChangeRequest);
                //m_pRdpSession
                m_pRdpSession.Open();
                IRDPSRAPIInvitation pInvitation = m_pRdpSession.Invitations.CreateInvitation("WJB_RemoteDesktop_Application","LAN_Network","2314",1);

                //use port 14242


                //NetManager.EstablishConnection();

                //NetManager.ReceiveConnection();


                //textBox2.AppendText(m_pRdpSession.Attendees.ToString() + Environment.NewLine);
                string invitationString = pInvitation.ConnectionString;
                WriteToFile(invitationString);
                //LogTextBox.Text += "Presentation Started. Your Desktop is being shared." + Environment.NewLine;
                //LogTextBox.AppendText("Presentation Started. Your Desktop is being shared." + Environment.NewLine);
                LogTextBox.AppendText("Starting . . ." + Environment.NewLine);

                //NetManager propTest = new NetManager("defaultMessage", null, false, 0);
                NetManager propTest = new NetManager();

                Thread netManager = new Thread(new ThreadStart(propTest.RunExample));

                netManager.Start();

                //Thread.Sleep(2500);

                while (!File.Exists("listeners.xml"))
                {
                    //LogTextBox.AppendText("Waiting for inv.xml to be written to file." + Environment.NewLine);
                }

                /*while (IsFileLocked("listeners.xml") == true)
                {
                    LogTextBox.AppendText(IsFileLocked("listeners.xml") + Environment.NewLine);
                }*/

                while (IsFileEmpty("listeners.xml") == true)
                {
                    //wait
                }
               

                //IsFileLocked("listeners.xml");

                //LogTextBox.AppendText(IsFileLocked("listeners.xml") + Environment.NewLine);

                //Thread.Sleep(5000);

                LogTextBox.AppendText("Presentation Started. Your Desktop is ready to be shared." + Environment.NewLine);

                string[] listeners = File.ReadAllLines("listeners.xml");

                foreach (var item in listeners)
                {
                    //Console.WriteLine(item.ToString());
                    LogTextBox.AppendText(item + Environment.NewLine);

                }

                StopButton.Enabled = true;

            }
            catch (Exception ex)
            {
                //LogTextBox.Text += "Error occured while starting presentation. Error: " + ex.ToString() + Environment.NewLine;
                LogTextBox.AppendText("Error occured while starting presentation. Error: " + ex.ToString() + Environment.NewLine);
            }
        }



        void OnControlLevelChangeRequest(object pObjAttendee, CTRL_LEVEL RequestedLevel)
        {
            IRDPSRAPIAttendee pAttendee = pObjAttendee as IRDPSRAPIAttendee;
            //pAttendee.ControlLevel = RequestedLevel;
            //textBox1.AppendText(RequestedLevel.ToString() + Environment.NewLine);
            //textBox1.AppendText(Environment.NewLine + RequestedLevel.ToString());
            //BinaryWriter
            //System.Net.Sockets.SendPacketsElement test = new S;
            //System.Net.Sockets.NetworkStream Test;
            //IRDPSRAPITransportStream.WriteBuffer.ToString();
            //IRDPSRAPITransportStream.WriteBuffer(RDPCOMAPILib.RDPTransportStreamBuffer)
            //if (_checkSwitch == true)
            if (checkBox1.Checked == true)
            {
                pAttendee.ControlLevel = RequestedLevel;
                textBox1.AppendText(Environment.NewLine + RequestedLevel.ToString());
            }
            else
            {
                pAttendee.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_VIEW;
                textBox1.AppendText(Environment.NewLine + RequestedLevel.ToString());
            }
        }

        protected RDPSession m_pRdpSession = null;

        private void StopButton_Click(object sender, EventArgs e)
        {

            StartButton.Enabled = true;
            StopButton.Enabled = false;

            try
            {
                MessagesClass propTest = new MessagesClass();
                propTest.netShutdown();
                m_pRdpSession.Close();
                //LogTextBox.Text += "Presentation Stopped." + Environment.NewLine;
                LogTextBox.AppendText("Presentation Stopped." + Environment.NewLine);
                Marshal.ReleaseComObject(m_pRdpSession);
                m_pRdpSession = null;
            }
            catch (Exception ex)
            {
                //LogTextBox.Text += "Error occured while stopping presentation. Error: " + ex.ToString();
                LogTextBox.AppendText("Error occured while stopping presentation. Error: " + ex.ToString() + Environment.NewLine);

                StartButton.Enabled = false;
                StopButton.Enabled = false;
                checkBox1.Enabled = false;

                LogTextBox.AppendText("Please exit the program." + Environment.NewLine);

            }



        }

        private void OnAttendeeConnected(object pObjAttendee)
        {

            _networkConnection = true;
            //textBox2.AppendText(Environment.NewLine + pObjAttendee.ToString());
            IRDPSRAPIAttendee pAttendee = pObjAttendee as IRDPSRAPIAttendee;


            //MessagesClass propTest = new MessagesClass();



            //OnControlLevelChangeRequest(pObjAttendee, CTRL_LEVEL.CTRL_LEVEL_VIEW);

            //if (_networkConnection == true)
            //{


            NetworkComms.AppendGlobalIncomingPacketHandler<string>("CtrlLvl", HandleIncomingCtrlLvlPacket);



            //if (_checkSwitch == false)
            //    {
            //        OnControlLevelChangeRequest(pObjAttendee, CTRL_LEVEL.CTRL_LEVEL_VIEW);
                    //propTest.sendMessages("CTRL_LEVEL_VIEWONLY");
                    //propTest.sendMessages("CTRL_LEVEL_NORMAL");

            //    }
                //else
                //{

                    //MessagesClass propTest = new MessagesClass();

                //    propTest.sendMessages("CTRL_LEVEL_VIEWONLY");


                //}

            //}




            //pAttendee.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_VIEW;
            textBox1.AppendText(Environment.NewLine + pAttendee.ControlLevel.ToString());
            textBox2.AppendText(pAttendee.RemoteName + Environment.NewLine);
            //textBox2.AppendText(Environment.NewLine + pAttendee.RemoteName..ConnectivityInfo.ToString());
            //LogTextBox.Text += ("Attendee Connected: " + pAttendee.RemoteName + Environment.NewLine);
            LogTextBox.AppendText("Attendee Connected: " + pAttendee.RemoteName + Environment.NewLine);
        }

        public void WriteToFile(string InviteString)
        {
            using (StreamWriter sw = File.CreateText("inv.xml"))
            {
                sw.WriteLine (InviteString);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //_checkSwitch = !_checkSwitch;

            MessagesClass propTest = new MessagesClass();

            if (_networkConnection == true)
            {
                //if (_checkSwitch == true)
                if (checkBox1.Checked == true)
                {

                    propTest.sendMessages("CTRL_LEVEL_NORMAL", "CtrlLvl");

                }
                else
                {

                    //MessagesClass propTest = new MessagesClass();

                    propTest.sendMessages("CTRL_LEVEL_VIEWONLY", "CtrlLvl");


                }

            }




        }


        //public static int SendReceiveTest1(Socket client)
        //{
        //    Socket socketSender = new Socket(AddressFamily.InterNetwork,
        //        SocketType.Stream, ProtocolType.Tcp);
        //    byte[] msg = System.Text.Encoding.ASCII.GetBytes("This is a test");
        //    int bytesSent = socketSender.Send(msg);
        //
        //    return 0;
        //}


        public void HandleIncomingCtrlLvlPacket(PacketHeader header, Connection connection, string incomingString)
        {
            /*if (checkBox1.Checked == true)
            {

            }*/
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming CtrlLvl packet from " + connection.ToString() + " saying '" + incomingString + "'.");


            if (incomingString == "GET_CTRL_LEVEL")
            {
                //if (_checkSwitch == true)
                if (checkBox1.Checked == true)
                {

                    propTest.sendMessages("CTRL_LEVEL_NORMAL", "CtrlLvl");

                }
                else
                {

                    //MessagesClass propTest = new MessagesClass();

                    propTest.sendMessages("CTRL_LEVEL_VIEWONLY", "CtrlLvl");


                }



            }


        }


    }
}