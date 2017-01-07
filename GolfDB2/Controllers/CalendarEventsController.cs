using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GolfDB2.Models;
using GolfDB2.Tools;
using System.Collections.Specialized;

namespace GolfDB2.Controllers
{
    public class CalendarEventsController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: CalendarEvents
        public ActionResult Index()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            string d1 = Request.Params.Get("fromDate");
            string d2 = Request.Params.Get("toDate");

            if (d1 != null && d2 != null && (DateTime.Parse(d1) >= DateTime.Parse(d2)))
            {
                d1 = null;
                d2 = null;
            }

            if (d1 == null)
            {
                DateTime dt = DateTime.Now;
                d1 = dt.ToString("yyyy-MM-dd");
                d2 = dt.AddDays(8).ToString("yyyy-MM-dd");
            }

            int courseId = GlobalSettingsApi.GetInstance().CourseId;

            var events = from s in db.Event select s;
            // Filter for current course selection only.
            events = events.Where(s => s.CourseId == courseId);

            // filter by start and end dates.
            DateTime from = DateTime.Parse(d1);
            DateTime to = DateTime.Parse(d2);

            events = events.Where(s => s.start >= from);
            events = events.Where(s => s.end <= to);

            // order by start time.
            events = events.OrderByDescending(s => s.start);

            ViewData["fromDate"] = d1;
            ViewData["toDate"] = d2;
            
            return View(events);
 
            //return View(db.Event.ToList());
        }

        // GET: CalendarEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.Event.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        //// GET: CalendarEvents/Create
        //public ActionResult Create()
        //{
        //    ViewData["CourseId"] = GolfDB2.Tools.GlobalSettingsApi.GetInstance().CourseId.ToString();

        //    return View();
        //}

        //// POST: CalendarEvents/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,CourseId,text,start,end")] CalendarEvent calendarEvent)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Event.Add(calendarEvent);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(calendarEvent);
        //}

        //// GET: CalendarEvents/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CalendarEvent calendarEvent = db.Event.Find(id);
        //    if (calendarEvent == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(calendarEvent);
        //}

        //// POST: CalendarEvents/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,CourseId,text,start,end")] CalendarEvent calendarEvent)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(calendarEvent).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(calendarEvent);
        //}

        // GET: CalendarEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.Event.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        // POST: CalendarEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalendarEvent calendarEvent = db.Event.Find(id);
            db.Event.Remove(calendarEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
