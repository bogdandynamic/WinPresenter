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

        public NetManager()
        {

        }

        public NetManager(string passMessage, string passAddress, bool peerSend, int peerExitCode)
        {
            //MessagesClass messagesClass = new MessagesClass();
            string peerMessage = passMessage;
            string peerAddress = passAddress;
            bool sendMessage = peerSend;
            int exitCode = peerExitCode;
            //messagesClass.me

            MessagesClass messagesClass = new MessagesClass(peerMessage, peerAddress, sendMessage, exitCode);
            //messagesClass.
        }

        /// <summary>
        /// The handler that we wish to execute when we receive a message packet.
        /// </summary>
        /// <param name="header">The associated packet header.</param>
        /// <param name="connection">The connection used for the incoming packet</param>
        /// <param name="incomingString">The incoming data converted to a string</param>
        /// 

        public void runMessagesClass()
        {
            MessagesClass messagesClass = new MessagesClass();
            messagesClass.RunExample();
        }

        public void runNetShutdown()
        {
            MessagesClass messagesClass = new MessagesClass();
            messagesClass.netShutdown();
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



