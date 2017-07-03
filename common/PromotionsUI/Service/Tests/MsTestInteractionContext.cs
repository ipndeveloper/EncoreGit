using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap;
using StructureMap.AutoMocking;

namespace Tests
{
    [TestClass]
    public abstract class MsTestInteractionContext<T> where T : class
    {
        private readonly MockMode _mode;

        public IContainer Container
        {
            get
            {
                return Services.Container;
            }
        }

        public RhinoAutoMocker<T> Services { get; private set; }

        public T ClassUnderTest
        {
            get
            {
                return Services.ClassUnderTest;
            }
        }

        public MsTestInteractionContext()
            : this(MockMode.AAA)
        {
        }

        public MsTestInteractionContext(MockMode mode)
        {
            _mode = mode;
        }

        [TestInitialize]
        public void SetUp()
        {
            Services = new RhinoAutoMocker<T>(_mode);
            beforeEach();
        }

        protected virtual void beforeEach()
        {
        }

        public SERVICE MockFor<SERVICE>() where SERVICE : class
        {
            return Services.Get<SERVICE>();
        }

        public void VerifyCallsFor<MOCK>() where MOCK : class
        {
            RhinoMocksExtensions.VerifyAllExpectations(MockFor<MOCK>());
        }
    }
}