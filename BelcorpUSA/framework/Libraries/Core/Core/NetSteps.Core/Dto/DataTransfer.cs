using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using NetSteps.Encore.Core.Properties;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Reflection.Emit;

namespace NetSteps.Encore.Core.Dto
{
	internal static class DataTransfer
	{
		static readonly Lazy<EmittedModule> __module = new Lazy<EmittedModule>(() =>
		{ return RuntimeAssemblies.DynamicAssembly.DefineModule("DTO", null); },
			LazyThreadSafetyMode.ExecutionAndPublication
			);

		static EmittedModule Module { get { return __module.Value; } }

		#region Emit ConcreteType<T>

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		internal static Type ConcreteType<T>()
		{
			Contract.Ensures(Contract.Result<Type>() != null);

			var targetType = typeof(T);
			string typeName = RuntimeAssemblies.PrepareTypeName(targetType, "DTO");

			var module = DataTransfer.Module;
			lock (module)
			{
				Type type = module.Builder.GetType(typeName, false, false);
				if (type == null)
				{
					type = BuildDtoType<T>(module, typeName);
				}
				return type;
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		static Type BuildDtoType<T>(EmittedModule module, string typeName)
		{
			Contract.Requires(module != null, Resources.Chk_CannotBeNull);
			Contract.Requires(typeName != null, Resources.Chk_CannotBeNull);
			Contract.Requires(typeName.Length > 0, Resources.Chk_CannotBeEmpty);
			Contract.Ensures(Contract.Result<Type>() != null);

			var builder = module.DefineClass(typeName, EmittedClass.DefaultTypeAttributes, typeof(DataTransferObject<T>), null);
			builder.Attributes = TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.BeforeFieldInit;

			builder.Builder.SetCustomAttribute(new CustomAttributeBuilder(
				typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes), new object[0])
				);

			var chashCodeSeed = builder.DefineField<int>("CHashCodeSeed");
			chashCodeSeed.IncludeAttributes(FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.InitOnly);
			var cctor = builder.DefineCCtor();
			cctor.ContributeInstructions((m, il) =>
			{
				il.LoadType(builder.Builder);
				il.Call(typeof(Util).GetMethod("GetKeyForType"));
				il.CallVirtual<object>("GetHashCode");
				il.StoreField(chashCodeSeed);
			});

			builder.DefineDefaultCtor();

			var dataType = BackingDataType<T>();
			var data = builder.DefineField("_data", dataType);

			foreach (var intf in from type in typeof(T).GetTypeHierarchyInDeclarationOrder()
								 where type.IsInterface
								 select type)
			{
				builder.AddInterfaceImplementation(intf);
				ImplementPropertiesForInterface(intf, builder, data, dataType);
				builder.StubMethodsForInterface(intf, true, true);
			}
			ImplementSpecializedEquals(builder, data, dataType);
			ImplementSpecializedGetHashCode(builder, data, dataType, chashCodeSeed);
			ImplementCopyState<T>(builder, data, dataType);
			ImplementCopySource<T>(builder, data, dataType);
			builder.Compile();
			return builder.Ref.Target;
		}

		static void ImplementPropertiesForInterface(Type intf, EmittedClass builder, EmittedField data, Type dataType)
		{
			var properties = intf.GetProperties();
			foreach (var p in properties)
			{
				var property = p as PropertyInfo;
				ImplementPropertyFor(builder, property, data, dataType);
			}
		}

		static void ImplementPropertyFor(EmittedClass builder, PropertyInfo property, EmittedField data, Type dataType)
		{
			var prop = builder.DefinePropertyFromPropertyInfo(property);
			var fieldName = property.FormatBackingFieldName();
			var backingField = dataType.GetField(fieldName);

			prop.AddGetter(property.GetGetMethod()).ContributeInstructions((m, il) =>
			{
				il.LoadArg_0();
				il.LoadFieldAddress(data);
				il.LoadField(backingField);
			});
			if (property.CanWrite)
			{
				prop.AddSetter(property.GetSetMethod()).ContributeInstructions((m, il) =>
				{
					il.Nop();
					il.LoadArg_0();
					il.Call(builder.Builder.BaseType.GetMethod("CheckWriteOnce", BindingFlags.Instance | BindingFlags.NonPublic));
					il.Nop();
					il.LoadArg_0();
					il.LoadFieldAddress(data);
					il.LoadArg_1();
					il.StoreField(backingField);
				});
			}
		}

		static void ImplementCopyState<T>(EmittedClass builder, EmittedField data, Type dataType)
		{
			/*
			protected override void PerformCopyState(DataTransferObject<T> other) {
				if (other is <subtype>) {
					this._data = ((<subtyle>)other)._data;
			  }
			}			 
			*/
			var baseMethod = typeof(DataTransferObject<T>).GetMethod("PerformCopyState", BindingFlags.NonPublic | BindingFlags.Instance);
			var method = builder.DefineMethodFromInfo(baseMethod);

			var dto = method.DefineLocal("dto", builder.Builder);
			method.DefineLocal("ifnotnull", typeof(bool));
			method.ContributeInstructions((m, il) =>
			{
				var exit = il.DefineLabel();
				il.Nop();
				il.LoadArg_0();
				il.Call(builder.Builder.BaseType.GetMethod("CheckWriteOnce", BindingFlags.Instance | BindingFlags.NonPublic));
				il.Nop();
				il.LoadArg_1();
				il.IsInstance(builder.Builder);
				il.StoreLocal_0();
				il.LoadLocal_0();
				il.LoadNull();
				il.CompareEqual();
				il.StoreLocal_1();
				il.LoadLocal_1();
				il.BranchIfTrue_ShortForm(exit);
				il.Nop();
				il.LoadArg_0();
				il.LoadLocal_0();
				il.LoadField(data);
				il.StoreField(data);
				il.MarkLabel(exit);
			});
		}

		static void ImplementCopySource<T>(EmittedClass builder, EmittedField data, Type dataType)
		{
			var baseMethod = typeof(DataTransferObject<T>).GetMethod("PerformCopySource", BindingFlags.NonPublic | BindingFlags.Instance);
			var method = builder.DefineOverrideMethod(baseMethod);

			method.ContributeInstructions((m, il) =>
			{
				il.Nop();
				il.LoadArg_0();
				il.Call(builder.Builder.BaseType.GetMethod("CheckWriteOnce", BindingFlags.Instance | BindingFlags.NonPublic));


				foreach (var prop in from src in typeof(T).GetReadableProperties()
									 join dest in dataType.GetFields()
									 on src.FormatBackingFieldName() equals dest.Name
									 select new
									 {
										 Source = src,
										 Destination = dest
									 })
				{
					//
					// result.<backing_data>.<property-name> = src.<property-name>;
					//
					//il.LoadArg_1();
					//il.LoadArg_2();
					//il.CallVirtual(prop.Source.GetGetMethod());
					//il.CallVirtual(prop.Destination.GetSetMethod());
					il.Nop();
				}
			});
		}

		static void ImplementSpecializedEquals(EmittedClass builder, EmittedField data, Type dataType)
		{
			Contract.Requires(builder != null, Resources.Chk_CannotBeNull);
			Contract.Requires(data != null, Resources.Chk_CannotBeNull);
			Contract.Requires(dataType != null, Resources.Chk_CannotBeNull);

			Type equatable = typeof(IEquatable<>).MakeGenericType(builder.Builder);
			builder.AddInterfaceImplementation(equatable);

			var specialized_equals = builder.DefineMethod("Equals");
			specialized_equals.ClearAttributes();
			specialized_equals.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual);
			specialized_equals.ReturnType = TypeRef.FromType<bool>();
			specialized_equals.DefineParameter("other", builder.Ref);

			specialized_equals.ContributeInstructions((m, il) =>
			{
				var exitFalse = il.DefineLabel();
				var exit = il.DefineLabel();

				il.DeclareLocal(typeof(bool));
				il.LoadArg_0();
				il.LoadFieldAddress(data);
				il.LoadArg_1();
				il.LoadField(data);
				il.Call(dataType.GetMethod("Equals", new Type[] { dataType }));
				il.StoreLocal_0();
				il.DefineAndMarkLabel();
				il.LoadLocal_0();
			});

			builder.DefineOverrideMethod(typeof(object).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(object) }, null))
				.ContributeInstructions((m, il) =>
				{
					var exitFalse = il.DefineLabel();
					var exit = il.DefineLabel();
					il.DeclareLocal(typeof(bool));
					il.LoadToken(builder.Builder);
					il.Call<Type>("GetTypeFromHandle", BindingFlags.Public | BindingFlags.Static, typeof(RuntimeTypeHandle));
					il.LoadArg_1();
					il.CallVirtual<Type>("IsInstanceOfType");
					il.BranchIfFalse(exitFalse);
					il.LoadArg_0();
					il.LoadArg_1();
					il.CastClass(builder.Builder);
					il.Call(specialized_equals);
					il.Branch(exit);
					il.MarkLabel(exitFalse);
					il.LoadValue(false);
					il.MarkLabel(exit);
					il.StoreLocal_0();
					il.DefineAndMarkLabel();
					il.LoadLocal_0();
				});
		}

		static EmittedMethod ImplementSpecializedGetHashCode(EmittedClass builder, EmittedField data, Type dataType, EmittedField chashCodeSeed)
		{
			Contract.Requires<ArgumentNullException>(builder != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(data != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(chashCodeSeed != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(dataType != null, Resources.Chk_CannotBeNull);

			var method = builder.DefineOverrideMethod(typeof(Object).GetMethod("GetHashCode", BindingFlags.Instance | BindingFlags.Public));
			method.ContributeInstructions((m, il) =>
			{
				var prime = il.DeclareLocal(typeof(Int32));
				var result = il.DeclareLocal(typeof(Int32));
				var exit = il.DefineLabel();
				il.DeclareLocal(typeof(Int32));
				il.Nop();
				il.LoadValue(0xf3e9b);
				il.StoreLocal(prime);
				il.LoadValue(chashCodeSeed);
				il.LoadLocal(prime);
				il.Multiply();
				il.StoreLocal(result);
				il.LoadLocal(result);
				il.LoadLocal(prime);
				il.LoadArg_0();
				il.LoadFieldAddress(data);
				il.Constrained(dataType);
				il.CallVirtual<object>("GetHashCode");
				il.Multiply();
				il.Xor();
				il.StoreLocal(result);
				il.LoadLocal(result);
				il.StoreLocal_2();
				il.Branch(exit);
				il.MarkLabel(exit);
				il.LoadLocal_2();
			});
			return method;
		}

		#endregion

		#region Emit BackingDataType<T>

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		internal static Type BackingDataType<T>()
		{
			Contract.Ensures(Contract.Result<Type>() != null);

			var targetType = typeof(T);
			string typeName = RuntimeAssemblies.PrepareTypeName(targetType, "DTO$Data");

			lock (targetType.GetLockForType())
			{
				var module = Module;
				Type type = module.Builder.GetType(typeName, false, false);
				if (type == null)
				{
					type = BuildBackingDataType<T>(module, typeName);
				}
				return type;
			}
		}

		static Type BuildBackingDataType<T>(EmittedModule module, string typeName)
		{
			Contract.Requires(module != null, Resources.Chk_CannotBeNull);
			Contract.Requires(typeName != null, Resources.Chk_CannotBeNull);
			Contract.Requires(typeName.Length > 0, Resources.Chk_CannotBeEmpty);
			Contract.Ensures(Contract.Result<Type>() != null);

			var builder = module.DefineClass(
				typeName,
				EmittedClass.DefaultTypeAttributes,
				typeof(ValueType),
				null
				);
			builder.Attributes = TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.BeforeFieldInit;

			builder.Builder.SetCustomAttribute(new CustomAttributeBuilder(
				typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes), new object[0])
				);

			var chashCodeSeed = builder.DefineField<int>("CHashCodeSeed");
			chashCodeSeed.IncludeAttributes(FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.InitOnly);
			var cctor = builder.DefineCCtor();
			cctor.ContributeInstructions((m, il) =>
			{
				il.LoadType(builder.Builder);
				il.Call(typeof(Util).GetMethod("GetKeyForType"));
				il.CallVirtual<object>("GetHashCode");
				il.StoreField(chashCodeSeed);
			});

			foreach (var intf in from type in typeof(T).GetTypeHierarchyInDeclarationOrder()
								 where type.IsInterface
								 select type)
			{
				AddFieldsForPropertyValues(builder, intf);
			}
			var equality = ImplementSpecializedDataTypeEquals(builder);
			ImplementSpecializedDataTypeGetHashCode(builder, chashCodeSeed);
			ImplementEqualityOperators(builder, equality);
			ImplementInequalityOperators(builder, equality);

			builder.Compile();
			return builder.Ref.Target;
		}

		static void AddFieldsForPropertyValues(EmittedClass builder, Type intf)
		{
			foreach (var p in intf.GetReadableProperties())
			{
				EmittedField field;
				var fieldName = p.FormatBackingFieldName();
				field = builder.DefineField(fieldName, p.PropertyType);
				field.ClearAttributes();
				field.IncludeAttributes(FieldAttributes.Public);
			}
		}

		static EmittedMethod ImplementSpecializedDataTypeGetHashCode(EmittedClass builder, EmittedField chashCodeSeed)
		{
			Contract.Requires<ArgumentNullException>(builder != null, Resources.Chk_CannotBeNull);

			var method = builder.DefineOverrideMethod(typeof(ValueType).GetMethod("GetHashCode", BindingFlags.Instance | BindingFlags.Public));
			method.ContributeInstructions((m, il) =>
			{
				var prime = il.DeclareLocal(typeof(Int32));
				var result = il.DeclareLocal(typeof(Int32));
				il.DeclareLocal(typeof(Int32));
				il.DeclareLocal(typeof(bool));
				il.Nop();
				il.LoadValue(Constants.RandomPrime);
				il.StoreLocal(prime);
				il.LoadValue(chashCodeSeed);
				il.LoadLocal(prime);
				il.Multiply();
				il.StoreLocal(result);
				var exit = il.DefineLabel();
				var fields = new List<EmittedField>(builder.Fields.Where(f => f.IsStatic == false));
				for (int i = 0; i < fields.Count; i++)
				{
					var field = fields[i];
					var fieldType = field.FieldType.Target;
					var tc = Type.GetTypeCode(fieldType);
					switch (tc)
					{
						case TypeCode.Boolean:
						case TypeCode.Byte:
						case TypeCode.Char:
						case TypeCode.Int16:
						case TypeCode.Int32:
						case TypeCode.SByte:
						case TypeCode.Single:
						case TypeCode.UInt16:
						case TypeCode.UInt32:
							il.LoadLocal(result);
							il.LoadLocal(prime);
							il.LoadArg_0();
							il.LoadField(field);
							il.Multiply();
							il.Xor();
							il.StoreLocal(result);
							break;
						case TypeCode.DateTime:
							il.LoadLocal(result);
							il.LoadLocal(prime);
							il.LoadArg_0();
							il.LoadFieldAddress(field);
							il.Constrained(typeof(DateTime));
							il.CallVirtual<object>("GetHashCode");
							il.Multiply();
							il.Xor();
							il.StoreLocal(result);
							break;
						case TypeCode.Decimal:
							il.LoadLocal(result);
							il.LoadLocal(prime);
							il.LoadArg_0();
							il.LoadFieldAddress(field);
							il.Call<Decimal>("GetHashCode");
							il.Multiply();
							il.Xor();
							il.StoreLocal(result);
							break;
						case TypeCode.Double:
							il.LoadLocal(result);
							il.LoadLocal(prime);
							il.LoadArg_0();
							il.LoadFieldAddress(field);
							il.Call<Double>("GetHashCode");
							il.Multiply();
							il.Xor();
							il.StoreLocal(result);
							break;
						case TypeCode.Int64:
							il.LoadLocal(result);
							il.LoadLocal(prime);
							il.LoadArg_0();
							il.LoadFieldAddress(field);
							il.Constrained(typeof(Int64));
							il.CallVirtual<object>("GetHashCode");
							il.Multiply();
							il.Xor();
							il.StoreLocal(result);
							break;
						case TypeCode.Object:
							if (typeof(Guid).IsAssignableFrom(fieldType))
							{
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadFieldAddress(field);
								il.Constrained(typeof(Guid));
								il.CallVirtual<object>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
							}
							else if (fieldType.IsArray)
							{
								var elmType = fieldType.GetElementType();
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadField(field);
								il.LoadLocal(result);
								il.Call(typeof(NetSteps.Encore.Core.Extensions).GetMethod("CalculateCombinedHashcode", BindingFlags.Public | BindingFlags.Static)
									.MakeGenericMethod(elmType));
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
							}
							else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
							{
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadFieldAddress(field);
								il.Constrained(fieldType);
								il.CallVirtual<object>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
							}
							else
							{
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadField(field);
								il.CallVirtual<object>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
							}
							break;
						case TypeCode.String:
							il.LoadArg_0();
							il.LoadField(field);
							il.Call<string>("IsNullOrEmpty");
							il.StoreLocal_3();
							il.LoadLocal_3();
							var lbl = il.DefineLabel();
							il.BranchIfTrue_ShortForm(lbl);
							il.LoadLocal(result);
							il.LoadLocal(prime);
							il.LoadArg_0();
							il.LoadField(field);
							il.CallVirtual<object>("GetHashCode");
							il.Multiply();
							il.Xor();
							il.StoreLocal(result);
							il.MarkLabel(lbl);
							break;
						case TypeCode.UInt64:
							il.LoadLocal(result);
							il.LoadLocal(prime);
							il.LoadArg_0();
							il.LoadFieldAddress(field);
							il.Constrained(typeof(UInt64));
							il.CallVirtual<object>("GetHashCode");
							il.Multiply();
							il.Xor();
							il.StoreLocal(result);
							break;
						default:
							throw new InvalidOperationException(String.Concat("Unable to produce hashcode for type: ", fieldType.GetReadableFullName()));
					}
				}
				il.LoadLocal(result);
				il.StoreLocal_2();
				il.Branch(exit);
				il.MarkLabel(exit);
				il.LoadLocal_2();
			});
			return method;
		}

		static EmittedMethod ImplementSpecializedDataTypeEquals(EmittedClass builder)
		{
			Contract.Requires<ArgumentNullException>(builder != null, Resources.Chk_CannotBeNull);

			var equatable = typeof(IEquatable<>).MakeGenericType(builder.Builder);
			builder.AddInterfaceImplementation(equatable);

			var specialized_equals = builder.DefineMethod("Equals");
			specialized_equals.ClearAttributes();
			specialized_equals.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual);
			specialized_equals.ReturnType = TypeRef.FromType<bool>();
			specialized_equals.DefineParameter("other", builder.Ref);

			var exitFalse = default(Label);

			specialized_equals.ContributeInstructions((m, il) =>
			{
				il.DeclareLocal(typeof(bool));
				exitFalse = il.DefineLabel();
				il.Nop();

				var fields = new List<EmittedField>(builder.Fields.Where(f => f.IsStatic == false));
				for (int i = 0; i < fields.Count; i++)
				{
					var field = fields[i];
					var fieldType = field.FieldType.Target;
					if (fieldType.IsArray)
					{
						LoadFieldFromTwoObjects(il, field, fieldType, true);
						il.Call(typeof(NetSteps.Encore.Core.Extensions).GetMethod("EqualsOrItemsEqual", BindingFlags.Public | BindingFlags.Static)
							.MakeGenericMethod(fieldType));
					}
					else if (fieldType.IsClass)
					{
						var op_Equality = fieldType.GetMethod("op_Equality", BindingFlags.Public | BindingFlags.Static);
						if (op_Equality != null)
						{
							LoadFieldFromTwoObjects(il, field, fieldType, true);
							il.Call(op_Equality);
						}
						else
						{
							il.Call(typeof(EqualityComparer<>).MakeGenericType(fieldType).GetMethod("get_Default", BindingFlags.Static | BindingFlags.Public));
							LoadFieldFromTwoObjects(il, field, fieldType, true);
							il.CallVirtual(typeof(IEqualityComparer<>).MakeGenericType(fieldType).GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance,
								null,
								new Type[] { fieldType, fieldType },
								null
								));
						}
					}
					else
					{
						LoadFieldFromTwoObjects(il, field, fieldType, false);
						il.CompareEquality(fieldType);
					}
					if (i < fields.Count - 1)
					{
						il.BranchIfFalse(exitFalse);
					}
				}
				var exit = il.DefineLabel();
				il.Branch(exit);
				il.MarkLabel(exitFalse);
				il.Load_I4_0();
				il.MarkLabel(exit);
				il.StoreLocal_0();
				var fin = il.DefineLabel();
				il.Branch(fin);
				il.MarkLabel(fin);
				il.LoadLocal_0();
			});

			builder.DefineOverrideMethod(typeof(ValueType).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(object) }, null))
				.ContributeInstructions((m, il) =>
				{
					var exitFalse2 = il.DefineLabel();
					var exit = il.DefineLabel();
					il.DeclareLocal(typeof(bool));
					il.LoadType(builder.Builder);
					il.LoadArg_1();
					il.CallVirtual<Type>("IsInstanceOfType");
					il.BranchIfFalse(exitFalse2);
					il.LoadArg_0();
					il.LoadArg_1();
					il.UnboxAny(builder.Builder);
					il.Call(specialized_equals);
					il.Branch(exit);
					il.MarkLabel(exitFalse2);
					il.LoadValue(false);
					il.MarkLabel(exit);
					il.StoreLocal_0();
					var fin = il.DefineLabel();
					il.Branch(fin);
					il.MarkLabel(fin);
					il.LoadLocal_0();
				});

			return specialized_equals;
		}

		static void LoadFieldFromTwoObjects(ILGenerator il, EmittedField field, Type fieldType, bool isStatic)
		{
			Contract.Requires<ArgumentNullException>(il != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(field != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(fieldType != null, Resources.Chk_CannotBeNull);
			il.LoadArg_0();
			il.LoadField(field);
			il.LoadArgAddress(1);
			il.LoadField(field);
		}

		static void ImplementInequalityOperators(EmittedClass builder, EmittedMethod equals)
		{
			Contract.Requires<ArgumentNullException>(builder != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(equals != null, Resources.Chk_CannotBeNull);

			var op_Inequality = builder.DefineMethod("op_Inequality");
			op_Inequality.ClearAttributes();
			op_Inequality.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static);
			op_Inequality.ReturnType = TypeRef.FromType<bool>();
			op_Inequality.DefineParameter("lhs", TypeRef.FromEmittedClass(builder));
			op_Inequality.DefineParameter("rhs", TypeRef.FromEmittedClass(builder));

			op_Inequality.ContributeInstructions((m, il) =>
			{
				il.Nop();
				il.LoadArg_0();
				il.LoadArg_1();
				il.Call(equals);
				il.Load_I4_0(); // load false
				il.CompareEqual();
			});
		}

		static void ImplementEqualityOperators(EmittedClass builder, EmittedMethod equals)
		{
			Contract.Requires<ArgumentNullException>(builder != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(equals != null, Resources.Chk_CannotBeNull);

			var op_Equality = builder.DefineMethod("op_Equality");
			op_Equality.ClearAttributes();
			op_Equality.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static);
			op_Equality.ReturnType = TypeRef.FromType<bool>();
			op_Equality.DefineParameter("lhs", TypeRef.FromEmittedClass(builder));
			op_Equality.DefineParameter("rhs", TypeRef.FromEmittedClass(builder));

			op_Equality.ContributeInstructions((m, il) =>
			{
				il.Nop();
				il.LoadArg_0();
				il.LoadArg_1();
				il.Call(equals);
			});
		}

		#endregion
	}
}