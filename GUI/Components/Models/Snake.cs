using System.Text.Json.Serialization;

namespace GUI.Components.Models;

//TODO: Documents
public class Snake
{

    [JsonPropertyName("snake")]
    public int ID { get; set; } // 2 snakes can't have the same ID, but snake and wall can have the same ID

    [JsonPropertyName("name")] public string Name { get; set; }

    /// <summary>
    /// A list representing the body of the snake. The first point represent the tail, the last point represent the head of the snake.
    /// </summary>
    [JsonPropertyName("body")]
    public List<Point2D> Body { get; set; }

    /// <summary>
    /// Representing the snake's orientation.
    /// </summary>
    [JsonPropertyName("dir")]
    public Point2D Direction { get; set; }

    /// <summary>
    /// An int representing the player's score (the number of power-ups it has eaten)
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; set; } = 0;

    /// <summary>
    /// A bool indicating if the snake died on this frame. This will only be true on the exact frame in which the snake died. 
    /// </summary>
    [JsonPropertyName("died")]
    public bool Died { get; set; } = true;

    /// <summary>
    /// A bool indicating whether a snake is alive or dead
    /// </summary>
    [JsonPropertyName("alive")]
    public bool Alive { get; set; } = true;

    /// <summary>
    /// A bool indicating if the player controlling that snake disconnected on that frame.
    /// </summary>
    [JsonPropertyName("dc")]
    public bool Disconnected { get; set; } = false;

    /// <summary>
    /// A bool indicating if the player joined on this frame. This will only be true for one frame. 
    /// </summary>
    [JsonPropertyName("join")]
    public bool Join { get; set; } = false;


    public Snake() // Default constructor
    {
        ID = 0;
        Name = string.Empty;
        Body = new(); 
        Direction = new Point2D();
        Score = 0;
        Alive = true;
        Died = false;
        Disconnected = false;
        Join = false;
        
    }
    

}