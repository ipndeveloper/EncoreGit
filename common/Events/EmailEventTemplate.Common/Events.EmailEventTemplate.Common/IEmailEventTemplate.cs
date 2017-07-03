using NetSteps.Encore.Core.Dto;

namespace NetSteps.Events.EmailEventTemplate.Common
{
	[DTO]
	public interface IEmailEventTemplate
	{
		int EmailEventEmailTemplateID { get; set; }
		int EventTypeID { get; set; }
		int EmailTemplateID { get; set; }
	}
}
