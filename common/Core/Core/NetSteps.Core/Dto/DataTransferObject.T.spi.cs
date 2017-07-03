using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.Dto.SPI;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Dto
{
	public abstract partial class DataTransferObject<T> : IDataTransferObjectSPI<T>
	{
		/// <summary>
		/// Copies state from another instance.
		/// </summary>
		/// <param name="other">the other instance</param>
		public void CopyState(DataTransferObject<T> other)
		{
			Contract.Assert(other != null, Resources.Chk_CannotBeNull);
			PerformCopyState(other);
		}

		/// <summary>
		/// Copies state from another instance.
		/// </summary>
		/// <param name="other">the other instance.</param>
		public void CopySource(T other)
		{
			Contract.Assert(other != null, Resources.Chk_CannotBeNull);
			PerformCopySource(other);
		}

		/// <summary>
		/// Overriden by subclasses to copy the state from another instance.
		/// </summary>
		/// <param name="other">the other instance</param>
		protected abstract void PerformCopySource(T other);

		/// <summary>
		/// Overriden by subclasses to copy the state from another instance.
		/// </summary>
		/// <param name="other">the other instance</param>
		protected abstract void PerformCopyState(DataTransferObject<T> other);

	}

	namespace SPI
	{
		/// <summary>
		/// DTO Service Provider Interface; used internally by the framework.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		public interface IDataTransferObjectSPI<T>
		{
			/// <summary>
			/// Copies state from another instance.
			/// </summary>
			/// <param name="other">the other instance</param>
			void CopyState(DataTransferObject<T> other);
			/// <summary>
			/// Copies state from another instance.
			/// </summary>
			/// <param name="other">the other instance.</param>
			void CopySource(T other);
		}
	}
}
