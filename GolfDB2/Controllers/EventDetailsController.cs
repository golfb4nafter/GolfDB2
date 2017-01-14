using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        //private GolfDB db = new GolfDB();

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
        public ActionResult Edit(int id)
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
            int eventDetailId = tool.LookupOrCreateEventDetailRecord(id, null);
            GolfDB2.EventDetail eventDetail = EventDetailTools.GetEventDetailRecord(eventDetailId, null);
            //GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(eventDetailId);


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
        public ActionResult Edit([Bind(Include = "Id,EventId,CourseId,PlayFormat,NumberOfHoles,IsShotgunStart,Sponsor,PlayListId,OrgId,StartHoleId,NumGroups,NumPerGroup")] GolfDB2.EventDetail eventDetail)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            string action = Request["ActionType"].ToString();

            if (ModelState.IsValid)
            {
                EventDetailTools.EventDetailUpdate(eventDetail, null);
                // Get the tee times list by eventId
                List<TeeTime> ttList = TeeTimeTools.GetTeeTimeTimesByEventId(eventDetail.EventId, null);

                int count = 0;

                foreach(TeeTime tt in ttList)
                {
                    count++;

                    if (count > eventDetail.NumGroups)
                    {
                        TeeTimeTools.DeleteTeeTimeById(tt.Id, null);
                        continue;
                    }

                    List<TeeTimeDetail> ttDetailList = TeeTimeTools.GetTeeTimeDetailList(eventDetail, tt, null);

                    int i = 0;

                    foreach (TeeTimeDetail ttd in ttDetailList)
                    {
                        string test = GetFormValue(Request.Form, "dirty_{0}_{1}", tt.Id, i);

                        if (!string.IsNullOrEmpty(test) && test == "true")
                        {
                            GolfDB2Logger.LogDebug("Edit", "dirty_" + tt.Id.ToString() + "_" + i.ToString());

                            // Harvest this row and update table
                            ttd.Name = GetFormValue(Request.Form, "golfers_{0}_{1}", tt.Id, i);
                            ttd.Cart = GetFormValue(Request.Form, "cart_{0}_{1}", tt.Id, i) == "on";
                            ttd.Pass = GetFormValue(Request.Form, "pass_{0}_{1}", tt.Id, i) == "on";
                            ttd.AmountPaid = decimal.Parse(GetFormValue(Request.Form, "paid_{0}_{1}", tt.Id, i), System.Globalization.NumberStyles.Currency);
                            ttd.Division = GetFormValue(Request.Form, "division_{0}_{1}", tt.Id, i);

                            TeeTimeTools.UpdateTeeTimeDetail(ttd, null);
                        }

                        i++;
                    }

                    //tt.PlayerNames = Request.Form["golfers" + tt.Id.ToString()].ToString();
                    //TeeTimeTools.UpdateTeeTime(tt, null); // update
                }

                //return RedirectToAction("/Index" + eventDetail.EventId.ToString());
                //return RedirectToAction("../Home/Index");
                //return RedirectToAction("../CalendarEvents");
            }
            return View(eventDetail);
        }

        public string GetFormValue(NameValueCollection form, string format, int id, int index)
        {
            return form[string.Format(format, id, index)];
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
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
