namespace GUI.Components.Models;


/// <summary>
/// Represents a point in 2D world space.
/// </summary>
public class Point2D
{
    /// <summary>
    /// Gets or sets the x-coordinate of the point.
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    /// Gets or sets the x-coordinate of the point.
    /// </summary>
    public int Y { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Point2D"/> class.
    /// </summary>
    public Point2D(){
        X = 0;
        Y = 0;
    }
    
    /// <summary>
    /// Initializea a new instance of the <see cref="Point2D"/> class.
    /// with the specified coordinates.
    /// </summary>
    /// <param name="x"> The x-coordinate</param>
    /// <param name="y"> The y-coordinate</param>
    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }
}