namespace SqlTransmogrifier.Common
{
    public class SQLTranInfo
    {
        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the core db name.
        /// </summary>
        public string CoreDBName { get; set; }

        /// <summary>
        /// Gets or sets the mail db name.
        /// </summary>
        public string MailDBName { get; set; }

        /// <summary>
        /// Gets or sets the commissions db name.
        /// </summary>
        public string CommissionsDBName { get; set; }

        /// <summary>
        /// Gets or sets the base directory.
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// Gets or sets the target directory.
        /// </summary>
        public string TargetDirectory { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether has base.
        /// </summary>
        public bool HasBase { get; set; }

        /// <summary>
        /// The get database name.
        /// </summary>
        /// <param name="dbname">
        /// The database name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetDBName(string dbname)
        {
            string name = string.Empty;

            switch (dbname.ToUpper().Trim())
            {
                case "COMM":
                    name = CommissionsDBName;
                    break;
                case "CORE":
                    name = CoreDBName;
                    break;
                case "MAIL":
                    name = MailDBName;
                    break;
                default:
                    break;
            }

            return name;
        }
    }
}
