using System;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Reflection.Emit;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Buffers;
using System.Threading;
using NetSteps.Encore.Core.Process;
using NetSteps.Encore.Core.Representation;

[module: Wireup(typeof(NetSteps.Encore.Core.ModuleWireup))]

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Wireup command. Performs default wireup for the module.
	/// </summary>
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Tracks whether this module has been wired up.
		/// </summary>
		/// <remarks>
		/// This mechanism only exists because of the chicken-and-egg
		/// relationship between the root IoC container and the wireup
		/// coordinator. It enables the IoC container to see whether
		/// the wireup has been triggered on this assembly and enables
		/// it to "auto-wire" in cases where the library is included
		/// in a project that is ignorant of the wireup dependency
		/// logic.
		/// </remarks>
		private static int __wireupCount = 0;

		internal static bool NeedsAutoWire { get { return Thread.VolatileRead(ref __wireupCount) == 0; } }

		/// <summary>
		/// Wires up this module.
		/// </summary>
		/// <param name="coordinator"></param>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			Interlocked.Increment(ref __wireupCount);
#if DEBUG
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;
#endif
			var root = Container.Root;

			root.ForType<IProcessIdentity>()
					.Register((c, p) => ProcessIdentity.MakeProcessIdentity())
					.ResolveAsSingleton()
					.End()
				.ForType<ILogSinkManager>()
					.Register((c, p) => LogSinkManager.Singleton)
					.ResolveAsSingleton()
					.End();

			// the default buffer writer will match the machine's
			// endinaness.
			if (BitConverter.IsLittleEndian)
			{
				root.ForType<IBufferReader>()
						.Register<LittleEndianBufferReader>()
						.ResolveAnInstancePerScope()
						.End()
					.ForType<IBufferWriter>()
						.Register<LittleEndianBufferWriter>()
						.ResolveAnInstancePerScope()
						.End();
			}
			else
			{
				root.ForType<IBufferReader>()
						.Register<BigEndianBufferReader>()
						.ResolveAnInstancePerScope()
						.End()
					.ForType<IBufferWriter>()
						.Register<BigEndianBufferWriter>()
						.ResolveAnInstancePerScope()
						.End();
			}
			root.ForType<IBufferReader>()
						.RegisterWithName<BigEndianBufferReader>("big-endian")
						.ResolveAnInstancePerScope()
						.End()
					.ForType<IBufferReader>()
						.RegisterWithName<LittleEndianBufferReader>("little-endian")
						.ResolveAnInstancePerScope()
						.End()
					.ForType<IBufferWriter>()
						.RegisterWithName<BigEndianBufferWriter>("big-endian")
						.ResolveAnInstancePerScope()
						.End()
					.ForType<IBufferWriter>()
						.RegisterWithName<LittleEndianBufferWriter>("little-endian")
						.ResolveAnInstancePerScope()
						.End();

			root.ForGenericType(typeof(IJsonRepresentation<>))
				.Register(typeof(JsonTransformLoose<>))
				.ResolveAnInstancePerScope()
				.End();

			root.ForType<DataGenerator>()
				.Register((c, p) => new DataGenerator(c.New<IBufferReader>()))
				.ResolveAnInstancePerScope()
				.End();
		}
	}
}
