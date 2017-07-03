
namespace NetSteps.WebService.Mobile.Models
{
	public class DeviceModel
	{
		public string deviceid;
		public DeviceType devicetype;
		public bool registered;
		public bool active;
	}

	public enum DeviceType
	{
		Android = 1,
		BlackBerry = 2,
		iOS,
		Symbian,
		WindowsPhone,
		webOS
	}
}