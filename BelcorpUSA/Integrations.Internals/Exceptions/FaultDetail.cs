using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSteps.Exceptions
{
    [DataContract(Namespace = "urn:EncoreOrderFulfillmentService")]
    public class FaultDetail
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public FaultDetail InnerFault { get; set; }

        [DataMember]
        public object OuterFault { get; set; }

        #region public FaultDetail()

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FaultDetail"/> class.
        /// </summary>
        public FaultDetail(string Message)
        {
            this.Message = Message;
        }
        public FaultDetail(string Message, string Type)
        {
            this.Message = Message;
            this.Type = Type;
        }

        public FaultDetail(Exception ex)
            : this(ex.Message, ex.GetType().FullName)
        {
            if (ex is FaultException)
            {
                System.Reflection.PropertyInfo pInfo = ex.GetType().GetProperty("Detail");
                if (pInfo != null && pInfo.CanRead && pInfo.GetIndexParameters().Length == 0)
                    OuterFault = pInfo.GetValue(ex, null);
            }
            if (ex.InnerException != null)
                InnerFault = new FaultDetail(ex.InnerException);
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
        public FaultDetail Fault { get; set; }
        public FaultDetailException(FaultDetail Detail)
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
