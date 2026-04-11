using System.Text.Json;
using GUI.Components.Models;
using CS3500.Networking;

namespace GUI.Components.Controllers;

/// <summary>
/// Represents the network controller for the Snake client.
/// Handles connection setup, incoming server messages,
/// world updates, and outgoing movement commands.
/// </summary>
public class NetworkController
{
    /// <summary>
    /// Stores the current local game world.
    /// </summary>
    private World _world;

    /// <summary>
    /// Represents the network connection to the server.
    /// </summary>
    private NetworkConnection? _connection;

    /// <summary>
    /// Stores whether the client has finished receiving the initial wall data and is ready to go.
    /// </summary>
    private bool _receivedNonWallObject;
    
    /// <summary>
    /// Gets a value indicating whether the client is currently connected.
    /// </summary>
    public bool IsConnected => _connection != null && _connection.IsConnected;
    
    /// <summary>
    /// Gets the current local game world.
    /// </summary>
    public World GameWorld => _world;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkController"/> class.
    /// </summary>
    public NetworkController()
    {
        _world = new World();
        _receivedNonWallObject =  false;
    }

    /// <summary>
    /// Connects to the server, sends the player name,
    /// and starts the background receive loop.
    /// </summary>
    /// <param name="serverAddress"> The host name or IP address of the server. </param>
    /// <param name="port"> The port number of the server. </param>
    /// <param name="playerName"> The player name to send to the server. </param>
    /// <returns> A task representing the asynchronous operation. </returns>
    public Task ConnectAsync(string serverAddress, int port, string playerName)
    {
        Disconnect(); // To prevent someone press connect twice

        _world = new World();
        _receivedNonWallObject = false;

        try
        {
            _connection = new NetworkConnection();
            _connection.Connect(serverAddress, port);
            _connection.Send(playerName);
            Task.Run(ReceiveLoop);
        }
        catch (Exception e) // Disconnect when there is an error
        {
            Disconnect();
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Disconnects from the server and cleans up connection resources.
    /// </summary>
    public void Disconnect()
    {
        _connection?.Disconnect();
        _connection = null;
    }

    /// <summary>
    /// Continuously reads messages from the server and updates the local world.
    /// </summary>
    private void ReceiveLoop()
    {
        if (_connection is null)
        {
            return;
        }

        try
        {
            string idLine = _connection.ReadLine();
            if (int.TryParse(idLine, out int playerID))
            {
                _world.SetPlayerID(playerID);
            }
            else // Cannot parse the ID, disconnect
            {
           
                Disconnect();
                return;
            }
            
            string sizeLine = _connection.ReadLine();
            if (int.TryParse(sizeLine, out int size))
            {
                _world.SetSize(size, size); 
            }
            else // Cannot get the world's size
            {
                Disconnect();
                return;
            } 
            
            while (IsConnected)
            {
                string line = _connection.ReadLine();
             
                DealingWithJsonLine(line);
            }
        }
        catch (Exception e) // Cannot read the first line
        {
            Console.WriteLine(e.Message); // TODO: Debug
            Disconnect();
        }
    }
    
    /// <summary>
    /// Dealing one JSON line received from the server, during the receive-loop
    /// </summary>
    /// <param name="line"> The raw JSON message. </param>
    private void DealingWithJsonLine(string line)
    {
        using JsonDocument document = JsonDocument.Parse(line);
        JsonElement root = document.RootElement;

        if (root.TryGetProperty("wall", out _))
        {
            Wall? wall = JsonSerializer.Deserialize<Wall>(line);
            if (wall != null)
            {
                _world.AddWall(wall);
            }
          
            return;
        }
                
        if (!_receivedNonWallObject)
        {
            _receivedNonWallObject = true;
            _world.SetHandshakeComplete();
        }

        if (root.TryGetProperty("snake", out _))
        {
            Snake? snake = JsonSerializer.Deserialize<Snake>(line);
            if (snake != null)
            {
                _world.UpdatePlayerID(snake);
            }

            return;
        }
                
        if (root.TryGetProperty("power", out _))
        {
            Powerup? powerup = JsonSerializer.Deserialize<Powerup>(line);
            if (powerup != null)
            {
                _world.UpdatePowerup(powerup);
            }

            return;
        }                
    }

    /// <summary>
    /// Sends a movement command to the server.
    /// Valid directions are "up", "left", "down", and "right".
    /// </summary>
    /// <param name="direction"> The requested movement direction. </param>
    public void SendMovementCommand(string direction)
    {
        if (!IsConnected || !_world.HandshakeComplete)
        {
            return;
        }

        string move = direction.ToLowerInvariant();

        if (move != "up" && move != "left" && move != "down" && move != "right")
        {
            return;
        }
        
        ControlCommand command = new ControlCommand(move);
        string json = JsonSerializer.Serialize(command);
        _connection.Send(json);
        
    }
}