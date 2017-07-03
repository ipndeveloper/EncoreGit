
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace NetSteps.Encore.Core.IoC.Constructors
{
	/// <summary>
	/// Gets a contructor set for a type.
	/// </summary>
	/// <typeparam name="T">target type T</typeparam>
	/// <typeparam name="C">concrete type C</typeparam>
	public sealed class ConstructorSet<T, C>
		where C : class, T
	{
		readonly Lazy<ConstructorCommand<T>[]> _constructors;

		readonly Param[] _parameters;
		ConstructorCommand<T> _default;
		ConstructorCommand<T> _mostRecent;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="parameters"></param>
		public ConstructorSet(Param[] parameters)
		{
			_parameters = parameters;
			_constructors = new Lazy<ConstructorCommand<T>[]>(BuildConstructorCommands, LazyThreadSafetyMode.ExecutionAndPublication);
		}

        ConstructorCommand<T> MRU {
            get
            {
                Thread.MemoryBarrier();
                var mru = _mostRecent;
                Thread.MemoryBarrier();
                return mru;
            }
            set
            {
                Thread.MemoryBarrier();
                _mostRecent = value;
                Thread.MemoryBarrier();                
            }
        }


		internal bool TryMatchAndBind(Param[] parameters, out CommandBinding<T> command)
		{
			var constructors = _constructors.Value;

			if ((parameters == null || parameters.Length == 0) && _default != null)
			{
				return _default.TryMatchAndBind(Param.EmptyParams, out command);
			}

            var mru = MRU;
            if (mru != null && mru.ParameterCount == parameters.Length)
			{
				if (mru.TryMatchAndBind(parameters, out command))
				{
					return true;
				}
			}

			var plen = (parameters != null) ? parameters.Length : 0;
			foreach (var c in constructors.Where(cc => cc.ParameterCount == plen))
			{
				if (c.TryMatchAndBind(parameters, out command))
				{
					MRU = c;
					return true;
				}
			}
			command = null;
			return false;
		}

		ConstructorCommand<T>[] BuildConstructorCommands()
		{
			var ord = 0;
			var result = (from c in typeof(C).GetConstructors(BindingFlags.Instance | BindingFlags.Public)
										let parms = c.GetParameters()
										orderby parms.Count()
										select new ConstructorCommand<T, C>(c, _parameters, ord++)).ToArray<ConstructorCommand<T>>();

			_default = result.FirstOrDefault(c => c.BoundToSuppliedDefaults);
			if (_default == null)
				_default = result.FirstOrDefault(c => c.ParameterCount == 0);

			return result;
		}
	}
}
