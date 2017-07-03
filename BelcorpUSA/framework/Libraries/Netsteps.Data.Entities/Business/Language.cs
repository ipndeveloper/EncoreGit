using System;
using System.Globalization;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities
{
    public partial class Language
    {
        #region Properties
        public static Language English
        {
            get
            {
                return SmallCollectionCache.Instance.Languages.First(l => l.LanguageCode == "EN");
            }
        }
        public static Language Spanish
        {
            get
            {
                return SmallCollectionCache.Instance.Languages.First(l => l.LanguageCode == "ES");
            }
        }

        private CultureInfo _culture;
        public CultureInfo Culture
        {
            get
            {
                if (_culture == null)
                {
                    try
                    {
                        if (!this.CultureInfo.IsNullOrEmpty())
                            _culture = new CultureInfo(this.CultureInfo);
                        else
                            _culture = CultureInfoCache.GetCultureInfo("en-US"); // Default to US if culture is not found - JHE
                    }
                    catch (Exception ex)
                    {
                        // Default to US if culture is not found - JHE
                        NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(ex, string.Format("CultureInfo ('{0}') not found.", this.CultureInfo.ToCleanString()), true);
                        _culture = CultureInfoCache.GetCultureInfo("en-US");
                    }
                }
                return _culture;
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}
