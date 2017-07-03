using System;
using System.Net;
using System.Net.Sockets;

namespace NetSteps.Encore.Core.Net
{
	/// <summary>
	/// Interface for sending and receiving data on a network endpoint.
	/// </summary>
	public interface INetEndpoint : IDisposable
	{
		/// <summary>
		/// Receives data from an endpoint.
		/// </summary>
		/// <param name="endpoint">reference to an endpoint.</param>
		/// <param name="data">buffer where the incomming data will be stored</param>
		/// <param name="offset">first byte within the buffer where data will be stored</param>
		/// <param name="count">number of bytes available in the buffer</param>
		/// <returns>number of bytes received</returns>
		int ReceiveFrom(ref EndPoint endpoint, byte[] data, int offset, int count);
		/// <summary>
		/// Receives data from an endpoint.
		/// </summary>
		/// <param name="endpoint">an endpoint.</param>
		/// <param name="data">buffer where the incomming data will be stored</param>
		/// <param name="offset">first byte within the buffer where data will be stored</param>
		/// <param name="count">number of bytes available in the buffer</param>
		/// <param name="callback">action called when the receive is complete</param>
		void ReceiveFromAsync(EndPoint endpoint, byte[] data, int offset, int count, Action<Exception, OverlappedOpState> callback);
		/// <summary>
		/// Sends data to an endpoint.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		void SendTo(EndPoint endpoint, byte[] data);
		/// <summary>
		/// Sends data to an endpoint.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		void SendTo(EndPoint endpoint, byte[] data, int count);
		/// <summary>
		/// Sends data to an endpoint.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="flags"></param>
		void SendTo(EndPoint endpoint, byte[] data, int offset, int count, SocketFlags flags);
		/// <summary>
		/// Asynchronously sends data to an endpoint.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		/// <param name="callback"></param>
		void SendToAsync(EndPoint endpoint, byte[] data, int count, Action<Exception, OverlappedOpState> callback);
		/// <summary>
		/// Asynchronously sends data to an endpoint.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="callback"></param>
		void SendToAsync(EndPoint endpoint, byte[] data, int offset, int count, Action<Exception, OverlappedOpState> callback);

		/// <summary>
		/// This method is used by the framework to release/reuse SocketAsyncEventArgs. You should not normally
		/// call this method..
		/// </summary>
		/// <param name="socketArgs"></param>
		void ReleaseAsyncEventArgs(SocketAsyncEventArgs socketArgs);

		/// <summary>
		/// Gets the underlying endpoint.
		/// </summary>
		IPEndPoint EndPoint { get; }
	}
}
