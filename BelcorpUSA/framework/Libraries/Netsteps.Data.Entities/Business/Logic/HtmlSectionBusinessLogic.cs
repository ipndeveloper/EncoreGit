using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class HtmlSectionBusinessLogic
    {
        public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IHtmlSectionRepository repository)
        {
            return new List<string>()
			{
				"HtmlElementID",
				"HtmlContentID",
				"HtmlSectionContentID",
				"HtmlSectionID"
			};
        }


        // Helper method to clone/create data for new objects(site, pages, etc..) from existing data - JHE
        public HtmlSection CloneHtmlSection(int htmlSectionIdToClone, int? siteID = null)
        {
            try
            {
                var htmlSectionToClone = HtmlSection.LoadFull(htmlSectionIdToClone);
                return CloneHtmlSection(htmlSectionIdToClone, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        // NOTE: The htmlSectionToClone variable passed in must be fully loaded. - JHE
        public HtmlSection CloneHtmlSection(HtmlSection htmlSectionToClone, int? existingSiteID = null, int? newSiteID = null)
        {
            try
            {
                HtmlSection newHtmlSection = AddHtmlSection(htmlSectionToClone);

                List<HtmlSectionContent> sectionContentsToClone = new List<HtmlSectionContent>();
                List<HtmlSectionChoice> sectionChoicesToClone = new List<HtmlSectionChoice>();

                GetListToClone(htmlSectionToClone, existingSiteID, ref sectionContentsToClone, ref sectionChoicesToClone);

                Dictionary<int, int> addedMatchingHtmlContents = new Dictionary<int, int>();

                AddHtmlSectionContents(newSiteID, newHtmlSection, sectionContentsToClone, sectionChoicesToClone, addedMatchingHtmlContents);
                
                AddHtmlSectionChoices(newSiteID, newHtmlSection, sectionChoicesToClone, addedMatchingHtmlContents);

                return newHtmlSection;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        private static void AddHtmlSectionChoices(int? newSiteID, HtmlSection newHtmlSection, List<HtmlSectionChoice> sectionChoicesToClone, Dictionary<int, int> addedMatchingHtmlContents)
        {
            foreach (var existingHtmlSectionChoice in sectionChoicesToClone)
            {
                KeyValuePair<int, int> key = new KeyValuePair<int, int>(existingHtmlSectionChoice.HtmlContentID, existingHtmlSectionChoice.HtmlSectionID);

                if (!addedMatchingHtmlContents.Contains(key))
                {
                    HtmlSectionChoice newHtmlSectionChoice = new HtmlSectionChoice()
                    {
                        SiteID = newSiteID.ToInt(),
                        HtmlSection = newHtmlSection,
                        HtmlContent = HtmlContent.CloneHtmlContent(existingHtmlSectionChoice.HtmlContent),
                        SortIndex = existingHtmlSectionChoice.SortIndex,

                    };

                    newHtmlSection.HtmlSectionChoices.Add(newHtmlSectionChoice);
                }                
            }
        }

        private static void AddHtmlSectionContents(int? newSiteID, HtmlSection newHtmlSection, List<HtmlSectionContent> sectionContentsToClone, List<HtmlSectionChoice> sectionChoicesToClone, Dictionary<int, int> addedMatchingHtmlContents)
        {
            foreach (var htmlSectionContent in sectionContentsToClone)
            {
                HtmlSectionContent newHtmlSectionContent = new HtmlSectionContent()
                {
                    SiteID = newSiteID
                };

                newHtmlSectionContent.HtmlContent = HtmlContent.CloneHtmlContent(htmlSectionContent.HtmlContent);

                var matchingSectionChoices = sectionChoicesToClone.Where(s => s.HtmlContentID == htmlSectionContent.HtmlContentID).ToList();

                if (matchingSectionChoices != null && matchingSectionChoices.Count > 0)
                {
                    foreach (var existingHtmlSectionChoice in matchingSectionChoices)
                    {
                        KeyValuePair<int, int> key = new KeyValuePair<int, int>(existingHtmlSectionChoice.HtmlContentID, existingHtmlSectionChoice.HtmlSectionID);

                        if (!addedMatchingHtmlContents.Contains(key))
                        {
                            AddHtmlSectionContent(newSiteID, newHtmlSection, newHtmlSectionContent, existingHtmlSectionChoice);

                            addedMatchingHtmlContents.Add(existingHtmlSectionChoice.HtmlContentID, existingHtmlSectionChoice.HtmlSectionID);
                        }
                    }
                }

                newHtmlSection.HtmlSectionContents.Add(newHtmlSectionContent);
            }
        }

        private static void GetListToClone(HtmlSection htmlSectionToClone, int? existingSiteID, ref List<HtmlSectionContent> sectionContentsToClone, ref List<HtmlSectionChoice> sectionChoicesToClone)
        {
            if (existingSiteID.HasValue && existingSiteID > 0)
            {
                sectionContentsToClone = htmlSectionToClone.HtmlSectionContents.Where(s => s.SiteID == existingSiteID).ToList();
                sectionChoicesToClone = htmlSectionToClone.HtmlSectionChoices.Where(s => s.SiteID == existingSiteID).ToList();
            }
            else
            {
                sectionContentsToClone = htmlSectionToClone.HtmlSectionContents.ToList();
                sectionChoicesToClone = htmlSectionToClone.HtmlSectionChoices.ToList();
            }
        }

        private static HtmlSection AddHtmlSection(HtmlSection htmlSectionToClone)
        {
            HtmlSection newHtmlSection = new HtmlSection();

            newHtmlSection.HtmlContentEditorTypeID = htmlSectionToClone.HtmlContentEditorTypeID;
            newHtmlSection.HtmlSectionEditTypeID = htmlSectionToClone.HtmlSectionEditTypeID;
            newHtmlSection.SectionName = htmlSectionToClone.SectionName;
            newHtmlSection.RequiresApproval = htmlSectionToClone.RequiresApproval;
            newHtmlSection.Width = htmlSectionToClone.Width;
            newHtmlSection.Height = htmlSectionToClone.Height;
            return newHtmlSection;
        }

        private static void AddHtmlSectionContent(int? newSiteID, HtmlSection newHtmlSection, HtmlSectionContent newHtmlSectionContent, HtmlSectionChoice existingHtmlSectionChoice)
        {
            HtmlSectionChoice newHtmlSectionChoice = new HtmlSectionChoice()
            {
                HtmlContent = newHtmlSectionContent.HtmlContent,
                SiteID = newSiteID.ToInt(),
                HtmlSection = newHtmlSection,
                SortIndex = existingHtmlSectionChoice.SortIndex
            };

            newHtmlSection.HtmlSectionChoices.Add(newHtmlSectionChoice);
        }

    }
}
