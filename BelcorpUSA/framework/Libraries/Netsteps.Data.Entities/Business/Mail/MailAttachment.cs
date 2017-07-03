using NetSteps.Common.Configuration;

namespace NetSteps.Data.Entities.Mail
{
	public partial class MailAttachment
	{
		#region Members
		#endregion

		#region Properties
		public string ActualFileName
		{
			get
			{
				return this.FileName.Substring(this.FileName.LastIndexOf("\\") + 1);
			}
		}

		public byte[] AttachmentData { get; set; }

		/// <summary>
		/// Adds temp to the upload folder path
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string UploadTempFolder
		{
			get
			{
				return ConfigurationManager.GetAbsoluteFolder("AttachmentsTemp");
			}
		}

		/// <summary>
		/// Adds temp to the upload folder path
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string UploadFinalFolder
		{
			get
			{
				return ConfigurationManager.GetAbsoluteFolder("Attachments");
			}
		}
		#endregion
	}
}
