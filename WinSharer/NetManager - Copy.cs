//
//  Copyright 2009-2014 NetworkComms.Net Ltd.
//
//  This source code is made available for reference purposes only.
//  It may not be distributed and it may not be made publicly available.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using NetworkCommsDotNet;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using NetworkCommsDotNet.DPSBase.SevenZipLZMACompressor;
using NetworkCommsDotNet.Connections.TCP;
using WinSharer;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace Examples.ExamplesConsole
{
    /// <summary>
    /// IntermediateSend demonstrates how to send and receive primitive objects (ints, strings etc).  
    /// This example aims to bridge the gap between the relatively simple BasicSend and much more
    /// extensive AdvancedSend
    /// </summary>
    public partial class NetManager
    {
        //private int exitCode { get; set; }
        //private string peerAddress { get; set; }
        //private string peerMessage { get; set; }
        //private bool sendMessage { get; set; }
        private int exitCode;
        private string peerAddress;
        private string peerMessage;
        private bool sendMessage;
        /// <summary>
        /// Run example
        /// </summary>
        /// 


        public string _peerAddress
        {
            get
            {
                peerAddress = File.ReadLines("peerAddress.txt").First();

                return peerAddress;
            }

        }


        public NetManager()
        {

        }

        public NetManager(string passMessage, string passAddress, bool peerSend, int peerExitCode)
        {
            peerMessage = passMessage;
            peerAddress = passAddress;
            sendMessage = peerSend;
            exitCode = peerExitCode;
        }

        public void newMessage(string passMessage, bool peerSend, int peerExitCode)
        {
            peerMessage = passMessage;
            sendMessage = peerSend;
            exitCode = peerExitCode;




        }


        public void netShutdown()
        {
            exitCode = 1;
            //NetworkComms.Shutdown();

        }


        public void RunExample()
        {
            Console.WriteLine("IntermediateSend Example ...\n");

            //Set the default send receive options to use for all communication
            //
            //Serializers convert custom objects into byte[] which is required for transmission
            //  - Here we have selected the ProtobufSerializer. For more please see the AdvancedSend example.
            //
            //Data processors manipulate the raw byte[] of an object, some encrypt, some compress etc etc.
            //  - Here we have selected a single data processor which will compress data, the LZMACompressor
            //       For more please see the AdvancedSend example.
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();

            //Ensure the packet construction time is included in all sent packets
            NetworkComms.DefaultSendReceiveOptions.IncludePacketConstructionTime = true;

            //Ensure all incoming packets are handled with the priority AboveNormal
            NetworkComms.DefaultSendReceiveOptions.ReceiveHandlePriority = QueueItemPriority.AboveNormal;

            //We need to define what happens when packets are received.
            //To do this we add an incoming packet handler for a 'Message' packet type. 
            //You are free to choose your own packet types.
            //
            //This handler will expect the incoming raw bytes to be converted to a string (this is what the <string> bit means).
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);

            //Start listening for incoming 'TCP' connections.
            //We want to select a random port on all available adaptors so provide 
            //an IPEndPoint using IPAddress.Any and port 0.
            //See also Connection.StartListening(ConnectionType.UDP, IPEndPoint)
            Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 0));

            TextWriter tw = File.CreateText("listeners.xml");
            //Print the IP addresses and ports we are listening on to make sure everything
            //worked as expected.
            //Console.WriteLine("Listening for TCP messages on:");
            //foreach (IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
            //    Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);
            tw.WriteLine("Listening for TCP messages on:");
            foreach (IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                tw.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            //TextWriter tw = File.CreateText("inv.xml");
            //tw.WriteLine(incomingString);
            tw.Close();


            //var winViewer = new WinViewer.WinViewer();
            //MessagesClass propTest = new MessagesClass();
            //NetManager propTest = new NetManager();

            Console.WriteLine(_peerAddress + Environment.NewLine);
            Console.WriteLine(peerMessage + Environment.NewLine);
            Console.WriteLine(sendMessage + Environment.NewLine);
            Console.WriteLine(exitCode + Environment.NewLine);
            var winSharer = new WinSharer.WinSharer();
            //We loop here to allow any number of test messages to be sent and received
            while (true)
            {
                //winViewer.peerReturn(out peerMessage, out peerAddress, out sendMessage, out exitCode);





                //Request a message to send somewhere
                //Console.WriteLine("\nPlease enter your message and press enter (Type 'exit' to quit):");
                //string stringToSend = Console.ReadLine();
                //Console.WriteLine("Message to send: " + propTest.peerMessage + " to " + propTest.peerAddress);
                //Console.WriteLine("Waiting...");

                string stringToSend = peerMessage;

                //If the user has typed exit then we leave our loop and end the example
                if (stringToSend == "exit" || exitCode == 1)
                {
                    break;
                }
                else if (sendMessage == true)
                {
                    Console.WriteLine("Message to send: " + peerMessage + " to " + _peerAddress);
                    winSharer.LogTextBox.AppendText("Message to send: " + peerMessage + " to " + _peerAddress);
                    sendMessage = false;
                    try
                    {
                        //Once we have a message we need to know where to send it
                        //We have created a small wrapper class to help keep things clean here
                        //ConnectionInfo targetServerConnectionInfo = ExampleHelper.GetServerDetails();
                        IPEndPoint lastServerIPEndPoint = IPTools.ParseEndPointFromString(_peerAddress);
                        ApplicationLayerProtocolStatus applicationLayerProtocol = ApplicationLayerProtocolStatus.Enabled;
                        ConnectionInfo targetServerConnectionInfo = new ConnectionInfo(lastServerIPEndPoint, applicationLayerProtocol);

                        //We get a connection to the desired target server
                        //This is performed using a static method, i.e. 'TCPConnection.GetConnection()' instead of
                        //using 'new TCPConnection()' to ensure thread safety. This means if you have a multi threaded application
                        //and attempt to get a connection the same target server simultaneously, you will only ever create
                        //a single connection.
                        Connection conn = TCPConnection.GetConnection(targetServerConnectionInfo);

                        //We send the string using a 'Message' packet type
                        //There are a large number of overrides to SendObject
                        //Please see our other examples or the online API
                        //http://www.networkcomms.net/api/
                        conn.SendObject("Message", stringToSend);
                    }
                    catch (CommsException ex)
                    {
                        //All NetworkComms.Net exception inherit from CommsException so we can easily
                        //catch all just by catching CommsException. For the break down of exceptions please 
                        //see our online API.
                        //http://www.networkcomms.net/api/

                        //If an error occurs we need to decide what to do.
                        //In this example we will just log to a file and continue.
                        LogTools.LogException(ex, "IntermediateSendExampleError");
                        Console.WriteLine("\nError: CommsException was caught. Please see the log file created for more information.\n");
                    }
                }
            }

            //We should always call shutdown on NetworkComms.Net if we have used it
            NetworkComms.Shutdown();
        }

        /// <summary>
        /// The handler that we wish to execute when we receive a message packet.
        /// </summary>
        /// <param name="header">The associated packet header.</param>
        /// <param name="connection">The connection used for the incoming packet</param>
        /// <param name="incomingString">The incoming data converted to a string</param>
        /// 
        private static readonly IDictionary<int, Action<object>> processor =
            new Dictionary<int, Action<object>>
                {
                    { 201, Process201 }
                ,   { 205, Process205 }
                };



        private static void HandleIncomingMessagePacket(PacketHeader header, Connection connection, string incomingString)
        {
            NetManager propTest = new NetManager();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");


            if (incomingString == "decodeAddress")
            {

                string[] splitAddress = connection.ToString().Split('>');
                string halfDecodedAddress = splitAddress[1];
                string[] splitAddress1 = halfDecodedAddress.ToString().Split('(');
                string decodedAddress = splitAddress1[0];

                string peerAddress = decodedAddress.Replace(" ", "");


                TextWriter tw = File.CreateText("peerAddress.txt");
                tw.WriteLine(peerAddress);
                tw.Close();


                string passMessage = "addressDecoded";
                bool peerSend = true;
                int peerExitCode = 0;

                propTest.newMessage(passMessage, peerSend, peerExitCode);

            }
            else if (incomingString == "fetch inv.xml")
            {
                //NetManager propTest = new NetManager();

                string passMessage = "openFetchListener";
                bool peerSend = true;
                int peerExitCode = 0;

                propTest.newMessage(passMessage, peerSend, peerExitCode);


            }
            else if (incomingString == "fetchListenerOpened")
            {
                string passMessage = File.ReadLines("inv.xml").First();
                bool peerSend = true;
                int peerExitCode = 0;

                propTest.newMessage(passMessage, peerSend, peerExitCode);

            }
            else if (incomingString == "fetchListenerClosed")
            {

            }
        }

        public static void decodeAddress(string connection)
        {
            NetManager propTest = new NetManager();
            string[] splitAddress = connection.ToString().Split('>');
            string halfDecodedAddress = splitAddress[1];
            string[] splitAddress1 = halfDecodedAddress.ToString().Split('(');
            string decodedAddress = splitAddress1[0];

            string peerAddress = decodedAddress.Replace(" ", "");


            TextWriter tw = File.CreateText("peerAddress.txt");
            tw.WriteLine(peerAddress);
            tw.Close();


            string passMessage = "addressDecoded";
            bool peerSend = true;
            int peerExitCode = 0;

            propTest.newMessage(passMessage, peerSend, peerExitCode);
        }

        public static void fetchInvXml()
        {
            NetManager propTest = new NetManager();
            //NetManager propTest = new NetManager();

            string passMessage = "openFetchListener";
            bool peerSend = true;
            int peerExitCode = 0;

            propTest.newMessage(passMessage, peerSend, peerExitCode);
        }

        public static void fetchListenerOpened()
        {
            NetManager propTest = new NetManager();
            string passMessage = File.ReadLines("inv.xml").First();
            bool peerSend = true;
            int peerExitCode = 0;

            propTest.newMessage(passMessage, peerSend, peerExitCode);
        }

        public static void fetchListenerClosed()
        {

        }

    }
}


