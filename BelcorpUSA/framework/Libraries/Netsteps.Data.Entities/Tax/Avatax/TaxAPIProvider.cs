
namespace NetSteps.Data.Entities.AvataxAPI
{
    /// <summary>
    /// To take care of initialization of various APIs / future integration of APIs
    /// through a single point 
    /// </summary>
    public static class TaxAPIProvider
    {
        public static AvataxAPI GetAvataxAPIInstance()
        {
            return new AvataxAPI();
        }

        //public static AvataxAPI GetAvataxAPIInstance(TaxSvc svc)
        //{
        //    return new AvataxAPI(svc);
        //}

        public static SampovaAPI GetSampovaAPIInstance()
        {
            return new SampovaAPI();
        }
    }

    /// <summary>
    /// Custom class to encapsulate all calls to the Sampova API
    /// </summary>
    public class SampovaAPI
    {
        //to be implemented
    }
}
