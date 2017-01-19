using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using GolfDB2.Tools;

namespace GolfDB2.Controllers
{
    public class BackendController : Controller
    {
        GolfDB2DataContext db = EventDetailTools.GetDB(null);

        public class JsonEvent
        {
            public string id { get; set; }
            public string CourseId { get; set; }
            public string text { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }

        public ActionResult Events(DateTime? start, DateTime? end)
        {
            GolfDB2Logger.LogDebug("Events", "Return list of events from SQL server using LINQ.");

            // SQL: SELECT * FROM [event] WHERE NOT (([end] <= @start) OR ([start] >= @end))
            var events = from ev in db.Events.AsEnumerable() where !(ev.end <= start || ev.start >= end) select ev;

            var result = events
            .Select(e => new JsonEvent()
            {
                start = e.start.ToString("s"),
                end = e.end.ToString("s"),
                text = e.text,
                id = e.id.ToString(),
                CourseId = e.CourseId.ToString()
            })
            .ToList();

            return new JsonResult { Data = result };
        }

        public ActionResult Create(string start, string end, string name)
        {
            GolfDB2Logger.LogDebug("Create", "Add new event to SQL server using LINQ.");

            var toBeCreated = new Event
            {
                CourseId = Tools.GlobalSettingsApi.GetInstance().CourseId,
                start = Convert.ToDateTime(start),
                end = Convert.ToDateTime(end),
                text = name
            };
            db.Events.InsertOnSubmit(toBeCreated);
            db.SubmitChanges();

            return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeCreated.id } } };

        }

        public ActionResult Move(int id, string newStart, string newEnd)
        {
            GolfDB2Logger.LogDebug("Move", "Update event start and end times in SQL server using LINQ.");

            var toBeResized = (from ev in db.Events where ev.id == id select ev).First();
            toBeResized.start = Convert.ToDateTime(newStart);
            toBeResized.end = Convert.ToDateTime(newEnd);
            db.SubmitChanges();

            return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.id } } };
        }

        public ActionResult Resize(int id, string newStart, string newEnd)
        {
            GolfDB2Logger.LogDebug("Resize", "Update event start and end times in SQL server using LINQ.");

            var toBeResized = (from ev in db.Events where ev.id == id select ev).First();
            toBeResized.start = Convert.ToDateTime(newStart);
            toBeResized.end = Convert.ToDateTime(newEnd);
            db.SubmitChanges();

            return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.id } } };
        }
    }
}