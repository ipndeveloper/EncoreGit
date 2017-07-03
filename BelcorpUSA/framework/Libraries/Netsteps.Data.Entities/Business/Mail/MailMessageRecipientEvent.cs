using System;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Mail
{
    public partial class MailMessageRecipientEvent : EntityBusinessBase<MailMessageRecipientEvent, Int32, IMailMessageRecipientEventRepository, IMailMessageRecipientEventBusinessLogic>
    {
        protected TimeZone _currentTimeZone = TimeZone.CurrentTimeZone;
        public virtual TimeZone CurrentTimeZone
        {
            get { return _currentTimeZone; }
            set { _currentTimeZone = value; }
        }

        public System.DateTime DateCreated
        {
            get { return DateCreatedUTC.UTCToLocal(CurrentTimeZone); }
            set { DateCreatedUTC = value.LocalToUTC(CurrentTimeZone); }
        }
    }
}
