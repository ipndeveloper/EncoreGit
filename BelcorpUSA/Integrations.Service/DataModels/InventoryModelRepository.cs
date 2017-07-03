using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Extensions;
using NetSteps.Integrations.Service.DataModels;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NetSteps.Foundation.Common;

namespace NetSteps.Integrations.Service.InventoryHandlers
{
	/*
	 * I do not know why Todd decided to do raw ADO.  Recomend rewritting this module using the existing EF db context found in NetSteps.Data.Entities.
	 */
	public class InventoryModelRepository
	{
		private UpdateInventoryItemModelResponse ConvertToUpdateModel(IDataRecord dataRecord, UpdateInventoryItemModelResponse model)
		{
			model.SKU = dataRecord.GetString((int)FieldNames.Sku);
			model.SAPCode = dataRecord.GetString((int)FieldNames.SAPCode);
			model.WarehouseID = dataRecord.GetInt32((int)FieldNames.WarehouseID);
			model.QuantityAllocated = dataRecord.GetInt32((int)FieldNames.QuantityAllocated);
			model.QuantityOnHand = dataRecord.GetInt32((int)FieldNames.QuantityOnHand);
			model.QuantityAvailable = model.QuantityOnHand - model.QuantityAllocated;
			return model;
		}

		private GetInventoryItemModelResponse ConvertToInventoryModel(IDataRecord dataRecord)
		{
			var model = new GetInventoryItemModelResponse();
			model.Sku = dataRecord.GetString((int)FieldNames.Sku);
			model.WarehouseId = dataRecord.GetInt32((int)FieldNames.WarehouseID);
			model.QuantityAllocated = dataRecord.GetInt32((int)FieldNames.QuantityAllocated);
			model.QuantityOnHand = dataRecord.GetInt32((int)FieldNames.QuantityOnHand);
			model.QuantityAvailable = model.QuantityOnHand - model.QuantityAllocated;
			model.SAPCode = dataRecord.IsDBNull((int)FieldNames.SAPCode) ? null : dataRecord.GetString((int)FieldNames.SAPCode);
			return model;
		}

		public enum FieldNames
		{
			Sku,
			WarehouseID,
			QuantityAllocated,
			QuantityOnHand,
			SAPCode
		}

		public GetInventoryItemModelResponseCollection GetAllInventory()
		{
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString))
			{
				var command = connection.CreateCommand();
				command.CommandText = @"SELECT 
											Products.Sku AS Sku,
											WarehouseProducts.WarehouseID AS WarehouseId,
											WarehouseProducts.QuantityAllocated AS QuantityAllocated,
											WarehouseProducts.QuantityOnHand AS QuantityOnHand,
											ProductProperties.PropertyValue AS SAPCode
										FROM 
											WarehouseProducts 
										INNER JOIN Products
											ON WarehouseProducts.ProductId = Products.ProductId
										LEFT JOIN ProductProperties
											ON ProductProperties.ProductID = WarehouseProducts.ProductID
											AND ProductProperties.ProductPropertyTypeID IN (SELECT ProductPropertyTypeID FROM ProductPropertyTypes WHERE ProductPropertyTypes.Name = 'SAP Code')";

				var collection = new GetInventoryItemModelResponseCollection();
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					collection.Add(ConvertToInventoryModel((IDataRecord)reader));
				}
				return collection;
			}
		}

		public GetInventoryItemModelResponseCollection GetInventory(GetInventoryItemModelCollection items)
		{
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString))
			{
				var command = connection.CreateCommand();
				command.CommandText = String.Format(@"SELECT 
											Products.Sku AS Sku,
											WarehouseProducts.WarehouseID AS WarehouseId,
											WarehouseProducts.QuantityAllocated AS QuantityAllocated,
											WarehouseProducts.QuantityOnHand AS QuantityOnHand,
											ProductProperties.PropertyValue AS SAPCode
										FROM 
											WarehouseProducts 
										INNER JOIN Products
											ON WarehouseProducts.ProductId = Products.ProductId
										LEFT JOIN ProductProperties
											ON ProductProperties.ProductID = WarehouseProducts.ProductID
											AND ProductProperties.ProductPropertyTypeID IN (SELECT ProductPropertyTypeID FROM ProductPropertyTypes WHERE ProductPropertyTypes.Name = 'SAP Code')
										WHERE 
											SKU in ({0})"
					, String.Join(",", items.Select(x => String.Format("'{0}'", x.Sku.Replace("'", "''")))));

				var collection = new GetInventoryItemModelResponseCollection();
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					collection.Add(ConvertToInventoryModel((IDataRecord)reader));
				}
				return collection;
			}
		}

		public UpdateInventoryItemModelResponseCollection UpdateInventoryItems(UpdateInventoryItemModelCollection updateItems)
		{
			if (updateItems == null)
			{
				return new UpdateInventoryItemModelResponseCollection();
			}

			var results = new UpdateInventoryItemModelResponseCollection();
			updateItems.Each(item => results.Add(UpdateInventoryItem(item)));
			return results;
		}

		public UpdateInventoryItemModelResponse UpdateInventoryItem(UpdateInventoryItemModel model)
		{
			UpdateInventoryItemModelResponse result;

			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString))
			{
				try
				{
					var updateCommand = connection.CreateCommand();
					var selectCommand = connection.CreateCommand();
					if (String.IsNullOrWhiteSpace(model.SAPCode))
					{
						updateCommand.CommandText = @"UPDATE 
												wp 
											SET 
												wp.QuantityOnHand = wp.QuantityOnHand + (@QuantityOnHand)
											FROM WarehouseProducts wp
											INNER JOIN Products p 
												ON wp.ProductId = p.ProductId 
												AND p.SKU = @SKU
											WHERE wp.WarehouseID = @WarehouseID";

						selectCommand.CommandText = @"SELECT 
															Products.Sku AS Sku,
															WarehouseProducts.WarehouseID AS WarehouseId,
															WarehouseProducts.QuantityAllocated AS QuantityAllocated,
															WarehouseProducts.QuantityOnHand AS QuantityOnHand,
															ProductProperties.PropertyValue AS SAPCode
														FROM 
															WarehouseProducts 
														INNER JOIN Products
															ON WarehouseProducts.ProductId = Products.ProductId
														LEFT JOIN ProductProperties
															ON ProductProperties.ProductID = WarehouseProducts.ProductID
															AND ProductProperties.ProductPropertyTypeID = (SELECT TOP 1 ProductPropertyTypeID FROM ProductPropertyTypes WHERE ProductPropertyTypes.Name = 'SAP Code')
														WHERE 
															Products.Sku = @SKU AND WarehouseProducts.WarehouseID = @WarehouseID";
					}
					else
					{
						updateCommand.CommandText = @"UPDATE 
												wp 
											SET 
												wp.QuantityOnHand = wp.QuantityOnHand + (@QuantityOnHand)
											FROM
												WarehouseProducts wp
											INNER JOIN Products p
												ON wp.ProductId = p.ProductId
												AND p.SKU = @SKU
											INNER JOIN ProductProperties pp
												ON pp.ProductID = wp.ProductID
												AND pp.PropertyValue = @SAPCode
											INNER JOIN ProductPropertyTypes ppt
												ON ppt.ProductPropertyTypeID = pp.ProductPropertyTypeID
												AND ppt.Name = 'SAP Code'
											WHERE wp.WarehouseID = @WarehouseID";

						selectCommand.CommandText = @"SELECT 
															Products.Sku AS Sku,
															WarehouseProducts.WarehouseID AS WarehouseId,
															WarehouseProducts.QuantityAllocated AS QuantityAllocated,
															WarehouseProducts.QuantityOnHand AS QuantityOnHand,
															ProductProperties.PropertyValue AS SAPCode
														FROM 
															WarehouseProducts 
														INNER JOIN Products
															ON WarehouseProducts.ProductId = Products.ProductId
														INNER JOIN ProductProperties
															ON ProductProperties.ProductID = WarehouseProducts.ProductID
															AND ProductProperties.ProductPropertyTypeID = (SELECT TOP 1 ProductPropertyTypeID FROM ProductPropertyTypes WHERE ProductPropertyTypes.Name = 'SAP Code')
															AND ProductProperties.PropertyValue = @SAPCode
														WHERE 
															Products.Sku = @SKU AND WarehouseProducts.WarehouseID = @WarehouseID";

						updateCommand.Parameters.Add(new SqlParameter("@SAPCode", model.SAPCode));
						selectCommand.Parameters.Add(new SqlParameter("@SAPCode", model.SAPCode));
					}

					updateCommand.Parameters.Add(new SqlParameter("@QuantityOnHand", model.UpdateQuantityOnHand));

					updateCommand.Parameters.Add(new SqlParameter("@SKU", model.Sku));
					selectCommand.Parameters.Add(new SqlParameter("@SKU", model.Sku));

					updateCommand.Parameters.Add(new SqlParameter("@WarehouseID", model.TargetWarehouseId));
					selectCommand.Parameters.Add(new SqlParameter("@WarehouseID", model.TargetWarehouseId));

					connection.Open();
					var recordsAffected = updateCommand.ExecuteNonQuery();
					result = new UpdateInventoryItemModelResponse();
					result.Id = model.Id;
					result.Success = recordsAffected == 1;
					if (!result.Success)
					{
						result.ErrorCode = (int)UpdateInventoryErrorKinds.NotFound;
						result.ErrorMessage = "SKU and/or SAP Code not available in requested warehouse";
					}
					else
					{
						var effect = selectCommand.ExecuteReader();
						if (effect.Read())
						{
							ConvertToUpdateModel((IDataRecord)effect, result);
						}
						else
						{
							result.ErrorCode = (int)UpdateInventoryErrorKinds.Unknown;
							result.ErrorMessage = "Unable to retrieve operational effect.";
						}
					}
				}
				catch (Exception ex)
				{
					// add additional inventory error handling here.
					result = new UpdateInventoryItemModelResponse() { Id = model.Id, Success = false, ErrorCode = (int)UpdateInventoryErrorKinds.Unknown, ErrorMessage = ex.Message };
				}
			}

			return result;
		}

		public enum UpdateInventoryErrorKinds
		{
			NoError = 0,
			NotFound = 1,
			Unknown = 2
		}

	}
}
