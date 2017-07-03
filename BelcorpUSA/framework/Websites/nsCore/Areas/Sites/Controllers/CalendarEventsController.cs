using System;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;

namespace nsCore.Areas.Sites.Controllers
{
    public class CalendarEventsController : BaseSitesController
    {
        /// <summary>
        /// Show the events for the current site
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [FunctionFilter("Sites-Calendar Events", "~/Sites/Overview")]
        public virtual ActionResult Index(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.LoadFull(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index", "Landing");
            return View();
        }

        /// <summary>
        /// Get events between 2 dates
        /// </summary>
        /// <param name="start">The unix timestamp to start from</param>
        /// <param name="end">The unix timestamp to end at</param>
        /// <returns></returns>
        public virtual ActionResult Get(ulong start, ulong end)
        {
            try
            {
                // Convert the timestamps to datetimes
                DateTime startDate = new DateTime(1970, 1, 1).AddSeconds(start);
                DateTime endDate = new DateTime(1970, 1, 1).AddSeconds(end);
                var events = CalendarEvent.LoadByDateRange(startDate, endDate, CurrentSite.SiteID);
                return Json(events.Select(e => new
                {
                    id = e.CalendarEventID,
                    title = e.HtmlSection == null || e.HtmlSection.ProductionContent(CurrentSite) == null ? "" : e.HtmlSection.ProductionContent(CurrentSite).FirstOrEmptyElement(Constants.HtmlElementType.Title).Contents,
                    allDayEvent = e.IsAllDayEvent,
                    start = (e.StartDate.LocalToUTC() - new DateTime(1970, 1, 1)).TotalSeconds,
                    end = (e.EndDate.LocalToUTC().ToDateTime() - new DateTime(1970, 1, 1)).TotalSeconds,
                    url = ("~/Sites/CalendarEvents/Edit/" + e.CalendarEventID).ResolveUrl()
                }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Show an event to edit
        /// </summary>
        /// <param name="id">The id of the event to edit</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Calendar Events", "~/Sites/Overview")]
        public virtual ActionResult Edit(int? id, DateTime? startDate)
        {
            try
            {
                if (CurrentSite == null)
                    return RedirectToAction("Index", "Landing");

                //ViewData["CalendarEventTypes"] = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarEventType.ToInt());
                //ViewData["CalendarPriority"] = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarPriority.ToInt());

                ViewData["CurrentSite"] = CurrentSite;

                if (id.HasValue && id.Value > 0)
                    return View(CalendarEvent.LoadFull(id.Value));

                return View(new CalendarEvent() { StartDate = startDate.HasValue ? startDate.Value : DateTime.Now, EndDate = startDate.HasValue ? startDate : null, Address = new Address() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult GetTranslation(int? eventId, int languageId)
        {
            try
            {
                if (!eventId.HasValue || eventId.Value == 0)
                    return Json(new { result = true, subject = "", caption = "", body = "" });

                CalendarEvent calEvent = CalendarEvent.LoadFull(eventId.Value);
                HtmlContent content = calEvent.HtmlSection.ProductionContent(CurrentSite, languageId);
                if (content == null)
                    return Json(new { result = true, subject = "", caption = "", body = "" });
                return Json(new
                {
                    result = true,
                    subject = content.FirstOrEmptyElement(Constants.HtmlElementType.Title).Contents,
                    caption = content.FirstOrEmptyElement(Constants.HtmlElementType.Caption).Contents,
                    body = content.FirstOrEmptyElement(Constants.HtmlElementType.Body).Contents
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Save an event
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="subject">The subject (title) of the event</param>
        /// <param name="type">The type of event</param>
        /// <param name="startDate">The date component of the starting date of the event</param>
        /// <param name="startTime">The time component of the starting date of the event</param>
        /// <param name="endDate">The date component of the ending date of the event</param>
        /// <param name="endTime">The time component of the ending date of the event</param>
        /// <param name="priority">The priority of the event</param>
        /// <param name="isPublic">Whether the event is public or not</param>
        /// <param name="state">The state to show this news for</param>
        /// <param name="caption">The caption of the content of the event</param>
        /// <param name="body">The body of the content of the event</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Calendar Events", "~/Sites/Overview")]
        [ValidateInput(false)]
        public virtual ActionResult Save(int? eventId, int languageId, string subject, int type, bool allDay, DateTime startDate, DateTime? startTime, DateTime endDate, DateTime? endTime, CalendarEvent.Priority priority, bool isPublic, int? state, string caption, string body)
        {
            try
            {
                CalendarEvent calendarEvent;
                if (eventId.HasValue && eventId.Value > 0)
                {
                    calendarEvent = CalendarEvent.LoadFull(eventId.Value);
                }
                else
                {
                    calendarEvent = new CalendarEvent();
                    calendarEvent.Sites.Add(CurrentSite);
                }

                try
                {
                    calendarEvent.CalendarEventTypeID = type;
                    // Build the full datetimes from the individual components
                    calendarEvent.StartDate = allDay ? startDate.StartOfDay() : startDate.AddTime(startTime);
                    calendarEvent.EndDate = allDay ? startDate.EndOfDay() : endDate.AddTime(endTime);
                    calendarEvent.IsAllDayEvent = allDay;
                    //calendarEvent.IsCorporate = true;
                    calendarEvent.CalendarPriorityID = priority.ToInt();
                    calendarEvent.IsPublic = isPublic;

                    if (state.HasValue && state.Value > 0)
                    {
                        if (calendarEvent.Address == null)
                        {
                            calendarEvent.Address = new Address();
                            calendarEvent.Address.StartEntityTracking();
                            calendarEvent.Address.AddressTypeID = Constants.AddressType.Event.ToShort();
                            calendarEvent.Address.CountryID = Constants.Country.UnitedStates.ToInt(); // TODO: change this to not be hardcoded when other countries are supported. - JHE
                            calendarEvent.Address.Address1 = string.Empty;
                            calendarEvent.Address.City = string.Empty;
                            calendarEvent.Address.PostalCode = string.Empty;
                        }
                        calendarEvent.Address.StateProvinceID = state.Value;
                    }

                    // Create an HTMLContent if there is a caption or body
                    if (!string.IsNullOrEmpty(subject) || !string.IsNullOrEmpty(caption) || !string.IsNullOrEmpty(body))
                    {
                        HtmlSection section = calendarEvent.HtmlSection ?? new HtmlSection()
                        {
                            HtmlSectionEditTypeID = (int)Constants.HtmlSectionEditType.CorporateOnly,
                            HtmlContentEditorTypeID = Constants.HtmlContentEditorType.RichText.ToShort(),
                            SectionName = subject
                        };

                        HtmlContent content = section.ProductionContent(CurrentSite, languageId);
                        if (content == null)
                        {
                            content = new HtmlContent()
                            {
                                HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production,
                                LanguageID = languageId,
                                Name = subject,
                                PublishDateUTC = DateTime.Now.LocalToUTC()
                            };
                            section.HtmlSectionContents.Add(new HtmlSectionContent()
                            {
                                HtmlContent = content,
                                SiteID = CurrentSite.SiteID
                            });
                        }

                        if (!string.IsNullOrEmpty(subject))
                        {
                            HtmlElement subjectElement = content.FirstOrNewElement(Constants.HtmlElementType.Title);
                            subjectElement.Contents = subject;
                        }

                        if (!string.IsNullOrEmpty(caption))
                        {
                            HtmlElement captionElement = content.FirstOrNewElement(Constants.HtmlElementType.Caption);
                            captionElement.Contents = caption;
                        }

                        if (!string.IsNullOrEmpty(body))
                        {
                            HtmlElement bodyElement = content.FirstOrNewElement(Constants.HtmlElementType.Body);
                            bodyElement.Contents = body;
                        }

                        if (calendarEvent.HtmlSection == null)
                        {
                            calendarEvent.HtmlSection = section;
                        }
                    }

                    calendarEvent.Save();

                    return Json(new { result = true, eventId = calendarEvent.CalendarEventID });
                }
                catch (Exception ex)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
