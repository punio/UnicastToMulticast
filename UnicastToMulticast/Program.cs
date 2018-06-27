using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace UnicastToMulticast
{
	class Program
	{
		static void Main(string[] args)
		{
			// this versin is client socket only
			Console.Write("From address:");
			var fromAddress = Console.ReadLine();
			if (string.IsNullOrEmpty(fromAddress)) fromAddress = "192.168.1.1";
			Console.Write("From port:");
			var fromPort = Console.ReadLine();
			if (string.IsNullOrEmpty(fromPort)) fromPort = "5000";

			var defaultLocalAddress = NetworkInterface.GetAllNetworkInterfaces().First().GetIPProperties().UnicastAddresses.Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().Address.ToString();
			Console.Write($"Multicast nic({defaultLocalAddress}):");
			var localAddress = Console.ReadLine();
			if (string.IsNullOrEmpty(localAddress)) localAddress = defaultLocalAddress;
			Console.Write("Multicast address:");
			var multicastAddress = Console.ReadLine();
			if (string.IsNullOrEmpty(multicastAddress)) multicastAddress = "239.0.0.1";
			Console.Write("Multicast port:");
			var multicastPort = Console.ReadLine();
			if (string.IsNullOrEmpty(multicastPort)) multicastPort = "5000";

			Console.WriteLine($"{fromAddress}:{fromPort} -> {multicastAddress}:{multicastPort}");


			var client = new TcpClient(fromAddress, int.Parse(fromPort));
			var server = new MulticastServer(localAddress,multicastAddress, int.Parse(multicastPort), client.Data);

			Console.WriteLine("Push any key to exit.");
			Console.ReadKey();
			client.End();
			server.End();
		}
	}
}
