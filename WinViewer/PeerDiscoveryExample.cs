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
using NetworkCommsDotNet;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Tools;
using NetworkCommsDotNet.Connections;

namespace Examples.ExamplesConsole
{
    static class PeerDiscoveryExample
    {
        public static void RunExample()
        {
            Console.WriteLine("Peer Discovery Example ...\n");

            Console.WriteLine("Please select mode:");
            Console.WriteLine("1 - Server (Discoverable)");
            Console.WriteLine("2 - Client (Discovers servers)");

            //Read in user choice
            bool serverMode;
            serverMode = false;

            //Both server and client must be discoverable
            PeerDiscovery.EnableDiscoverable(PeerDiscovery.DiscoveryMethod.UDPBroadcast);

            //Write out the network adaptors that are discoverable
            Console.WriteLine("\nPeer Identifier: " + NetworkComms.NetworkIdentifier);
            Console.WriteLine("\nDiscoverable on:");
            foreach (IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.UDP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            if (serverMode)
            {
                //The server does nothing else now but wait to be discovered.
                Console.WriteLine("\nPress any key to quit.");
                ConsoleKeyInfo key = Console.ReadKey(true);
            }
            else
            {
                while (true)
                {
                    
                    //Ensure a previous example loop does not duplicate the asynchronous event delegate
                    PeerDiscovery.OnPeerDiscovered -= PeerDiscovered;

                        #region Discover Asynchronously
                        Console.WriteLine("\nDiscovering servers asynchronously ... ");

                        //Append the OnPeerDiscovered event
                        //The PeerDiscovered delegate will just write to the console.
                        PeerDiscovery.OnPeerDiscovered += PeerDiscovered;

                        //Trigger the asynchronous discovery
                        PeerDiscovery.DiscoverPeersAsync(PeerDiscovery.DiscoveryMethod.UDPBroadcast);
                        #endregion
                    
                }
            }

            //We should always call shutdown when our application closes.
            NetworkComms.Shutdown();
        }

        /// <summary>
        /// Static locker used to ensure we only write information to the console in a clear fashion
        /// </summary>
        static object locker = new object();

        /// <summary>
        /// Execute this method when a peer is discovered 
        /// </summary>
        /// <param name="peerIdentifier">The network identifier of the discovered peer</param>
        /// <param name="discoveredPeerEndPoints">The discoverable endpoints found for the provided peer</param>
        private static void PeerDiscovered(ShortGuid peerIdentifier, Dictionary<ConnectionType, List<EndPoint>> discoveredPeerEndPoints)
        {
            //Lock to ensure we do not write to the console in parallel.
            lock (locker)
            {
                Console.WriteLine("\nEndpoints discovered for peer with networkIdentifier {0} ...", peerIdentifier);
                foreach (ConnectionType connectionType in discoveredPeerEndPoints.Keys)
                {
                    Console.WriteLine("  ... endPoints of type {0}:", connectionType);
                    foreach (EndPoint endPoint in discoveredPeerEndPoints[connectionType])
                        Console.WriteLine("    -> {0}", endPoint.ToString());
                }
            }                        
        }
    }
}
