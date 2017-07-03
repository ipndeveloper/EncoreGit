using System;
using System.Linq;
using System.ServiceModel;
using NetSteps.Integrations.Service.Interfaces;
using NetSteps.Integrations.Service.DataModels;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Integrations.Service.InventoryHandlers;
using NetSteps.Integrations.Internals.Security;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Integrations.Service
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, Name = "inventoryAPI", IncludeExceptionDetailInFaults = true)]
	public class InventoryAPI : IInventoryAPI
	{
		#region Properties

		protected InventoryModelRepository Repo { get; set; }

		#endregion

		#region Contructors

		public InventoryAPI()
		{
			Repo = new InventoryModelRepository();
		}

		#endregion

		#region Methods

		public UpdateInventoryItemModelResponseCollection UpdateInventory(string username, string password, UpdateInventoryItemModelCollection items)
		{
			if (!IntegrationsSecurity.Authenticate(username, password))
			{
				throw new System.Security.Authentication.AuthenticationException();
			}

			try
			{
				using (this.TraceActivity("UpdateInventory"))
				{
					return Repo.UpdateInventoryItems(items);
				}
			}
			catch (Exception ex)
			{
				this.TraceException(ex); ;
				throw;
			}
		}

		public GetInventoryItemModelResponseCollection GetInventory([Required]string username, [Required]string password, GetInventoryItemModelCollection items)
		{
			if (!IntegrationsSecurity.Authenticate(username, password))
			{
				throw new System.Security.Authentication.AuthenticationException();
			}

			try
			{
				using (this.TraceActivity("GetInventory"))
				{
					if (items != null && items.Any())
					{
						return Repo.GetInventory(items);
					}
					else
					{
						return Repo.GetAllInventory();
					}
				}
			}
			catch (Exception ex)
			{
				this.TraceException(ex); ;
				throw;
			}
		}

		#endregion
	}
}
