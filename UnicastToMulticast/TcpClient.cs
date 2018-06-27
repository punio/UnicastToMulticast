using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using RxSocket;

namespace UnicastToMulticast
{
	class TcpClient
	{
		public IObservable<byte[]> Data { get; } = new Subject<byte[]>();

		private readonly RxTcpClient _client;
		private readonly Subject<object> _connectRequest = new Subject<object>();
		private readonly string _address;
		private readonly int _port;
		private readonly CompositeDisposable _disposable = new CompositeDisposable();
		public TcpClient(string address, int port)
		{
			_address = address;
			_port = port;

			_client = new RxTcpClient();
			_disposable.Add(_client.Error.Subscribe(OnError));
			_disposable.Add(_client.Closed.Subscribe(OnClose));
			_disposable.Add(_client.Received.Subscribe(OnReceive));

			_disposable.Add(_connectRequest.Throttle(TimeSpan.FromSeconds(1)).Subscribe(_ => Connect()));
			_connectRequest.OnNext(null);
		}

		public void End()
		{
			_disposable.Dispose();
			_client.Close();
		}

		private void Connect()
		{
			try
			{
				_client.Connect(_address, _port);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Connect error.{e.Message}");
				_connectRequest.OnNext(null);
			}
		}

		private void OnError(ErrorData data)
		{
			Console.WriteLine($"Client socket error.{data.Exception}");
			_client.Close();
			_connectRequest.OnNext(null);
		}

		private void OnClose(EndPoint endPoint)
		{
			Console.WriteLine("Client closed.");
			_connectRequest.OnNext(null);
		}

		private void OnReceive(TcpData data)
		{
			((Subject<byte[]>)Data).OnNext(data.Data);
		}
	}
}
