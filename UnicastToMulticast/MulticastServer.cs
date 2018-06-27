using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UnicastToMulticast
{
	class MulticastServer
	{
		private readonly UdpClient _clinet;
		private readonly IDisposable _disposable;
		private readonly IPEndPoint _target;
		private readonly IPAddress _multicastAddress;
		public MulticastServer(string localAddress, string address, int port, IObservable<byte[]> source)
		{
			_clinet = new UdpClient(new IPEndPoint(IPAddress.Parse(localAddress), 0));
			_target = new IPEndPoint(IPAddress.Parse(address), port);
			_multicastAddress = IPAddress.Parse(address);
			_clinet.JoinMulticastGroup(_multicastAddress);
			_disposable = source.Subscribe(OnData);
		}

		public void End()
		{
			_disposable.Dispose();
			_clinet.DropMulticastGroup(_multicastAddress);
		}

		private void OnData(byte[] data)
		{
			Console.WriteLine($"Transfer {data.Length} bytes.");
			_clinet.Send(data, data.Length, _target);
		}
	}
}
