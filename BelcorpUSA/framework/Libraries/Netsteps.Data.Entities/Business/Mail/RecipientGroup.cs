using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NetSteps.Data.Entities.Mail
{
	[DataContract]
	[Serializable]
	public class RecipientGroup
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public IEnumerable<MailMessageRecipient> Recipients { get; set; }
	}
}
