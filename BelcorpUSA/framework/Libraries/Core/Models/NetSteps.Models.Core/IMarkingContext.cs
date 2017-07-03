using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Interface for contexts used to avoid cycles when
	/// marking Models.
	/// </summary>
	public interface IMarkingContext
	{
		/// <summary>
		/// Tries to add a target model.
		/// </summary>
		/// <typeparam name="M">target type M.</typeparam>
		/// <param name="model">the model being marked</param>
		/// <returns><em>true</em> if the model was added; otherwise <em>false</em>. 
		/// A result of <em>true</em> indicates the caller is safe to cascade the mark to the model;
		/// otherwise it is already marked and a subsequent call may cause a cycle.</returns>
		bool TryAddMark<M>(M model);
	}

	/// <summary>
	/// Default marking context.
	/// </summary>
	public sealed class MarkingContext : IMarkingContext
	{
		ConcurrentDictionary<object, object> _marks = new ConcurrentDictionary<object, object>();

		/// <summary>
		/// Tries to add a target model.
		/// </summary>
		/// <typeparam name="M">target type M.</typeparam>
		/// <param name="model">the model being marked</param>
		/// <returns><em>true</em> if the model was added; otherwise <em>false</em>. 
		/// A result of <em>true</em> indicates the caller is safe to cascade the mark to the model;
		/// otherwise it is already marked and a subsequent call may cause a cycle.</returns>
		public bool TryAddMark<M>(M model)
		{
			return _marks.TryAdd(model, model);
		}
	}
}
