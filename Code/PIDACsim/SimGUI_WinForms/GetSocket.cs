using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SimGUI
{
  public class GetSocket
  {
    public static Socket ConnectSocket(string server, int port)
    {
      Socket s = null;
      IPHostEntry hostEntry = Dns.GetHostEntry(server);

      IPAddress address = IPAddress.Parse("127.0.0.1");
      IPEndPoint ipe = new IPEndPoint(address, port);
      Socket tempSock = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        tempSock.Connect(ipe);
      }
      catch (Exception e)
      {
        // TODO: Proper exception handling
      }

      if (tempSock.Connected)
        s = tempSock;

      return s;
    }

    public static Socket ListenSocket(int port)
    {
      // Establish the local endpoint for the socket.  
      // Dns.GetHostName returns the name of the   
      // host running the application.
      IPHostEntry hostEntry = Dns.GetHostEntry("127.0.0.1");

      //foreach (IPAddress address2 in hostEntry.AddressList)
      //{
        IPAddress address = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(address, port);

        Socket listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and   
        // listen for incoming connections.  
        try
        {
          listener.Bind(localEndPoint);
          listener.Listen(10);

          Socket s = listener.Accept();

          return s;

        }
        catch (Exception e)
        {
          Console.WriteLine(e.ToString());
        }
      //}
      

      Console.WriteLine("\nPress ENTER to continue...");
      Console.Read();

      return null;
    }

    // This method requests the home page content for the specified server.
    public static string SocketSendReceive(string server, int port)
    {
      string request = "GET / HTTP/1.1\r\nHost: " + server +
          "\r\nConnection: Close\r\n\r\n";
      Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
      Byte[] bytesReceived = new Byte[256];
      string page = "";

      // Create a socket connection with the specified server and port.
      using (Socket s = ConnectSocket(server, port))
      {

        if (s == null)
          return ("Connection failed");

        // Send request to the server.
        s.Send(bytesSent, bytesSent.Length, 0);

        // Receive the server home page content.
        int bytes = 0;
        page = "Default HTML page on " + server + ":\r\n";

        // The following will block until the page is transmitted.
        do
        {
          bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
          page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
        }
        while (bytes > 0);
      }

      return page;
    }
  }
}
