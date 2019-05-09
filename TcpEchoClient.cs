// This code is adapted from a sample found at the URL
// "http://blogs.msdn.com/b/jmanning/archive/2004/12/19/325699.aspx"

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TcpEchoClient
{
	class TcpEchoClient
	{
		static void Main(string[] args)
		{
			new TcpEchoClient().init();
		}
		
		
		public void init() {
			Console.WriteLine("Starting broadcast echo client...");

			TcpClient client = new TcpClient("localhost", 1234);
			NetworkStream stream = client.GetStream();
			
			// Inicia thread para receber msgs
			new Thread(() => {
				StreamReader reader = new StreamReader(stream);
				string inputLine = "";
				while (inputLine != null) {
					inputLine = reader.ReadLine();
					Console.Write("\nReceived from server: " + inputLine + "\nEnter text to send: ");
				}

			}).Start();

			StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

			while (true) {
				Console.Write("Enter text to send: ");
				writer.WriteLine(Console.ReadLine());
			}
		}
	}
}