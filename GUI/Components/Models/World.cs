namespace GUI.Components.Models;

//TODO: Docs
public class World
{
    private readonly object _lock = new();

    private readonly Dictionary<int,Snake> _players;
    private readonly Dictionary<int,Wall> _walls;
    private readonly Dictionary<int,Powerup> _powerups;
    
    public int Width { get; private set; }
    public int Height { get; private set; }

    public int PlayerID { get; private set; }

    public bool HandshakeComplete { get; private set; }
    public World()
    {
        _players = new Dictionary<int, Snake>();
        _walls = new Dictionary<int, Wall>();
        _powerups = new Dictionary<int, Powerup>();
        Width = 0;
        Height = 0;
        PlayerID = -1;
        HandshakeComplete = false;
    }
    
    public void SetPlayerID(int id)
    {
    lock (_lock)
        {
            PlayerID = id;
        }
    }

    public void SetSize(int width, int height)
    {
        lock (_lock)
        {
            Width = width;
            Height = height;
        }
    }

    public void SetHandshakeComplete()
    {
        lock (_lock)
        {
            HandshakeComplete = true;
        }
    }

    public void UpdatePlayerID(Snake snake)
    {
        lock (_lock)
        {
            if (snake.Disconnected)
            {
                _players.Remove(snake.ID);
            }
            else 
            {
                if (!_players.ContainsKey(snake.ID))
                {
                    _players.Add(snake.ID, snake);
                }
                _players[snake.ID] = snake;
            }
            
        }
    }
    
    public void AddWall(Wall wall)
    {
        lock (_lock)
            {
                _walls.Add(wall.ID, wall);
            }
    }

    public void UpdatePowerup(Powerup powerup)
    {
        lock (_lock)
        {
            if (powerup.Died)
            {
                _powerups.Remove(powerup.ID);
            }
            else
            {
                if (!_powerups.ContainsKey(powerup.ID))
                {
                    _powerups.Add(powerup.ID, powerup);
                }
                _powerups[powerup.ID] = powerup;
            }
        }
    }

    public Snake? GetPlayerSnake(int playerID)
    {
        lock (_lock)
        {
            if (!_players.ContainsKey(playerID)) 
            {
                return null;    
            }
            else
            {
                return _players[playerID];
            }
        }
    }

    public List<Snake> GetPlayers()
    {
        lock (_lock)
        {
            return _players.Values.ToList();
        }
    }
    public List<Wall> GetWalls()
    {
        lock (_lock)
        {
            return _walls.Values.ToList();
        }
    }

    public List<Powerup> GetPowerups()
    {
        lock (_lock)
        {
            return _powerups.Values.ToList();
        }
    }
    
}