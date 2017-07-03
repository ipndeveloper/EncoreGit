using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Buffers;
using System.Diagnostics.Contracts;

namespace NetSteps.Core.Cache.CacheNet
{
	/// <summary>
	/// CachNet message indicating cache expirations originating elsewhere on the 
	/// network.
	/// </summary>
	[CLSCompliant(false)]
	public class ExpirationByIdMessage : CacheNetMessage
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public ExpirationByIdMessage(CacheNetMessageHeader header) : base(header) 
		{
			Contract.Requires<InvalidOperationException>(header.MessageKind == CacheNetMessageKind.ExpirationById);
		}

		/// <summary>
		/// Gets the expiration's identity kind.
		/// </summary>
		public CacheNetIdentityKind IdentityKind { get; private set; }
		/// <summary>
		/// Gets the expired identities (corresponding to CacheNetIdentityKind.Int32)
		/// </summary>
		public IEnumerable<int> Int32Identities { get; private set; }
		/// <summary>
		/// Gets the expired identities (corresponding to CacheNetIdentityKind.Int64)
		/// </summary>
		public IEnumerable<long> Int64Identities { get; private set; }
		/// <summary>
		/// Gets the expired identities (corresponding to CacheNetIdentityKind.String)
		/// </summary>
		public IEnumerable<string> StringIdentities { get; private set; }
		/// <summary>
		/// Gets the expired identities (corresponding to CacheNetIdentityKind.Guid)
		/// </summary>
		public IEnumerable<Guid> GuidIdentities { get; private set; }

		/// <summary>
		/// Reads the message from the source buffer given.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="source"></param>
		/// <param name="offset"></param>
		protected override void PerformReadFromBuffer(IBufferReader reader, byte[] source, ref int offset)
		{
			var idKind = (CacheNetIdentityKind)reader.ReadInt32(source, ref offset);
			IdentityKind = idKind;
			switch (idKind)
			{
				case CacheNetIdentityKind.Int32:
					this.Int32Identities = CacheNetProtocol.ReadInt32Identities(reader, source, ref offset);
					break;
				case CacheNetIdentityKind.Int64:
					this.Int64Identities = CacheNetProtocol.ReadInt64Identities(reader, source, ref offset);
					break;
				case CacheNetIdentityKind.String:
					this.StringIdentities = CacheNetProtocol.ReadStringIdentities(reader, source, ref offset);
					break;
				case CacheNetIdentityKind.Guid:
					this.GuidIdentities = CacheNetProtocol.ReadGuidIdentities(reader, source, ref offset);
					break;
				default:
					throw new InvalidOperationException(String.Concat("Unrecognized identity kind: ", idKind));
			}
		}

		/// <summary>
		/// Writes the expirations to the target buffer.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="target"></param>
		/// <param name="offset"></param>
		protected override void PerformWriteToBuffer(IBufferReader reader, byte[] target, ref int offset)
		{
			throw new NotImplementedException();
		}
	}
}
