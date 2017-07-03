using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using DDay.iCal;
using DDay.iCal.Components;
using DDay.iCal.Serialization;

namespace NetSteps.Web.Mvc.ActionResults
{
    public class iCalResult<T> : FileResult
    {
        public List<T> Items { get; set; }
        public Func<T, Event> ConvertToEvent { get; set; }

        public iCalResult(string filename)
            : base("text/calendar")
        {
            this.FileDownloadName = filename;
        }

        public iCalResult(List<T> dinners, string filename, Func<T, Event> convertToEvent)
            : this(filename)
        {
            Items = dinners;
            ConvertToEvent = convertToEvent;
        }

        public iCalResult(T dinner, string filename, Func<T, Event> convertToEvent)
            : this(filename)
        {
            ConvertToEvent = convertToEvent;
            Items = new List<T>();
            Items.Add(dinner);
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            iCalendar iCal = new iCalendar();
            foreach (T d in this.Items)
            {
                try
                {
                    Event e = ConvertToEvent(d);
                    iCal.Events.Add(e);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            iCalendarSerializer serializer = new iCalendarSerializer(iCal);
            string result = serializer.SerializeToString();
            response.ContentEncoding = Encoding.UTF8;
            response.Write(result);
        }
    }
}
