using System.Net;
using System.Net.Sockets;
using System.Text;
using MySql.Data.MySqlClient;

namespace WebServer;

public static class Program
{
    // Connection string for the MySQL database used by the web server.
    private const string ConnectionString =
        "server=atr.eng.utah.edu;database=u1555035;uid=u1555035;password=RistarWu123";

    public static void Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();

        Console.WriteLine("WebServer running on http://localhost:8080");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            _ = Task.Run(() => HandleClient(client));
        }
    }

    /// <summary>
    /// Handles one HTTP client connection.
    /// Reads the request line, ignores the remaining headers,
    /// determines which page was requested, and sends back
    /// the corresponding HTML response.
    /// </summary>
    /// <param name="client">The connected TCP client.</param>
    private static void HandleClient(TcpClient client)
    {
        using (client)
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream, new UTF8Encoding(false)))
        using (StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true })
        {
            string? requestLine = reader.ReadLine();

            if (string.IsNullOrEmpty(requestLine))
            {
                return;
            }

            Console.WriteLine(requestLine);

            // Read and discard the rest of the HTTP headers
            string? line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
            }

            string html;

            if (requestLine.StartsWith("GET / HTTP/1.1"))
            {
                html = "<html><h3>Welcome to the Snake Games Database!</h3><a href=\"/games\">View Games</a></html>";
                SendResponse(writer, html);
            }
            else if (requestLine.StartsWith("GET /games?gid="))
            {
                int start = requestLine.IndexOf("gid=") + 4;
                int end = requestLine.IndexOf(' ', start);

                string gidText = requestLine.Substring(start, end - start);

                if (int.TryParse(gidText, out int gameID))
                {
                    html = GetOneGameHtml(gameID);
                    SendResponse(writer, html);
                }
                else
                {
                    html = "<html><h3>Invalid Game ID</h3></html>";
                    SendResponse(writer, html, "400 Bad Request");
                }
            }
            else if (requestLine.StartsWith("GET /games HTTP/1.1"))
            {
                html = GetGamesTableHtml();
                SendResponse(writer, html);
            }
            else
            {
                html = "<html><h3>404 Not Found</h3></html>";
                SendResponse(writer, html, "404 Not Found");
            }

        }
    }

    /// <summary>
    /// Sends a complete HTTP response containing the given HTML body.
    /// Includes the status line, required headers, and the correct content length.
    /// </summary>
    /// <param name="writer">The stream writer used to send the response.</param>
    /// <param name="html">The HTML body to send.</param>
    /// <param name="status">The HTTP status string, such as 200 OK or 404 Not Found.</param>
    private static void SendResponse(StreamWriter writer, string html, string status = "200 OK")
    {
        int contentLength = Encoding.UTF8.GetByteCount(html);

        writer.Write($"HTTP/1.1 {status}\r\n");
        writer.Write("Connection: close\r\n");
        writer.Write("Content-Type: text/html; charset=UTF-8\r\n");
        writer.Write($"Content-Length: {contentLength}\r\n");
        writer.Write("\r\n");
        writer.Write(html);
    }

    /// <summary>
    /// Queries the database for all recorded games and returns
    /// an HTML table showing each game's ID, start time, and end time.
    /// Each game ID is a link to that specific game's stats page.
    /// </summary>
    /// <returns>An HTML page containing the table of all games.</returns>
    private static string GetGamesTableHtml()
    {
        using MySqlConnection conn = new MySqlConnection(ConnectionString);
        conn.Open();

        string query = "SELECT GameID, StartTime, EndTime FROM Games ORDER BY GameID;";

        using MySqlCommand cmd = new MySqlCommand(query, conn);
        using MySqlDataReader reader = cmd.ExecuteReader();

        string html = "<html><table border=\"1\"><thead><tr><td>ID</td><td>Start</td><td>End</td></tr></thead><tbody>";

        while (reader.Read())
        {
            int gameID = reader.GetInt32(0);
            string start = reader.GetDateTime(1).ToString("M/d/yyyy h:mm:ss tt");
            string end = reader.GetDateTime(2).ToString("M/d/yyyy h:mm:ss tt");

            html += $"<tr><td><a href=\"/games?gid={gameID}\">{gameID}</a></td><td>{start}</td><td>{end}</td></tr>";
        }

        html += "</tbody></table></html>";
        return html;
    }

    /// <summary>
    /// Queries the database for all players recorded in the specified game
    /// and returns an HTML page showing that game's player statistics.
    /// </summary>
    /// <param name="gameID">The ID of the game whose player stats should be displayed.</param>
    /// <returns>An HTML page containing the stats table for the given game.</returns>
    private static string GetOneGameHtml(int gameID)
    {
        using MySqlConnection conn = new MySqlConnection(ConnectionString);
        conn.Open();

        string query =
            "SELECT SnakeID, Name, MaxScore, EnterTime, LeaveTime " +
            "FROM Players WHERE GameID = @gid ORDER BY SnakeID;";

        using MySqlCommand cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@gid", gameID);

        using MySqlDataReader reader = cmd.ExecuteReader();

        string html =
            $"<html><h3>Stats for Game {gameID}</h3>" +
            "<table border=\"1\">" +
            "<thead><tr><td>Player ID</td><td>Player Name</td><td>Max Score</td><td>Enter Time</td><td>Leave Time</td></tr></thead>" +
            "<tbody>";

        while (reader.Read())
        {
            int snakeID = reader.GetInt32(0);
            string name = reader.GetString(1);
            int maxScore = reader.GetInt32(2);
            string enterTime = reader.GetDateTime(3).ToString("M/d/yyyy h:mm:ss tt");
            string leaveTime = reader.GetDateTime(4).ToString("M/d/yyyy h:mm:ss tt");

            html +=
                $"<tr><td>{snakeID}</td><td>{name}</td><td>{maxScore}</td><td>{enterTime}</td><td>{leaveTime}</td></tr>";
        }

        html += "</tbody></table></html>";
        return html;
    }
}