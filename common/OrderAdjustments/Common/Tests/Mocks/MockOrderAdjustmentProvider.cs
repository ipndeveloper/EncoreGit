using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common.Test.Mocks
{
    public class MockOrderAdjustmentProvider : IOrderAdjustmentProvider
    {
        public ConcurrentDictionary<int, MockOrderAdjustmentProfile> _templateCache;
        public ConcurrentDictionary<int, MockOrderAdjustmentExtension> _extensionCache;

        public const string ProviderKey = "e8c46653-9e02-4f40-8b04-06281aa138e7";

        public string GetProviderKey()
        {
            return ProviderKey;
        }

        public MockOrderAdjustmentProvider()
        {
            _templateCache = new ConcurrentDictionary<int, MockOrderAdjustmentProfile>();
            _extensionCache = new ConcurrentDictionary<int, MockOrderAdjustmentExtension>();
        }

        public MockOrderAdjustmentProfile AddAdjustment(MockOrderAdjustmentProfile adjustment)
        {
            while (_templateCache.Keys.Contains(adjustment.MockOrderAdjustmentProfileID))
                adjustment.MockOrderAdjustmentProfileID = new Random().Next();
            _templateCache.TryAdd(adjustment.MockOrderAdjustmentProfileID, adjustment);
            return adjustment;
        }

        public IEnumerable<IOrderAdjustmentProfile> GetApplicableAdjustments(IOrderContext orderContext)
        {
            return _templateCache.Values.Cast<IOrderAdjustmentProfile>();
        }

        public IOrderAdjustmentProfile GetOrderAdjustmentProfile(IOrderContext orderContext, int OrderAdjustmentID)
        {
            MockOrderAdjustmentExtension extension = _extensionCache[OrderAdjustmentID];
            return _templateCache[extension.MockProfileID];
        }

        public bool IsInstanceOfProfile(IOrderAdjustment adjustment, IOrderAdjustmentProfile adjustmentProfile)
        {
            MockOrderAdjustmentExtension appliedMockOrderAdjustment = (MockOrderAdjustmentExtension)Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(adjustment.ExtensionProviderKey).GetDataObjectExtension(adjustment);
            MockOrderAdjustmentProfile mockProfile = (MockOrderAdjustmentProfile)adjustmentProfile;
            return appliedMockOrderAdjustment.MockProfileID == mockProfile.MockOrderAdjustmentProfileID;
        }

        public IDataObjectExtension SaveDataObjectExtension(IExtensibleDataObject extensibleEntity)
        {
            MockOrderAdjustmentExtension extension = (MockOrderAdjustmentExtension)extensibleEntity.Extension;
            while (extension.MockProfileID == 0)
            {
                int next = new Random().Next();
                MockOrderAdjustmentExtension existing = _extensionCache.Values.Where(x => x.MockProfileID == next).SingleOrDefault();
                if (existing == null)
                    extension.MockProfileID = next;
            }
            _extensionCache.TryAdd(extension.OrderAdjustmentID, extension);
            return extension;
        }

        public IDataObjectExtension GetDataObjectExtension(IExtensibleDataObject entity)
        {
            try
            {
                entity.Extension = _extensionCache[((IOrderAdjustment)entity).OrderAdjustmentID];
            }
            catch (Exception ex)
            {

            }
            return entity.Extension;
        }

        public IDataObjectExtension CreateOrderAdjustmentDataObjectExtension(IOrderAdjustmentProfile profile)
        {
            int mockProfileID = ((MockOrderAdjustmentProfile)profile).MockOrderAdjustmentProfileID;
            MockOrderAdjustmentExtension extension = new MockOrderAdjustmentExtension() { MockProfileID = mockProfileID };
            _extensionCache.TryAdd(mockProfileID, extension);
            return extension;
        }


        public void UpdateDataObjectExtension(IExtensibleDataObject entity)
        {
            ((MockOrderAdjustmentExtension)entity.Extension).OrderAdjustmentID = ((IOrderAdjustment)entity).OrderAdjustmentID;
        }


        public void DeleteDataObjectExtension(IExtensibleDataObject x)
        {
            throw new NotImplementedException();
        }


        public void NotifyOfRemoval(IOrderContext orderContext, IOrderAdjustment adjustment)
        {

        }


        public void CommitAdjustment(IOrderAdjustment adjustment, IOrderContext orderContext)
        {
            (adjustment as MockOrderAdjustment).WasCommitted = true;
        }


    }
}
