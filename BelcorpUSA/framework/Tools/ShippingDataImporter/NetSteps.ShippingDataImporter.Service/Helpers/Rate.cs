using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Excel;

namespace NetSteps.ShippingDataImporter.Helpers
{
    public class Rate : Base
    {
        #region enums

        public enum ShppingRateType
        {
            TotalOrderCost = 1,
            TotalShippmentWeight = 2
        }

        #endregion

        #region properties

        private List<ShippingRateGroupStruct> _uniqueShippingRateGroups { get; set; }
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Rate"/> class.
        /// </summary>
        /// <param name="worksheet">
        /// The worksheet.
        /// </param>
        /// <param name="orderTypes">
        /// The order types.
        /// </param>
        /// <param name="stateRegions">
        /// The state regions.
        /// </param>
        public Rate(Worksheet worksheet)
            : base(worksheet)
        {
            this.Execute();
            this._uniqueShippingRateGroups = this.GetUniqueShippingRateGroups();
        }

        #endregion

        /// <summary>
        /// Sets the structure list
        /// </summary>
        public override void SetStructureList()
        {
            var sw = new Stopwatch();
            var startRange = this.GetRangeByName("RateStart");

            var collection = new List<ObjectStruct>();
            var fails = 0;
            var row = startRange.Row;
            var column = startRange.Column;

            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!   Starting Read of Spreadsheet Rows for ShippingService Rates   !!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine(string.Empty);
            Console.WriteLine(" Started at : {0}", DateTime.Now);
            Console.WriteLine(string.Empty);

            sw.Start();

            while (true)
            {
                row++;

                ObjectStruct addItem = new ObjectStruct();

                Range range = this.CurrentWorksheet.Cells[row, column];

                var rangeValid = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 8]);

                if (rangeValid != null && !string.IsNullOrWhiteSpace(rangeValid.Text) && ((string)rangeValid.Text).ToLower() != "lb ranges (from)".ToLower())
                {
                    addItem.LBRangeFrom = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 0]).Text;
                    addItem.LBRangeTo = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 1]).Text;

                    addItem.SubTotalRangeFrom = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 3]).Text;
                    addItem.SubTotalRangeTo = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 4]).Text;
                    addItem.FlatAmount = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 5]).Text;

                    addItem.PercentAmount = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 7]).Text;

                    addItem.CurrencyName = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 8]).Text;

                    addItem.ShippingRegionName = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 9]).Text;
                    addItem.ShippingMethodName = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 10]).Text;

                    addItem.HandlingFee = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 11]).Text;
                    addItem.DirectShipAmount = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 12]).Text;

                    addItem.OrderType = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 13]).Text;
                    addItem.OrderTypeExclusions01 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 14]).Text;
                    addItem.OrderTypeExclusions02 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 15]).Text;
                    addItem.OrderTypeExclusions03 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 16]).Text;
                    addItem.OrderTypeExclusions04 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 17]).Text;
                    addItem.OrderTypeExclusions05 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 18]).Text;
                    addItem.OrderTypeExclusions06 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 19]).Text;
                    addItem.OrderTypeExclusions07 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 20]).Text;
                    addItem.OrderTypeExclusions08 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 21]).Text;
                    addItem.OrderTypeExclusions09 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 22]).Text;
                    addItem.OrderTypeExclusions10 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 23]).Text;
                    addItem.OrderTypeExclusions11 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 24]).Text;
                    addItem.OrderTypeExclusions12 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 25]).Text;
                    addItem.OrderTypeExclusions13 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 26]).Text;
                    addItem.OrderTypeExclusions14 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 27]).Text;
                    addItem.OrderTypeExclusions15 = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 28]).Text;

                    addItem.MinimumAmount = ((Range)this.CurrentWorksheet.Cells[range.Row, range.Column + 29]).Text;

                    collection.Add(addItem);
                }
                else if (fails++ > 10)
                    break;
            }

            this._structureList = collection;
            sw.Stop();
            var ts = sw.Elapsed;
            var elapsedTime = string.Format(
                "{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Console.WriteLine(string.Empty);
            Console.WriteLine("Stopped at : {0}", DateTime.Now);
            Console.WriteLine("Elapsed time : {0}", elapsedTime);
            Console.WriteLine(string.Empty);
        }

        public override void BuildSQLStatement()
        {
            //    int count = 0;

            //    AppendCommentLine(SB);
            //    SB.AppendLine("-- Start ShippingService Rates Section");
            //    AppendCommentLine(SB);

            //    //BuildShippingRateGroupsSQL(ref count);

            //    //count = BuildShippingOrderTypesSQL(count, _uniqueRateGroups);

            //    //count = BuildShippingRatesSQL(count, uniqueRateGroups);
        }

        /// <summary>
        /// Generates SQL for ShippingOrderTypes
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// returns a StringBuilder
        /// </returns>
        public string BuildShippingOrderTypesSQL(ref int count)
        {
            var sqlStringBuilder = new StringBuilder();

            this.AppendCommentLine(sqlStringBuilder);
            sqlStringBuilder.AppendLine("-- Start ShippingService Order Types");
            this.AppendCommentLine(sqlStringBuilder);

            //Get Unique Rate Groups
            List<ShippingOrderTypeStruct> shippingOrderTypes = this.GetShippingOrderTypes(this._uniqueShippingRateGroups);

            foreach (var item in shippingOrderTypes)
            {
                count++;

                sqlStringBuilder.AppendLine(string.Empty);

                this.AppendCommentLine(sqlStringBuilder);
                sqlStringBuilder.AppendLine(string.Format("-- ShippingService Order Type - {0}", count));
                this.AppendCommentLine(sqlStringBuilder);

                string template = GetTemplate("ShippingOrderTypeTemplate");

                ReplaceToken(ref template, "WorksheetID", this.WorksheetIdentifier);
                ReplaceToken(ref template, "Count", count.ToString());

                ReplaceToken(ref template, "ShippingOrdertypeOrderType", item.ShippingOrdertypeOrderType);
                ReplaceToken(ref template, "ShippingOrdertypeShippingMethodName", item.ShippingOrdertypeShippingMethodName);
                ReplaceToken(ref template, "ShippingOrdertypeShippingRegionName", item.ShippingOrdertypeShippingRegionName);
                ReplaceToken(ref template, "ShippingOrdertypeShippingRateGroupName", item.ShippingOrdertypeShippingRateGroupName);
                ReplaceToken(ref template, "ShippingOrdertypeCountryName", item.ShippingOrdertypeCountryName);
                ReplaceToken(ref template, "ShippingOrdertypeOverrideAmount", item.ShippingOrdertypeOverrideAmount);
                ReplaceToken(ref template, "ShippingOrdertypeOverrideInclusive", item.ShippingOrdertypeOverrideInclusive ? "1" : "0");
                ReplaceToken(ref template, "ShippingOrdertypeAllowDirectShipments", item.ShippingOrdertypeAllowDirectShipments ? "1" : "0");
                ReplaceToken(ref template, "ShippingOrdertypeIsDefaultShippingMethod", item.ShippingOrdertypeIsDefaultShippingMethod ? "1" : "0");

                sqlStringBuilder.AppendLine(template);
                sqlStringBuilder.AppendLine(string.Empty);
            }

            return sqlStringBuilder.ToString();
        }

        /// <summary>
        /// Generates SQL for ShippingService Rate Groups
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// returns a StringBuilder
        /// </returns>
        public string BuildShippingRateGroupsSQL(ref int count)
        {
            var sqlStringBuilder = new StringBuilder();

            this.AppendCommentLine(sqlStringBuilder);
            sqlStringBuilder.AppendLine("-- Start ShippingService Rate Groups");
            this.AppendCommentLine(sqlStringBuilder);

            //Get Unique Rate Groups
            var uniqueRateGroups = this.GetUniqueShippingRateGroups();

            foreach (var item in uniqueRateGroups)
            {
                count++;

                sqlStringBuilder.AppendLine(string.Empty);

                this.AppendCommentLine(sqlStringBuilder);
                sqlStringBuilder.AppendLine(string.Format("-- ShippingService Rate Group - ({0} - {1})", item.ShippingRateGroupName, item.CurrencyName));
                this.AppendCommentLine(sqlStringBuilder);

                string template = GetTemplate("RateGroupTemplate");

                ReplaceToken(ref template, "WorksheetID", this.WorksheetIdentifier);
                ReplaceToken(ref template, "Count", count.ToString());

                ReplaceToken(ref template, "ShippingRateGroupName", item.ShippingRateGroupName);
                ReplaceToken(ref template, "ShippingRateGroupDescription", item.ShippingRateGroupDescription);
                ReplaceToken(ref template, "ShippingRateGroupGroupCode", item.ShippingRateGroupGroupCode);

                sqlStringBuilder.AppendLine(template);

                sqlStringBuilder.AppendLine(string.Empty);
            }
            return sqlStringBuilder.ToString();
        }

        /// <summary>
        /// Generates ShippingService Rates SQL
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// returns a StringBuilder
        /// </returns>
        //public StringBuilder BuildShippingRatesSQL(ref int count)
        //{
        //    var sqlStringBuilder = new StringBuilder();

        //    AppendCommentLine(sqlStringBuilder);
        //    sqlStringBuilder.AppendLine("-- Start ShippingService Rates");
        //    AppendCommentLine(sqlStringBuilder);

        //    //Get Unique Rate Groups
        //    List<ShippingRateStruct> shippingRates = GetShippingRates();

        //    count = 0;

        //    foreach (var item in shippingRates)
        //    {
        //        count++;

        //        sqlStringBuilder.AppendLine(string.Empty);

        //        AppendCommentLine(sqlStringBuilder);
        //        sqlStringBuilder.AppendLine(string.Format("-- ShippingService Rate - ROW {0}", count + 3));
        //        AppendCommentLine(sqlStringBuilder);

        //        string template = GetTemplate("ShippingRateTemplate");

        //        ReplaceToken(ref template, "WorksheetID", WorksheetIdentifier);
        //        ReplaceToken(ref template, "Count", count.ToString());

        //        ReplaceToken(ref template, "ShippingRateGroupName", item.ShippingRateGroupName);
        //        ReplaceToken(ref template, "CurrencyName", item.CurrencyName);
        //        ReplaceToken(ref template, "ValueName", item.ValueName);
        //        ReplaceToken(ref template, "ValueFrom", item.ValueFrom);
        //        ReplaceToken(ref template, "ValueTo", item.ValueTo);
        //        ReplaceToken(ref template, "ShippingAmount", item.ShippingAmount);
        //        ReplaceToken(ref template, "DirectShipmentFee", item.DirectShipmentFee);
        //        ReplaceToken(ref template, "HandlingFee", item.HandlingFee);
        //        ReplaceToken(ref template, "IncrementalAmount", item.IncrementalAmount);
        //        ReplaceToken(ref template, "IncrementalFee", item.IncrementalFee);
        //        ReplaceToken(ref template, "ShippingPercentage", item.ShippingPercentage);
        //        ReplaceToken(ref template, "ShippingRateTypeID", item.ShippingRateTypeID);
        //        ReplaceToken(ref template, "MinimumAmount", item.MinimumAmount);

        //        sqlStringBuilder.AppendLine(template);

        //        sqlStringBuilder.AppendLine(string.Empty);
        //    }

        //    return sqlStringBuilder;
        //}

        #region HelperMethods

        public List<ShippingRateStruct> GetShippingRates()
        {
            var shippingRates = new List<ShippingRateStruct>();

            foreach (var item in this._structureList)
            {
                string shippingRateGroupName = this.GetShippingRateGroupName(this._uniqueShippingRateGroups, item);
                decimal valueFrom = this.GetValueFrom(item);
                decimal valueTo = this.GetValueTo(item);
                decimal? shippingAmount = this.GetShippingAmount(item);
                decimal directShipmentFee = this.GetDirectShipmentFee(item);
                decimal handlingFee = this.GetHandlingFee(item);
                decimal incrementalAmount = this.GetIncrementalAmount(item);
                decimal incrementalFee = this.GetIncrementalFee(item);
                decimal? shippingPercentage = this.GetShippingPercentage(item);
                int shippingRateTypeID = this.GetShippingRateTypeID(item);
                decimal? minimumAmount = this.GetSMinimumAmount(item);

                shippingRates.Add(new ShippingRateStruct()
                {
                    ShippingRateGroupName = shippingRateGroupName,
                    CurrencyName = item.CurrencyName,
                    ValueName = shippingRateGroupName,
                    ValueFrom = valueFrom.ToString(),
                    ValueTo = valueTo.ToString(),
                    ShippingAmount = shippingAmount.HasValue ? shippingAmount.Value.ToString() : "NULL",
                    DirectShipmentFee = directShipmentFee.ToString(),
                    HandlingFee = handlingFee.ToString(),
                    IncrementalAmount = incrementalAmount.ToString(),
                    IncrementalFee = incrementalFee.ToString(),
                    ShippingPercentage = shippingPercentage.HasValue ? (shippingPercentage.Value / 100).ToString() : "NULL",
                    ShippingRateTypeID = shippingRateTypeID.ToString(),
                    MinimumAmount = minimumAmount.HasValue ? minimumAmount.Value.ToString() : "NULL"
                });
            }

            return shippingRates;
        }

        private decimal? GetSMinimumAmount(ObjectStruct item)
        {
            decimal? result = null;

            if (!string.IsNullOrWhiteSpace(item.MinimumAmount))
            {
                result = this.SanatizeDecimal(item.MinimumAmount);
            }

            return result;
        }

        private int GetShippingRateTypeID(ObjectStruct item)
        {
            int result = 0;

            if (!string.IsNullOrWhiteSpace(item.LBRangeFrom) && !string.IsNullOrWhiteSpace(item.LBRangeTo))
            {
                result = (int)ShppingRateType.TotalShippmentWeight;
            }
            else if (!string.IsNullOrWhiteSpace(item.SubTotalRangeFrom) && !string.IsNullOrWhiteSpace(item.SubTotalRangeTo))
            {
                result = (int)ShppingRateType.TotalOrderCost;
            }

            return result;
        }

        private decimal? GetShippingPercentage(ObjectStruct item)
        {
            decimal? result = null;

            if (!string.IsNullOrWhiteSpace(item.PercentAmount))
            {
                result = this.SanatizeDecimal(item.PercentAmount);
            }

            return result;
        }

        private decimal GetIncrementalFee(ObjectStruct item)
        {
            decimal result = 0;

            return result;
        }

        private decimal GetIncrementalAmount(ObjectStruct item)
        {
            decimal result = 0;

            return result;
        }

        private decimal GetHandlingFee(ObjectStruct item)
        {
            decimal result = 0;

            result = this.SanatizeDecimal(item.HandlingFee);

            return result;
        }

        private decimal GetDirectShipmentFee(ObjectStruct item)
        {
            decimal result = 0;

            result = this.SanatizeDecimal(item.DirectShipAmount);

            return result;
        }

        private decimal? GetShippingAmount(ObjectStruct item)
        {
            decimal? result = 0;

            if (this.GetShippingPercentage(item) == null)
            {
                result = this.SanatizeDecimal(item.FlatAmount);
            }
            else
            {
                result = null;
            }

            return result;
        }

        private decimal GetValueTo(ObjectStruct item)
        {
            decimal result = 0;

            switch ((ShppingRateType)this.GetShippingRateTypeID(item))
            {
                case ShppingRateType.TotalOrderCost:
                    result = this.SanatizeDecimal(item.SubTotalRangeTo);
                    break;
                case ShppingRateType.TotalShippmentWeight:
                    result = this.SanatizeDecimal(item.LBRangeTo);
                    break;
                default:
                    break;
            }

            return result;
        }

        private decimal GetValueFrom(ObjectStruct item)
        {
            decimal result = 0;

            switch ((ShppingRateType)this.GetShippingRateTypeID(item))
            {
                case ShppingRateType.TotalOrderCost:
                    result = this.SanatizeDecimal(item.SubTotalRangeFrom);
                    break;
                case ShppingRateType.TotalShippmentWeight:
                    result = this.SanatizeDecimal(item.LBRangeFrom);
                    break;
                default:
                    break;
            }

            return result;
        }

        private decimal SanatizeDecimal(string value)
        {
            decimal result = 0;

            decimal.TryParse(value.Replace("$", "").Replace(",", "").Replace("%", "").Trim(), out result);

            return result;
        }

        private string GetShippingRateGroupName(List<ShippingRateGroupStruct> uniqueRateGroups, ObjectStruct item)
        {
            string result = string.Empty;

            foreach (var rateGroup in uniqueRateGroups)
            {
                if (
                       rateGroup.CurrencyName == item.CurrencyName
                    && rateGroup.ShippingRegionName == item.ShippingRegionName
                    && rateGroup.ShippingMethodName == item.ShippingMethodName
                    && rateGroup.OrderType == item.OrderType
                    && rateGroup.OrderTypeExclusions01 == item.OrderTypeExclusions01
                    && rateGroup.OrderTypeExclusions01 == item.OrderTypeExclusions01
                    && rateGroup.OrderTypeExclusions02 == item.OrderTypeExclusions02
                    && rateGroup.OrderTypeExclusions03 == item.OrderTypeExclusions03
                    && rateGroup.OrderTypeExclusions04 == item.OrderTypeExclusions04
                    && rateGroup.OrderTypeExclusions05 == item.OrderTypeExclusions05
                    && rateGroup.OrderTypeExclusions06 == item.OrderTypeExclusions06
                    && rateGroup.OrderTypeExclusions07 == item.OrderTypeExclusions07
                    && rateGroup.OrderTypeExclusions08 == item.OrderTypeExclusions08
                    && rateGroup.OrderTypeExclusions09 == item.OrderTypeExclusions09
                    && rateGroup.OrderTypeExclusions10 == item.OrderTypeExclusions10
                    && rateGroup.OrderTypeExclusions11 == item.OrderTypeExclusions11
                    && rateGroup.OrderTypeExclusions12 == item.OrderTypeExclusions12
                    && rateGroup.OrderTypeExclusions13 == item.OrderTypeExclusions13
                    && rateGroup.OrderTypeExclusions14 == item.OrderTypeExclusions14
                    && rateGroup.OrderTypeExclusions15 == item.OrderTypeExclusions15
                    && rateGroup.MinimumAmount == item.MinimumAmount
                    )
                {
                    result = rateGroup.ShippingRateGroupName;
                    break;
                }
            }

            return result;
        }

        private List<ShippingOrderTypeStruct> GetShippingOrderTypes(List<ShippingRateGroupStruct> uniqueRateGroups)
        {
            List<ShippingOrderTypeStruct> results = new List<ShippingOrderTypeStruct>();
            List<StateRegion.ObjectStruct> stateRegions = this.GetUniqueCountryRegions();
            List<OrderType.ObjectStruct> orderTypes;

            foreach (var item in uniqueRateGroups)
            {
                orderTypes = this.GetUniqueRatesGroupOrderTypeList(item);

                foreach (var orderType in orderTypes)
                {
                    var uniqueStateRegions = stateRegions.Where(x => x.ShippingRegionName == item.ShippingRegionName);

                    foreach (var country in uniqueStateRegions)
                    {
                        results.Add(new ShippingOrderTypeStruct()
                        {
                            ShippingOrdertypeOrderType = orderType.Name,
                            ShippingOrdertypeShippingMethodName = item.ShippingMethodName,
                            ShippingOrdertypeShippingRegionName = item.ShippingRegionName,
                            ShippingOrdertypeShippingRateGroupName = item.ShippingRateGroupName,
                            ShippingOrdertypeCountryName = country.CountryName,
                            ShippingOrdertypeOverrideAmount = "0.00",
                            ShippingOrdertypeOverrideInclusive = true,
                            ShippingOrdertypeAllowDirectShipments = true,
                            ShippingOrdertypeIsDefaultShippingMethod = false
                        });
                    }
                }
            }

            return results;
        }

        private List<OrderType.ObjectStruct> GetUniqueRatesGroupOrderTypeList(ShippingRateGroupStruct item)
        {
            List<OrderType.ObjectStruct> orderTypes = new List<OrderType.ObjectStruct>();

            if (item.OrderType.ToLower().Trim() == "all".ToLower().Trim())
            {
                if (!this.HasOrderTypeExclusions(item))
                {
                    StaticCrap.OrderTypes.ForEach(x => orderTypes.Add(new OrderType.ObjectStruct() { Name = x.Name }));
                }
                else
                {
                    foreach (var orderType in StaticCrap.OrderTypes)
                    {
                        if (!this.HasOrderTypeExclusion(item, orderType.Name))
                        {
                            orderTypes.Add(orderType);
                        }
                    }
                }
            }
            else
            {
                orderTypes.Add(new OrderType.ObjectStruct { Name = item.OrderType });
            }

            return orderTypes;
        }

        private bool HasOrderTypeExclusions(ShippingRateGroupStruct item)
        {
            if (!string.IsNullOrWhiteSpace(item.OrderTypeExclusions01)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions01)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions02)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions03)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions04)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions05)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions06)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions07)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions08)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions09)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions10)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions11)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions12)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions13)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions14)
                || !string.IsNullOrWhiteSpace(item.OrderTypeExclusions15)
                )
            {
                return true;
            }

            return false;
        }

        private bool HasOrderTypeExclusion(ShippingRateGroupStruct item, string orderTypeName)
        {

            if (item.OrderTypeExclusions01.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions02.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions03.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions04.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions05.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions06.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions07.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions08.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions09.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions10.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions11.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions12.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions13.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions14.ToLower() == orderTypeName.ToLower()
                || item.OrderTypeExclusions15.ToLower() == orderTypeName.ToLower()
                )
            {
                return true;
            }

            return false;
        }

        private List<ShippingRateGroupStruct> GetUniqueShippingRateGroups()
        {
            int count = 1;

            //return
            var structList = this._structureList.Select(x =>
                            new
                            {
                                ShippingRegionName = x.ShippingRegionName,
                                ShippingMethodName = x.ShippingMethodName,
                                OrderType = x.OrderType,
                                CurrencyName = x.CurrencyName,
                                OrderTypeExclusions01 = x.OrderTypeExclusions01,
                                OrderTypeExclusions02 = x.OrderTypeExclusions02,
                                OrderTypeExclusions03 = x.OrderTypeExclusions03,
                                OrderTypeExclusions04 = x.OrderTypeExclusions04,
                                OrderTypeExclusions05 = x.OrderTypeExclusions05,
                                OrderTypeExclusions06 = x.OrderTypeExclusions06,
                                OrderTypeExclusions07 = x.OrderTypeExclusions07,
                                OrderTypeExclusions08 = x.OrderTypeExclusions08,
                                OrderTypeExclusions09 = x.OrderTypeExclusions09,
                                OrderTypeExclusions10 = x.OrderTypeExclusions10,
                                OrderTypeExclusions11 = x.OrderTypeExclusions11,
                                OrderTypeExclusions12 = x.OrderTypeExclusions12,
                                OrderTypeExclusions13 = x.OrderTypeExclusions13,
                                OrderTypeExclusions14 = x.OrderTypeExclusions14,
                                OrderTypeExclusions15 = x.OrderTypeExclusions15,
                                MinimumAmount = x.MinimumAmount
                            }).Distinct();

            return structList.Select(x =>
                                new ShippingRateGroupStruct
                                {
                                    ShippingRateGroupName = this.GetShippingRateGroupName(count, x.ShippingMethodName),
                                    ShippingRateGroupDescription = this.GetShippingRateGroupDescription(count, x.ShippingMethodName, x.OrderType, x.CurrencyName),
                                    ShippingRateGroupGroupCode = this.GetShippingRateGroupGroupCode(count++, x.ShippingMethodName),
                                    ShippingRegionName = x.ShippingRegionName,
                                    ShippingMethodName = x.ShippingMethodName,
                                    CurrencyName = x.CurrencyName,
                                    OrderType = x.OrderType,
                                    OrderTypeExclusions01 = x.OrderTypeExclusions01,
                                    OrderTypeExclusions02 = x.OrderTypeExclusions02,
                                    OrderTypeExclusions03 = x.OrderTypeExclusions03,
                                    OrderTypeExclusions04 = x.OrderTypeExclusions04,
                                    OrderTypeExclusions05 = x.OrderTypeExclusions05,
                                    OrderTypeExclusions06 = x.OrderTypeExclusions06,
                                    OrderTypeExclusions07 = x.OrderTypeExclusions07,
                                    OrderTypeExclusions08 = x.OrderTypeExclusions08,
                                    OrderTypeExclusions09 = x.OrderTypeExclusions09,
                                    OrderTypeExclusions10 = x.OrderTypeExclusions10,
                                    OrderTypeExclusions11 = x.OrderTypeExclusions11,
                                    OrderTypeExclusions12 = x.OrderTypeExclusions12,
                                    OrderTypeExclusions13 = x.OrderTypeExclusions13,
                                    OrderTypeExclusions14 = x.OrderTypeExclusions14,
                                    OrderTypeExclusions15 = x.OrderTypeExclusions15,
                                    MinimumAmount = x.MinimumAmount
                                }).ToList();
        }

        private List<StateRegion.ObjectStruct> GetUniqueCountryRegions()
        {
            return StaticCrap.StateRegions.Select(x =>
                            new
                            {
                                ShippingRegionName = x.ShippingRegionName,
                                CountryName = x.CountryName
                            }).Distinct().Select(x =>
                                new StateRegion.ObjectStruct
                                {
                                    ShippingRegionName = x.ShippingRegionName,
                                    CountryName = x.CountryName
                                }

                                ).ToList();
        }

        private string GetShippingRateGroupName(int count, string shippingMethodName)
        {
            var result = string.Empty;

            result = string.Format("{0} - {1}", count, shippingMethodName);

            if (result.Length > 50)
            {
                result = result.Substring(0, 49);
            }

            return result;
        }

        private string GetShippingRateGroupDescription(int count, string shippingMethodName, string orderType, string currencyName)
        {
            var result = string.Empty;

            result = string.Format("{0} - {1} ({2} - {3})", count, shippingMethodName, orderType, currencyName);

            if (result.Length > 100)
            {
                result = result.Substring(0, 99);
            }

            return result;
        }

        private string GetShippingRateGroupGroupCode(int count, string shippingMethodName)
        {
            return this.GetShippingRateGroupName(count, shippingMethodName);
        }

        #endregion

        #region structs

        public struct ObjectStruct
        {
            public string LBRangeFrom { get; set; }
            public string LBRangeTo { get; set; }
            public string SubTotalRangeFrom { get; set; }
            public string SubTotalRangeTo { get; set; }
            public string FlatAmount { get; set; }
            public string PercentAmount { get; set; }
            public string CurrencyName { get; set; }
            public string ShippingRegionName { get; set; }
            public string ShippingMethodName { get; set; }
            public string HandlingFee { get; set; }
            public string DirectShipAmount { get; set; }
            public string OrderType { get; set; }
            public string OrderTypeExclusions01 { get; set; }
            public string OrderTypeExclusions02 { get; set; }
            public string OrderTypeExclusions03 { get; set; }
            public string OrderTypeExclusions04 { get; set; }
            public string OrderTypeExclusions05 { get; set; }
            public string OrderTypeExclusions06 { get; set; }
            public string OrderTypeExclusions07 { get; set; }
            public string OrderTypeExclusions08 { get; set; }
            public string OrderTypeExclusions09 { get; set; }
            public string OrderTypeExclusions10 { get; set; }
            public string OrderTypeExclusions11 { get; set; }
            public string OrderTypeExclusions12 { get; set; }
            public string OrderTypeExclusions13 { get; set; }
            public string OrderTypeExclusions14 { get; set; }
            public string OrderTypeExclusions15 { get; set; }
            public string MinimumAmount { get; set; }
        }

        public struct ShippingRateGroupStruct
        {
            public string ShippingRateGroupName { get; set; }
            public string ShippingRateGroupDescription { get; set; }
            public string ShippingRateGroupGroupCode { get; set; }
            public string ShippingRegionName { get; set; }
            public string ShippingMethodName { get; set; }
            public string CurrencyName { get; set; }
            public string OrderType { get; set; }
            public string OrderTypeExclusions01 { get; set; }
            public string OrderTypeExclusions02 { get; set; }
            public string OrderTypeExclusions03 { get; set; }
            public string OrderTypeExclusions04 { get; set; }
            public string OrderTypeExclusions05 { get; set; }
            public string OrderTypeExclusions06 { get; set; }
            public string OrderTypeExclusions07 { get; set; }
            public string OrderTypeExclusions08 { get; set; }
            public string OrderTypeExclusions09 { get; set; }
            public string OrderTypeExclusions10 { get; set; }
            public string OrderTypeExclusions11 { get; set; }
            public string OrderTypeExclusions12 { get; set; }
            public string OrderTypeExclusions13 { get; set; }
            public string OrderTypeExclusions14 { get; set; }
            public string OrderTypeExclusions15 { get; set; }
            public string MinimumAmount { get; set; }

			public override string ToString()
			{
				return string.Format("{0} {1}", ShippingRateGroupName, OrderType);
			}
        }

        public struct ShippingOrderTypeStruct
        {
            public string ShippingOrdertypeOrderType { get; set; }
            public string ShippingOrdertypeShippingMethodName { get; set; }
            public string ShippingOrdertypeShippingRegionName { get; set; }
            public string ShippingOrdertypeShippingRateGroupName { get; set; }
            public string ShippingOrdertypeCountryName { get; set; }
            public string ShippingOrdertypeOverrideAmount { get; set; }
            public bool ShippingOrdertypeOverrideInclusive { get; set; }
            public bool ShippingOrdertypeAllowDirectShipments { get; set; }
            public bool ShippingOrdertypeIsDefaultShippingMethod { get; set; }
        }

        public struct ShippingRateStruct
        {
            public string ShippingRateGroupName { get; set; }
            public string CurrencyName { get; set; }
            public string ValueName { get; set; }
            public string ValueFrom { get; set; }
            public string ValueTo { get; set; }
            public string ShippingAmount { get; set; }
            public string DirectShipmentFee { get; set; }
            public string HandlingFee { get; set; }
            public string IncrementalAmount { get; set; }
            public string IncrementalFee { get; set; }
            public string ShippingPercentage { get; set; }
            public string ShippingRateTypeID { get; set; }
            public string MinimumAmount { get; set; }
        }

        List<ObjectStruct> _structureList = null;

        #endregion

    }
}
