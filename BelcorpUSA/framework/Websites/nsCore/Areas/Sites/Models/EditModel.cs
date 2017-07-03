using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Sites.Models
{
    public class EditModel
    {
        #region Constructors
        public EditModel()
        {
            this.SiteLanguages = new List<int>();
            this.SiteStatuses = new List<SelectListItem>();
            this.SiteUrls = new List<EditSiteUrlModel>();
        }

        public EditModel(Site site, Site baseSite) : this()
        {
            this.SiteID = site.SiteID;
            this.SiteName = site.Name;
            this.Description = site.Description;
            this.StatusId = site.SiteStatusID;
            this.MarketId = site.MarketID;
            this.DefaultLanguageId = site.DefaultLanguageID;
            this.SiteLanguages = site.Languages.Select(l => l.LanguageID).ToList();
            site.SiteUrls.Where(su => !su.Url.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase)).Each(su => this.SiteUrls.Add(new EditSiteUrlModel(su, baseSite)));
            this.ImportReadOnly(site, baseSite);
        }
        #endregion

        #region Properties
        public int SiteID { get; set; }

        [NSDisplayName("Site Name")]
        public string SiteName { get; set; }

        [NSDisplayName("Description")]
        public string Description { get; set; }

        [NSDisplayName("Status")]
        public short StatusId { get; set; }

        [NSDisplayName("Market")]
        public int MarketId { get; set; }

        [NSDisplayName("DefaultLanguage", "Default Language")]
        public int DefaultLanguageId { get; set; }

        [NSDisplayName("AvailableLanguages", "Available Languages")]
        public List<int> SiteLanguages { get; set; }

        [NSDisplayName("URLs")]
        public List<EditSiteUrlModel> SiteUrls { get; set; }

        public string SuccessMessage { get; set; }

        #region Read Only
        public string PageTitle { get; private set; }

        public List<SelectListItem> SiteStatuses { get; private set; }

        public bool AreURLsEditable { get; private set; }

        public Dictionary<int, string> TranslatedLanguages { get; private set; }
        #endregion
        #endregion

        #region Methods
        public void ImportReadOnly(Site site, Site baseSite = null)
        {
            this.PageTitle = site.SiteID == 0 ? Translation.GetTerm("NewSite", "New Site") : Translation.GetTerm("EditSite", "Edit Site");
            this.SiteStatuses = SmallCollectionCache.Instance.SiteStatuses.Select(ss => new SelectListItem()
            {
                Selected = ss.SiteStatusID == this.StatusId,
                Text = ss.GetTerm(),
                Value = ss.SiteStatusID.ToString()
            }).ToList();
            this.AreURLsEditable = site.SiteTypeID == Constants.SiteType.Replicated.ToInt() && !site.IsBase;
            this.TranslatedLanguages = TermTranslation.GetTranslatedLanguages();
            this.SiteUrls.Each(su => su.ImportReadOnly(baseSite));
        }

        public void Validate(ModelStateDictionary modelState)
        {
            if (this.SiteName.IsNullOrWhiteSpace())
            {
                modelState.AddModelError("SiteName", Translation.GetTerm("SiteNameRequired", "Site Name is required."));
            }

            if (this.SiteLanguages == null || !this.SiteLanguages.Any())
            {
                modelState.AddModelError("SiteLanguages", Translation.GetTerm("LanguageErrorDisplay", "A language must be selected from the checkbox."));
            }

            if (this.AreURLsEditable)
            {
                if (!this.SiteUrls.Any())
                {
                    modelState.AddModelError("SiteUrls", Translation.GetTerm("AtLeastOneURLRequired", "At least one URL is required"));
                }
                else if (!this.SiteUrls.Any(sm => sm.IsPrimaryUrl))
                {
                    modelState.AddModelError("SiteUrls", Translation.GetTerm("PrimaryURLRequired", "One of your URLs needs to be set as the primary URL"));
                }

                int iFirstPrimaryURL = this.SiteUrls.IndexOf(su => su.IsPrimaryUrl);
                foreach (EditSiteUrlModel urlModel in this.SiteUrls)
                {
                    int i = this.SiteUrls.IndexOf(urlModel);
                    string memberName = string.Format("SiteUrls[{0}].FullURL", i);
                    
                    if (urlModel.Subdomain.IsNullOrWhiteSpace())
                    {
                        modelState.AddModelError(memberName, Translation.GetTerm("URLRequired", "URL is required"));
                    }
                    else if (urlModel.Domain.IsNullOrWhiteSpace())
                    {
                        modelState.AddModelError(memberName, Translation.GetTerm("URLDomainRequired", "Please select a URL domain"));
                    }
                    else if (!SiteUrl.IsAvailable(this.SiteID, urlModel.FullURL))
                    {
                        modelState.AddModelError(memberName, Translation.GetTerm("URLNotAvailable", "That URL is not available, please try a different one"));
                    }
                    else if (urlModel.IsPrimaryUrl && i != iFirstPrimaryURL)
                    {
                        modelState.AddModelError(memberName, Translation.GetTerm("OnlyOnePrimaryURL", "Only one URL may be the primary"));
                    }
                }
            }
        }

        public void Export(Site site)
        {
            site.Name = this.SiteName;
            site.Description = this.Description;
            site.SiteStatusID = this.StatusId;
            site.MarketID = this.MarketId;
            site.DefaultLanguageID = this.DefaultLanguageId;
            this.SetSiteLanguages(site.Languages);

            // site urls
            if (site.SiteTypeID == Constants.SiteType.Replicated.ToInt() && !site.IsBase)
            {
                foreach (var suModel in this.SiteUrls)
	            {
                    SiteUrl siteUrl = site.SiteUrls.FirstOrDefault(su => su.Url.StartsWith(suModel.FullURL, StringComparison.OrdinalIgnoreCase));
                    if (siteUrl == null)
                    {
                        siteUrl = new SiteUrl();
                        siteUrl.SiteID = site.SiteID;
                        siteUrl.Url = suModel.FullURL;
                        site.SiteUrls.Add(siteUrl);
                    }
                    siteUrl.IsPrimaryUrl = suModel.IsPrimaryUrl;
	            }

                site.SiteUrls.RemoveAll(su => !this.SiteUrls.Any(suModel => su.Url.StartsWith(suModel.FullURL, StringComparison.OrdinalIgnoreCase)) &&
                    !su.Url.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Add or Remove languages based on checked values
        /// </summary>
        private void SetSiteLanguages(IList<Language> siteLanguagesCollection)
        {
            // If the language is not in site.Languages collection then Add
            foreach (var languageID in this.SiteLanguages)
            {
                if (!siteLanguagesCollection.Any(x => x.LanguageID == languageID))
                {
                    siteLanguagesCollection.Add(Language.Load(languageID));
                }
            }

            // If a previously checked element is unchecked then remove from collection
            for (int i = siteLanguagesCollection.Count - 1; i >= 0; i--)
            {
                if (!this.SiteLanguages.Any(x => x == siteLanguagesCollection[i].LanguageID))
                {
                    siteLanguagesCollection.Remove(siteLanguagesCollection[i]);
                }
            }
        }
        #endregion

        #region Sub Models
        public class EditSiteUrlModel
        {
            #region Constructors
            public EditSiteUrlModel()
            {
                this.Domains = new List<SelectListItem>();
            }

            public EditSiteUrlModel(SiteUrl siteUrl, Site baseSite)
            {
                this.Subdomain = siteUrl.Url.GetURLSubdomain();
                this.Domain = baseSite.GetDomains().FirstOrDefault(d => siteUrl.Url.Contains(d));
                this.IsPrimaryUrl = siteUrl.IsPrimaryUrl;
                this.ImportReadOnly(baseSite);
            }
            #endregion

            #region Properties
            public string Subdomain { get; set; }

            public string Domain { get; set; }

            public string FullURL { get { return "http://" + this.Subdomain + "." + this.Domain; } }

            public bool IsPrimaryUrl { get; set; }

            #region Read Only
            public List<SelectListItem> Domains { get; private set; }
            #endregion
            #endregion

            #region Methods
            public void ImportReadOnly(Site baseSite)
            {
                List<string> domains = baseSite.GetDomains();
                this.Domains = domains.Select(d => new SelectListItem()
                {
                    Selected = domains.Count() == 1 || d == this.Domain,
                    Text = d,
                    Value = d
                }).ToList();
            }
            #endregion
        } 
        #endregion
    }
}