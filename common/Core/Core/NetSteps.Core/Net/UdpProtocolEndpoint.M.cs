using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Parallel;

namespace NetSteps.Encore.Core.Net
{
	/// <summary>
	/// UDP endpoint for communicating according to a network protocol.
	/// </summary>
	/// <typeparam name="M">message type M</typeparam>
	public class UdpProtocolEndpoint<M> : Disposable
	{
		static readonly ILogSink __log = typeof(UdpProtocolEndpoint<M>).GetLogSink();

		static readonly int DefaultReceiveBufferLength = 65535;
		readonly INetworkProtocol<M> _protocol;
		Reactor<OverlappedOpState> _receiver;
		Reactor<InboundMessage> _notifier;
		INetEndpoint _socket;
		EndPoint _anyEP;
		Reactor<byte[]> _buffers;
		ConcurrentQueue<Action<byte[]>> _bufferWaiters = new ConcurrentQueue<Action<byte[]>>();

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="protocol">a protocol</param>
		public UdpProtocolEndpoint(INetworkProtocol<M> protocol)
		{
			_protocol = protocol;
		}

		/// <summary>
		/// Gets the endpoint's protocol.
		/// </summary>
		public INetworkProtocol<M> Protocol { get { return _protocol; } }

		/// <summary>
		/// Begins receiving protocol messages in parallel from the endpoint given.
		/// </summary>
		/// <param name="endpoint">an endpoint</param>
		/// <returns>this object (for chaining)</returns>
		public UdpProtocolEndpoint<M> ParallelReceive(IPEndPoint endpoint)
		{
			Contract.Assert(_receiver == null);

			_anyEP = (endpoint.AddressFamily == AddressFamily.InterNetworkV6)
					? new IPEndPoint(IPAddress.IPv6Any, 0)
					: new IPEndPoint(IPAddress.Any, 0);

			_receiver = new Reactor<OverlappedOpState>(Background_Recieve);
			_socket = new OverlappedSocketEndpoint().Initialize(CreateSocketWithBind(endpoint));
			_protocol.Initialize(_socket);
			_notifier = new Reactor<InboundMessage>(Background_Notify);
			_buffers = new Reactor<byte[]>(Background_BufferAvailable);
			Background_ParallelReceiveFromAsync(null);
			return this;
		}

		/// <summary>
		/// Performs the instance's disposal.
		/// </summary>
		/// <param name="disposing"></param>
		/// <returns></returns>
		protected override bool PerformDispose(bool disposing)
		{
			_socket.Dispose();
			_socket = null;

			return disposing;
		}

		/// <summary>
		/// Tries to acquire a buffer.
		/// </summary>
		/// <param name="buffer">variable where the buffer will be returned upon success</param>
		/// <returns>true if successful; othewise false</returns>
		protected virtual bool TryAcquireBuffer(out byte[] buffer)
		{
			// default implementation just allocates the buffer
			buffer = new byte[DefaultReceiveBufferLength];
			return true;
		}
		/// <summary>
		/// Releases a buffer previously acquired from the endpoint.
		/// </summary>
		/// <param name="buffer">the buffer being released</param>
		protected virtual void ReleaseBuffer(byte[] buffer)
		{

		}

		/// <summary>
		/// Recycles the buffer given.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		private void RecycleBuffer(byte[] buffer)
		{
			_buffers.Push(buffer);
		}

		private void Background_BufferAvailable(Reactor<byte[]> reactor, byte[] buffer)
		{
			Action<byte[]> waiter;
			if (_bufferWaiters.TryDequeue(out waiter))
			{
				waiter(buffer);
			}
			else
			{
				ReleaseBuffer(buffer);
			}
		}

		private void Background_ParallelReceiveFromAsync(object unused)
		{
			if (_socket != null)
			{
				byte[] buffer;
				if (TryAcquireBuffer(out buffer))
				{
					// Use overlapped IO - the callback will fire when bytes arrive.
					_socket.ReceiveFromAsync(_anyEP, buffer, 0, buffer.Length, Background_ParallelReceiveCallback);
				}
				else
				{
					_bufferWaiters.Enqueue(b =>
					{
						_socket.ReceiveFromAsync(_anyEP, b, 0, b.Length, Background_ParallelReceiveCallback);
					});
				}
			}
		}

		private void Background_ParallelReceiveCallback(Exception ex, OverlappedOpState state)
		{
			if (ex == null)
			{
				if (state.SocketError != SocketError.OperationAborted)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(Background_ParallelReceiveFromAsync));
				}
				if (state.SocketError == SocketError.Success)
				{
					if (state.BytesTransferred > 0)
					{
						_receiver.Push(state);
					}
				}
			}
			else
				__log.Error("Exception during overlapped receive: ", ex.FormatForLogging());
		}

		private void Background_Recieve(Reactor<OverlappedOpState> reactor, OverlappedOpState op)
		{
			try
			{
				DeserializeMessageAndQueueForReceiveNotification(op.RemoteEndPoint, op.Buffer, 0, op.BytesTransferred);
			}
			finally
			{
				RecycleBuffer(op.Buffer);
				op.EndAsyncOp();
			}
		}

		private void Background_Notify(Reactor<InboundMessage> reactor, InboundMessage incomming)
		{
			_protocol.HandleEndpointMessage(incomming.Endpoint, incomming.Timestamp, incomming.Message);
		}

		private void DeserializeMessageAndQueueForReceiveNotification(EndPoint rcep
				, byte[] buffer
				, int offset, int length)
		{
			IPEndPoint ep = (IPEndPoint)rcep;
			DateTime timestamp = DateTime.UtcNow;
			var decode = _protocol.TryDecodeMessage(ep, buffer, offset, length);
			switch (decode.Kind)
			{
				case DecodeMessageResultKind.Success:
					_notifier.Push(new InboundMessage(timestamp, decode.Message, ep));
					break;
				case DecodeMessageResultKind.Partial:
					break;
				case DecodeMessageResultKind.Garbage:
					break;
				case DecodeMessageResultKind.Blacklist:
					break;
				default:
					break;
			}
		}

		private Socket CreateSocketWithBind(IPEndPoint endpoint)
		{
			var sock = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.IP);
			sock.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, 1);
			sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
			sock.Bind(endpoint);
			return sock;
		}

		private Socket CreateSocket(IPEndPoint endpoint)
		{
			var sock = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.IP);
			sock.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, 1);
			sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
			return sock;
		}

		struct InboundMessage
		{
			public DateTime Timestamp;
			public IPEndPoint Endpoint;
			public M Message;

			public InboundMessage(DateTime timestamp, M message, IPEndPoint ep)
			{
				this.Timestamp = timestamp;
				this.Message = message;
				this.Endpoint = ep;
			}
		}
	}

}
