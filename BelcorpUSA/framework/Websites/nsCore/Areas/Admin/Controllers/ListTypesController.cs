using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Admin.Models.ListTypes;

namespace nsCore.Areas.Admin.Controllers
{
    public class ListTypesController : Controller
    {
        protected readonly object _lock = new object();

        [FunctionFilter("Admin", "~/Accounts")]
        public virtual ActionResult Index()
        {
            var editableListTypes = GetEditableListTypes();
            var viewModel = new ListTypesViewModel(editableListTypes);
            return View(viewModel);
        }

        [FunctionFilter("Admin-Create and Edit List Value", "~/Admin/ListTypes")]
        public virtual ActionResult Values(string id)
        {
            try
            {
                ViewData["ListType"] = id;
                IEnumerable<IListValue> model = null;
                var editableListType = (Constants.EditableListTypes)Enum.Parse(typeof (Constants.EditableListTypes), id);
                switch (editableListType)
                {
                    case Constants.EditableListTypes.AccountStatusChangeReason:
                        model = SmallCollectionCache.Instance.AccountStatusChangeReasons.ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ArchiveType:
                        model = SmallCollectionCache.Instance.ArchiveTypes.ToIListValueList();
                        break;
                    case Constants.EditableListTypes.CommunicationPreference:
                        model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CommunicationPreference.ToInt()).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ContactCategory:
                        model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.ContactCategory.ToInt()).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ContactMethod:
                        model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.ContactMethod.ToInt()).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ContactStatus:
                        model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.ContactStatus.ToInt()).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ContactType:
                        model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.ContactType.ToInt()).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.NewsType:
                        model = SmallCollectionCache.Instance.NewsTypes.OrderBy(nt => nt.GetSortIndexByLanguage(CoreContext.CurrentLanguageID, SmallCollectionCache.Instance.NewsTypes)).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ReturnReasons:
                        model = SmallCollectionCache.Instance.ReturnReasons.ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ReturnTypes:
                        model = SmallCollectionCache.Instance.ReturnTypes.ToIListValueList();
                        break;
                    case Constants.EditableListTypes.SiteStatusChangeReason:
                        model = SmallCollectionCache.Instance.SiteStatusChangeReasons.ToIListValueList();
                        break;
                    case Constants.EditableListTypes.SupportTicketCategory:
                        model = SmallCollectionCache.Instance.SupportTicketCategories.OrderBy(p => p.SortIndex).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.SupportTicketPriority:
                        model = SmallCollectionCache.Instance.SupportTicketPriorities.OrderBy(p => p.SortIndex).ToIListValueList();
                        break;
                    case Constants.EditableListTypes.ReplacementReason:
                        model = SmallCollectionCache.Instance.ReplacementReasons.ToIListValueList();
                        break;
                    //case Constants.EditableListTypes.CalendarEventType:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarEventType.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.CalendarCategory:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarCategory.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.CalendarPriority:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarPriority.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.CalendarStatus:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarStatus.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.CalendarColorCoding:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarColorCoding.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.TaskStatus:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskStatus.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.TaskType:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskType.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.TaskPriority:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskPriority.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.TaskCategory:
                    //    model = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskCategory.ToInt()).ToIListValueList();
                    //    break;
                    //case Constants.EditableListTypes.SupportTicketStatus:
                    //    model = SmallCollectionCache.Instance.SupportTicketStatuses.OrderBy(p => p.SortIndex).ToIListValueList();
                    //    break;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [FunctionFilter("Admin-Create and Edit List Value", "~/Admin/ListTypes")]
        public virtual ActionResult DeleteValue(Constants.EditableListTypes type, int listValueId)
        {
            try
            {
                switch (type)
                {
                    case Constants.EditableListTypes.AccountStatusChangeReason:
                        AccountStatusChangeReason.Delete(listValueId.ToShort());
                        break;
                    case Constants.EditableListTypes.ArchiveType:
                        ArchiveType.Delete(listValueId.ToShort());
                        break;
                    case Constants.EditableListTypes.NewsType:
                        NewsType.Delete(listValueId.ToShort());
                        SmallCollectionCache.Instance.NewsTypes.ExpireCache();
                        var languageSorts = SmallCollectionCache.Instance.NewsTypes.SelectMany(nt => nt.NewsTypeLanguageSorts).Where(ls => ls.LanguageID == 1 /*todo: set language*/).ToList();
                        languageSorts.Each(ls => ls.StartEntityTracking());
                        languageSorts.ReIndex();
                        languageSorts.Each(ls => ls.Save());
                        break;
                    case Constants.EditableListTypes.ReturnReasons:
                        ReturnReason.Delete(listValueId.ToShort());
                        break;
                    case Constants.EditableListTypes.ReturnTypes:
                        ReturnType.Delete(listValueId.ToShort());
                        break;
                    case Constants.EditableListTypes.SiteStatusChangeReason:
                        SiteStatusChangeReason.Delete(listValueId.ToShort());
                        break;
                    case Constants.EditableListTypes.SupportTicketCategory:
                        SupportTicketCategory.Delete(listValueId.ToShort());

                        SmallCollectionCache.Instance.SupportTicketCategories.ReIndex();

                        // Remove the item from the cache
                        var category = SmallCollectionCache.Instance.SupportTicketCategories.FirstOrDefault(x => x.SupportTicketCategoryID ==
                                                                                                    listValueId.ToShort());
                        if (category != null)
                            category.Delete();

                        foreach (var item in SmallCollectionCache.Instance.SupportTicketCategories)
                        {
                            item.Save();
                        }

                        break;
                    //case Constants.EditableListTypes.SupportTicketStatus:
                    //    SupportTicketPriority.Delete(listValueId.ToShort());

                    //    SmallCollectionCache.Instance.SupportTicketStatuses.ReIndex();

                    //    foreach (var item in SmallCollectionCache.Instance.SupportTicketStatuses)
                    //    {
                    //        item.Save();
                    //    }
                    //    break;
                    case Constants.EditableListTypes.SupportTicketPriority:

                        SupportTicketPriority.Delete(listValueId.ToShort());

                        SmallCollectionCache.Instance.SupportTicketPriorities.ReIndex();

                        var priority = SmallCollectionCache.Instance.SupportTicketPriorities.FirstOrDefault(x => x.SupportTicketPriorityID ==
                                                                                                    listValueId.ToShort());
                        if (priority != null)
                            priority.Delete();


                        foreach (var item in SmallCollectionCache.Instance.SupportTicketPriorities)
                        {
                            item.Save();
                        }
                        break;
                    case Constants.EditableListTypes.ReplacementReason:
                        ReplacementReason.Delete(listValueId.ToShort());
                        break;
                    default:
                        AccountListValue.Delete(listValueId);
                        break;
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Admin-Create and Edit List Value", "~/Admin/ListTypes")]
        public virtual ActionResult SaveValues(Constants.EditableListTypes type, Dictionary<int, string> listValues, Dictionary<int, int> sortIndexValues)
        {
            try
            {
                lock (_lock)
                {
                    int maxSortIndex = 0;

                    if (sortIndexValues != null && sortIndexValues.Count > 0)
                    {
                        maxSortIndex = sortIndexValues.ToList().Max(p => p.Value);
                    }

                    foreach (var listValue in listValues)
                    {
                        IListValue lv = null;

                        switch (type)
                        {
                            case Constants.EditableListTypes.AccountStatusChangeReason:
                                lv = SmallCollectionCache.Instance.AccountStatusChangeReasons.GetByIdOrNew(listValue.Key.ToShort());
								if (lv.ID == 0)
								{
									(lv as AccountStatusChangeReason).Editable = true;
									(lv as AccountStatusChangeReason).Active = true;
								}
                        		break;
                            case Constants.EditableListTypes.ArchiveType:
                                lv = SmallCollectionCache.Instance.ArchiveTypes.GetByIdOrNew(listValue.Key.ToShort());
                                if (lv.ID == 0)
                                {
                                    (lv as ArchiveType).Editable = true;
                                }
                                break;
                            case Constants.EditableListTypes.NewsType:
                                lv = SmallCollectionCache.Instance.NewsTypes.GetByIdOrNew(listValue.Key.ToShort());

                                if (lv.ID == 0)
                                {
                                    (lv as NewsType).Editable = true;
                                }

                                break;

                            case Constants.EditableListTypes.ReturnReasons:
                                lv = SmallCollectionCache.Instance.ReturnReasons.GetByIdOrNew(listValue.Key.ToShort());
                                break;
                            //case Constants.EditableListTypes.SupportTicketStatus:
                            //    lv = SmallCollectionCache.Instance.SupportTicketStatuses.GetByIdOrNew(listValue.Key.ToShort());

                            //    if (lv.ID == 0)
                            //    {
                            //        (lv as SupportTicketPriority).Active = true;
                            //    }
                            //    break;
                            case Constants.EditableListTypes.SupportTicketCategory:
                                lv = SmallCollectionCache.Instance.SupportTicketCategories.GetByIdOrNew(listValue.Key.ToShort());

                                if (lv.ID == 0)
                                {
                                    (lv as SupportTicketCategory).Active = true;
                                    (lv as SupportTicketCategory).TermName = listValue.Value.Replace(" ", "");
                                }
                                break;
                            case Constants.EditableListTypes.SupportTicketPriority:
                                lv = SmallCollectionCache.Instance.SupportTicketPriorities.GetByIdOrNew(listValue.Key.ToShort());

                                if (lv.ID == 0)
                                {
                                    (lv as SupportTicketPriority).Active = true;
                                }
                                break;
                            case Constants.EditableListTypes.ReturnTypes:
                                lv = SmallCollectionCache.Instance.ReturnTypes.GetByIdOrNew(listValue.Key.ToShort());
                                if (lv.ID == 0)
                                {
                                    (lv as ReturnType).Editable = true;
                                }

                                break;
                            case Constants.EditableListTypes.SiteStatusChangeReason:
                                lv = SmallCollectionCache.Instance.SiteStatusChangeReasons.GetByIdOrNew(listValue.Key.ToShort());

                                if (lv.ID == 0)
                                {
                                    (lv as SiteStatusChangeReason).Editable = true;
                                }
                                break;
                            case Constants.EditableListTypes.ReplacementReason:
                                lv = SmallCollectionCache.Instance.ReplacementReasons.GetByIdOrNew(listValue.Key.ToShort());
                                if (lv.ID == 0)
                                    (lv as ReplacementReason).Active = true;
                                break;
                            default:
                                if (listValue.Key > 0)
                                    lv = AccountListValue.Load(listValue.Key);
                                else
                                {
                                    Constants.ListValueType listType = type.ToString().ToEnum<Constants.ListValueType>(Constants.ListValueType.NotSet);
                                    lv = new AccountListValue() { ListValueTypeID = listType.ToShort() };
                                }
                                (lv as AccountListValue).IsCorporate = true;
                                (lv as AccountListValue).Active = true;
                                (lv as AccountListValue).Editable = true;
                                break;
                        }
                        if (lv != null)
                        {
                            if (lv is NetSteps.Common.Interfaces.ISortIndex)
                            {
                                ISortIndex sortLV = (ISortIndex)lv;

                                if (sortLV.SortIndex == 0)
                                {
                                    maxSortIndex = maxSortIndex + 1;
                                    sortLV.SortIndex = maxSortIndex;
                                }
                            }

                            if (lv is ITermName)
                            {
                                if (lv.Title.IsNullOrEmpty())
                                    lv.Title = listValue.Value.ToCleanString();

                                var termNameObj = lv as ITermName;
                                if (termNameObj != null)
                                {
                                    string typeName = type.ToString();
                                    if (termNameObj.TermName.IsNullOrEmpty() || !termNameObj.TermName.ToCleanString().StartsWith(typeName))
                                        termNameObj.TermName = string.Format("{0}_{1}", typeName, lv.Title);

                                    TermTranslation term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(termNameObj.TermName, CoreContext.CurrentLanguageID);
                                    if (term != null)
                                    {
                                        term.Term = listValue.Value.ToCleanString();
                                        term.Save();
                                    }
                                    else
                                    {
                                        // make sure we have an "English" translation before saving for the given language
                                        if (CoreContext.CurrentLanguageID != Constants.Language.English.ToInt())
                                        {
                                            var engTerm = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(termNameObj.TermName, Constants.Language.English.ToInt());
                                            if (engTerm == null)
                                            {
                                                engTerm = new TermTranslation();
                                                engTerm.LanguageID = Constants.Language.English.ToInt();
                                                engTerm.TermName = termNameObj.TermName;
                                                engTerm.Term = listValue.Value.ToCleanString();
                                                engTerm.Active = true;
                                                engTerm.Save();
                                            }
                                        }

                                        term = new TermTranslation();
                                        term.LanguageID = CoreContext.CurrentLanguageID;
                                        term.TermName = termNameObj.TermName;
                                        term.Term = listValue.Value.ToCleanString();
                                        term.Active = true;
                                        term.Save();
                                    }
                                }
                            }
                            else
                                lv.Title = listValue.Value.ToCleanString();

                            lv.Save();
                        }
                    }

                    switch (type)
                    {
						case Constants.EditableListTypes.AccountStatusChangeReason:
							SmallCollectionCache.Instance.AccountStatusChangeReasons.ExpireCache();
                    		break;
						case Constants.EditableListTypes.ArchiveType:
							SmallCollectionCache.Instance.ArchiveTypes.ExpireCache();
                    		break;
						case Constants.EditableListTypes.CommunicationPreference:
                    		break;
						case Constants.EditableListTypes.ContactCategory:
                    		break;
						case Constants.EditableListTypes.ContactMethod:
                    		break;
						case Constants.EditableListTypes.ContactStatus:
                    		break;
						case Constants.EditableListTypes.ContactType:
                    		break;
						case Constants.EditableListTypes.NewsType:
							SmallCollectionCache.Instance.NewsTypes.ExpireCache();
                    		break;
						case Constants.EditableListTypes.ReplacementReason:
                            SmallCollectionCache.Instance.ReplacementReasons.ExpireCache();
                    		break;
						case Constants.EditableListTypes.ReturnReasons:
							SmallCollectionCache.Instance.ReturnReasons.ExpireCache();
                    		break;
						case Constants.EditableListTypes.ReturnTypes:
							SmallCollectionCache.Instance.ReturnTypes.ExpireCache();
                    		break;
						case Constants.EditableListTypes.SiteStatusChangeReason:
							SmallCollectionCache.Instance.SiteStatusChangeReasons.ExpireCache();
                    		break;
						case Constants.EditableListTypes.SupportTicketCategory:
							SmallCollectionCache.Instance.SupportTicketCategories.ExpireCache();
                    		break;
						case Constants.EditableListTypes.SupportTicketPriority:
							SmallCollectionCache.Instance.SupportTicketPriorities.ExpireCache();
                    		break;
                    }

					return Json(new { result = true, message = Translation.GetTerm("SavedSuccessfully", "Saved successfully!") });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Admin-Create and Edit List Value", "~/Admin/ListTypes")]
        public virtual ActionResult Move(Constants.EditableListTypes type, int sortIndex, Constants.SortDirection direction)
        {
            try
            {
                lock (_lock)
                {
                    switch (type)
                    {
                        //case Constants.EditableListTypes.SupportTicketStatus:

                        //    SmallCollectionCache.Instance.SupportTicketStatuses.Move(sortIndex, direction);

                        //    foreach (var item in SmallCollectionCache.Instance.SupportTicketStatuses)
                        //    {
                        //        item.Save();
                        //    }

                        //    break;
                        case Constants.EditableListTypes.SupportTicketPriority:
                            SmallCollectionCache.Instance.SupportTicketPriorities.Move(sortIndex, direction);

                            foreach (var item in SmallCollectionCache.Instance.SupportTicketPriorities)
                            {
                                item.Save();
                            }

                            break;
                        case Constants.EditableListTypes.SupportTicketCategory:
                            SmallCollectionCache.Instance.SupportTicketCategories.Move(sortIndex, direction);

                            foreach (var item in SmallCollectionCache.Instance.SupportTicketCategories)
                            {
                                item.Save();
                            }

                            break;
                        case Constants.EditableListTypes.NewsType:
                            var languageSorts = SmallCollectionCache.Instance.NewsTypes.SelectMany(nt => nt.NewsTypeLanguageSorts).Where(ls => ls.LanguageID == CoreContext.CurrentLanguageID);
                            languageSorts.Each(ls => ls.StartEntityTracking());
                            languageSorts.Move(sortIndex, direction);
                            languageSorts.Each(ls => ls.Save());
                            break;
                    }

                    return Json(new { result = true });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private IEnumerable<string> GetEditableListTypes()
        {
            return typeof(Constants.EditableListTypes).GetValues<Constants.EditableListTypes>().OrderBy(lt => lt.PascalToSpaced()).Select(type => Enum.GetName(typeof(Constants.EditableListTypes), type));
        } 
    }
}