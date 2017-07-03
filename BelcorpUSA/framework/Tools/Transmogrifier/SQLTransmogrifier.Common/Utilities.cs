namespace SqlTransmogrifier.Common
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	public static class Utilities
   {
      public static bool GetArgumentValueBoolean(string p, List<string> commandArguments)
      {
	      return commandArguments.Contains(p);
      }

      public static string GetArgumentValueString(string p, List<string> commandArguments)
      {
         Contract.Requires<ArgumentNullException>(p != null);
         Contract.Requires<ArgumentNullException>(commandArguments != null);

         var i = commandArguments.IndexOf(p);
         return (i >= 0) ? commandArguments[i + 1] : string.Empty;
      }

      public static string GetStringValueFromArguments(string[] args, string switchName)
      {
         var commandArguments = args.ToList();

         return GetArgumentValueString(switchName, commandArguments);
      }

      public static bool GetBooleanFromArguments(string[] args, string switchName)
      {
         List<string> commandArguments = args.ToList();

         bool hasBase = bool.TryParse(GetArgumentValueString(switchName, commandArguments), out hasBase);

         return hasBase;
      }

		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1303:ConstFieldNamesMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
		public static string GetNextCode(string targetDirectory)
      {
         string nextCode;

	      List<int> codes = new List<int>();

         string[] filePaths = Directory.GetFiles(targetDirectory, "*.sql").Where(x => x.ToLower().EndsWith(".sql")).ToArray();

         foreach (var filePath in filePaths)
         {
            string[] filePathArray = filePath.Split('\\');
            string fileName = string.Empty;

            if (filePathArray.Any())
            {
               fileName = filePathArray[filePathArray.Count() - 1];
            }

            const int Start = 0;
            int end = fileName.IndexOf('-', 0);

            int code = Convert.ToInt32(fileName.Substring(Start, end).Trim());

            codes.Add(code);
         }

         if (codes.Count > 0)
         {
	         int maxCode = codes.Max();
	         nextCode = string.Format("{0}0", Convert.ToInt32(maxCode.ToString().Substring(0, maxCode.ToString().Length - 1)) + 1);
         }
         else
         {
            nextCode = "10";
         }

         return nextCode.PadLeft(6, '0');
      }

      public static string GetTemplate(string assemblyNamespace, string folderNamespace, string name, bool isBase)
      {
	      string fileName = string.Format("{0}.{1}{2}.sql", folderNamespace, name, isBase ? "Base" : string.Empty);

         Assembly assembly = Assembly.Load(string.Format("{0}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", assemblyNamespace));

         var sr = new StreamReader(assembly.GetManifestResourceStream(fileName));
         string template = sr.ReadToEnd();

         sr.Close();

         return template;
      }
   }
}
