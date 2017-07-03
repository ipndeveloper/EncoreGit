﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;


namespace NetSteps.Encore.Core.Reflection.Emit
{
	/// <summary>
	/// Helper class for working with methods in the IL stream.
	/// </summary>
	public class EmittedMethod : EmittedMethodBase
	{
		/// <summary>
		/// MethodAttributes for public interface implementations.
		/// </summary>
		public static readonly MethodAttributes PublicInterfaceImplementationAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;

		MethodInfo _baseMethod;
		MethodBuilder _builder;
		TypeRef _returnType;
		Type[] _parameterTypes;

		/// <summary>
		/// Creates a new instace.
		/// </summary>
		/// <param name="type">owning type</param>
		/// <param name="name">the method's name</param>
		public EmittedMethod(EmittedClass type, string name)
			: base(type, name)
		{
			this.CallingConvention = CallingConventions.Standard | CallingConventions.HasThis;
		}

		/// <summary>
		/// Creates a new instance based on the given method info.
		/// </summary>
		/// <param name="type">owning type</param>
		/// <param name="method">method info describing the method to emit</param>
		public EmittedMethod(EmittedClass type, MethodInfo method)
			: this(type, method, false)
		{
		}

		/// <summary>
		/// Creates a new instance that overrides the given method.
		/// </summary>
		/// <param name="type">owning type</param>
		/// <param name="method">method info describing the method to emit</param>
		/// <param name="isOverride">indicates whether the emitted method will override the given method</param>
		public EmittedMethod(EmittedClass type, MethodInfo method, bool isOverride)
			: base(type, method.Name)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(method != null);
			
			if (isOverride) _baseMethod = method;
			if (method.IsGenericMethod || method.IsGenericMethodDefinition)
			{
				var attributes = method.Attributes & ~(MethodAttributes.Abstract | MethodAttributes.VtableLayoutMask);
				var parameters = method.GetParameters();
				_parameterTypes = ParameterHelper.GetParameterTypes(method, parameters);
				_builder = type.Builder.DefineMethod(method.Name, attributes);

				this.GenericArgumentTypes = method.GetGenericArguments();
				var genericTypeParameters = GetGenericTypeParameters(_builder, method.GetGenericArguments());

				ParameterHelper.SetUpParameterConstraints(_parameterTypes, genericTypeParameters);

				var returnType = SetUpReturnType(method, _builder, genericTypeParameters);				
				this._returnType = new TypeRef(returnType);

				int i = 0;
				foreach (var p in ParameterHelper.SetUpParameters(_parameterTypes, parameters, _builder))
				{
					base.AddParameter(new EmittedParameter(p, _parameterTypes[i++]));
				}
			}
			else
			{
				var attributes = method.Attributes & ~(MethodAttributes.Abstract | MethodAttributes.VtableLayoutMask);
				var parameters = method.GetParameters();
				_parameterTypes = ParameterHelper.GetParameterTypes(method, parameters);

				_builder = type.Builder.DefineMethod(method.Name, attributes);
				
				var returnType = SetUpReturnType(method, _builder, null);
				this._returnType = new TypeRef(returnType);

				int i = 0;
				foreach (var p in ParameterHelper.SetUpParameters(_parameterTypes, parameters, _builder))
				{
					base.AddParameter(new EmittedParameter(p, _parameterTypes[i++]));
				}
			}
		}

		/// <summary>
		/// Gets a method's parameter types.
		/// </summary>
		public override Type[] ParameterTypes
		{
			get
			{
				return (_parameterTypes == null) ? base.ParameterTypes : _parameterTypes;
			}
		}

		/// <summary>
		/// Gets the method's 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="genericArguments"></param>
		/// <returns></returns>
		private static GenericTypeParameterBuilder[] GetGenericTypeParameters(MethodBuilder builder, 
			Type[] genericArguments)
		{
			return builder.DefineGenericParameters((from n in genericArguments
																										select n.Name).ToArray());
		}

		private static Type SetUpReturnType(MethodInfo method, MethodBuilder builder, GenericTypeParameterBuilder[] genericTypeParameters)
		{
			Type result = typeof(void);
			if (method.ReturnType != null)
			{
				if (method.IsGenericMethodDefinition && method.ReturnType.IsGenericType)
				{
					result = genericTypeParameters.Where(x => x.Name == method.ReturnType.Name).First();
				}
				else
				{
					result = method.ReturnType;
				}
			}
			builder.SetReturnType(result);
			return result;
		}

		/// <summary>
		/// Indicates whether the method has a builder.
		/// </summary>
		protected override bool HasBuilder
		{
			get { return _builder != null; }
		}

		/// <summary>
		/// Gets the method's builder.
		/// </summary>
		public MethodBuilder Builder
		{
			get
			{
				if (_builder == null)
				{
					_builder = this.TargetClass.Builder.DefineMethod(this.Name
					, this.Attributes
					, this.CallingConvention
					, (this.ReturnType != null) ? this.ReturnType.Target : null
					, this.ParameterTypes
					);
				}
				return _builder;
			}
		}

		/// <summary>
		/// Gets a reference to the method's return type.
		/// </summary>
		public TypeRef ReturnType
		{
			get { return _returnType; }
			set
			{
				Contract.Assert(_builder == null, "ReturnType must be set before the MethodBuilder is created");
				_returnType = value;
			}
		}

		/// <summary>
		/// Emits a instructions to call the method.
		/// </summary>
		/// <param name="il"></param>
		public override void EmitCall(ILGenerator il)
		{
			il.Call(Builder);
		}

		internal void CheckOverrideOnCompile(MethodInfo methodInfo)
		{
			_baseMethod = methodInfo;
		}

		/// <summary>
		/// Compiles the method.
		/// </summary>
		protected internal override void OnCompile()
		{
			ILGenerator il = SetILGenerator(Builder.GetILGenerator());
			try
			{
				CompileParameters(Builder);
				CompileLocals(il);

				EmitInstructions(il);
				if (_baseMethod != null)
				{
					if (_baseMethod.IsAbstract)
					{
						this.TargetClass.Builder.DefineMethodOverride(Builder, _baseMethod);
					}
				}
				il.Return();
			}
			finally
			{
				SetILGenerator(null);
			}
		}

		/// <summary>
		/// Gets the method's generic argument types.
		/// </summary>
		public virtual Type[] GenericArgumentTypes { get; set; }

	}
}