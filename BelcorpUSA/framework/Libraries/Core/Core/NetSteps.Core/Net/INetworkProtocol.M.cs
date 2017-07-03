using System;
using System.Net;

namespace NetSteps.Encore.Core.Net
{
	/// <summary>
	/// Indicates a decoded message's result (kind)
	/// </summary>
	public enum DecodeMessageResultKind
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,
		/// <summary>
		/// Decoding was successful.
		/// </summary>
		Success = 1,
		/// <summary>
		/// Partially decoded, needs more data.
		/// </summary>
		Partial = 2,
		/// <summary>
		/// Message was garbage.
		/// </summary>
		Garbage = 100,
		/// <summary>
		/// Indicates the endpoint should be blacklisted.
		/// </summary>
		Blacklist = 101
	}
	/// <summary>
	/// Result of an attempt to decode a message.
	/// </summary>
	/// <typeparam name="M">message type M</typeparam>
	public class DecodeMessageResult<M>
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="kind">the kind</param>
		/// <param name="message">the message</param>
		public DecodeMessageResult(DecodeMessageResultKind kind, M message)
		{
			Kind = kind;
			Message = message;
		}
		/// <summary>
		/// Gets the result's kind.
		/// </summary>
		public DecodeMessageResultKind Kind { get; private set; }
		/// <summary>
		/// Gets the result's message.
		/// </summary>
		public M Message { get; private set; }
	}
	/// <summary>
	/// Interface for network protocols of message type M
	/// </summary>
	/// <typeparam name="M">message type M</typeparam>
	public interface INetworkProtocol<M>
	{
		/// <summary>
		/// Indicates whether the protocol is full duplex.
		/// </summary>
		bool IsFullDuplex { get; }
		/// <summary>
		/// Indicates whether the protocol instance is thread-safe.
		/// </summary>
		bool IsThreadSafe { get; }

		/// <summary>
		/// Initialized the protocol instance on an endpoint.
		/// </summary>
		/// <param name="endpoint"></param>
		void Initialize(INetEndpoint endpoint);

		/// <summary>
		/// Tries to decode a received message.
		/// </summary>
		/// <param name="endpoint">the source endpoint</param>
		/// <param name="buffer">a buffer containing received bytes</param>
		/// <param name="offset">offset to first received byte</param>
		/// <param name="length">number of useable bytes in the buffer</param>
		/// <returns>message result</returns>
		DecodeMessageResult<M> TryDecodeMessage(IPEndPoint endpoint, byte[] buffer, int offset, int length);
		/// <summary>
		/// Encodes a message.
		/// </summary>
		/// <param name="message">the message object</param>
		/// <returns>the message's bytes</returns>
		byte[] EncodeMessage(M message);

		/// <summary>
		/// Handles endpoint messages that have been successfully decoded.
		/// </summary>
		/// <param name="endpoint">endpoint</param>
		/// <param name="timestamp">timestamp of the message's arrival</param>
		/// <param name="message">the message</param>
		void HandleEndpointMessage(IPEndPoint endpoint, DateTime timestamp, M message);
	}
}
