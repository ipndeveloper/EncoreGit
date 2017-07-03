using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetSteps.Encore.Core.Net
{
	/// <summary>
	/// Encapsulates the state of an operation that occurred
	/// in parallel on an EndPoint.
	/// </summary>
	public sealed class OverlappedOpState
	{
		readonly SocketAsyncEventArgs _socketArgs;
		readonly INetEndpoint _sourceEndpoint;
		int _returnedArgsToSource = 0;
		int _sequence;
		int _totalSequence;

		internal OverlappedOpState(INetEndpoint sourceEndpoint, SocketAsyncEventArgs args, int sequence, int totalSequence)
		{
			_sourceEndpoint = sourceEndpoint;
			_socketArgs = args;
			_sequence = sequence;
			_totalSequence = totalSequence;
		}

		/// <summary>
		/// Accesses the raw byte buffer containing data. For SendXXX operations this
		/// is the data sent; for ReceiveXXX operations this is the data received.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819", Justification = "By design; accesses the underlying SocketAsyncEventArgs's buffer.")]
		public byte[] Buffer
		{
			get { return _socketArgs.Buffer; }
		}

		/// <summary>
		/// Number of bytes used in the operation.
		/// </summary>
		public int BytesTransferred
		{
			get { return _socketArgs.BytesTransferred; }
		}

		/// <summary>
		/// Offset within the buffer where valid data begins. WARNING: May not be the beginning of the buffer.
		/// </summary>
		public int Offset
		{
			get { return _socketArgs.Offset; }
		}

		/// <summary>
		/// Identifies the operation that was performed.
		/// </summary>
		/// <see cref="System.Net.Sockets.SocketAsyncOperation"/>
		public SocketAsyncOperation Operation
		{
			get { return _socketArgs.LastOperation; }
		}

		/// <summary>
		/// Identifies the remote side of the communication.
		/// </summary>
		public EndPoint RemoteEndPoint
		{
			get { return _socketArgs.RemoteEndPoint; }
		}

		/// <summary>
		/// Identifies the socket error. NOTE: SocketError.Success indicates no error occurred!
		/// </summary>
		public SocketError SocketError
		{
			get { return _socketArgs.SocketError; }
		}

		/// <summary>
		/// Ends the asynchronous operation. Allows the framework to
		/// recycle internal structures allocated for the op.
		/// </summary>
		public void EndAsyncOp()
		{
			if (Interlocked.Increment(ref _returnedArgsToSource) == 0)
			{
				_sourceEndpoint.ReleaseAsyncEventArgs(_socketArgs);
			}
		}
	}

}
