// This code is adapted from a sample found at the URL 
// "http://blogs.msdn.com/b/jmanning/archive/2004/12/19/325699.aspx"

using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace TcpEchoServer
{
	public class TcpEchoServer
	{
		private List<TcpClient> clients = new List<TcpClient>();
		
		public void start()
		{
			Console.WriteLine("Starting broadcast echo server...");

			TcpListener listener = new TcpListener(IPAddress.Loopback, 1234);        
			listener.Start();

			while (true) {
				Console.WriteLine("Waiting for client to connect...");
            	
				TcpClient client = listener.AcceptTcpClient();
				clients.Add(client);
				new Thread(() => handleMessage(client)).Start();
				Console.WriteLine("Client " + client.Client.RemoteEndPoint + " connected!");
			}

		}
		
		public void handleMessage(TcpClient sender)
		{
			StreamReader senderStreamReader = new StreamReader(sender.GetStream(), Encoding.ASCII);
			try {
				string inputLine = "";
				while (inputLine != null) {

					inputLine = senderStreamReader.ReadLine();
					Console.WriteLine("Broadcasting string \"" + inputLine + "\" from client " + sender.Client.RemoteEndPoint);
					broadcastMessage(inputLine, sender);
				}
			} catch (IOException e) {
				// handle
			}
            
			Console.WriteLine("Server saw disconnect from client.");
			clients.Remove(sender);
			
		}
		
		public void broadcastMessage(string message, TcpClient sender)
		{
			
			
			foreach (TcpClient client in clients) {
				if(client == sender) {
					continue;
				}
				StreamWriter recipientStreamWriter = new StreamWriter(client.GetStream(), Encoding.ASCII) { AutoFlush = true };
				recipientStreamWriter.WriteLine(message);
			}


		}
		
		public static void Main()
		{
			new TcpEchoServer().start();
		}
		
	}
}