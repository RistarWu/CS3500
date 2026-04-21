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
}