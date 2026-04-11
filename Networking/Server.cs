﻿// <summary>
//   <para>
//     <authors> Quoc Thinh Le and Ristar Wu </authors>
//     <date> 3/23/2026 </date>
//       Implements a simple TCP server that listens for incoming client connections
//       and spawns a new thread to handle each connection using a provided callback.
//   </para>
// </summary>

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace CS3500.Networking;

/// <summary>
///   Represents a server task that waits for connections on a given
///   port and calls the provided delegate when a connection is made.
/// </summary>
public static class Server
{

    /// <summary>
    ///   Wait on a TcpListener for new connections. Alert the main program
    ///   via a callback (delegate) mechanism.
    /// </summary>
    /// <param name="handleConnect">
    ///   Handler for what the user wants to do when a connection is made.
    ///   This should be run asynchronously via a new thread.
    /// </param>
    /// <param name="port"> The port (e.g., 11000) to listen on. </param>
    public static void StartServer( Action<NetworkConnection> handleConnect, int port )
    {
        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        
        while(true)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            NetworkConnection connection = new NetworkConnection(tcpClient);
            
            
            Thread thread = new Thread( () => handleConnect(connection));
            thread.Start();
            
        }

    }
}
