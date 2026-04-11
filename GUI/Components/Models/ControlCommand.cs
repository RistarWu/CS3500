using System.Text.Json.Serialization;

namespace GUI.Components.Models;

/// <summary>
/// Represents a control command sent from the client to the server
/// </summary>
public class ControlCommand
{
    
    /// <summary>
    /// Gets or sets the requested movement direction
    /// Includes "none", "up", "left", "right", "down"
    /// </summary>
    [JsonPropertyName("moving")]
    public string Moving { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlCommand"/> class
    /// Set the default movement to "none"
    /// </summary>
    public ControlCommand()
    {
        Moving = "none";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlCommand"/> class
    /// with a specific direction
    /// </summary>
    /// <param name="moving"> The specific direction</param>
    public ControlCommand(string moving)
    {
        Moving = moving;
    }
}