using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Mvc.Controls.Configuration
{
    public class AddressConfiguration
    {
        //private static XElement isoConfiguration = null;
        private static ISOs _config;
        private static XmlSerializer serializer = new XmlSerializer(typeof(ISOs));

        //TODO: Change all of this to use the dynamic wrapper in NetSteps.Common.XML - DES
        static AddressConfiguration()
        {
            string filePath = HttpContext.Current.Request.PhysicalApplicationPath + "ISOConfiguration.xml";
            if (File.Exists(filePath))
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    //isoConfiguration = XElement.Parse(new StreamReader(stream).ReadToEnd());
                    _config = (ISOs)serializer.Deserialize(stream);
                }
                //stream.Close();

                FileSystemWatcher watcher = new FileSystemWatcher(HttpContext.Current.Request.PhysicalApplicationPath);
                watcher.Filter = "ISOConfiguration.xml";
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

                watcher.Changed += new FileSystemEventHandler((sender, e) =>
                {
                    try
                    {
                        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            _config = (ISOs)serializer.Deserialize(stream);
                        }
                    }
                    catch (IOException)
                    {
                        System.Threading.Thread.Sleep(500);
                        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            _config = (ISOs)serializer.Deserialize(stream);
                        }
                    }
                });

                watcher.EnableRaisingEvents = true;
            }
        }

        public static bool ConfigurationExists
        {
            //get { return isoConfiguration != null; }
            get { return _config != null; }
        }

        public static ISO GetISO(string id)
        {
            return _config.ISO.FirstOrDefault(i => i.Id == id);
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.netsteps.com/ISOConfiguration")]
    [XmlRoot(Namespace = "http://www.netsteps.com/ISOConfiguration", IsNullable = false)]
    public partial class ISOs
    {
        private ISO[] iSOField;

        [XmlElement("ISO")]
        public ISO[] ISO
        {
            get
            {
                return this.iSOField;
            }
            set
            {
                this.iSOField = value;
            }
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.netsteps.com/ISOConfiguration")]
    public partial class ISO
    {
        private PostalCodeLookup postalCodeLookupField;

        private ProvinceValueToUse _provinceValueToUse;

        private Tag[] tagsField;

        private string idField;

        public PostalCodeLookup PostalCodeLookup
        {
            get
            {
                return this.postalCodeLookupField;
            }
            set
            {
                this.postalCodeLookupField = value;
            }
        }

        public ProvinceValueToUse ProvinceValueToUse
        {
            get
            {
                return _provinceValueToUse;
            }
            set
            {
                _provinceValueToUse = value;
            }
        }

        [XmlArrayItem("Tag", IsNullable = false)]
        public Tag[] Tags
        {
            get
            {
                return this.tagsField;
            }
            set
            {
                this.tagsField = value;
            }
        }

        [XmlAttribute]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.netsteps.com/ISOConfiguration")]
    public partial class PostalCodeLookup
    {
        private bool enabledField;
        private string regexField;
        private string postalCodeFieldNameField;
        private string size;
        private string sizeSearch;
        private string lookupURLField;
        
        [XmlAttribute]
        public bool Enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }

        [XmlAttribute]
        public string Regex
        {
            get
            {
                return this.regexField;
            }
            set
            {
                this.regexField = value;
            }
        }

        [XmlAttribute]
        public string PostalCodeFieldName
        {
            get
            {
                return this.postalCodeFieldNameField;
            }
            set
            {
                this.postalCodeFieldNameField = value;
            }
        }

        [XmlAttribute]
        public string Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        [XmlAttribute]
        public string SizeSearch
        {
            get
            {
                return this.sizeSearch;
            }
            set
            {
                this.sizeSearch = value;
            }
        }


        [XmlAttribute]
        public string LookupURL
        {
            get
            {
                return this.lookupURLField;
            }
            set
            {
                this.lookupURLField = value;
            }
        }


    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.netsteps.com/ISOConfiguration")]
    public partial class ProvinceValueToUse
    {
        private bool _useProvinceName;

        [XmlAttribute]
        public bool UseProvinceName
        {
            get
            {
                return _useProvinceName;
            }
            set
            {
                _useProvinceName = value;
            }
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.netsteps.com/ISOConfiguration")]
    public partial class Tag
    {
        private string beforeTagField;
        private string inTagField;
        private string afterTagField;
        private string idField;
        private string tagNameField;
        private string defaultLabelField;
        private string labelTermNameField;
        private bool isRequiredField;
        private string defaultRequiredMessageField;
        private string requiredMessageTermNameField;
        private string fieldField;
        private int? maxLengthField;
        private string regexField;
        private bool liveRegexCheckField;
        private string defaultRegexFailMessageField;
        private string regexFailMessageTermNameField;
        private int? widthField;
        private string focusElementOnFilledField;


        public string BeforeTag
        {
            get
            {
                return this.beforeTagField;
            }
            set
            {
                this.beforeTagField = value;
            }
        }

        public string InTag
        {
            get
            {
                return this.inTagField;
            }
            set
            {
                this.inTagField = value;
            }
        }

        public string AfterTag
        {
            get
            {
                return this.afterTagField;
            }
            set
            {
                this.afterTagField = value;
            }
        }

        [XmlAttribute]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [XmlAttribute]
        public string TagName
        {
            get
            {
                return this.tagNameField;
            }
            set
            {
                this.tagNameField = value;
            }
        }

        [XmlAttribute]
        public string DefaultLabel
        {
            get
            {
                return this.defaultLabelField;
            }
            set
            {
                this.defaultLabelField = value;
            }
        }

        [XmlAttribute]
        public string LabelTermName
        {
            get
            {
                return this.labelTermNameField;
            }
            set
            {
                this.labelTermNameField = value;
            }
        }

        [XmlAttribute]
        public bool IsRequired
        {
            get
            {
                return this.isRequiredField;
            }
            set
            {
                this.isRequiredField = value;
            }
        }

        [XmlAttribute]
        public string DefaultRequiredMessage
        {
            get
            {
                return this.defaultRequiredMessageField;
            }
            set
            {
                this.defaultRequiredMessageField = value;
            }
        }

        [XmlAttribute]
        public string RequiredMessageTermName
        {
            get
            {
                return this.requiredMessageTermNameField;
            }
            set
            {
                this.requiredMessageTermNameField = value;
            }
        }

        [XmlAttribute]
        public string Field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
            }
        }

        [XmlAttribute(DataType = "integer")]
        public string MaxLength
        {
            get
            {
                return this.maxLengthField.HasValue ? this.maxLengthField.Value.ToString() : null;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value == "-1")
                    this.maxLengthField = null;
                else
                    this.maxLengthField = value.ToIntNullable();
            }
        }

        [XmlIgnore]
        public bool MaxLengthSpecified
        {
            get { return this.maxLengthField.HasValue; }
        }

        [XmlAttribute]
        public string Regex
        {
            get
            {
                return this.regexField;
            }
            set
            {
                this.regexField = value;
            }
        }

        [XmlAttribute]
        public bool LiveRegexCheck
        {
            get
            {
                return this.liveRegexCheckField;
            }
            set
            {
                this.liveRegexCheckField = value;
            }
        }

        [XmlAttribute]
        public string DefaultRegexFailMessage
        {
            get
            {
                return this.defaultRegexFailMessageField;
            }
            set
            {
                this.defaultRegexFailMessageField = value;
            }
        }

        [XmlAttribute]
        public string RegexFailMessageTermName
        {
            get
            {
                return this.regexFailMessageTermNameField;
            }
            set
            {
                this.regexFailMessageTermNameField = value;
            }
        }

        [XmlAttribute(DataType = "integer")]
        public string Width
        {
            get
            {
                return this.widthField.HasValue ? this.widthField.Value.ToString() : null;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value == "-1")
                    this.widthField = null;
                else
                    this.widthField = value.ToIntNullable();
            }
        }

        [XmlIgnore]
        public bool WidthSpecified
        {
            get { return this.widthField.HasValue; }
        }

        [XmlAttribute]
        public string FocusElementOnFilled
        {
            get
            {
                return this.focusElementOnFilledField;
            }
            set
            {
                this.focusElementOnFilledField = value;
            }
        }

    }
}
