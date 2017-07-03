using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fasterflect;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Mvc.ActionResults
{
    public class ExcelResult<T> : ActionResult
    {
        private Dictionary<string, MemberGetter> _overridenValueGetters = null;
        private Dictionary<string, string> _columnHeaders = null;
        private List<string> _properties = new List<string>();
        private bool _useFields = false;
        private Regex _charactersToEscape = new Regex("[\",\n]", RegexOptions.Compiled);
        private Regex _pascalToSpaced = new Regex("([A-Z])", RegexOptions.Compiled);

        private Type _dataType = null;
        private Type DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        public string FileName
        {
            get;
            set;
        }

        public IEnumerable<T> Data
        {
            get;
            set;
        }

        public List<string> Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// columnNames is an optional parameter with the key being the the PropertyName and the value being the header text of the excel file - JHE
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="fields"></param>
        /// <param name="columnNames"></param>
        public ExcelResult(string fileName, IEnumerable<T> data, Dictionary<string, string> columnHeaders = null, Dictionary<string, MemberGetter> overridenValueGetters = null, params string[] fields)
        {
            FileName = fileName;
            Data = data;
            if (fields.Length > 0)
            {
                _useFields = true;
                _properties = fields.ToList();
            }
            _columnHeaders = columnHeaders;
            _overridenValueGetters = overridenValueGetters;
        }

        public ExcelResult(string fileName, IEnumerable<T> data)
        {
            FileName = fileName;
            Data = data;
            _useFields = true;

            if (data != null)
            {
                var first = data.FirstOrDefault();
                if (first != null)
                    DataType = first.GetType();
            }

            if (DataType == null)
                DataType = typeof(T);

            foreach (PropertyInfo property in DataType.GetPropertiesCached().Where(p => p.CanRead && p.GetGetMethod().IsPublic))
            {
                Properties.Add(property.Name);
            }
        }

        public ExcelResult(string fileName, IEnumerable<T> data, bool useFields)
        {
            FileName = fileName;
            Data = data;
            _useFields = useFields;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var columnHeaders = new List<string>();
            if (_columnHeaders != null)
            {
                foreach (var property in Properties)
                    columnHeaders.Add(_columnHeaders[property]);
            }
            else
            {
                columnHeaders = Properties.Select(p => _pascalToSpaced.Replace(p, " $1").Trim()).ToList();
            }

            var stream = Common.Utility.ExcelDocument.Create(null, Data, columnHeaders, Properties, _overridenValueGetters);

            context.HttpContext.Response.Clear();
            if (!string.IsNullOrEmpty(FileName))
            {
                var cd = new ContentDisposition
                {
                    FileName = FileName
                };
                context.HttpContext.Response.AddHeader("Content-Disposition", cd.ToString());
            }
            context.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.WriteTo(context.HttpContext.Response.OutputStream);
            stream.Close();
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }
    }
}