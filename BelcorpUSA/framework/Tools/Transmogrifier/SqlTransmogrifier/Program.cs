using System;

namespace SqlTransmogrifier
{
	using System.Linq;
    using SqlTransmogrifier.Common;

	class Program
	{
		static void Main(string[] args)
		{
			//throw new Exception(string.Format("{0}={1}", args[0], args[1]));

			bool result = false;

			string errorMessage = string.Empty;

			OutputHeader();

			SQLTranInfo info = BuildInfo(args);

			GenerateFile(ref result, ref errorMessage, info);

			OutputCompletedMessage(result, errorMessage);
		}

		private static SQLTranInfo BuildInfo(string[] args)
		{
			var info = new SQLTranInfo
				{
					TargetDirectory = Utilities.GetStringValueFromArguments(args, "-td"),
					BaseDirectory = Utilities.GetStringValueFromArguments(args, "-s"),
					TemplateName = Utilities.GetStringValueFromArguments(args, "-t"),
					ClientName = Utilities.GetStringValueFromArguments(args, "-c"),
					HasBase = Utilities.GetBooleanFromArguments(args, "-b"),
					CoreDBName = Utilities.GetStringValueFromArguments(args, "-coredb"),
					CommissionsDBName = Utilities.GetStringValueFromArguments(args, "-commdb"),
					MailDBName = Utilities.GetStringValueFromArguments(args, "-maildb")
				};

			if (string.IsNullOrWhiteSpace(info.ClientName))
			{
				info.ClientName = "FRAMEWORK";
			}

			if (string.IsNullOrWhiteSpace(info.TemplateName))
			{
				info.TemplateName = "Template";
			}

			if (string.IsNullOrWhiteSpace(info.CoreDBName))
			{
				info.CoreDBName = "EncoreCore";
			}

			if (string.IsNullOrWhiteSpace(info.CommissionsDBName))
			{
				info.CommissionsDBName = "EncoreCommissions";
			}

			if (string.IsNullOrWhiteSpace(info.MailDBName))
			{
				info.MailDBName = "EncoreMail";
			}

			return info;
		}

		private static void GenerateFile(ref bool result, ref string errorMessage, SQLTranInfo info)
		{
			try
			{
				result = Transmogrfier.CreateFile(ref errorMessage, info);
			}
			catch (Exception ex)
			{
				errorMessage = string.Format("Failed on create with: {0}", ex.Message);
			}
		}

		private static void OutputHeader()
		{
			Console.WriteLine("----------------------------------------------------------------");
			Console.WriteLine(" SQL Transmogrifier 2.0 - Writen By Spencer Killian 10-10-2011. ");
			Console.WriteLine("----------------------------------------------------------------");
		}

		/// <summary>
		/// The output completed message.
		/// </summary>
		/// <param name="result">
		/// The result.
		/// </param>
		/// <param name="errorMessage">
		/// The error message.
		/// </param>
		private static void OutputCompletedMessage(bool result, string errorMessage)
		{
			if (result)
			{
				Console.WriteLine("----------------------------------------------------------------");
				Console.WriteLine("Completed Successfuly.");
				Console.WriteLine("----------------------------------------------------------------");
			}
			else
			{
				Console.WriteLine("----------------------------------------------------------------");
				Console.WriteLine("Completed with Errors.");
				Console.WriteLine(errorMessage);
				Console.WriteLine("----------------------------------------------------------------");
			}
		}

	}
}