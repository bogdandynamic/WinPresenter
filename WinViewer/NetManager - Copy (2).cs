﻿//
//  Copyright 2009-2014 NetworkComms.Net Ltd.
//
//  This source code is made available for reference purposes only.
//  It may not be distributed and it may not be made publicly available.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using System.ComponentModel;

namespace Examples.ExamplesConsole
{
    /// <summary>
    /// Example which demonstrates the ability to establish SSL encrypted TCP connections
    /// </summary>
    public partial class SSLExample
    {
        /// <summary>
        /// A suitable certificate to use for the example
        /// </summary>
        static X509Certificate certificate;

        /// <summary>
        /// SSLOptions which will be used for incoming connections
        /// </summary>
        static SSLOptions listenerSSLOptions;

        /// <summary>
        /// SSLOptions which will be used for outgoing connections
        /// </summary>
        static SSLOptions connectionSSLOptions;

        /// <summary>
        /// The SendReceiveOptions used for sending
        /// </summary>
        static SendReceiveOptions sendingSendReceiveOptions;

        /// <summary>
        /// Run example
        /// </summary>
        /// 

        public void RunExample()
        {


            string peerMessage;
            string peerAddress;
            bool sendMessage;
            int exitCode;


            //WinViewer.WinViewer winViewer = new WinViewer.WinViewer();

            var winViewer = new WinViewer.WinViewer();


            Console.WriteLine("Secure Socket Layer (SSL) Example ...\n");

            //Ensure we have a certificate to use
            //Maximum security is achieved by using a trusted certificate
            //To keep things simple here we just create a self signed certificate for testing
            string certName = "testCertificate.pfx";
            if (!File.Exists(certName))
            {
                Console.WriteLine("Creating self-signed test certificate - " + certName);
                CertificateDetails details = new CertificateDetails("CN=networkcomms.net", DateTime.Now, DateTime.Now.AddYears(1));

                //We could increase/decrease the default key length if we want
                details.KeyLength = 1024;

                //Save the certificate to disk
                SSLTools.CreateSelfSignedCertificatePFX(details, certName);
                certificate = new X509Certificate2(certName);
                Console.WriteLine("\t... certificate successfully created.");
            }
            else
            {
                //Load an existing certificate
                Console.WriteLine("Loading existing certificate - " + certName);
                certificate = new X509Certificate2(certName);
                Console.WriteLine("\t... certificate successfully loaded.");
            }
            //winViewer.
            //Add a global incoming packet handler for packets of type "Message"
            //This handler will convert the incoming raw bytes into a string (this is what 
            //the <string> bit means) and then write that string to the local console window.
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", (packetHeader, connection, incomingString) =>
            {
                winViewer.LogTextBox.AppendText("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");
                Console.WriteLine("\n  ... Incoming message from " + connection.ToString() + " saying '" + incomingString + "'.");
            });


            //peerReturnMessage = returnString;

            //Create suitable SSLOptions to use with TCP
            //SelectSSLOptions();

            //Get a list of all local endPoints using the default port
            List<IPEndPoint> desiredlocalEndPoints = (from current in HostInfo.IP.FilteredLocalAddresses()
                                                      select new IPEndPoint(current, 0)).ToList();

            //Create a list of matching TCP listeners where we provide the listenerSSLOptions
            List<ConnectionListenerBase> listeners = (from current in desiredlocalEndPoints
                                                      select (ConnectionListenerBase)(new TCPConnectionListener(NetworkComms.DefaultSendReceiveOptions,
                                                           ApplicationLayerProtocolStatus.Enabled,
                                                           listenerSSLOptions))).ToList();

            //Start listening for incoming TCP connections
            Connection.StartListening(listeners, desiredlocalEndPoints, true);

            //Print out the listening addresses and ports
            Console.WriteLine("\nListening for incoming TCP (SSL) connections on:");
            foreach (IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            //We loop here to allow any number of test messages to be sent and received
            while (true)
            {
                //Request a message to send somewhere
                //Console.WriteLine("\nPlease enter your message and press enter (Type 'exit' to quit):");
                //string stringToSend = Console.ReadLine();
                //string stringToSend = peerMessage;

                //peerMessage.


                winViewer.peerReturn(out peerMessage, out peerAddress, out sendMessage, out exitCode);


                if (sendMessage==true)
                {
                    string stringToSend = peerMessage;
                    sendMessage = false;
                    //If the user has typed exit then we leave our loop and end the example
                    if (stringToSend == "exit") break;
                    else
                    {
                        //Once we have a message we need to know where to send it
                        //We have created a small wrapper class to help keep things clean here
                        //ConnectionInfo targetServerConnectionInfo = ExampleHelper.GetServerDetails();
                        IPEndPoint lastServerIPEndPoint = IPTools.ParseEndPointFromString(peerAddress);
                        ApplicationLayerProtocolStatus applicationLayerProtocol = ApplicationLayerProtocolStatus.Enabled;
                        ConnectionInfo targetServerConnectionInfo = new ConnectionInfo(lastServerIPEndPoint, applicationLayerProtocol);
                        try
                        {
                            //Get a connection to the target server using the connection SSL options we configured earlier
                            //If there is a problem with the SSL handshake this will throw a CommsSetupShutdownException
                            TCPConnection connection = TCPConnection.GetConnection(targetServerConnectionInfo,
                                sendingSendReceiveOptions,
                                connectionSSLOptions);

                            //Send our message of the encrypted connection
                            connection.SendObject("Message", stringToSend);
                        }
                        catch (CommsException)
                        {
                            //We catch all exceptions by using CommsException
                            Console.WriteLine("\nERROR - Connection to " + targetServerConnectionInfo + " was unsuccessful." +
                                " Server is either not listening or the entered SSL configurations" +
                                "are not compatible. Please check settings and try again.");
                        }
                    }
                }
                else if (exitCode == 1)
                {
                    break;
                }

                //else NetworkComms.Shutdown();
            }

            //If we have used comms features we must gracefully shutdown
            NetworkComms.Shutdown();
        }

        /// <summary>
        /// Select the SSL options
        /// </summary>
        private static void SelectSSLOptions()
        {
            int selectedOption;

            //Configure the server options
            //These will be applied for incoming connections
            Console.WriteLine("\nRequire connecting clients to provide certificate?\n1 - Yes (Connection will only be successful if client provides certificate.) \n2 - No (Client only requires certificate name to connect.)");
            while (true)
            {
                bool parseSucces = int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out selectedOption);
                if (parseSucces && selectedOption <= 2) break;
                Console.WriteLine("Invalid connection type choice. Please try again.");
            }

            if (selectedOption == 1)
            {
                Console.WriteLine(" ... selected yes.");
                listenerSSLOptions = new SSLOptions(certificate, true, true);
            }
            else if (selectedOption == 2)
            {
                Console.WriteLine(" ... selected no.");
                listenerSSLOptions = new SSLOptions(certificate, true, false);
            }
            else
                throw new Exception("Unable to determine selected option.");

            //Configure the connection options
            //These will be used when establishing outgoing connections
            Console.WriteLine("\nProvide certificate for outgoing connections?\n1 - Yes (Connection will only be successful if client and server certificate match.)\n2 - No (Client will accept any certificate from server.)");
            while (true)
            {
                bool parseSucces = int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out selectedOption);
                if (parseSucces && selectedOption <= 2) break;
                Console.WriteLine("Invalid connection type choice. Please try again.");
            }

            if (selectedOption == 1)
            {
                Console.WriteLine(" ... selected yes.");
                connectionSSLOptions = new SSLOptions(certificate, true);
            }
            else if (selectedOption == 2)
            {
                Console.WriteLine(" ... selected no.");
                connectionSSLOptions = new SSLOptions("networkcomms.net", true);
            }
            else
                throw new Exception("Unable to determine selected option.");

            //Select if the dataPadder will be enabled
            Console.WriteLine("\nWhen sending encrypted data" +
            " the quantity of traffic can give away a significant amount of information. To prevent this" +
            " traffic analysis attack we have included a data processor which if enabled ensures every packet sent" +
            " is of a fixed size. Do you want to enable this padding data processor? " + "\n1 - Yes\n2 - No");
            while (true)
            {
                bool parseSucces = int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out selectedOption);
                if (parseSucces && selectedOption <= 2) break;
                Console.WriteLine("Invalid choice. Please try again.");
            }

            if (selectedOption == 1)
            {
                Console.WriteLine(" ... selected yes.");
                sendingSendReceiveOptions = new SendReceiveOptions<ProtobufSerializer, DataPadder>();
                DataPadder.AddPaddingOptions(sendingSendReceiveOptions.Options, 1024, DataPadder.DataPaddingType.Random, true);
            }
            else if (selectedOption == 2)
            {
                Console.WriteLine(" ... selected no.");
                sendingSendReceiveOptions = NetworkComms.DefaultSendReceiveOptions;
            }
            else
                throw new Exception("Unable to determine selected option.");
        }
    }
}
