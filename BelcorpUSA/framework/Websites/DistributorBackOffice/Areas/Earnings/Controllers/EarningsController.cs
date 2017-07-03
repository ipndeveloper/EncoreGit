using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistributorBackOffice.Controllers;
using DistributorBackOffice.Areas.Earnings.Models;
using NetSteps.Data.Entities;
using DistributorBackOffice.Models;
using NetSteps.Data.Entities.Cache;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Business;
using NetSteps.Commissions.Common.Models;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Exceptions;
using System.Text;
using NetSteps.Commissions.Service.Models;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.EntityModels;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.Logic;
using DistributorBackOffice.Areas.Orders.Models;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Web.Mvc.Helpers;


namespace DistributorBackOffice.Areas.Earnings.Controllers
{
    public class EarningsController : AccountReportController<DownlineReportParameters>
    {
        public static class Functions
        {
            public const string Earnings = "Earnings";
        }

        protected ICommissionsService _commissionsService;
        public ICommissionsService CommissionsService
        {
            get
            {
                if (_commissionsService == null)
                {
                    _commissionsService = Create.New<ICommissionsService>();
                }
                return _commissionsService;
            }
        }

        public ActionResult Index(int periodId)
        {
            PeriodEarningsModel model = new PeriodEarningsModel();
          

            try
            {
              

                //var period = CommissionsService.GetPeriodsForAccount(CurrentAccountId).OrderByDescending(p => p.PeriodId).FirstOrDefault();
                var period = (periodId != 0) ? CommissionsService.GetPeriodsForAccount(CurrentAccountId).Where(p => p.PeriodId == periodId).FirstOrDefault() : CommissionsService.GetPeriodsForAccount(CurrentAccountId).OrderByDescending(p => p.PeriodId).FirstOrDefault();
                NetSteps.Data.Entities.Business.Earnings ablEarnings = new NetSteps.Data.Entities.Business.Earnings();

                if (period != null)
                {

                    DateTime pStartDate = period.StartDateUTC.ToLocalTime(), pEndDate = period.EndDateUTC.ToLocalTime();
                    CompanyAddressSearchData companyAddress = new AddressBusinessLogic().GetCompanyAddress(1);

                    if (companyAddress == null)
                    {
                        model.ErrorMessage = Translation.GetTerm("NoCompanyAddress", "Company Address not found.");
                    }
                    else
                    {
                        model = new PeriodEarningsModel()
                        {
                            PeriodId = periodId,
                            PeriodStartDate = pStartDate,
                            PeriodEndDate = pEndDate,
                            BonusPayouts = CommissionsService.GetDistributorBonusData(CurrentAccountId, periodId),
                            Earnings = ApplyTerms(CommissionsService.GetEarningReportData(CurrentAccountId, periodId), periodId),


                            EarningReportBasics = ablEarnings.GetEarningReportBasics(CurrentAccountId, periodId),
                            EarningsTotals = ablEarnings.GetEarningsTotals(CurrentAccountId, periodId),

                            AccountQuickFacts = new AccountQuickFacts().ForAccount(CurrentAccount, periodId),
                            AccountAddress = CurrentAccount.Addresses.FirstOrDefault(a => a.AddressTypeID == (int)Constants.AddressType.Main),
                            CompanyAddress = new Address()
                            {
                                FirstName = companyAddress.FirstName,
                                LastName = companyAddress.LastName,
                                Address1 = companyAddress.Address1,
                                Address2 = companyAddress.Address2,
                                City = companyAddress.City,
                                StateProvince = SmallCollectionCache.Instance.StateProvinces.First(sp => sp.StateProvinceID == companyAddress.StateProvinceID && sp.CountryID == (int)Constants.Country.Brazil),
                                PostalCode = companyAddress.PostalCode
                            }
                            //CompanyAddress = new Address()
                            //{
                            //    FirstName = "L'Bel",
                            //    LastName = "USA",
                            //    Address1 = "101 California St.",
                            //    Address2 = "Suite 800",
                            //    City = "San Francisco",
                            //    StateProvince = SmallCollectionCache.Instance.StateProvinces.First(sp => sp.StateAbbreviation == "AC" && sp.CountryID == (int)Constants.Country.Brazil),
                            //    //StateProvince = SmallCollectionCache.Instance.StateProvinces.First(sp => sp.StateAbbreviation == "CA" && sp.CountryID == (int)Constants.Country.UnitedStates),
                            //    PostalCode = "94111"
                            //}
                        };
                    }

                }
               // var country = SmallCollectionCache.Instance.Countries.GetById((int)Constants.Country.Brazil);
               // ViewData["language"] = country.CultureInfo;
                var country = CoreContext.CurrentCultureInfo.Name;
                ViewData["language"] = country;

                SetMasterPageViewData();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                model.ErrorMessage = exception.Message;
            }
            
            return View(model);
        }

        [OutputCache(CacheProfile = "DontCache")]
        public ActionResult GetBonusDetail(int periodId, string bonusCode)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<table class=\"tableModal\"><tr><th>ID</th><th>Name</th><th>" + Translation.GetTerm(5, "PQV", "Venda Pessoal") + "</th><th>" + 
                               Translation.GetTerm(5, "PCV", "Venda Bonificação") + "</th><th>" + Translation.GetTerm(5, "Commission/Bonus", "% Bonificação") + 
                               "</th><th>" + Translation.GetTerm(5, "AmountPaid", "Bônus") + "</th></tr>");

                var data = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, bonusCode);

                foreach (var item in data)
                {
                    builder.Append(string.Format("<tr><td># {0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>"
                        , item.DownlineID
                        , item.DownlineName
                        , string.Format("{0:0}", item.PQV)
                        , string.Format("{0:0.00}", item.PCV)
                        , PerformanceExtensions.GetPercentage(item.CB)
                        , item.AmountPaid.ToMoneyString()));
                }

                builder.Append("</table>");
                return Json(new { result = true, data = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region Download Report PDF
        public ActionResult Download(int periodId)
        {
            NetSteps.Data.Entities.Business.Earnings ablEarnings = new NetSteps.Data.Entities.Business.Earnings();
            IEnumerable<EarningReportBasic> EarningReportBasics;
            EarningReportBasics = ablEarnings.GetEarningReportBasics(CurrentAccountId, periodId);
            IEnumerable<EarningsTotal> EarningsTotals;
            EarningsTotals = ablEarnings.GetEarningsTotals(CurrentAccountId, periodId);

            Microsoft.Reporting.WebForms.LocalReport report = new Microsoft.Reporting.WebForms.LocalReport();
            List<IReportBonusDetail> bonusDetails = new List<IReportBonusDetail>();

            var earningReportData = ApplyTerms(CommissionsService.GetEarningReportData(CurrentAccountId, periodId), periodId);
            foreach (var earning in earningReportData)
            {
                var bonusdetailAdvancementBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.AdvancementBonusCode);
                foreach (ReportBonusDetail item in bonusdetailAdvancementBonus)
                {
                    item.BonusValue = earning.AdvancementBonusCB;
                }

                bonusDetails.AddRange(bonusdetailAdvancementBonus);

                var bonusdetailCoachingBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.CoachingBonusCode);
                foreach (ReportBonusDetail item in bonusdetailCoachingBonus)
                {
                    item.BonusValue = earning.CoachingBonusCB;
                }

                bonusDetails.AddRange(bonusdetailCoachingBonus);

                var bonusdetailConsistencyBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.ConsistencyBonusCode);
                foreach (ReportBonusDetail item in bonusdetailConsistencyBonus)
                {
                    item.BonusValue = earning.ConsistencyBonusCB;
                }

                //mescobar

                bonusDetails.AddRange(bonusdetailConsistencyBonus);

                var bonusdetailFastStartBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.FastStartBonusCode);
                foreach (ReportBonusDetail item in bonusdetailFastStartBonus)
                {
                    item.BonusValue = earning.FastStartBonusCB;
                }

                bonusDetails.AddRange(bonusdetailFastStartBonus);

                var bonusdetailGeneration1Title10 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Generation1Title10Code);
                foreach (ReportBonusDetail item in bonusdetailGeneration1Title10)
                {
                    item.BonusValue = earning.Generation1Title10CB;
                }

                bonusDetails.AddRange(bonusdetailGeneration1Title10);

                var bonusdetailGeneration1Title7 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Generation1Title7Code);
                foreach (ReportBonusDetail item in bonusdetailGeneration1Title7)
                {
                    item.BonusValue = earning.Generation1Title7CB;
                }

                bonusDetails.AddRange(bonusdetailGeneration1Title7);

                var bonusdetailGeneration2Title10 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Generation2Title10Code);
                foreach (ReportBonusDetail item in bonusdetailGeneration2Title10)
                {
                    item.BonusValue = earning.Generation2Title10CB;
                }

                bonusDetails.AddRange(bonusdetailGeneration2Title10);

                var bonusdetailGeneration2Title7 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Generation2Title7Code);
                foreach (ReportBonusDetail item in bonusdetailGeneration2Title7)
                {
                    item.BonusValue = earning.Generation2Title7CB;
                }

                bonusDetails.AddRange(bonusdetailGeneration2Title7);

                var bonusdetailGeneration3Title7 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Generation3Title7Code);
                foreach (ReportBonusDetail item in bonusdetailGeneration3Title7)
                {
                    item.BonusValue = earning.Generation3Title7CB;
                }

                bonusDetails.AddRange(bonusdetailGeneration3Title7);

                var bonusdetailGeneration4Title7 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Generation4Title7Code);
                foreach (ReportBonusDetail item in bonusdetailGeneration4Title7)
                {
                    item.BonusValue = earning.Generation4Title7CB;
                }

                bonusDetails.AddRange(bonusdetailGeneration4Title7);

                var bonusdetailGeneration5Title7 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Generation5Title7Code);
                foreach (ReportBonusDetail item in bonusdetailGeneration5Title7)
                {
                    item.BonusValue = earning.Generation5Title7CB;
                }

                bonusDetails.AddRange(bonusdetailGeneration5Title7);

                var bonusdetailLevel1 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Level1Code);
                foreach (ReportBonusDetail item in bonusdetailLevel1)
                {
                    item.BonusValue = earning.Level1CB;
                }

                bonusDetails.AddRange(bonusdetailLevel1);

                var bonusdetailLevel2 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Level2Code);
                foreach (ReportBonusDetail item in bonusdetailLevel2)
                {
                    item.BonusValue = earning.Level2CB;
                }

                bonusDetails.AddRange(bonusdetailLevel2);

                var bonusdetailLevel3 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Level3Code);
                foreach (ReportBonusDetail item in bonusdetailLevel3)
                {
                    item.BonusValue = earning.Level3CB;
                }

                bonusDetails.AddRange(bonusdetailLevel3);

                var bonusdetailLevel4 = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.Level4Code);
                foreach (ReportBonusDetail item in bonusdetailLevel4)
                {
                    item.BonusValue = earning.Level4CB;
                }

                bonusDetails.AddRange(bonusdetailLevel4);

                var bonusdetailMatchingAdvacementBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.MatchingAdvacementBonusCode);
                foreach (ReportBonusDetail item in bonusdetailMatchingAdvacementBonus)
                {
                    item.BonusValue = earning.MatchingAdvacementBonusCB;
                }

                bonusDetails.AddRange(bonusdetailMatchingAdvacementBonus);

                var bonusdetailRetailProfitBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.RetailProfitBonusCode);
                foreach (ReportBonusDetail item in bonusdetailRetailProfitBonus)
                {
                    item.BonusValue = earning.RetailProfitBonusCB;
                }

                bonusDetails.AddRange(bonusdetailRetailProfitBonus);

                var bonusdetailTeamBuildingBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.TeamBuildingBonusCode);
                foreach (ReportBonusDetail item in bonusdetailTeamBuildingBonus)
                {
                    item.BonusValue = earning.TeamBuildingBonusCB;
                }

                bonusDetails.AddRange(bonusdetailTeamBuildingBonus);

                var bonusdetailTurboInfinityBonus = CommissionsService.GetReportBonusDetail(periodId, this.CurrentAccountId, earning.TurboInfinityBonusCode);
                foreach (ReportBonusDetail item in bonusdetailTurboInfinityBonus)
                {
                    item.BonusValue = earning.TurboInfinityBonusCB;
                }

                bonusDetails.AddRange(bonusdetailTurboInfinityBonus);
            }

            decimal? totalPeriod = null;
            decimal? totalYear = null;

            CommissionsService.GetEarningsAmountOnly(periodId, CurrentAccountId, out totalPeriod, out totalYear);

            //string reportRepository = System.Configuration.ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + @"\Reports\RptConsultora.rdlc";
            string reportRepository = Server.MapPath("~/Reports/RptConsultora.rdlc");
            ///Portada
            CreatePdfArray<dynamic>(report, reportRepository, new string[] { "DstPortada" }, new List<dynamic>() { new { AccountName = CurrentAccount.FullName, Period = DateTime.Now, TotalPeriodEarnings = totalPeriod, TotalYearEarnings = totalYear } });
            //Page2
            CreatePdfArray<IEarningReport>(report, reportRepository, new string[] { "DstPage2" }, earningReportData);
            //Page 3
            CreatePdfArray<IReportBonusDetail>(report, reportRepository, new string[] { "DstPage3" }, bonusDetails);

            CreatePdfArray<EarningsTotal>(report, reportRepository, new string[] { "DtsConjuntoTotales" }, EarningsTotals);

            CreatePdfArray<EarningReportBasic>(report, reportRepository, new string[] { "DtsConjuntoTitulos" }, EarningReportBasics);

            string extension;
            string encoding;
            string mimeType;
            string[] streams;
            Microsoft.Reporting.WebForms.Warning[] warnings;

            Byte[] mybytes = report.Render("pdf", null,
            out extension, out encoding,
            out mimeType, out streams, out warnings); //for exporting to PDF

            byte[] Libro = ExtractPages(mybytes);

            return File(Libro, "application/pdf", string.Format("{0}-{1}.pdf", "Earnings", periodId));
        }

        public static byte[] ExtractPages(Byte[] sourcePdfPath)
        {
            iTextSharp.text.pdf.PdfReader reader = null;
            iTextSharp.text.Document sourceDocument = null;
            iTextSharp.text.pdf.PdfCopy pdfCopyProvider = null;
            iTextSharp.text.pdf.PdfImportedPage importedPage = null;
            System.IO.MemoryStream target = new System.IO.MemoryStream();
            reader = new iTextSharp.text.pdf.PdfReader(sourcePdfPath);
            int numberOfPages = reader.NumberOfPages;

            sourceDocument = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
            pdfCopyProvider = new iTextSharp.text.pdf.PdfCopy(sourceDocument, target);
            sourceDocument.Open();
            for (int i = 1; i <= numberOfPages; i++)
            {
                String pageText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i);

                if (pageText.Equals(""))
                    continue;

                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
            }

            sourceDocument.Close();
            reader.Close();

            return target.ToArray();
        }

        static void CreatePdfArray<T>(Microsoft.Reporting.WebForms.LocalReport report, string PathRdl, string[] nameDataset, params IEnumerable<T>[] Sources)
        {
            report.ReportPath = PathRdl;
            if (Sources.Length != nameDataset.Length)
                throw new Exception("No conciden la cantidad de Dataset y los nombre de los mismos");

            for (int index = 0; index < Sources.Length; index++)
            {
                Microsoft.Reporting.WebForms.ReportDataSource ReportDataSource = new Microsoft.Reporting.WebForms.ReportDataSource();
                ReportDataSource.Name = nameDataset[index];
                ReportDataSource.Value = Sources[index];
                report.DataSources.Add(ReportDataSource);
            }
        }

        #endregion

        protected virtual void SetMasterPageViewData()
        {
            ViewData["CurrentAccountReports"] = CurrentAccountReports.Where(r => r.AccountReportTypeID == (short)Constants.AccountReportType.DownlineReport).ToList();
            ViewData["CorporateAccountReports"] = CorporateAccountReports.Where(r => r.AccountReportTypeID == (short)Constants.AccountReportType.DownlineReport).ToList();
            ViewData["CurrentReportParameters"] = CurrentReportParameters;
        }

        public override NetSteps.Data.Entities.Generated.ConstantsGenerated.AccountReportType AccountReportType
        {
            get { throw new NotImplementedException(); }
        }

        private IEnumerable<IEarningReport> ApplyTerms(IEnumerable<IEarningReport> enumerable, int periodId)
        {
            var period = (periodId != 0) ? CommissionsService.GetPeriodsForAccount(CurrentAccountId).Where(p => p.PeriodId == periodId).FirstOrDefault() : CommissionsService.GetPeriodsForAccount(CurrentAccountId).OrderByDescending(p => p.PeriodId).FirstOrDefault();
            DateTime pStartDate = period.StartDateUTC.ToLocalTime(), pEndDate = period.EndDateUTC.ToLocalTime();

            foreach (NetSteps.Commissions.Service.Models.EarningReport item in enumerable)
            {
                item.CompanyTerm = string.Format("{0} {1} - {2}", "L'Bel USA Commissions Statement", pStartDate.ToString("MMM d yyyy"), pEndDate.ToString("MMM d yyyy"));
                item.Company = string.Format("{0} {1} {2}", "L'Bel", "USA", "Commissions Statemens");
                item.AccountNumberTerm = Translation.GetTerm("AccountNumber", "Asesora de Belleza ID");
                item.AccountNumber = CurrentAccount.AccountNumber;

                item.AccountNameTerm = Translation.GetTerm("Name", "Titulo de Carrera:");
                //item.AccountName = 
                item.AddressTerm = Translation.GetTerm("Address", "Address");

                item.CareerTitleTerm = Translation.GetTerm("CareerTitle", "Career Title");
                item.EnrollmentDateTerm = Translation.GetTerm("EnrollmentDate", "Enrollment Date");
                item.StateTerm = Translation.GetTerm("State", "State");
                item.PaidAsTitleTerm = Translation.GetTerm("PaidTitle", "Paid Title");

                item.Level1Term = Translation.GetTerm(item.Level1Code, "Level 1");
                item.Level2Term = Translation.GetTerm(item.Level2Code, "Level 2");
                item.Level3Term = Translation.GetTerm(item.Level3Code, "Level 3");
                item.Level4Term = Translation.GetTerm(item.Level4Code, "Level 4");
                item.Generation1Title7Term = Translation.GetTerm(item.Generation1Title7Code, "M3- Gen 1");
                item.Generation2Title7Term = Translation.GetTerm(item.Generation2Title7Code, "M3- Gen 2");
                item.Generation3Title7Term = Translation.GetTerm(item.Generation3Title7Code, "M3- Gen 3");
                item.Generation4Title7Term = Translation.GetTerm(item.Generation4Title7Code, "M3- Gen 4");
                item.Generation5Title7Term = Translation.GetTerm(item.Generation5Title7Code, "M3- Gen 5");
                item.Generation1Title10Term = Translation.GetTerm(item.Generation1Title10Code, "L3- Gen 1");
                item.Generation2Title10Term = Translation.GetTerm(item.Generation2Title10Code, "L3- Gen 2");
                item.TurboInfinityBonusTerm = Translation.GetTerm(item.TurboInfinityBonusCode, "Turbo Infinity Bonus");
                item.FastStartBonusTerm = Translation.GetTerm(item.FastStartBonusCode, "Fast Start Bonus");
                item.CoachingBonusTerm = Translation.GetTerm(item.CoachingBonusCode, "Coaching");
                item.TeamBuildingBonusTerm = Translation.GetTerm(item.TeamBuildingBonusCode, "Team Building Bonus");
                item.AdvancementBonusTerm = Translation.GetTerm(item.AdvancementBonusCode, "Advancement Bonus");
                item.MatchingAdvacementBonusTerm = Translation.GetTerm(item.MatchingAdvacementBonusCode, "Matching Advancement Bonus");
                item.ConsistencyBonusTerm = Translation.GetTerm(item.ConsistencyBonusCode, "Consistency Bonus");
                item.RetailProfitBonusTerm = Translation.GetTerm(item.RetailProfitBonusCode, "Advancement Bonus");
            }

            return enumerable;
        }
    }
}
