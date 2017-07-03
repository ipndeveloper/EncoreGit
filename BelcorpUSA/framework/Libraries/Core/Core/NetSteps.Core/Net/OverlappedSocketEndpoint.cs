using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Net
{
	/// <summary>
	/// Socket adapter that provides a simplified convention for working with
	/// overlapped IO.
	/// </summary>
	public class OverlappedSocketEndpoint : Disposable, INetEndpoint
	{
		static int __totalSequence = 0;

		Socket _socket;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public OverlappedSocketEndpoint()
		{
		}

		/// <summary>
		/// Gets the underlying endpoint.
		/// </summary>
		public IPEndPoint EndPoint
		{
			get
			{
				return _socket == null ? null : _socket.LocalEndPoint as IPEndPoint;
			}
		}

		/// <summary>
		/// Initializes
		/// </summary>
		/// <param name="socket"></param>
		/// <returns></returns>
		public OverlappedSocketEndpoint Initialize(Socket socket)
		{
			Contract.Requires<ArgumentNullException>(socket != null, Resources.Chk_CannotBeNull);
			Contract.Assert(_socket == null, "already initialized");
			_socket = socket;
			return this;
		}
				
		/// <summary>
		/// Accepts connections asynchronously
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="callback"></param>
		public void AcceptAsync(EndPoint endpoint, Action<Exception, OverlappedOpState> callback)
		{
			var sock = CheckedSocket();

			SocketAsyncEventArgs args = AcquireAsyncEventArgs();
			args.RemoteEndPoint = endpoint;
			args.UserToken = new Completion(Interlocked.Increment(ref __totalSequence), callback);

			bool willRaiseEvent = sock.AcceptAsync(args);
			if (!willRaiseEvent)
			{
				IO_Completed(this, args);
			}
		}

		/// <summary>
		/// Connects to an endpoint asynchronously.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="callback"></param>
		public void ConnectAsync(EndPoint endpoint, Action<Exception, OverlappedOpState> callback)
		{
			var sock = CheckedSocket();

			SocketAsyncEventArgs args = AcquireAsyncEventArgs();
			args.RemoteEndPoint = endpoint;
			args.UserToken = new Completion(Interlocked.Increment(ref __totalSequence), callback);

			bool willRaiseEvent = sock.ConnectAsync(args);
			if (!willRaiseEvent)
			{
				IO_Completed(this, args);
			}
		}

		/// <summary>
		/// Receives from an endpoint asynchronously.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int ReceiveFrom(ref EndPoint endpoint, byte[] data, int offset, int count)
		{
			var sock = CheckedSocket();

			return sock.ReceiveFrom(data, offset, count, SocketFlags.None, ref endpoint);
		}

		/// <summary>
		/// Receives from an endpoint asynchronously.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="callback"></param>
		public void ReceiveFromAsync(EndPoint endpoint, byte[] data, int offset, int count, Action<Exception, OverlappedOpState> callback)
		{
			var sock = CheckedSocket();

			SocketAsyncEventArgs args = AcquireAsyncEventArgs();
			args.RemoteEndPoint = endpoint;
			args.SetBuffer(data, offset, count);
			args.UserToken = new Completion(Interlocked.Increment(ref __totalSequence), callback);

			bool willRaiseEvent = sock.ReceiveFromAsync(args);
			if (!willRaiseEvent)
			{
				IO_Completed(this, args);
			}
		}

		/// <summary>
		/// Sends data to an endpoint.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		public void SendTo(EndPoint endpoint, byte[] data)
		{
			var sock = CheckedSocket();
			sock.SendTo(data, endpoint);
		}

		/// <summary>
		/// Sends data to an endpoint asynchronously.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		public void SendTo(EndPoint endpoint, byte[] data, int count)
		{
			var sock = CheckedSocket();
			sock.SendTo(data, count, SocketFlags.None, endpoint);
		}

		/// <summary>
		/// Sends data to an endpoint asynchronously.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="flags"></param>
		public void SendTo(EndPoint endpoint, byte[] data, int offset, int count, SocketFlags flags)
		{
			var sock = CheckedSocket();
			sock.SendTo(data, offset, count, flags, endpoint);
		}

		/// <summary>
		/// Sends data to an endpoint asynchronously.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		/// <param name="callback"></param>
		public void SendToAsync(EndPoint endpoint, byte[] data, int count, Action<Exception, OverlappedOpState> callback)
		{
			SendToAsync(endpoint, data, 0, count, callback);
		}

		/// <summary>
		/// Sends data to an endpoint asynchronously.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="callback"></param>
		public void SendToAsync(EndPoint endpoint, byte[] data, int offset, int count, Action<Exception, OverlappedOpState> callback)
		{
			var sock = CheckedSocket();

			SocketAsyncEventArgs args = AcquireAsyncEventArgs();
			args.RemoteEndPoint = endpoint;
			args.SetBuffer(data, offset, count);
			args.UserToken = new Completion(Interlocked.Increment(ref __totalSequence), callback);

			bool willRaiseEvent = sock.SendToAsync(args);
			if (!willRaiseEvent)
			{
				IO_Completed(this, args);
			}
		}

		private Socket CheckedSocket()
		{
			if (IsDisposed) throw new ObjectDisposedException("Already disposed");

			Thread.MemoryBarrier();
			var sock = _socket;
			Thread.MemoryBarrier();
			if (sock == null)
				throw new InvalidOperationException("Not initialized");
			return sock;
		}

		/// <summary>
		/// Acquires async event args and attaches it to this object's
		/// IO_Completed method.
		/// </summary>
		/// <returns></returns>
		protected virtual SocketAsyncEventArgs AcquireAsyncEventArgs()
		{
			SocketAsyncEventArgs args = new SocketAsyncEventArgs();
			args.Completed += IO_Completed;
			return args;
		}

		/// <summary>
		/// Releases an async event args previously acquired.
		/// </summary>
		/// <param name="args"></param>
		public virtual void ReleaseAsyncEventArgs(SocketAsyncEventArgs args)
		{
			args.Completed -= IO_Completed;
			args.Dispose();
		}

		class Completion
		{
			public Action<Exception, OverlappedOpState> Callback;
			public int Sequence;

			internal Completion(int sequence, Action<Exception, OverlappedOpState> cb)
			{
				Callback = cb;
				Sequence = sequence;
			}
		}

		void IO_Completed(object sender, SocketAsyncEventArgs e)
		{
			Completion compl = (Completion)e.UserToken;
			if (compl.Callback != null)
			{
				compl.Callback(null, new OverlappedOpState(this, e, compl.Sequence, Interlocked.Increment(ref __totalSequence)));
			}
		}

		/// <summary>
		/// Performs this object's disposal.
		/// </summary>
		/// <param name="disposing"></param>
		/// <returns></returns>
		protected override bool PerformDispose(bool disposing)
		{
			Thread.MemoryBarrier();
			var sock = _socket;
			Thread.MemoryBarrier();
			if (sock != null)
			{
				if (Interlocked.CompareExchange(ref _socket, null, sock) == sock)
				{
					if (sock.Connected)
					{
						try { sock.Close(); }
						catch (IOException) { /* error eaten on purpose; may be called from the GC */ }
					}

					try { sock.Dispose(); }
					catch (ObjectDisposedException) { /* error eaten on purpose; may be called from the GC */ }
				}
			}
			return disposing && sock != null;
		}
	}

}
