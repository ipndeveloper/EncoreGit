namespace NetSteps.WebService.Mobile.Models
{
	public class LoginModel
	{
		public bool LoginSuccess { get; set; }
	    public string AccountNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string ProgramIdentifier { get; set; }
		public string Hash { get; set; }
	}
}