using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Belcorp.Policies.Core;
using Belcorp.Policies.Service.DTO;
using NetSteps.Encore.Core.IoC;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Belcorp.Policies.Service
{
    public class PoliciesService : Belcorp.Policies.Service.IPoliciesService
    {
        private readonly IWorkPolicy _workPolicy;

        public PoliciesService(IWorkPolicy workPolicy)
        {
            this._workPolicy = workPolicy;
        }

        public AccountPolicyDetailsDTO AccountPolicyDetail(int pAccountID, int pTypeBA, int pLanguageID) /*R2908 - HUNDRED(JAUF)*/
        {
           
            return new AccountPolicyDetailsDTO {
                AccountID = pAccountID,
                IsApplicableAccount = _workPolicy.IsApplicableAccount(pAccountID, pTypeBA),
                DateAccepted = _workPolicy.DateAcceptedPolicy(pAccountID),
                FilePath =_workPolicy.GetAllActivePoliciesByLanguage(pLanguageID).FirstOrDefault().FilePath,
                LanguageID = _workPolicy.GetAllActivePoliciesByLanguage(pLanguageID).FirstOrDefault().LanguageID
            };            
        }

        public void AddAccountPolicyDetail(AccountPolicyDetailsDTO pAccountPolicyDetailsDTO)
        {
            
            
            int PolicyID = _workPolicy.GetAllActivePoliciesByLanguage(pAccountPolicyDetailsDTO.LanguageID).FirstOrDefault().PolicyID;
            
            
            _workPolicy.AddAccountPolicyDetail(pAccountPolicyDetailsDTO.AccountID, 
                                                PolicyID, 
                                                pAccountPolicyDetailsDTO.IPAddress,
                                                pAccountPolicyDetailsDTO.DateAccepted);

        }

        public byte[] GeneratePDFBytes(string pSignature, string pDateofSign, string pFilePath)
        {
            PdfReader pdfReader = new PdfReader(pFilePath);
            MemoryStream ms = new MemoryStream();
            PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
            PdfContentByte pdfContentByte = pdfStamper.GetOverContent(pdfReader.NumberOfPages);
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            pdfContentByte.SetColorFill(BaseColor.BLACK);
            pdfContentByte.SetFontAndSize(baseFont, 8);
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, pSignature, 135, 85, 0);
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, pDateofSign, 335, 85, 0);
            pdfContentByte.EndText();
            pdfStamper.Close();
            ms.Flush();
            ms.Close();
            return ms.ToArray();
        } /*R2908 - HUNDRED(JAUF)*/


        public void Commit()
        {
            _workPolicy.Commit();
        }
    }
}
