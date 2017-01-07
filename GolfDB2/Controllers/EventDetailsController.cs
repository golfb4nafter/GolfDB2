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

namespace GolfDB2.Controllers
{
    public class EventDetailsController : Controller
    {
        private GolfDB db = new GolfDB();

        //// GET: EventDetails
        //public ActionResult Index()
        //{
        //    return View(db.EventDetails.ToList());
        //}

        //// GET: EventDetails/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(id);
        //    if (eventDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(eventDetail);
        //}

        //// GET: EventDetails/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: EventDetails/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,EventId,CourseId,PlayFormat,NumberOfHoles,IsShotgunStart,Sponsor,PlayListId")] GolfDB2.Models.EventDetail eventDetail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.EventDetails.Add(eventDetail);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(eventDetail);
        //}

        // GET: EventDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventDetailTools tool = new EventDetailTools();

            // We ar passed the event id.
            // If no detail record exists a new detail record is created
            // either way we get the actual eventDetailId back from the call.
            int eventDetailId = tool.LookupOrCreateEventDetailRecord(id.Value, null);

            GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(eventDetailId);

            if (eventDetail == null)
            {
                return HttpNotFound();
            }




            return View(eventDetail);
        }

        // GET: EventDetails/TeeTimes/5
        public ActionResult TeeTimes(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventDetailTools tool = new EventDetailTools();

            // We ar passed the event id.
            // If no detail record exists a new detail record is created
            // either way we get the actual eventDetailId back from the call.
            int eventDetailId = tool.LookupOrCreateEventDetailRecord(id.Value, null);

            GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(eventDetailId);

            if (eventDetail == null)
            {
                return HttpNotFound();
            }
            return View(eventDetail);
        }


        // POST: EventDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EventId,CourseId,PlayFormat,NumberOfHoles,IsShotgunStart,Sponsor,PlayListId,OrgId,StartHoleId,NumGroups,NumPerGroup")] GolfDB2.Models.EventDetail eventDetail)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            string action = Request["ActionType"].ToString();

            if (ModelState.IsValid)
            {
                db.Entry(eventDetail).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("../EventDetails/TeeTimes/" + eventDetail.EventId.ToString());
                //return RedirectToAction("../CalendarEvents");
            }
            return View(eventDetail);
        }

        //// GET: EventDetails/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(id);
        //    if (eventDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(eventDetail);
        //}

        //// POST: EventDetails/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(id);
        //    db.EventDetails.Remove(eventDetail);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
