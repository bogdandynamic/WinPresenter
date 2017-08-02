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
//using WinViewer;
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
        //public bool fetchListener;
        //public bool resolutionListener;
        //public bool monitorCountListener;


        /// <summary>
        /// Run example
        /// </summary>
        /// 

        public NetManager()
        {

        }


        public void netShutdown()
        {


            MessagesClass netClose = new MessagesClass();


            netClose.netShutdown1();


            NetworkComms.RemoveGlobalIncomingPacketHandler();


            //exitCode = 1;
            NetworkComms.Shutdown();

        }



        //public NetManager(string passMessage, string passAddress, bool peerSend, int peerExitCode)
        //{
        //    peerMessage = passMessage;
        //    peerAddress = passAddress;
        //    sendMessage = peerSend;
        //    exitCode = peerExitCode;
        //}



        public void RunExample()
        {
            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);

            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("");
            
            //MessagesClass propTest1 = new MessagesClass("defaultMessage", null, false, 0);

            MessagesClass propTest = new MessagesClass();
            //propTest1 = null;

            Thread messagesClass = new Thread(new ThreadStart(propTest.RunExample1));

            //messagesClass.Start();


            //TextWriter tw = File.CreateText("test.txt");
            //tw.WriteLine(propTest1);
            //tw.WriteLine(messagesClass);
            //tw.Close();




            //NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();

            //Ensure the packet construction time is included in all sent packets
            //NetworkComms.DefaultSendReceiveOptions.IncludePacketConstructionTime = true;

            //Ensure all incoming packets are handled with the priority AboveNormal
            //NetworkComms.DefaultSendReceiveOptions.ReceiveHandlePriority = QueueItemPriority.AboveNormal;


            NetworkComms.AppendGlobalIncomingPacketHandler<string>("DecAddFin", HandleIncomingPacket_DecAddFin);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("XMLInfo", HandleIncomingPacket_XMLInfo);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("MONPos", HandleIncomingPacket_MONPos);


            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);

            //NetworkComms.RemoveGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);

            messagesClass.Start();

            //MessagesClass messagesClass = new MessagesClass();
            //messagesClass.RunExample1();

            //propTest1 = null;


        }

        public void HandleIncomingPacket_DecAddFin(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");


            TextWriter twMonPos = File.CreateText("monPosition.xml");
            twMonPos.Close();

            propTest.sendMessages("", "XMLFetch");
            
        }

        public void HandleIncomingPacket_XMLInfo(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");

            /*TextWriter tw = File.CreateText("inv.xml");
            tw.WriteLine(incomingString);
            tw.Close();*/

            using (StreamWriter swXMLInfo = new StreamWriter("inv.xml", true))
            {
                swXMLInfo.WriteLine(incomingString);
            }

            propTest.sendMessages("", "MONFetch");
        }

        //TextWriter twMonPos = File.CreateText("monPosition.xml");

        public void HandleIncomingPacket_MONPos(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");

            using (StreamWriter swMonPos = new StreamWriter("monPosition.xml", true))
            {
                swMonPos.WriteLine(incomingString);
            }



            //TextWriter twMonPos = File.CreateText("monPosition.xml");

            //twMonPos.WriteLine(incomingString);

            //propTest.sendMessages("", "ScrnDims");

            //tw.WriteLine(incomingString);
            //tw.Close();
        }

        /*public void HandleIncomingPacket_ScrnDims(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            //MessagesClass propTest = new MessagesClass();
            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");

            using (StreamWriter swMonPos = new StreamWriter("monPosition.xml", true))
            {
                swMonPos.WriteLine(incomingString);
            }



            //TextWriter twMonPos = File.CreateText("monPosition.xml");

            //twMonPos.WriteLine(incomingString);


            //tw.WriteLine(incomingString);
            //tw.Close();
        }*/


    }

}
