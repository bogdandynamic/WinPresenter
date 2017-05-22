﻿//
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

namespace Examples.ExamplesConsole
{
    /// <summary>
    /// IntermediateSend demonstrates how to send and receive primitive objects (ints, strings etc).  
    /// This example aims to bridge the gap between the relatively simple BasicSend and much more
    /// extensive AdvancedSend
    /// </summary>
    public partial class NetManager
    {
        /// <summary>
        /// Run example
        /// </summary>
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

            //Print the IP addresses and ports we are listening on to make sure everything
            //worked as expected.
            Console.WriteLine("Listening for TCP messages on:");
            foreach (IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);


            string peerMessage;
            string[] peerAddress;
            string peerAddressParsed;
            bool sendMessage;
            int exitCode;
            string[] peerIP;
            string peerPort;

            var winViewer = new WinViewer.WinViewer();


            //We loop here to allow any number of test messages to be sent and received
            while (true)
            {
                winViewer.peerReturn(out peerMessage, out peerAddress, out sendMessage, out exitCode);





                //Request a message to send somewhere
                //Console.WriteLine("\nPlease enter your message and press enter (Type 'exit' to quit):");
                //string stringToSend = Console.ReadLine();
                Console.WriteLine("Message to send: " + peerMessage);

                string stringToSend = peerMessage;

                //If the user has typed exit then we leave our loop and end the example
                if (stringToSend == "exit" || exitCode == 1)
                {
                    break;
                }
                else if (sendMessage == true)
                {
                    winViewer.IPAddressCombiner(peerAddress, out peerIP, out peerPort);

                    peerAddressParsed = string.Join(".", peerIP) + ":" + peerPort;
                    Console.WriteLine(peerAddressParsed);

                    sendMessage = false;
                    try
                    {
                        //Once we have a message we need to know where to send it
                        //We have created a small wrapper class to help keep things clean here
                        //ConnectionInfo targetServerConnectionInfo = ExampleHelper.GetServerDetails();
                        IPEndPoint lastServerIPEndPoint = IPTools.ParseEndPointFromString(peerAddressParsed);
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
        private static void HandleIncomingMessagePacket(PacketHeader header, Connection connection, string incomingString)
        {
            Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");
        }
    }
}
