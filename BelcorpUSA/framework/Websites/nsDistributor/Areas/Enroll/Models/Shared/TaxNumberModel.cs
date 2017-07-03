using System.Collections.Generic;
using NetSteps.Data.Entities;
using nsDistributor.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class TaxNumberModel : SubstringGroupModel
    {
        public virtual bool IsEntity { get; set; }
        public virtual int CountryID { get; set; }

        // TODO: Make these data-driven
        protected Dictionary<int, SubstringConfig[]> _entityConfigsField;
        protected virtual Dictionary<int, SubstringConfig[]> _entityConfigs
        {
            get
            {
                if (_entityConfigsField == null)
                {
                    _entityConfigsField = new Dictionary<int, SubstringConfig[]>
                    {
                        {
                            (int)Constants.Country.UnitedStates, new[]
                            {
                                new SubstringConfig { Length = 2, InputRegex = @"\d", ValidRegex = @"^\d{2}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 7, InputRegex = @"\d", ValidRegex = @"^\d{7}$", IsRequired = true }
                            }
                        }
                    };
                }
                return _entityConfigsField;
            }
        }
        protected Dictionary<int, SubstringConfig[]> _individualConfigsField;
        protected virtual Dictionary<int, SubstringConfig[]> _individualConfigs
        {
            get
            {
                if (_individualConfigsField == null)
                {
                    _individualConfigsField = new Dictionary<int, SubstringConfig[]>
                    {
                        {
                            (int)Constants.Country.UnitedStates, new[]
                            {
                                new SubstringConfig { Length = 3, InputRegex = @"\d", ValidRegex = @"^\d{3}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 2, InputRegex = @"\d", ValidRegex = @"^\d{2}$", IsRequired = true, AfterHtml = "-" },
                                new SubstringConfig { Length = 4, InputRegex = @"\d", ValidRegex = @"^\d{4}$", IsRequired = true }
                            }
                        }
                    };
                }
                return _individualConfigsField;
            }
        }

        protected override SubstringConfig[] _substringConfigs
        {
            get
            {
                if (IsEntity && _entityConfigs.ContainsKey(CountryID))
                {
                    return _entityConfigs[CountryID];
                }
                if (!IsEntity && _individualConfigs.ContainsKey(CountryID))
                {
                    return _individualConfigs[CountryID];
                }
                return new[] { new SubstringConfig { IsRequired = true } };
            }
        }

        public virtual TaxNumberModel LoadValues(
            string value,
            bool isEntity,
            int countryID)
        {
            IsEntity = isEntity;
            CountryID = countryID;

            LoadSubstringValues(value);

            return this;
        }

        public virtual TaxNumberModel LoadResources()
        {
            LoadSubstringResources();

            return this;
        }
    }
}