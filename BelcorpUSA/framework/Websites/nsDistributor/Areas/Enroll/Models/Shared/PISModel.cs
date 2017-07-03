using System.Collections.Generic;
using NetSteps.Data.Entities;
using nsDistributor.Models.Shared;



namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class PISModel : SubstringGroupModel
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
                                new SubstringConfig { Length = 9, InputRegex = @"\d", ValidRegex = @"^\d{9}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 2, InputRegex = @"\d", ValidRegex = @"^\d{2}$", IsRequired = true }                                
                            }
                        },
                        {
                            (int)Constants.Country.Brazil, new[]
                            {
                                new SubstringConfig { Length = 9, InputRegex = @"\d", ValidRegex = @"^\d{9}$", IsRequired = false, AfterHtml = "-" },
                                new SubstringConfig { Length = 2, InputRegex = @"\d", ValidRegex = @"^\d{2}$", IsRequired = false}
                                 
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

        public virtual PISModel LoadValues(
            string value,
            int countryID)
        {
            CountryID = countryID;

            LoadSubstringValues(value);

            return this;
        }

        public virtual PISModel LoadResources()
        {
            LoadSubstringResources();

            return this;
        }
    }
}
