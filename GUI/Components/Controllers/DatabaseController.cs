using MySql.Data.MySqlClient;

namespace GUI.Components.Controllers;

/// <summary>
/// Handles database operations for recording Snake game information.
/// </summary>
public class DatabaseController
{
    /// <summary>
    /// The connection string used to connect to the MySQL database.
    /// Replace the database name, uid, and password with your own values.
    /// </summary>
    private const string connectionString =
        "server=atr.eng.utah.edu;database=u1555035;uid=u1555035;password=Lele009811";

    /// <summary>
    /// Inserts a new row into the Games table and returns the new game ID.
    /// </summary>
    /// <returns> The ID of the newly created game. </returns>
    public int StartGame()
    {
        using MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        string now = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

        string insertQuery =
            "INSERT INTO Games (StartTime, EndTime) VALUES (@start, @end);";

        using MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
        cmd.Parameters.AddWithValue("@start", now);
        cmd.Parameters.AddWithValue("@end", now);
        cmd.ExecuteNonQuery();

        using MySqlCommand idCmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn);
        object? result = idCmd.ExecuteScalar();

        return Convert.ToInt32(result);
    }

    /// <summary>
    /// Updates the ending time of the given game.
    /// </summary>
    /// <param name="gameID"> The ID of the game to update. </param>
    public void EndGame(int gameID)
    {
        using MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        string now = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

        string updateQuery =
            "UPDATE Games SET EndTime = @end WHERE GameID = @id;";

        using MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
        cmd.Parameters.AddWithValue("@end", now);
        cmd.Parameters.AddWithValue("@id", gameID);
        cmd.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Inserts a new player row for the given game.
    /// </summary>
    /// <param name="snakeID"> The snake ID sent by the server. </param>
    /// <param name="name"> The player name. </param>
    /// <param name="maxScore"> The player's current maximum score. </param>
    /// <param name="gameID"> The game this player belongs to. </param>
    public void AddPlayer(int snakeID, string name, int maxScore, int gameID)
    {
        using MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        string now = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

        string insertQuery =
            "INSERT INTO Players (SnakeID, Name, MaxScore, EnterTime, LeaveTime, GameID) " +
            "VALUES (@snakeID, @name, @maxScore, @enterTime, @leaveTime, @gameID);";

        using MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
        cmd.Parameters.AddWithValue("@snakeID", snakeID);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@maxScore", maxScore);
        cmd.Parameters.AddWithValue("@enterTime", now);
        cmd.Parameters.AddWithValue("@leaveTime", now);
        cmd.Parameters.AddWithValue("@gameID", gameID);
        cmd.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Updates the maximum score recorded for a player in a given game.
    /// </summary>
    /// <param name="snakeID"> The snake ID sent by the server. </param>
    /// <param name="gameID"> The game this player belongs to. </param>
    /// <param name="maxScore"> The new maximum score. </param>
    public void UpdatePlayerMaxScore(int snakeID, int gameID, int maxScore)
    {
        using MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        string updateQuery =
            "UPDATE Players SET MaxScore = @maxScore " +
            "WHERE SnakeID = @snakeID AND GameID = @gameID;";

        using MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
        cmd.Parameters.AddWithValue("@maxScore", maxScore);
        cmd.Parameters.AddWithValue("@snakeID", snakeID);
        cmd.Parameters.AddWithValue("@gameID", gameID);
        cmd.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Updates the leave time of a player in a given game.
    /// </summary>
    /// <param name="snakeID"> The snake ID sent by the server. </param>
    /// <param name="gameID"> The game this player belongs to. </param>
    public void SetPlayerLeaveTime(int snakeID, int gameID)
    {
        using MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        string now = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

        string updateQuery =
            "UPDATE Players SET LeaveTime = @leaveTime " +
            "WHERE SnakeID = @snakeID AND GameID = @gameID;";

        using MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
        cmd.Parameters.AddWithValue("@leaveTime", now);
        cmd.Parameters.AddWithValue("@snakeID", snakeID);
        cmd.Parameters.AddWithValue("@gameID", gameID);
        cmd.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Sets the leave time for all players in the given game.
    /// This is used when the client disconnects before some players send dc = true.
    /// </summary>
    /// <param name="gameID"> The game to update. </param>
    public void SetAllPlayersLeaveTime(int gameID)
    {
        using MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        string now = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

        string updateQuery =
            "UPDATE Players SET LeaveTime = @leaveTime " +
            "WHERE GameID = @gameID;";

        using MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
        cmd.Parameters.AddWithValue("@leaveTime", now);
        cmd.Parameters.AddWithValue("@gameID", gameID);
        cmd.ExecuteNonQuery();
    }
    
    
}