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



        public void RunExample()
        {
            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);

            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("");


            MessagesClass propTest1 = new MessagesClass("defaultMessage", null, false, 0);

            Thread messagesClass = new Thread(new ThreadStart(propTest1.RunExample1));

            //messagesClass.Start();



            //NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();

            //Ensure the packet construction time is included in all sent packets
            //NetworkComms.DefaultSendReceiveOptions.IncludePacketConstructionTime = true;

            //Ensure all incoming packets are handled with the priority AboveNormal
            //NetworkComms.DefaultSendReceiveOptions.ReceiveHandlePriority = QueueItemPriority.AboveNormal;

            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", HandleIncomingMessagePacket);

            messagesClass.Start();

            //MessagesClass messagesClass = new MessagesClass();
            //messagesClass.RunExample1();





        }

        public static void HandleDecodeAddressRequest(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
            Console.WriteLine("\n  ... Incoming request from " + connection.ToString() + " saying '" + incomingString + "'.");


            string[] splitAddress = connection.ToString().Split('>');
            string halfDecodedAddress = splitAddress[1];
            string[] splitAddress1 = halfDecodedAddress.ToString().Split('(');
            string decodedAddress = splitAddress1[0];

            string peerAddress = decodedAddress.Replace(" ", "");


            TextWriter tw = File.CreateText("peerAddress.txt");
            tw.WriteLine(peerAddress);
            tw.Close();

            Console.WriteLine("decodeAddress");

            //string passMessage = "addressDecoded";
            //bool peerSend = true;
            //int peerExitCode = 0;

            propTest.newMessage("addressDecoded", true, 0);


        }
















        public static void HandleIncomingMessagePacket(PacketHeader header, Connection connection, string incomingString)
        {
            //NetManager propTest = new NetManager();
            MessagesClass propTest = new MessagesClass();
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

                Console.WriteLine("decodeAddress");

                //string passMessage = "addressDecoded";
                //bool peerSend = true;
                //int peerExitCode = 0;

                propTest.newMessage("addressDecoded", true, 0);

            }
            else if (incomingString == "fetch inv.xml")
            {
                //NetManager propTest = new NetManager();

                //string passMessage = "openFetchListener";
                //bool peerSend = true;
                //int peerExitCode = 0;

                Console.WriteLine("fetch inv.xml");

                propTest.newMessage("openFetchListener", true, 0);


            }
            else if (incomingString == "fetchListenerOpened")
            {
                string passMessage = File.ReadLines("inv.xml").First();
                //bool peerSend = true;
                //int peerExitCode = 0;

                Console.WriteLine("fetchListenerOpened");

                propTest.newMessage(passMessage, true, 0);

            }
            else if (incomingString == "fetchListenerClosed")
            {
                Console.WriteLine("fetchListenerClosed");
            }

        }

    }

}



