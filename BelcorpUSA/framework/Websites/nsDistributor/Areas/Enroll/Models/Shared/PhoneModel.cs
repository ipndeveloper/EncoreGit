using System.Collections.Generic;
using NetSteps.Data.Entities;
using nsDistributor.Models.Shared;
using System.Linq;
using System.Text.RegularExpressions;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class PhoneModel : SubstringGroupModel
    {
        public virtual int CountryID { get; set; }

        // TODO: Make these data-driven
        protected Dictionary<int, SubstringConfig[]> _configsField;
        protected virtual Dictionary<int, SubstringConfig[]> _configs
        {
            get
            {
                if (_configsField == null)
                {
                    _configsField = new Dictionary<int, SubstringConfig[]>
                    {
                        {
                            (int)Constants.Country.UnitedStates, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^\d{5}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^[0-9]{3,4}$", IsRequired = true }
                            }
                        },
                        {
                            (int)Constants.Country.Canada, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^\d{5}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^[0-9]{3,4}$", IsRequired = true }
                            }
                        },
                        {
                            (int)Constants.Country.Brazil, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^[0-9]{4,5}$", IsRequired = true, AfterHtml = "-" },
                                
                                //new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^[0-9]{3,4}$", IsRequired = true }
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^\d{4}$", IsRequired = false},
                            }
                        }
                    };
                }
                return _configsField;
            }
        }

        protected override SubstringConfig[] _substringConfigs
        {
            get
            {
                return _configs.ContainsKey(CountryID)
                    ? _configs[CountryID]
                    : new[] { new SubstringConfig { IsRequired = true } };
            }
        }

        public virtual PhoneModel LoadValues(
            string value,
            int countryID)
        {
            CountryID = countryID;

            LoadSubstringValues(value);

            return this;
        }

        public virtual PhoneModel LoadResources()
        {
            LoadSubstringResources();

            return this;
        }
    }

    /// <summary>
    /// Phone which is not required
    /// </summary>
    public class AdditionalPhoneModel : PhoneModel
    {
        protected override Dictionary<int, SubstringConfig[]> _configs
        {
            get
            {
                if (_configsField == null)
                {
                    _configsField = new Dictionary<int, SubstringConfig[]>
                    {
                        {
                            (int)Constants.Country.UnitedStates, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = false, AfterHtml = "-" },
                                new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^\d{5}$", IsRequired = false, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^[0-9]{3,4}$", IsRequired = false }
                            }
                        },
                        {
                            (int)Constants.Country.Canada, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = false, AfterHtml = "-" },
                                new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^\d{5}$", IsRequired = false, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^[0-9]{3,4}$", IsRequired = false }
                            }
                        },
                        {
                            (int)Constants.Country.Brazil, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = false, AfterHtml = "-" },
                                
                                //new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^\d{5}$", IsRequired = false, AfterHtml = "-" },
                                new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^[0-9]{4,5}$", IsRequired = false, AfterHtml = "-" },
                                
                                //new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^[0-9]{3,4}$", IsRequired = false }
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^\d{4}$", IsRequired = false},
                            }
                        }
                    };
                }
                return _configsField;
            }
        }

        protected override SubstringConfig[] _substringConfigs
        {
            get
            {
                return _configs.ContainsKey(CountryID)
                    ? _configs[CountryID]
                    : new[] { new SubstringConfig { IsRequired = false } };
            }
        }

        protected override bool ValidateSubstrings(string[] substrings)
        {            
            //si alguno NO es nulo retornar expresion invalida,
            //validacion aplica todos o nada.
            var nullcount = substrings.Count(m => string.IsNullOrEmpty(m));
            if (nullcount > 0  && nullcount < substrings.Count())
            {
                return false;
            }

            return base.ValidateSubstrings(substrings);
        }
    }

    /// <summary>
    /// Phone which is not required
    /// </summary>
    public class MobilePhoneModel : PhoneModel
    {
        protected override Dictionary<int, SubstringConfig[]> _configs
        {
            get
            {
                if (_configsField == null)
                {
                    _configsField = new Dictionary<int, SubstringConfig[]>
                    {
                        {
                            (int)Constants.Country.UnitedStates, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^\d{4}$", IsRequired = true }
                            }
                        },
                        {
                            (int)Constants.Country.Canada, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^\d{4}$", IsRequired = true }
                            }
                        },
                        {
                            (int)Constants.Country.Brazil, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                //new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^\d{4}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 5, InputRegex = @"\d", ValidRegex = @"^[0-9]{4,5}$", IsRequired = false, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^\d{4}$", IsRequired = true }
                            }
                        }
                    };
                }
                return _configsField;
            }
        }

        protected override SubstringConfig[] _substringConfigs
        {
            get
            {
                return _configs.ContainsKey(CountryID)
                    ? _configs[CountryID]
                    : new[] { new SubstringConfig { IsRequired = false } };
            }
        }

        protected override bool ValidateSubstrings(string[] substrings)
        {
            //si alguno NO es nulo retornar expresion invalida,
            //validacion aplica todos o nada.
            var nullcount = substrings.Count(m => string.IsNullOrEmpty(m));
            if (nullcount > 0 && nullcount < substrings.Count())
            {
                return false;
            }

            return base.ValidateSubstrings(substrings);
        }
    }
}