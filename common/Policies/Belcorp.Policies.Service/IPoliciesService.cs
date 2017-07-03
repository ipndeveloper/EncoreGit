using System;
namespace Belcorp.Policies.Service
{
    public interface IPoliciesService
    {
        Belcorp.Policies.Service.DTO.AccountPolicyDetailsDTO AccountPolicyDetail(int pAccountID, int pTypeBA, int pLanguageID);/*R2908 - HUNDRED(JAUF) Agregar parametro pLanguageID*/
        void AddAccountPolicyDetail(Belcorp.Policies.Service.DTO.AccountPolicyDetailsDTO pAccountPolicyDetailsDTO);
        void Commit();
        byte[] GeneratePDFBytes(string pSignature, string pDateofSign, string pFilePath); /*R2908 - HUNDRED(JAUF)*/
    }
}
