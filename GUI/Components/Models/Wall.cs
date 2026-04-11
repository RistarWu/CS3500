using System.Text.Json.Serialization;

namespace GUI.Components.Models;


/// <summary>
/// Represents a wall object received from the server
/// </summary>
public class Wall
{
    /// <summary>
    /// Gets or sets the wall's ID
    /// </summary>
    [JsonPropertyName("wall")] 
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets one endpoint of the wall
    /// </summary>
    [JsonPropertyName("p1")] 
    public Point2D P1{ get; set; }
    
    /// <summary>
    /// Gets or sets another endpoint of the wall    
    /// </summary>
    [JsonPropertyName("p2")] 
    public Point2D P2{ get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Wall"/> class.
    /// </summary>
    public Wall()
    {
        ID = 0;
        P1 = new Point2D();
        P2 = new Point2D();
    }

    
}