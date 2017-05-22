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
using System.Threading;

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

        /// <summary>
        /// Run example
        /// </summary>
        /// 

        public NetManager()
        {

        }

        //public NetManager(string passMessage, string passAddress, bool peerSend, int peerExitCode)
        //{
        //    peerMessage = passMessage;
        //    peerAddress = passAddress;
        //    sendMessage = peerSend;
        //    exitCode = peerExitCode;
        //}

        public void netShutdown()
        {


            MessagesClass netClose = new MessagesClass();


            netClose.netShutdown1();


            NetworkComms.RemoveGlobalIncomingPacketHandler();


            //exitCode = 1;
            NetworkComms.Shutdown();

        }

        public void RunExample()
        {
            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);

            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("");


            //MessagesClass propTest1 = new MessagesClass("defaultMessage", null, false, 0);
            MessagesClass propTest1 = new MessagesClass();

            Thread messagesClass = new Thread(new ThreadStart(propTest1.RunExample1));

            //messagesClass.Start();



            //NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();

            //Ensure the packet construction time is included in all sent packets
            //NetworkComms.DefaultSendReceiveOptions.IncludePacketConstructionTime = true;

            //Ensure all incoming packets are handled with the priority AboveNormal
            //NetworkComms.DefaultSendReceiveOptions.ReceiveHandlePriority = QueueItemPriority.AboveNormal;


            NetworkComms.AppendGlobalIncomingPacketHandler<string>("DecAdd", HandleIncomingPacket_DecAdd);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("XMLFetch", HandleIncomingPacket_XMLFetch);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("MONFetch", HandleIncomingPacket_MONFetch);

            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);



            messagesClass.Start();

            //MessagesClass messagesClass = new MessagesClass();
            //messagesClass.RunExample1();





        }


        public void HandleIncomingPacket_DecAdd(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");

            string[] splitAddress = connection.ToString().Split('>');
            string halfDecodedAddress = splitAddress[1];
            string[] splitAddress1 = halfDecodedAddress.ToString().Split('(');
            string decodedAddress = splitAddress1[0];
            string peerAddress = decodedAddress.Replace(" ", "");

            TextWriter tw = File.CreateText("peerAddress.txt");
            tw.WriteLine(peerAddress);
            tw.Close();

            Console.WriteLine("decodeAddress");
            propTest.sendMessages("", "DecAddFin");
        }

        public void HandleIncomingPacket_XMLFetch(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");

            string passMessage = File.ReadLines("inv.xml").First();
            propTest.sendMessages(passMessage, "XMLInfo");
        }



        public void HandleIncomingPacket_MONFetch(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");

            string monPos = System.Windows.Forms.Screen.AllScreens.ToString();

            //TextWriter tw = File.CreateText("monPos.txt");
            //tw.WriteLine(monPos);

            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                //tw2.WriteLine(screen.Bounds.ToString());
                monPos = screen.Bounds.ToString();
                propTest.sendMessages(monPos, "MONPos");

            }

            //tw.Close();

        }


    }

}