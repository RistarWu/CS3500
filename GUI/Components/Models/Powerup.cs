using System.Text.Json.Serialization;

namespace GUI.Components.Models;

/// <summary>
/// Represents a power-up object received from the server.
/// </summary>
public class Powerup
{
    /// <summary>
    /// Gets or sets the powerup's ID
    /// </summary>
    [JsonPropertyName("power")]
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets the location of hte powerup
    /// </summary>
    [JsonPropertyName("loc")]
    public Point2D Location { get; set; }

    /// <summary>
    /// Gets or sets the bool indicating whether
    /// this powerup is collected or not
    /// </summary>
    [JsonPropertyName("died")]
    public bool Died { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Powerup"/> class.
    /// </summary>
    public Powerup()
    {
        ID = 0;
        Location = new Point2D();
        Died = false;
    }
}