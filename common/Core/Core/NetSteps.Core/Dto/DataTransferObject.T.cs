﻿using System;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Properties;
using NetSteps.Encore.Core.Dto.SPI;

namespace NetSteps.Encore.Core.Dto
{
	/// <summary>
	/// Base class for IDataTransferObject implementations generated by the framework.
	/// </summary>
	/// <typeparam name="T">interface type T</typeparam>
	[Serializable]
	public abstract partial class DataTransferObject<T>
	{
		bool _writable = true;

		/// <summary>
		/// Called by the framework to mark an DataTransferObject as readonly.
		/// </summary>
		public void MarkReadonly()
		{
			_writable = false;
		}

		/// <summary>
		/// Ensures the instance is writable.
		/// </summary>    
		protected void CheckWriteOnce()
		{
			if (!_writable) throw new InvalidOperationException("Data transfer objects are immutable.");
		}

		internal static T Mutate(IContainer container, T source, Action<T> mutator)
		{
			T result = container.NewImplementationOf<T>(LifespanTracking.Automatic, DataTransfer.ConcreteType<T>());
			var dto = source as DataTransferObject<T>;
			if (dto != null)
			{
				((IDataTransferObjectSPI<T>)result).CopyState(dto);
			}
			else
			{
				((IDataTransferObjectSPI<T>)result).CopySource(source);
			}
			mutator(result);
			return result;
		}

		internal static T Copy(IContainer container, T source)
		{
			T result = container.NewImplementationOf<T>(LifespanTracking.Automatic, DataTransfer.ConcreteType<T>());
			var dto = source as DataTransferObject<T>;
			if (dto != null)
			{
				((IDataTransferObjectSPI<T>)result).CopyState(dto);
			}
			else
			{
				((IDataTransferObjectSPI<T>)result).CopySource(source);
			}
			return result;
		}
	}	
}