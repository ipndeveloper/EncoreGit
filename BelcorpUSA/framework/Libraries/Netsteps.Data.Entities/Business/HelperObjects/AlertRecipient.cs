using System;
using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
    public class AlertRecipient
    {
        public EmailTemplate EmailTemplate { get; set; }
        public int Id { get; set; }
        public string ReceiverType { get; set; }
        public bool Active { get; set; }

        public AlertRecipient()
        {
            EmailTemplate = new EmailTemplate();
        }

        public static List<AlertRecipient> LoadCollection(int alertTriggerTypeId)
        {
            //return DataAdapterFactory.Current.AlertRecipientAdapter.LoadCollection(alertTriggerTypeId);
            throw new NotImplementedException();
        }

        public AlertRecipient Save(int alertTriggerTypeId)
        {
            //if (Id != 0)
            //{
            //    DataAdapterFactory.Current.AlertRecipientAdapter.Update(this);
            //}
            //else
            //{
            //    Id = DataAdapterFactory.Current.AlertRecipientAdapter.Insert(this, alertTriggerTypeId);
            //}


            //// return new alertRecipient
            //return new AlertRecipient { Id = Id }.Load();
            throw new NotImplementedException();
        }

        public AlertRecipient Load()
        {
            //return DataAdapterFactory.Current.AlertRecipientAdapter.Load(Id);
            throw new NotImplementedException();
        }
    }
}
