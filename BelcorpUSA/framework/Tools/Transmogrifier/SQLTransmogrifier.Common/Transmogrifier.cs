using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlTransmogrifier.Common
{
    public static class Transmogrfier
    {
        public static bool CreateNewScript(string tempalteName, string fileName, string targetDirectory, ref string errorMessage)
        {
            bool result = false;

            try
            {
                SaveNewScript(tempalteName, fileName, targetDirectory);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return result;
        }

        public static bool CreateFile(ref string errorMessage, SQLTranInfo info)
        {
            bool result;

            string client;
            string version;
            string databaseName;
            string newFileEnding;

            string[] filePaths;

            var newFileHeader = new StringBuilder();
            var transmorgrifiedFile = new StringBuilder();

            string currentBaseDirectory;
            try
            {
                currentBaseDirectory = GetCurrentBaseDirectory(info.BaseDirectory);
            }
            catch (Exception exception)
            {
                errorMessage = string.Format("Failed getting base directory with: {0}", exception.Message);
                return false;
            }

            string template;
            string baseTemplate;
            try
            {
                template = GetTemplate(info.TemplateName, false);
                baseTemplate = GetTemplate(info.TemplateName, true);
            }
            catch (Exception exception)
            {
                errorMessage = string.Format("Failed getting templates with: {0}", exception.Message);
                return false;
            }
            

            GetFileInputs(currentBaseDirectory, out filePaths, out client, out version, out databaseName, out newFileEnding);

            bool saveFile = BuildFile(template, transmorgrifiedFile, filePaths, newFileEnding, newFileHeader, info);

            if (saveFile)
            {
                SaveFile(transmorgrifiedFile, newFileEnding, newFileHeader, info.HasBase, baseTemplate, info);
                result = true;
            }
            else
            {
                result = false;
                errorMessage = "No files in directory to process.";
            }

            return result;
        }

        private static bool WrapBase(string baseTemplate, StringBuilder transmorgrifiedFile)
        {
            bool result = true;

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(baseTemplate.Replace("{TEMPLATEDATA}", transmorgrifiedFile.ToString()));

                transmorgrifiedFile = sb;
            }
            catch
            {
                result = false;
            }


            return result;

        }

        private static string GetCurrentBaseDirectory(string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                baseDirectory = Directory.GetCurrentDirectory();
            }

            return baseDirectory;
        }

        /// <summary>
        /// The get template.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="isBase">
        /// The is base.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetTemplate(string name, bool isBase)
        {
            string fileName = string.Format("SQLTransmogrifier.Common.Templates.{0}{1}.sql", name, isBase ? "Base" : string.Empty);

            Assembly assembly = Assembly.Load("SQLTransmogrifier.Common");
            if (assembly == null)
            {
                throw new Exception("Failed to load assembly (SQLTransmogrifier.Common).");
            }

            var stream = assembly.GetManifestResourceStream(fileName);
            if (stream == null)
            {
                throw new Exception(string.Format("Failed to get resource from assembly ({0}).", fileName));
            }

            var sr = new StreamReader(stream);

            string template = sr.ReadToEnd();

            sr.Close();

            return template;
        }

        private static void GetFileInputs(string currentBaseDirectory, out string[] filePaths, out string client, out string version, out string dbName, out string newFileEnding)
        {
            filePaths = Directory.GetFiles(currentBaseDirectory, "*.sql").Where(x => x.ToLower().EndsWith(".sql")).ToArray();

            client = "Client"; // GetClientName(currentBaseDirectory);
            version = "Version"; // GetVersion(currentBaseDirectory);
            dbName = "DBName"; // GetDBName(currentBaseDirectory);
            newFileEnding = "- SQL Deployment Script.sql";
        }

        private static bool BuildFile(string template, StringBuilder transmorgrifiedFile, string[] filePaths, string newFileEnding, StringBuilder newFileHeader, SQLTranInfo info)
        {
            StringBuilder newFileHeaderContents = new StringBuilder();

            List<Tuple<int, string>> sortedFilePaths = new List<Tuple<int, string>>();

            List<string> codeVersionDBName = new List<string>();

            foreach (var filePath in filePaths)
            {
                string[] filePathArray = filePath.Split('\\');
                string fileName = string.Empty;

                if (filePathArray.Count() > 0)
                {
                    fileName = filePathArray[filePathArray.Count() - 1];
                }

                int start = 0;
                int end = fileName.IndexOf('-', 0);

                int code = Convert.ToInt32(fileName.Substring(start, end).Trim());

                sortedFilePaths.Add(new Tuple<int, string>(code, filePath));
            }

            bool result = false;
            bool hasSqlReplaceLogic = false;

            string currentUsername = "NA";
            string commentLine = "--******************************************************";

            int sortedFileCount = 0;
            int count = 0;

            try { currentUsername = WindowsIdentity.GetCurrent().Name; }
            catch { }

            if (sortedFilePaths.Count() > 0)
            {
                sortedFileCount = sortedFilePaths.Count();

                foreach (var filePath in sortedFilePaths.OrderBy(x => x.Item1).Select(x => x.Item2))
                {
                    StreamReader currentFile = new StreamReader(filePath);

                    string sql = currentFile.ReadToEnd();
                    string description = "NA";
                    string filename = "NA";
                    string code = "NA";
                    string version = "NA";
                    string dbname = "NA";

                    string templateFile = string.Empty;

                    string[] filePathArray = filePath.Split('\\');

                    if (filePathArray.Count() > 0)
                    {
                        filename = filePathArray[filePathArray.Count() - 1].Replace("'", "''");
                        description = filename;
                    }

                    string[] fileNameArray = filename.Split('-');

                    if (fileNameArray.Count() > 0)
                    {
                        code = fileNameArray[0].Trim().PadLeft(6, '0');
                    }

                    if (fileNameArray.Count() > 1)
                    {
                        version = fileNameArray[1].Trim();
                    }

                    if (fileNameArray.Count() > 2)
                    {
                        dbname = fileNameArray[2].Trim();
                    }

                    templateFile = template.Clone().ToString();

                    templateFile = templateFile.Replace("{DBNAME}", info.GetDBName(dbname));
                    templateFile = templateFile.Replace("{CLIENT}", info.ClientName);
                    templateFile = templateFile.Replace("{VERSION}", version);
                    templateFile = templateFile.Replace("{CODE}", code);
                    templateFile = templateFile.Replace("{DESCRIPTION}", description);
                    templateFile = templateFile.Replace("{FILENAME}", filename);
                    templateFile = templateFile.Replace("{UNIQUEID}", "_" + count.ToString());

                    if (!hasSqlReplaceLogic)
                    {
                        hasSqlReplaceLogic = Regex.IsMatch(sql, @"<[^<>]*,[^<>]*,[^<>]*>");
                    }

                    templateFile = SplitOnGoLogic(templateFile, sql, count);

                    transmorgrifiedFile.AppendLine(commentLine);
                    transmorgrifiedFile.AppendLine(string.Format("-- {0}", filename));
                    transmorgrifiedFile.AppendLine(commentLine);

                    transmorgrifiedFile.AppendLine(templateFile);

                    codeVersionDBName.Add(string.Format("{0} - {1} - {2}", code.ToUpper().Trim(), version.ToUpper().Trim(), dbname.ToUpper().Trim()));

                    newFileHeaderContents.AppendLine(string.Format("-- {0}", filename));

                    count++;
                }

                if (codeVersionDBName.Count > codeVersionDBName.Distinct().Count())
                {
                    var something = (from x in codeVersionDBName
                                     group x by x into xg
                                     where xg.Count() > 1
                                     select xg.Key).ToList();

                    transmorgrifiedFile.Clear();
                    newFileHeader.Clear();
                    newFileHeader.AppendLine(string.Format(commentLine));
                    newFileHeader.AppendLine(string.Format("-- DUPLICATE ENTRIES FOUND! "));
                    newFileHeader.AppendLine(string.Format(commentLine));
                    something.ForEach(x => newFileHeader.AppendLine("--" + x));
                    newFileHeader.AppendLine(string.Format(commentLine));

                }
                else
                {
                    if (hasSqlReplaceLogic)
                    {
                        newFileHeader.AppendLine(string.Format(commentLine));
                        newFileHeader.AppendLine(string.Format("-- VALUE PLACEHOLDERS FOUND IN FILE! "));
                        newFileHeader.AppendLine(string.Format("-- Please use (Ctrl-Shift-M) to fill in the parameters."));
                    }

                    newFileHeader.AppendLine(string.Format(commentLine));
                    newFileHeader.AppendLine(string.Format("-- SELECT * FROM ver.SchemaMigrations ORDER BY Code, [Version]", info.ClientName));
                    newFileHeader.AppendLine(string.Format(commentLine));
                    newFileHeader.AppendLine(string.Format("-- {0}", info.ClientName));
                    newFileHeader.AppendLine(string.Format("-- File generated by user {0}", currentUsername));
                    newFileHeader.AppendLine(string.Format("-- {0}", DateTime.Now.ToString()));
                    newFileHeader.AppendLine(string.Format("-- {0} file{1} added", sortedFileCount, (sortedFileCount > 1) ? "s" : string.Empty));
                    newFileHeader.AppendLine(string.Format(commentLine));

                    newFileHeader.Append(newFileHeaderContents.ToString());

                    newFileHeader.AppendLine(commentLine);
                }

                result = true;
            }

            return result;
        }

        private static string SplitOnGoLogic(string templateFile, string sql, int count)
        {
            string result = string.Empty;
            string fixedUpSql = sql.Replace("'", "''");

            StringBuilder declareSql = new StringBuilder();
            StringBuilder executeSql = new StringBuilder();

            int i = 0;

            string[] splitByGo = Regex.Split(fixedUpSql, "GO--GO", RegexOptions.IgnoreCase | RegexOptions.Multiline).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (splitByGo.Count() > 0)
            {


                foreach (string item in splitByGo)
                {
                    declareSql.AppendLine(BuildDeclareRow(item, i, count));
                    executeSql.AppendLine(BuildExecuteRow(item, i, count));

                    i++;
                }
            }
            else
            {
                declareSql.AppendLine(BuildDeclareRow(fixedUpSql, i, count));
                executeSql.AppendLine(BuildExecuteRow(fixedUpSql, i, count));
            }

            templateFile = templateFile.Replace("{DECLARESQL}", declareSql.ToString());
            templateFile = templateFile.Replace("{EXECUTESQL}", executeSql.ToString());
            templateFile = templateFile.Replace("{TranCount}", count.ToString());

            result = templateFile.ToString();

            return result;
        }

        private static string BuildDeclareRow(string item, int i, int count)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(string.Format("DECLARE @SQL_{0}_{1} NVARCHAR(MAX) =", count, i));
            result.AppendLine(string.Format("'"));
            result.AppendLine(string.Format("{0}", item));
            result.AppendLine(string.Format("'"));

            return result.ToString();
        }

        private static string BuildExecuteRow(string item, int i, int count)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(string.Format("\tEXEC sys.sp_executesql @statement = @SQL_{0}_{1}", count, i));

            return result.ToString();
        }

        private static void SaveFile(StringBuilder transmorgrifiedFile, string newFileEnding, StringBuilder newFileHeader, bool hasBase, string baseTemplate, SQLTranInfo info)
        {
            string stringToWrite = transmorgrifiedFile.ToString();

            string fileName = string.Format("{0}{1}{2} {3}_{4}_{5} - {6} {7}"
                , DateTime.Now.Year
                , DateTime.Now.Month.ToString().PadLeft(2, '0')
                , DateTime.Now.Day.ToString().PadLeft(2, '0')
                , DateTime.Now.Hour.ToString().PadLeft(2, '0')
                , DateTime.Now.Minute.ToString().PadLeft(2, '0')
                , DateTime.Now.Second.ToString().PadLeft(2, '0')
                , info.ClientName
                , newFileEnding);

            string transmorgrifiedFilePath = Path.Combine(info.TargetDirectory, fileName);

            if (hasBase && !string.IsNullOrWhiteSpace(baseTemplate))
            {
                stringToWrite = baseTemplate
                    .Replace("{TEMPLATEDATA}", stringToWrite)
                    .Replace("{COMM_DBNAME}", info.GetDBName("COMM"))
                    .Replace("{MAIL_DBNAME}", info.GetDBName("MAIL"))
                    .Replace("{CORE_DBNAME}", info.GetDBName("CORE"));
            }

            var fs = File.Create(transmorgrifiedFilePath);

            var sw = new StreamWriter(fs, Encoding.UTF8);

            sw.Write(newFileHeader + stringToWrite);
            sw.Close();
        }

        private static void SaveNewScript(string tempalteName, string fileName, string targetDirectory)
        {
            string template = GetTemplate(tempalteName, false);

            string stringToWrite = template.ToString();

            var remove = (new List<char>() { '\\' }).ToArray();

            string transmorgrifiedFilePath =
                string.Format("{0}\\{1}"
                , targetDirectory.Trim(remove)
                , fileName);


            if (File.Exists(transmorgrifiedFilePath))
            {
                throw new Exception("File Already Exists!");
            }

            FileStream fs = File.Create(transmorgrifiedFilePath);

            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            sw.Write(stringToWrite);
            sw.Close();

            try
            {
                string exec = string.Format("add \"{0}\"", transmorgrifiedFilePath);
                System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\TF.exe", exec);
                System.Diagnostics.Process.Start("explorer.exe", "/select," + transmorgrifiedFilePath);
            }
            catch
            {

            }

        }

        private static string GetClientName(string baseDirectory)
        {
            string result = "NA";

            string[] array = baseDirectory.ToUpper().Replace("C:\\DEVELOPMENT\\", string.Empty).Split('\\');

            if (array.Count() > 0)
            {
                result = array[0];
            }

            return result;
        }

        private static string GetVersion(string baseDirectory)
        {
            string result = "NA";

            string[] array = baseDirectory.ToUpper().Split('\\');

            if (array.Count() > 0)
            {
                result = array[array.Count() - 2];
            }

            return result;
        }

        private static string GetDBName(string baseDirectory)
        {
            string result = "NA";

            string[] array = baseDirectory.ToUpper().Split('\\');

            if (array.Count() > 0)
            {
                result = array[array.Count() - 1];
            }

            return result;
        }

        public static string GenerateFilename(string scriptCode, string description, string databaseName, string branchCode)
        {
            string filename = string.Empty;

            string dbCode = GetDBCode(databaseName);
            string formattedScriptCode = scriptCode.ToUpper().Trim().PadLeft(6, '0');

            filename = string.Format("{0} - {1} - {2} - {3}.sql", formattedScriptCode, branchCode.Trim().ToUpper(), dbCode.ToUpper(), description.Trim());

            return filename;
        }

        private static string GetDBCode(string databaseName)
        {
            string code = string.Empty;

            if (databaseName.Length > 4)
            {
                code = databaseName.ToUpper().Substring(0, 4);
            }
            else
            {
                code = databaseName.ToUpper();
            }

            return code;
        }
    }
}
