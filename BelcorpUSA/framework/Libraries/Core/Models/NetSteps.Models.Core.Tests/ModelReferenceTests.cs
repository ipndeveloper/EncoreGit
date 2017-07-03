using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Dto;
using NetSteps.Models.Core.ModelReferences;
using NetSteps.Encore.Core.Collections;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.Representation;
using NetSteps.Models.Core.SPI;
using NetSteps.Models.Core.Metadata;

namespace NetSteps.Models.Core.Tests
{
	public class UninterestingObject
	{
		public int Number { get; set; }
	}

	[TestClass]
	public class ModelReferenceTests
	{
		[TestMethod]
		public void ModelReference_CanBeCloned()
		{
			var test = new UninterestingObject() { Number = 1 };
			var r = new ImmutableModelReference<UninterestingObject>(test);

			Assert.IsNotNull(r);
			Assert.IsFalse(r.IsEmpty);
			Assert.IsTrue(r.HasModel);
			Assert.IsFalse(r.HasIdentityKey);
			Assert.AreSame(test, r.Model);

			var r1 = (IModelReference<UninterestingObject>)r.Clone();
			Assert.IsNotNull(r1);
			Assert.IsFalse(r1.IsEmpty);
			Assert.IsTrue(r1.HasModel);
			Assert.IsFalse(r1.HasIdentityKey);
			Assert.AreSame(test, r1.Model);
			Assert.AreNotSame(r, r1);
		}

		[TestMethod]
		public void ModelReference_CanCopyViaCopyContext()
		{
			ChildImplementation child = new ChildImplementation(ChildModelData.Init(), ModelStates.Writing);
			child.ID = 13;
			child.Name = "Item 13";
			child.ChangeModelStates(ModelStates.Immutable, true, new MarkingContext());

			var r = new ImmutableIdentifiableModelReference<IChild, int>(child);
			using (var container = Create.NewContainer())
			{
				var copy = new CopyContext(container);

				var rr = copy.GetOrAddIdentifiableReference(r);

				Assert.IsNotNull(rr);
				Assert.IsTrue(rr.HasModel);
				Assert.IsTrue(rr.HasIdentityKey);
				Assert.AreEqual(child.ID, rr.IdentityKey);
				Assert.IsNotNull(rr.Model);
				Assert.AreNotSame(child, rr.Model);

				Assert.AreEqual(child.ID, rr.Model.ID);
				Assert.AreEqual(child.Name, rr.Model.Name);

				var rrr = copy.GetOrAddIdentifiableReference(r);
				Assert.AreEqual(rr, rrr);

				Assert.AreSame(rr.Model, rrr.Model);
			}
		}

		[TestMethod]
		public void ModelReference_Model()
		{
			var child = new ChildImplementation();

			using (var container = Create.NewContainer())
			{
				var context = new MutationContext(container);
				var m = child.Mutate(context, new { ID = 13, Name = "Item 13" });
				Assert.AreNotEqual(child.ID, m.ID);
				Assert.AreNotEqual(child.Name, m.Name);

				Assert.IsTrue(child.IsPropertyUpdated("ID"));
				Assert.IsTrue(child.IsPropertyUpdated("Name"));
			}

		}
	}

	public interface IChild
	{
		[IdentityKey]
		int ID { get; }
		string Name { get; set; }
	}

	public class ChildImplementation : Model<IChild>, IChild, ICompositionParent, IEquatable<IChild>, IIdentifiable<int>
	{
		static readonly int CHashCodeSeed = typeof(SampleModel).AssemblyQualifiedName.GetHashCode();

		ChildModelData _data;

		public ChildImplementation() : this(ChildModelData.Init(), ModelStates.Immutable) { }
		public ChildImplementation(ChildModelData data, ModelStates state)
			: base(state)
		{
			_data = data;
		}

		public int ID
		{
			get { return _data.IChild_ID; }
			set
			{
				CheckMutation();

				if (_data.WriteIChild_ID(value))
				{
					MarkMutated(new MarkingContext(), MutationKinds.Direct);
				}
			}
		}

		public string Name
		{
			get { return _data.IChild_Name; }
			set
			{
				CheckMutation();

				if (_data.WriteIChild_Name(value))
				{
					MarkMutated(new MarkingContext(), MutationKinds.Direct);
				}
			}
		}

		public override bool Equals(IChild other)
		{
			if (other is ChildImplementation)
			{
				return this.Equals((ChildImplementation)other);
			}
			else
			{
				return other != null
					&& this._data.IChild_ID == other.ID
					&& String.Equals(this._data.IChild_Name, other.Name);
			}
		}

		public bool Equals(ChildImplementation other)
		{
			return other != null
				&& base.Equals(other)
				&& _data.Equals(other._data);
		}

		public override bool Equals(object obj)
		{
			return obj is ChildImplementation
				&& Equals((ChildImplementation)obj);
		}

		public override int GetHashCode()
		{
			int prime = 0xf3e9b;
			int result = CHashCodeSeed * prime;
			result ^= base.GetHashCode();
			result ^= _data.GetHashCode() * prime;
			return result;
		}

		public override string ToString()
		{
			var rep = new JsonTransformLoose<ChildImplementation>();
			return rep.TransformItem(this);
		}

		public override IEnumerable<Mutation> GetMutations()
		{
			List<Mutation> result = new List<Mutation>();
			if (_data.DirtyFlags[0]) result.Add(new Mutation("ID", MutationKinds.Direct));
			if (_data.DirtyFlags[1]) result.Add(new Mutation("Name", MutationKinds.Direct));
			return result.ToReadOnly();
		}

		public bool IsPropertyUpdated(string property)
		{
			if (property == null) throw new ArgumentNullException("cannot be null", "property");
			if (property.Length > 0) throw new ArgumentException("cannot be empty", "property");

			switch (property)
			{
				case "ID": return _data.DirtyFlags[0];
				case "Name": return _data.DirtyFlags[1];
				default:
					throw new ArgumentOutOfRangeException(String.Concat("Property not found: ", property));
			}
		}

		public void MarkDirtyByComposition()
		{
			MarkMutated(new MarkingContext(), MutationKinds.ByComposition);
		}

		public override IChild Copy(ICopyContext context)
		{
			if (context == null) throw new ArgumentNullException("context");

			return context.GetOrAddIdentifiable(this, ID, __staticCopier);
		}

		protected override void PerformMutate<MU>(IMutationContext context, IChild copy, MU mutation)
		{
			if (context == null) throw new ArgumentNullException("context");
			if (copy == null) throw new ArgumentNullException("copy");

			Mutator<IChild, ChildImplementation>.Mutate(context, (ChildImplementation)copy, mutation);
		}

		static Copier __staticCopier = new Copier();

		internal class Copier : IModelCopier<IChild>
		{
			public object MakeCopy(ICopyContext context, IChild source)
			{
				var model = (ChildImplementation)source;
				var copy = new ChildImplementation();
				copy.IncludeModelStates(ModelStates.Writing, true, new MarkingContext());
				copy._data = model._data.MakeCopy(copy, context);
				copy.ExcludeModelStates(ModelStates.Writing, true, new MarkingContext());
				return copy;
			}
		}

		public int GetIdentityKey()
		{
			return this.ID;
		}

		protected override MutationKinds PerformGetMutationForProperty(string property)
		{
			if (property == null) throw new ArgumentNullException("cannot be null", "property");
			if (property.Length > 0) throw new ArgumentException("cannot be empty", "property");

			switch (property)
			{
				case "ID":
					if (_data.DirtyFlags[0]) return MutationKinds.Direct;
					break;
				case "Name":
					if (_data.DirtyFlags[1]) return MutationKinds.Direct;
					break;
				default:
					throw new ArgumentOutOfRangeException(String.Concat("Property not found: ", property));
			}
			return MutationKinds.None;
		}

		public Type GetIdentityKeyType()
		{
			return typeof(int);
		}

		public object GetUntypedIdentityKey()
		{
			return GetIdentityKey();
		}
	}

	public struct ChildModelData
	{
		public BitVector DirtyFlags;
		public int IChild_ID;
		public string IChild_Name;

		public bool WriteIChild_ID(int value)
		{
			if (this.IChild_ID != value)
			{
				this.IChild_ID = value;
				this.DirtyFlags[0] = true;
				return true;
			}
			return false;
		}
		public bool WriteIChild_Name(string value)
		{
			if (this.IChild_Name != value)
			{
				this.IChild_Name = value;
				this.DirtyFlags[1] = true;
				return true;
			}
			return false;
		}

		internal ChildModelData MakeCopy(ICompositionParent parent, ICopyContext context)
		{
			ChildModelData copy = this;
			// cascade, detecting cyclic references if they exist...
			return copy;
		}

		internal void MarkClean(IMarkingContext context)
		{
			// cascade, detecting cyclic references if they exist...
		}

		internal static ChildModelData Init()
		{
			var result = new ChildModelData();
			result.DirtyFlags = new BitVector(2);
			return result;
		}

		internal void CascadeState(IMarkingContext context, ModelStates state)
		{
		}
	}

	public class ChildReferenceFactory : IdentifiableModelReferenceFactory<IChild, int>
	{
		protected override void PerfromCascadeModelStates(IModelReference<IChild> reference, ModelStates states, IMarkingContext context)
		{
			if (!Equals(reference, (IChild)null))
			{
				var m = reference.Model as IModelSPI<IChild>;
				if (m != null) m.IncludeModelStates(states, true, context);
			}
		}

		protected override IModelReference<IChild> PerfromMarkClean(IModelReference<IChild> reference, IMarkingContext context)
		{
			throw new NotImplementedException();
		}

		protected override bool PerformEquals(IChild x, IChild y)
		{
			if (x != null)
			{
				return x.Equals(y);
			}
			else
			{
				return y == null;
			}
		}

		protected override int CalculateHashCode(IChild obj)
		{
			return obj.GetHashCode();
		}
	}

	public interface ISample : IModel
	{
		[IdentityKey]
		int ID { get; }
		string Name { get; set; }
		DateTime DateOfBirth { get; set; }
		IChild Child { get; set; }
	}

	public class SampleModel : Model<ISample>, ISample, ICompositionParent, IIdentifiable<int>
	{
		static readonly int CHashCodeSeed = typeof(SampleModel).GetKeyForType().GetHashCode();
		SampleModelData _data;

		public SampleModel() : this(ModelStates.Immutable) { }
		public SampleModel(ModelStates state) : this(SampleModelData.Init(), state) { }
		public SampleModel(SampleModelData data, ModelStates state)
			: base(state)
		{
			_data = data;
		}

		public int ID
		{
			get { return _data.ISampleDto_ID; }
			set
			{
				CheckMutation();

				if (_data.WriteISampleDto_ID(value))
				{
					MarkMutated(new MarkingContext(), MutationKinds.Direct);
				}
			}
		}

		public string Name
		{
			get { return _data.ISampleDto_Name; }
			set
			{
				CheckMutation();

				if (_data.WriteISampleDto_Name(value))
				{
					MarkMutated(new MarkingContext(), MutationKinds.Direct);
				}
			}
		}

		public DateTime DateOfBirth
		{
			get { return _data.ISampleDto_DateOfBirth; }
			set
			{
				CheckMutation();

				if (_data.WriteISampleDto_DateOfBirth(value))
				{
					MarkMutated(new MarkingContext(), MutationKinds.Direct);
				}
			}
		}

		public IChild Child
		{
			get { return _data.ResolveISampleDto_Child(this); }
			set
			{
				CheckMutation();

				if (_data.WriteISampleDto_Child(this, value))
				{
					MarkMutated(new MarkingContext(), MutationKinds.Direct);
				}
			}
		}
		public override bool Equals(ISample other)
		{
			if (other is SampleModel)
			{
				return this.Equals((SampleModel)other);
			}
			else
			{
				return other != null
					&& this._data.ISampleDto_ID == other.ID
					&& String.Equals(this._data.ISampleDto_Name, other.Name)
					&& this._data.ISampleDto_DateOfBirth == other.DateOfBirth;
			}
		}

		public bool Equals(SampleModel other)
		{
			return ModelEquals(other)
				&& _data.Equals(other._data);
		}

		public override bool Equals(object obj)
		{
			return obj is ISample
				&& Equals((ISample)obj);
		}

		public override int GetHashCode()
		{
			int prime = 0xf3e9b;
			int result = CHashCodeSeed * prime;
			result ^= base.GetHashCode();
			result ^= _data.GetHashCode() * prime;
			return result;
		}

		public override string ToString()
		{
			var rep = new JsonTransformLoose<SampleModel>();
			return rep.TransformItem(this);
		}

		public override IEnumerable<Mutation> GetMutations()
		{
			List<Mutation> result = new List<Mutation>();
			if (_data.DirtyFlags[0]) result.Add(new Mutation("ID", MutationKinds.Direct));
			if (_data.DirtyFlags[1]) result.Add(new Mutation("Name", MutationKinds.Direct));
			if (_data.DirtyFlags[2]) result.Add(new Mutation("DateOfBirth", MutationKinds.Direct));
			if (_data.DirtyFlags[3]) result.Add(new Mutation("Child", MutationKinds.Direct));
			return result.ToReadOnly();
		}

		public bool IsPropertyUpdated(string property)
		{
			if (property == null) throw new ArgumentNullException("cannot be null", "property");
			if (property.Length > 0) throw new ArgumentException("cannot be empty", "property");

			switch (property)
			{
				case "ID": return _data.DirtyFlags[0];
				case "Name": return _data.DirtyFlags[1];
				case "DateOfBirth": return _data.DirtyFlags[2];
				case "Child": return _data.DirtyFlags[3];
				default:
					throw new ArgumentOutOfRangeException(String.Concat("Property not found: ", property));
			}
		}

		public void MarkDirtyByComposition()
		{
			MarkMutated(new MarkingContext(), MutationKinds.ByComposition);
		}

		public override ISample Copy(ICopyContext context)
		{
			if (context == null) throw new ArgumentNullException("container");

			return context.GetOrAddIdentifiable(this, ID, __staticCopier);
		}

		protected override void PerformMutate<MU>(IMutationContext context, ISample copy, MU mutation)
		{
			if (context == null) throw new ArgumentNullException("context");
			if (copy == null) throw new ArgumentNullException("copy");

			Mutator<ISample, SampleModel>.Mutate(context, (SampleModel)copy, mutation);
		}

		static Copier __staticCopier = new Copier();

		internal class Copier : IModelCopier<SampleModel>
		{
			public object MakeCopy(ICopyContext context, SampleModel source)
			{
				var copy = new SampleModel();
				copy.IncludeModelStates(ModelStates.Writing, true, new MarkingContext());
				copy._data = source._data.MakeCopy(context);
				copy.ExcludeModelStates(ModelStates.Writing, true, new MarkingContext());
				return copy;
			}
		}

		public int GetIdentityKey()
		{
			return this.ID;
		}

		protected override MutationKinds PerformGetMutationForProperty(string property)
		{
			if (property == null) throw new ArgumentNullException("cannot be null", "property");
			if (property.Length == 0) throw new ArgumentException("cannot be empty", "property");

			switch (property)
			{
				case "ID":
					if (_data.DirtyFlags[0]) return MutationKinds.Direct;
					break;
				case "Name":
					if (_data.DirtyFlags[1]) return MutationKinds.Direct;
					break;
				case "DateOfBirth":
					if (_data.DirtyFlags[2]) return MutationKinds.Direct;
					break;
				case "Child":
					if (_data.DirtyFlags[3])
					{
						return MutationKinds.Direct;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(String.Concat("Property not found: ", property));
			}
			return MutationKinds.None;
		}

		protected override void PerformCascadeChangeModelStates(ModelStates states, IMarkingContext context)
		{
			_data.CascadeChangeModelStates(states, context);
		}

		protected override void PerformCascadeExcludeModelStates(ModelStates states, IMarkingContext context)
		{
			_data.CascadeExcludeModelStates(states, context);
		}

		protected override void PerformCascadeIncludeModelStates(ModelStates states, IMarkingContext context)
		{
			_data.CascadeIncludeModelStates(states, context);
		}

		public Type GetIdentityKeyType()
		{
			return typeof(int);
		}

		protected override IModelReference<R> PerformGetReference<R>(string referentName)
		{
			if (referentName == "Child")
			{
				return _data.ISampleDto_Child as IModelReference<R>;
			}
			else
			{
				return base.PerformGetReference<R>(referentName);
			}
		}

		protected override R PerformGetReferent<R>(string referentName)
		{
			if (referentName == "Child")
			{
				return (R)_data.ResolveISampleDto_Child(this);
			}
			else
			{
				return base.PerformGetReferent<R>(referentName);
			}
		}

		protected override IK PerformGetReferentID<IK>(string referentName)
		{
			if (referentName == "Child")
			{
				return (IK)(object)_data.ISampleDto_Child.IdentityKey;
			}
			else
			{
				return base.PerformGetReferentID<IK>(referentName);
			}
		}


		public object GetUntypedIdentityKey()
		{
			return GetIdentityKey();
		}
	}

	public struct SampleModelData
	{
		static readonly IModelReferenceFactory<IChild> ChildReferences = new ChildReferenceFactory();

		public BitVector DirtyFlags;
		public int ISampleDto_ID;
		public string ISampleDto_Name;
		public DateTime ISampleDto_DateOfBirth;
		public IModelReference<IChild, int> ISampleDto_Child;

		public bool WriteISampleDto_ID(int value)
		{
			if (this.ISampleDto_ID != value)
			{
				this.ISampleDto_ID = value;
				this.DirtyFlags[0] = true;
				return true;
			}
			return false;
		}
		public bool WriteISampleDto_Name(string value)
		{
			if (this.ISampleDto_Name != value)
			{
				this.ISampleDto_Name = value;
				this.DirtyFlags[1] = true;
				return true;
			}
			return false;
		}
		public bool WriteISampleDto_DateOfBirth(DateTime value)
		{
			if (this.ISampleDto_DateOfBirth != value)
			{
				this.ISampleDto_DateOfBirth = value;
				this.DirtyFlags[2] = true;
				return true;
			}
			return false;
		}

		public bool WriteISampleDto_Child(ICompositionParent parent, IChild value)
		{
			if (!ChildReferences.Equals(this.ISampleDto_Child, value))
			{
				this.ISampleDto_Child = (IModelReference<IChild, int>)ChildReferences.MakeFromReferent(value);
				this.DirtyFlags[3] = true;
				return true;
			}
			return false;
		}

		internal SampleModelData MakeCopy(ICopyContext context)
		{
			SampleModelData copy = this;
			copy.DirtyFlags = DirtyFlags.Copy();
			// cascade, detecting cyclic references if they exist...
			copy.ISampleDto_Child = context.GetOrAddIdentifiableReference((IModelReference<IChild, int>)ISampleDto_Child);
			return copy;
		}

		internal void MarkClean(IMarkingContext context)
		{
			DirtyFlags = new BitVector(3);

			this.ISampleDto_Child = (IModelReference<IChild, int>)ChildReferences.MarkClean(this.ISampleDto_Child, context);
		}

		internal void CascadeState(IMarkingContext context, ModelStates state)
		{
		}

		internal static SampleModelData Init()
		{
			var result = new SampleModelData();
			result.DirtyFlags = new BitVector(4);
			result.ISampleDto_Child = (IModelReference<IChild, int>)ModelReference<IChild, int>.Empty;
			return result;
		}

		internal IChild ResolveISampleDto_Child(ICompositionParent parent)
		{
			if (this.ISampleDto_Child.IsEmpty) return default(IChild);

			if (!this.ISampleDto_Child.HasModel)
			{
				this.ISampleDto_Child = (IModelReference<IChild, int>)this.ISampleDto_Child.ResolveModel();
			}
			return this.ISampleDto_Child.Model;
		}

		internal void CascadeChangeModelStates(ModelStates states, IMarkingContext context)
		{
		}

		internal void CascadeExcludeModelStates(ModelStates states, IMarkingContext context)
		{
		}

		internal void CascadeIncludeModelStates(ModelStates states, IMarkingContext context)
		{
			ChildReferences.CascadeIncludeModelStates(ISampleDto_Child, states, context);
		}
	}

}
