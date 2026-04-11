﻿// <summary>
//   <para>
//     <authors> Quoc Thinh Le and Ristar Wu </authors>
//     <date> 3/23/2026 </date>
//       Implements a multi-client chat server that accepts connections, assigns each client a name,
//       and broadcasts messages and disconnection notices to all connected clients.
//   </para>
// </summary>

using CS3500.Networking;
using System.Text;

namespace CS3500.Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{
    /// <summary>
    ///  All current connected clients.
    /// </summary>
    private static readonly List<NetworkConnection> _clients = new();
    
    /// <summary>
    ///   The main program.
    /// </summary>
    /// <param name="args"> ignored. </param>
    /// <returns> A Task. Not really used. </returns>
    private static void Main( string[] args )
    {
        Server.StartServer( HandleConnect, 11_000 );
        Console.Read(); // don't stop the program.
    }
    

    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect( NetworkConnection connection )
    {
        // handle all messages until disconnect.
        connection.Send( "Type in the box chat for your name" );
        
        string name = connection.ReadLine(); // Store the name

        connection.Send( $"Hello {name}" );
        try
        {
            

            lock (_clients) // Do not allow multiple adding
            {
                _clients.Add(connection);
            }
            
            while ( true )
            {
                var message =  connection.ReadLine( );

                lock (_clients) // Prevent modifying collection
                {
                    foreach (var client in _clients)
                    {
                        client.Send( $"{name} : {message}" );
                    }
                }

                
            }
        }
        catch ( Exception e)
        {
            // do anything necessary to handle a disconnected client in here
            lock (_clients)
            {
                _clients.Remove(connection);
                
                foreach (var client in _clients)
                {
                    client.Send( $"{name} has disconnected." );
                }
            }
            
        }
    }

    

    
}