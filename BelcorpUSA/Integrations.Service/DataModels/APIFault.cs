using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name="faultDetail")]
    public class APIFault
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public APIFault InnerFault { get; set; }

        [DataMember]
        public object OuterFault { get; set; }

        #region public FaultDetail()

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FaultDetail"/> class.
        /// </summary>
        public APIFault(string Message)
        {
            this.Message = Message;
        }
        public APIFault(string Message, string Type)
        {
            this.Message = Message;
            this.Type = Type;
        }

        public APIFault(Exception ex)
            : this(ex.Message, ex.GetType().FullName)
        {
            if (ex is FaultException)
            {
                System.Reflection.PropertyInfo pInfo = ex.GetType().GetProperty("Detail");
                if (pInfo != null && pInfo.CanRead && pInfo.GetIndexParameters().Length == 0)
                    OuterFault = pInfo.GetValue(ex, null);
            }
            if (ex.InnerException != null)
                InnerFault = new APIFault(ex.InnerException);
        }

        #endregion
        public override string ToString()
        {
            return string.Format("'{0}' ({1})", Message, Type);
        }
    }

    [Serializable]
    public class FaultDetailException : Exception
    {
        public APIFault Fault { get; set; }
        public FaultDetailException(APIFault Detail)
            : base(Detail.Message)
        {
            Fault = Detail;
        }

        protected FaultDetailException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
