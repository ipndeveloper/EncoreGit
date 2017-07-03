using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EFTQuery.Common;
using EFTQuery.NachaGenerator.Common;
using NetSteps.Common.Ach;
using NetSteps.Encore.Core.IoC;


namespace EFTQuery.NachaGenerator.Core
{
    public class NachaGenerator : INachaGenerator
    {
        public INachaGeneratorResult Generate(INachaGeneratorRequest request)
        {
            var response = SetupResponse();

            try
            {
                int countryID;
                Int32.TryParse(request.CountryCode, out countryID);

                List<BatchHeaderRecord> batches = new List<BatchHeaderRecord>();
                int batchNum = 1;

                string[] EFTClassTypes = Convert.ToString(request.EftClassType).Split(',');

                foreach (string classType in EFTClassTypes)
                {
                    var ordersByCountry = Create.New<IEFTQueryProcessor>().GetTransfersByClassTypeAndCountryID(classType, countryID);

                    if (ordersByCountry == null || ordersByCountry.ToList().Count == 0)
                        throw new NullReferenceException("No query processor results");

                    response.OrderPaymentIDs.AddRange(ordersByCountry.Select(x => x.OrderPaymentId).ToList().ConvertAll(new Converter<string, int>(Convert.ToInt32)));
                    BatchHeaderRecord batchRecord = SetupBatchHeaderRecord(request, ordersByCountry, batchNum, classType, response);
                    if(batchRecord.EntryDetail != null && batchRecord.EntryDetail.Count > 0)
                        batches.Add(batchRecord);
                }

                if(batches.Count <= 0)
                    throw new NullReferenceException("No query processor results");

                var fileHeaderRecord = SetupFileHeaderRecord(request, batches);

                string record = request.SecurityString + " \r\n" + fileHeaderRecord.ToString();
                byte[] nacha = Encoding.UTF8.GetBytes(record);
                response.NachaFile = new MemoryStream(nacha);

                response.Status = ResultState.Success;
            } 
            catch (Exception ex)
            {
                var error = Create.Mutation(Create.New<IError>(), m => { m.Error = ex.Message; });
                response.Errors.Add(error);
                response.Status = ResultState.Exception;
            }

            return response;
        }
        
        BatchHeaderRecord SetupBatchHeaderRecord(INachaGeneratorRequest request, IEnumerable<IEFTQueryProcessorResult> queryProcessorResults, int batchNum, string classType, INachaGeneratorResult response)
        {
            BatchHeaderRecord record = new BatchHeaderRecord();
                
                    record.EntryDetail = ProcessEntryDetails(queryProcessorResults, request.DetailTrace, response).ToList();
                    record.RecordTypeCode = "5";
                    record.ServiceClassCode = request.ServiceClassCode;
                    record.CompanyName = request.CompanyName;
                    record.DiscretionaryData = " ";
                    record.CompanyIdentification = request.EftComapnyIdentificationNumber;
                    record.StandardEntryClass = request.CountryCode == "2" ? "PPD" : classType;
                    record.CompanyEntryDescription = request.DescriptionOfTransction;
                    record.CompanyDescriptiveDate = request.CompanyDescriptiveDate.ToString("MMddyy");
                    record.EffectiveEntryDate = request.EffectiveDate.ToString("yyMMdd");
                    record.SettlementDate = "   ";
                    record.OriginatorStatusCode = "1";
                    record.OriginatingFinancialInstitution = request.CompanyRoutingNumber.Length > 8 ? request.CompanyRoutingNumber.Remove(8) : request.CompanyRoutingNumber;
                    record.BatchNumber = batchNum.ToString();
                

            decimal hashTotal = 0;
            decimal totalDebits = 0;
            foreach (var detail in record.EntryDetail)
            {
                hashTotal += decimal.Parse(detail.ReceivingDFIIdentification);
                totalDebits += decimal.Parse(detail.Amount);
            }

            //Here we take care of the batch record header control file
            record.BatchControl = new BatchControlRecord();
            
                record.BatchControl.RecordTypeCode = "8";
                record.BatchControl.ServiceClassCode = request.ServiceClassCode;
                record.BatchControl.EntryAddendaCount = record.EntryDetail.Count.ToString();
                record.BatchControl.EntryHash = hashTotal.ToString();
                record.BatchControl.TotalDebitEntryDollarAmount = totalDebits.ToString();
                record.BatchControl.TotalCreditEntryDollarAmount = "0000000000";
                record.BatchControl.CompanyIdentification = record.CompanyIdentification;
                record.BatchControl.MessageAuthenticationCode = "                   ";
                record.BatchControl.Reserved = "      ";
                record.BatchControl.OriginatingFinancialInstitutionID = request.CountryCode == "2" ? request.DetailTrace : record.OriginatingFinancialInstitution;
                record.BatchControl.BatchNumber = record.BatchNumber;




                return record;
        }

        FileHeaderRecord SetupFileHeaderRecord(INachaGeneratorRequest request, List<BatchHeaderRecord> batchHeaderRecords)
        {
            var fileHeaderRecord = Create.New<FileHeaderRecord>();

            decimal hashTotal = 0;
            decimal totalDebits = 0;
            foreach (BatchHeaderRecord batchHeaderRecord in batchHeaderRecords)
            {
                foreach (EntryDetailRecord detail in batchHeaderRecord.EntryDetail)
                {
                    hashTotal += decimal.Parse(detail.ReceivingDFIIdentification);
                    totalDebits += decimal.Parse(detail.Amount);
                }
            }
            fileHeaderRecord.BatchHeader = new List<BatchHeaderRecord>();
            fileHeaderRecord.BatchHeader = batchHeaderRecords;
            fileHeaderRecord.RecordTypeCode = "1";
            fileHeaderRecord.PriorityCode = "01";
            fileHeaderRecord.ImmediateDestination = request.CompanyRoutingNumber;
            fileHeaderRecord.ImmediateOrigin = request.EftComapnyIdentificationNumber;
            fileHeaderRecord.FileCreationDate = request.CompanyDescriptiveDate.ToString("yyMMdd"); ;
            fileHeaderRecord.FileCreationTime = request.CompanyDescriptiveDate.ToString("HHmm");
            fileHeaderRecord.FileIDModifier = "A";
            fileHeaderRecord.RecordSize = "094";
            fileHeaderRecord.BlockingFactor = "10";
            fileHeaderRecord.FormatCode = "1";
            fileHeaderRecord.ImmediateDestinationName = request.ImmediateDestinationName;
            fileHeaderRecord.ImmediateOriginName = request.CompanyName;
            fileHeaderRecord.ReferenceCode = "        ";

            int detailCount = 0;
            foreach (BatchHeaderRecord record in fileHeaderRecord.BatchHeader)
                detailCount += record.EntryDetail.Count;
            fileHeaderRecord.FileControl = new FileControlRecord
            {
                RecordTypeCode = "9",
                BatchCount = "000001",
                BlockCount = (detailCount + (fileHeaderRecord.BatchHeader.Count * 2) + 2).ToString(), //Count all the entry detail blocks + all the batchs*2(since we have a batch control in each batch) + 2 since we only have 1 File Header and 1 File Control
                EntryAddendaCount = detailCount.ToString(),
                EntryHash = hashTotal.ToString(),
                TotalDebitEntryDollarAmountInFile = totalDebits.ToString(),
                TotalCreditEntryDollarAmountInFile = "00000000000",
                Reserved = "                                       "
            };



            return fileHeaderRecord;
        }

        INachaGeneratorResult SetupResponse()
        {
            var response = Create.New<INachaGeneratorResult>();
            response.Errors = Create.New<List<IError>>();
            response.OrderPaymentIDs = new List<int>();
            return response;
        }

        IEnumerable<EntryDetailRecord> ProcessEntryDetails(IEnumerable<IEFTQueryProcessorResult> results, string companyRoutingNum, INachaGeneratorResult response)
        {
            for (int i = 0; i < results.Count(); i++)
            {
                var result = results.ElementAt(i);
                int traceNum = i + 1;
                companyRoutingNum = companyRoutingNum.Length > 8 ? companyRoutingNum.Remove(8) : companyRoutingNum;
                decimal testDecimal;

                var isNum = decimal.TryParse(result.RoutingNumber, out testDecimal);

                if (!isNum)
                {
                    int orderPaymentID;
                    isNum = int.TryParse(result.OrderPaymentId, out orderPaymentID);
                    if (isNum)
                        response.OrderPaymentIDs.Remove(orderPaymentID);
                    continue;
                }

                isNum = decimal.TryParse(result.Amount, out testDecimal);

                if (!isNum)
                {
                    int orderPaymentID;
                    isNum = int.TryParse(result.OrderPaymentId, out orderPaymentID);
                    if(isNum)
                        response.OrderPaymentIDs.Remove(orderPaymentID);
                    continue;
                }

                if (string.IsNullOrEmpty(result.AccountNumber))
                {
                    int orderPaymentID;
                    isNum = int.TryParse(result.OrderPaymentId, out orderPaymentID);
                    if (isNum)
                        response.OrderPaymentIDs.Remove(orderPaymentID);
                    continue;
                }

                if (result.CountryCode == "CA")
                {
					yield return new EntryDetailRecord()
						{
							RecordTypeCode = "6",
							TransactionCode = string.IsNullOrEmpty(result.TransactionCode) ? "" : result.TransactionCode,
							ReceivingDFIIdentification = result.RoutingNumber.Length > 8 ? result.RoutingNumber.Remove(8) : PadItemLength(result.RoutingNumber, 9, true, '0').Remove(8),
							CheckDigit = result.RoutingNumber.Length > 8 ? result.RoutingNumber.LastOrDefault().ToString() : PadItemLength(result.RoutingNumber, 9, true, '0').LastOrDefault().ToString(),
							DFIAccountNumber = result.AccountNumber.Length > 17 ? result.AccountNumber.Substring(result.AccountNumber.Length - 17) : result.AccountNumber,
							Amount = result.Amount,
							IndividualIdentificationNumber = result.OrderId,
							IndividualName = string.IsNullOrEmpty(result.IndividualName) ? "" : result.IndividualName.Length > 22 ? result.IndividualName.Substring(0, 22) : result.IndividualName,
							PaymentTypeCode = string.IsNullOrEmpty(result.PaymentTypeCode) ? "" : result.PaymentTypeCode,
							AddendaRecordIndicator = "0",
							TraceNumber = companyRoutingNum + PadItemLength(traceNum.ToString(), string.IsNullOrEmpty(companyRoutingNum) ? 15 : 7, true, '0')
						};
                }
                else
                {
                    yield return new EntryDetailRecord()
                    {
                        RecordTypeCode = "6",
                        TransactionCode = string.IsNullOrEmpty(result.TransactionCode) ? "" : result.TransactionCode,
                        ReceivingDFIIdentification = result.RoutingNumber.Length > 8 ? result.RoutingNumber.Remove(8) : PadItemLength(result.RoutingNumber, 8, true, '0'),
                        CheckDigit = result.RoutingNumber.LastOrDefault().ToString(),
                        DFIAccountNumber = result.AccountNumber.Length > 17 ? result.AccountNumber.Substring(result.AccountNumber.Length - 17) : result.AccountNumber,
                        Amount = result.Amount,
                        IndividualIdentificationNumber = result.OrderId,
                        IndividualName = string.IsNullOrEmpty(result.IndividualName) ? "" : result.IndividualName.Length > 22 ? result.IndividualName.Substring(0, 22) : result.IndividualName,
                        PaymentTypeCode = string.IsNullOrEmpty(result.PaymentTypeCode) ? "" : result.PaymentTypeCode,
                        AddendaRecordIndicator = "0",
                        TraceNumber = companyRoutingNum + PadItemLength(traceNum.ToString(), string.IsNullOrEmpty(companyRoutingNum) ? 15 : 7, true, '0')
                    };
                }
            }
        }

        public string PadItemLength(string original, int fixedLength, bool padLeft = true, char padChar = ' ')
        {
            string fixedString = String.Empty;

            if (original == null)
            {
                original = String.Empty;
            }

            if (fixedLength > original.Length)
            {
                if (padLeft)
                {
                    fixedString = original.PadLeft(fixedLength, padChar);
                }
                else
                {
                    fixedString = original.PadRight(fixedLength, padChar);
                }
            }
            else if (fixedLength < original.Length)
            {
                throw new Exception("The original item length is longer than the fixed length of the item");
            }
            else
            {
                fixedString = original;
            }

            return fixedString;
        }

        public INachaGeneratorResult GenerateAndSave(INachaGeneratorRequest request, string destination)
        {
            var response = SetupResponse();

            try
            {
                response = Generate(request);

                if (response.Status == ResultState.Success)
                {
                    StreamWriter writer = new StreamWriter(destination);
                    StreamReader reader = new StreamReader(response.NachaFile);

                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                    writer.Close();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                var error = Create.Mutation(Create.New<IError>(), m => { m.Error = ex.Message; });
                response.Errors.Add(error);
                response.Status = ResultState.Exception;
            }
            
            return response;
        }

        public INachaGeneratorResult UpdateNachaSentDate(List<int> orderIDs)
        {
            var response = SetupResponse();

            try
            {
                var queryProcessorResults = Create.New<IEFTQueryProcessor>().UpdateNachaQueryProcessorResults(orderIDs);
                if (queryProcessorResults == null || queryProcessorResults.ToList().Count == 0)
                    throw new NullReferenceException("No query processor results");

                if (queryProcessorResults.All(r => r.Success))
                    response.Status = ResultState.Success;
                else
                {
                    response.Status = ResultState.Error;
                    var error = Create.Mutation(Create.New<IError>(), m => { m.Error = "Not all OrderPaymentIDs were updated!"; });
                    response.Errors.Add(error);
					var unsuccessfulResults = queryProcessorResults.Where(r => !r.Success);
					response.OrderPaymentIDs = unsuccessfulResults.Select(r => r.OrderPaymentId).ToList();
					foreach(var message in unsuccessfulResults.Select(r => r.Message).Distinct())
						response.Errors.Add(Create.Mutation(Create.New<IError>(), m => m.Error = message));
                }
            }
            catch (Exception ex)
            {
                var error = Create.Mutation(Create.New<IError>(), m => { m.Error = ex.Message; });
                response.Errors.Add(error);
                response.Status = ResultState.Exception;
            }
            
            return response;
        }

    }
}
