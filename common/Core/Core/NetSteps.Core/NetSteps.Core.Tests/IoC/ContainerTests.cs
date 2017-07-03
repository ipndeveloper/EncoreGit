using System;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Encore.Core.Tests.IoC
{
	public class A
	{
		public A() { Name = this.GetType().Name; }
		public string Name { get; protected set; }
	}
	public class B : A { }
	public class C : A { internal C() : base() { } }
	public class D : C, IDisposable
	{
		public D() : base() { }
		public bool IsDisposed { get; private set; }

		~D() { Dispose(false); }
		public void Dispose()
		{
			Dispose(true);
		}
		private void Dispose(bool disposing)
		{
			IsDisposed = disposing;
		}
	}
	public class E : D
	{
		public E(string name)
		{
			Name = name;
		}
	}
	public class F : A
	{
		public F(double aDouble, object anObject, A anA)
		{
			ADouble = aDouble;
			AnObject = anObject;
			AnA = anA;
		}

		public F(int anInt, double aDouble, object anObject, A anA)
		{
			AnInt = anInt;
			ADouble = aDouble;
			AnObject = anObject;
			AnA = anA;
		}

		public int AnInt { get; private set; }
		public double ADouble { get; private set; }
		public object AnObject { get; private set; }
		public A AnA { get; private set; }
	}

	public interface I
	{
		string Name { get; }
	}
	public class J : I
	{
		public J() { Name = this.GetType().Name; }
		public string Name { get; protected set; }
	}
	public class K : J
	{
	}

	public interface L
	{
		string Name { get; }
	}
	public class M : L
	{
		public M() { Name = this.GetType().Name; }
		public string Name { get; protected set; }
	}

	public class N
	{
		public int Ordinal { get; set; }
		public string Name { get; set; }
	}

	public class O : L
	{
		public string Name { get; set; }

		public A AnA { get; set; }

		public O(string name, A anA)
		{
			Name = name;
			AnA = anA;
		}
	}

	public class TenantResolver : ITenantResolver
	{
		internal static void PushTenantID(string id)
		{
			_tenantID = id;
		}
		static string _tenantID = "flip";
		public bool TryResolveTenant(out object handback)
		{
			handback = _tenantID;
			return true;
		}
	}

	[TestClass]
	public partial class ContainerTests
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
		}

		[TestMethod]
		public void ContainerAlwaysResolvesToItself()
		{
			var root = Container.Root;
			var c = root.New<IContainer>();
			Assert.AreEqual(root, c);

			using (var scope = Create.NewContainer())
			{
				var cc = scope.New<IContainer>();
				Assert.AreEqual(scope, cc);
			}
		}

		[TestMethod]
		public void SimpleContainer()
		{
			var root = Container.Root;
			root.ForType<A>()
				.Register<B>()
				.End();

			using (var c = Create.NewContainer())
			{
				Assert.IsNotNull(c);

				var b = c.New<A>();
				Assert.IsInstanceOfType(b, typeof(A));
				Assert.IsInstanceOfType(b, typeof(B));
				Assert.AreEqual("B", b.Name);
			}
		}

		[TestMethod]
		public void CanInitializeDuringCreate()
		{
			var root = Container.Root;

			int counter = 0;
			root.Subscribe<N>((type, instance, name, kind) =>
			{
				Assert.AreEqual(typeof(N), type);
				Assert.IsNotNull(instance);
				Assert.IsNull(name);
				if (kind == CreationEventKind.Created)
				{
					Assert.AreEqual(0, instance.Ordinal);
					Assert.IsNull(instance.Name);
				}
				else if (kind == CreationEventKind.Initialized)
				{
					Assert.AreEqual(counter, instance.Ordinal);
					Assert.AreEqual(String.Concat("Name: ", counter), instance.Name);
				}
			});

			using (var c = Create.NewContainer())
			{
				Assert.IsNotNull(c);

				while (counter++ < 100)
				{
					var n = c.NewInit<N>().Init(new
					{
						Ordinal = counter,
						Name = String.Concat("Name: ", counter)
					});
					Assert.IsNotNull(n);
				}
			}
		}

		[TestMethod]
		public void MultiTenantContainerCanSpecialize()
		{
			Container.Root
				.ForType<A>()
					.Register<B>()
					.End();

			if (!Container.Root.SupportsMultipleTenants)
			{
				Container.Root
				.RegisterMultiTenant<TenantResolver>()
					.End();
				Container.Root
					.RegisterTenant("flip")
						.ForType<A>()
							.Register((c, p) => new C())
							.End();
				Container.Root
					.RegisterTenant("flop")
						.ForType<A>()
							.Register((c, p) => new D())
							.End();
			}

			TenantResolver.PushTenantID("flip");
			using (var flip = Create.TenantContainer())
			using (var child_of_flip = Create.NewContainer())
			{
				Assert.IsFalse(flip.IsRoot, "must not be a root container");
				Assert.IsTrue(flip.IsTenant, "must be a tenant");

				Assert.IsFalse(child_of_flip.IsRoot, "child of flip must not be a root container");
				Assert.IsTrue(child_of_flip.IsTenant, "child of flip must not be a tenant");

				Assert.AreNotEqual(flip, child_of_flip, "flip and its child must not be the same container");
				Assert.AreEqual(flip.TenantID, child_of_flip.TenantID, "flip and its child must identify with the same tenant");

				// Child of tenant "flip" resolves A to C
				var c = child_of_flip.New<A>();
				Assert.IsInstanceOfType(c, typeof(A), "child scope should resolve type A");
				Assert.IsInstanceOfType(c, typeof(C), "child scope should resolve type A to type C");
				Assert.AreEqual("C", c.Name, "valid type C knows its name");

				// Root continues to resolve A to B
				var b = Container.Root.New<A>();
				Assert.AreNotEqual(c, b);
				Assert.IsInstanceOfType(b, typeof(A));
				Assert.IsInstanceOfType(b, typeof(B));
				Assert.AreEqual("B", b.Name);

				// Tenants don't conflict...
				TenantResolver.PushTenantID("flop");
				using (var flop = Create.TenantContainer())
				{
					Assert.IsFalse(flop.IsRoot);
					Assert.IsTrue(flop.IsTenant);
					Assert.AreEqual("flop", flop.TenantID);

					// Tenant "flop" resolves A to D
					var d = flop.New<A>();
					Assert.IsInstanceOfType(d, typeof(A));
					Assert.IsInstanceOfType(d, typeof(D));
					Assert.AreEqual("D", d.Name);
				}
			}

		}

		[TestMethod]
		public void ChildContainerCanSpecialize()
		{
			Container.Root
				.ForType<A>()
					.Register<B>()
					.End();

			using (var scope = Create.NewContainer())
			{
				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				scope.ForType<A>()
					.Register((c, p) => new C())
					.End();

				Assert.IsNotNull(scope);

				using (var inner = scope.MakeChildContainer())
				{
					Assert.IsFalse(inner.IsRoot);
					Assert.IsFalse(inner.IsTenant);
					var c = scope.New<A>();
					Assert.IsInstanceOfType(c, typeof(A));
					Assert.IsInstanceOfType(c, typeof(C));
					Assert.AreEqual("C", c.Name);

					inner.ForType<A>().Register<B>();

					var b = inner.New<A>();
					Assert.AreNotEqual(c, b);
					Assert.IsInstanceOfType(b, typeof(A));
					Assert.IsInstanceOfType(b, typeof(B));
					Assert.AreEqual("B", b.Name);
				}
			}
		}

		[TestMethod]
		public void CreationObserversAreCalledDuringCreate()
		{
			Container.Root
				.ForType<A>()
					.Register<B>()
					.End();

			var creations = 0;
			using (var scope = Create.NewContainer())
			{
				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				scope.Subscribe<A>((t, item, name, kind) =>
				{
					Assert.IsInstanceOfType(item, typeof(A));
					Assert.IsInstanceOfType(item, typeof(B));
					creations++;
				});

				var b = scope.New<A>();
				Assert.IsInstanceOfType(b, typeof(A));
				Assert.IsInstanceOfType(b, typeof(B));
				Assert.AreEqual("B", b.Name);
				Assert.AreEqual(1, creations, "should have observed one creation event");
			}
		}

		[TestMethod]
		public void CreationObserversApplyToScopes()
		{
			Container.Root
				.ForType<A>()
					.Register((c, p) => new C())
					.End();

			int creations = 0, innerCreations = 0;
			using (var scope = Create.NewContainer())
			{
				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				scope.Subscribe<A>((t, item, name, kind) =>
				{
					Assert.IsInstanceOfType(item, typeof(A));
					creations++;
				});

				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				var c = scope.New<A>();
				Assert.IsInstanceOfType(c, typeof(A));
				Assert.IsInstanceOfType(c, typeof(C));
				Assert.AreEqual("C", c.Name);

				using (var inner = scope.MakeChildContainer())
				{
					// subscription can be out of order in relation to the registration.
					inner.Subscribe<A>((t, item, name, kind) =>
					{
						Assert.IsInstanceOfType(item, typeof(A));
						Assert.IsInstanceOfType(item, typeof(B));
						innerCreations++;
					});

					inner.ForType<A>().Register<B>();

					var b = inner.New<A>();
					Assert.AreNotEqual(c, b);
					Assert.IsInstanceOfType(b, typeof(A));
					Assert.IsInstanceOfType(b, typeof(B));
					Assert.AreEqual("B", b.Name);
				}

				// Create another C, should blow up the inner observer
				// if it hasn't properly gone out of scope.
				c = scope.New<A>();
				Assert.IsInstanceOfType(c, typeof(A));
				Assert.IsInstanceOfType(c, typeof(C));
				Assert.AreEqual("C", c.Name);

				Assert.AreEqual(3, creations, "should have observed 3 creations in the outer scope");
				Assert.AreEqual(1, innerCreations, "should have observed 3 creations in the inner scope");
			}
		}

		[TestMethod]
		public void ContainerCanLazyRegisterAndResolveTypes()
		{
			Container.Root
				.ForType<A>()
					.LazyRegister((t) =>
					{
						Assert.AreSame(typeof(A), t);
						return typeof(D);
					})
					.End();

			D d;
			using (var scope = Create.NewContainer())
			{
				Assert.IsNotNull(scope);
				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				d = scope.New<A>() as D;
				Assert.IsInstanceOfType(d, typeof(A));
				Assert.IsInstanceOfType(d, typeof(D));
				Assert.AreEqual("D", d.Name);
				Assert.IsFalse(d.IsDisposed);
			}
			Assert.IsTrue(d.IsDisposed);
		}

		[TestMethod]
		public void InstacesCreatedInGlobalScopeDontParticipateInCleanup()
		{
			D outside, inside;
			using (var scope = Create.NewContainer())
			{
				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				outside = Create.New<D>();

				Assert.IsNotNull(outside);
				Assert.IsFalse(outside.IsDisposed);

				inside = scope.New<D>();

				Assert.IsNotNull(inside);
				Assert.IsFalse(inside.IsDisposed);
			}
			Assert.IsTrue(inside.IsDisposed);

			Assert.IsFalse(outside.IsDisposed);
			outside.Dispose();
			Assert.IsTrue(outside.IsDisposed);
		}

		[TestMethod]
		public void ContainerRegisterTypesNotHavingDefaultConstructor()
		{
			using (var scope = Create.NewContainer())
			{
				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				scope.ForType<A>()
					.Register<E>(Param.Named("name", "N"))
					.End();

				Assert.IsNotNull(scope);

				var e = scope.New<A>();

				Assert.IsInstanceOfType(e, typeof(A));
				Assert.IsInstanceOfType(e, typeof(E));
				Assert.AreEqual("N", e.Name);

				// Even parameters supplied during type registration
				// may be overriden at create time...
				var ee = scope.NewWithParams<A>(Param.Value("M"));

				Assert.IsInstanceOfType(ee, typeof(A));
				Assert.IsInstanceOfType(ee, typeof(E));
				Assert.AreEqual("M", ee.Name);
			}
		}

		[TestMethod]
		public void ContainerCanCreateInstanceWithConstructorParameterSetSuppliedDuringResolve()
		{
			var args = new
			{
				AnInt = 1967,
				ADouble = 3.14,
				AnObject = new Object(),
				AnA = Create.New<A>(),
			};

			using (var scope = Create.NewContainer())
			{
				Assert.IsFalse(scope.IsRoot);
				Assert.IsFalse(scope.IsTenant);

				F f = scope.NewWithParams<F>(
						Param.Value(args.AnInt),
						Param.Value(args.ADouble),
						Param.Value(args.AnObject),
						Param.Value(args.AnA));

				Assert.IsNotNull(f);
				Assert.AreEqual(args.AnInt, f.AnInt);
				Assert.AreEqual(args.ADouble, f.ADouble);
				Assert.AreEqual(args.AnObject, f.AnObject);
				Assert.AreEqual(args.AnA, f.AnA);
			}
		}

		[TestMethod]
		public void ContainerCanCreateInstanceAndMatchAmongMultipleConstructors()
		{
			var args = new
			{
				AnInt = 1967,
				ADouble = 3.14,
				AnObject = new Object(),
				AnA = new A(),
			};

			using (var scope = Create.NewContainer())
			{
				F f = scope
					.NewWithParams<F>(
						Param.Value(args.ADouble),
						Param.Resolve<object>(),
						Param.Resolve<A>());

				Assert.IsNotNull(f);
				Assert.AreNotEqual(args.AnInt, f.AnInt);

				// The arguments supplied to the container came back in the instance...
				Assert.AreEqual(args.ADouble, f.ADouble);

				// The container created new instances of these...
				Assert.AreNotEqual(args.AnObject, f.AnObject);
				Assert.AreNotEqual(args.AnA, f.AnA);

				F ff = scope.NewWithParams<F>(
					Param.Value(args.AnInt),
					Param.Value(args.ADouble),
					Param.Value(args.AnObject),
					Param.Value(args.AnA)
					);

				Assert.IsNotNull(ff);
				Assert.AreNotEqual(f, ff);

				// The arguments supplied to the container came back in the instance...				
				Assert.AreEqual(args.AnInt, ff.AnInt);
				Assert.AreEqual(args.ADouble, ff.ADouble);
				Assert.AreEqual(args.AnObject, ff.AnObject);
				Assert.AreEqual(args.AnA, ff.AnA);
			}
		}

		[TestMethod]
		public void ContainerCanResolveNamedTypesOfT()
		{
			Container.Root
				.ForType<A>()
					.Register<B>()
					.End();

			using (var scope = Create.NewContainer())
			{
				scope
					.ForType<A>().RegisterWithName("CC", (c, p) => new C()).End()
					.ForType<A>().RegisterWithName<D>("DD").End()
					;

				var b = scope.New<A>();
				Assert.IsInstanceOfType(b, typeof(A));
				Assert.IsInstanceOfType(b, typeof(B));
				Assert.AreEqual("B", b.Name);

				var cc = scope.NewNamed<A>("CC");
				Assert.IsInstanceOfType(cc, typeof(A));
				Assert.IsInstanceOfType(cc, typeof(C));
				Assert.AreEqual("C", cc.Name);

				var d = scope.NewNamed<A>("DD");
				Assert.IsInstanceOfType(d, typeof(A));
				Assert.IsInstanceOfType(d, typeof(D));
				Assert.AreEqual("D", d.Name);
			}
		}

		[TestMethod]
		public void InstancePerScopeIsRespectedAmongScopes()
		{
			Container.Root
				.ForType<A>()
					.Register<B>().ResolveAnInstancePerScope()
					.End();

			var outer = Container.Root.New<A>();
			Assert.IsInstanceOfType(outer, typeof(A));
			Assert.IsInstanceOfType(outer, typeof(B));
			Assert.AreEqual("B", outer.Name);

			using (var scope = Create.NewContainer())
			{
				var b = scope.New<A>();
				Assert.IsInstanceOfType(b, typeof(A));
				Assert.IsInstanceOfType(b, typeof(B));
				Assert.AreEqual("B", b.Name);
				Assert.AreNotSame(outer, b);

				using (var child = scope.MakeChildContainer())
				{
					var inner = child.New<A>();
					Assert.IsInstanceOfType(inner, typeof(A));
					Assert.IsInstanceOfType(inner, typeof(B));
					Assert.AreEqual("B", inner.Name);
					Assert.AreNotSame(outer, inner);
					Assert.AreNotSame(outer, b);
				}

				var cc = scope.New<A>();
				Assert.IsInstanceOfType(cc, typeof(A));
				Assert.IsInstanceOfType(cc, typeof(B));
				Assert.AreEqual("B", cc.Name);
				Assert.AreSame(b, cc);
			}
		}

		[TestMethod]
		public void ResolveAsSingletonRespectedForTypesRegisteredByFactory()
		{
			Container.Root
				.ForType<L>()
					.Register((c, p) => new M())
					.ResolveAsSingleton()
					.End();

			L ell;
			L ellell;

			using (var scope = Create.NewContainer())
			{
				ell = scope.New<L>();
				Assert.IsInstanceOfType(ell, typeof(L));
				Assert.IsInstanceOfType(ell, typeof(M));
				Assert.AreEqual("M", ell.Name);

				ellell = scope.New<L>();
				Assert.IsInstanceOfType(ellell, typeof(L));
				Assert.IsInstanceOfType(ellell, typeof(M));
				Assert.AreEqual("M", ellell.Name);

				Assert.AreSame(ell, ellell);
			}
		}

		[TestMethod]
		public void SingletonTypeDefinedInParentScopeCannotBeOverriddenInChildScope()
		{
			Container.Root
				.ForType<I>()
					.Register<J>()
					.ResolveAsSingleton()
					.DisallowSpecialization()
					.End();

			I a;
			I b;
			I cc;

			using (var scope = Create.NewContainer())
			{
				try
				{
					Assert.IsFalse(scope.ForType<I>().CanSpecializeRegistration);

					scope.ForType<I>()
						.Register((c, p) => new K())
						.End();
					Assert.Fail("Should have blown up when trying to specialize the registration for typeof I");
				}
				catch (ContainerRegistryException e)
				{
					Assert.AreEqual(
						String.Concat("The type is registered in an outer scope and its registration cannot be specialized: ", typeof(I).GetReadableFullName()),
						e.Message);
				}

				a = scope.New<I>();
				Assert.IsInstanceOfType(a, typeof(I));
				Assert.IsInstanceOfType(a, typeof(J));
				Assert.AreEqual("J", a.Name);

				b = scope.New<I>();
				Assert.IsInstanceOfType(b, typeof(I));
				Assert.IsInstanceOfType(b, typeof(J));
				Assert.AreEqual("J", b.Name);

				Assert.AreSame(a, b);
			}

			cc = Create.New<I>();
			Assert.IsInstanceOfType(cc, typeof(I));
			Assert.IsInstanceOfType(cc, typeof(J));
			Assert.AreEqual("J", cc.Name);
			Assert.AreSame(b, cc);
		}
	}
}
