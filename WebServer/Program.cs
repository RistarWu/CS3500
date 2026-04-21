using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener listener = new TcpListener(IPAddress.Any, 8080);
listener.Start();

Console.WriteLine("WebServer running on http://localhost:8080");

while (true)
{
    TcpClient client = listener.AcceptTcpClient();
    _ = Task.Run(() => HandleClient(client));
}

static void HandleClient(TcpClient client)
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

        if (requestLine.StartsWith("GET / "))
        {
            html = "<html><h3>Welcome to the Snake Games Database!</h3><a href=\"/games\">View Games</a></html>";
            SendResponse(writer, html);
        }
        else
        {
            html = "<html><h3>404 Not Found</h3></html>";
            SendResponse(writer, html, "404 Not Found");
        }
    }
}

static void SendResponse(StreamWriter writer, string html, string status = "200 OK")
{
    int contentLength = Encoding.UTF8.GetByteCount(html);

    writer.Write($"HTTP/1.1 {status}\r\n");
    writer.Write("Connection: close\r\n");
    writer.Write("Content-Type: text/html; charset=UTF-8\r\n");
    writer.Write($"Content-Length: {contentLength}\r\n");
    writer.Write("\r\n");
    writer.Write(html);
}