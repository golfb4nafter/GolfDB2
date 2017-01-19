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
    public class ScoreBoardController : Controller
    {
        // GET: ScoreCard
        public ActionResult Index()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            EventDetailTools tool = new EventDetailTools();

            // We ar passed the event id.
            // If no detail record exists a new detail record is created
            // either way we get the actual eventDetailId back from the call.
            int eventDetailId = tool.LookupOrCreateEventDetailRecord(8, null);
            GolfDB2.EventDetail eventDetail = EventDetailTools.GetEventDetailRecord(eventDetailId, null);
            //GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(eventDetailId);


            if (eventDetail == null)
            {
                return HttpNotFound();
            }

            Response.AddHeader("Refresh", "30");

            return View(eventDetail);

            //return View();
        }


        public ActionResult Edit(int id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            EventDetailTools tool = new EventDetailTools();

            int eventDetailId = tool.LookupOrCreateEventDetailRecord(id, null);
            GolfDB2.EventDetail eventDetail = EventDetailTools.GetEventDetailRecord(eventDetailId, null);

            if (eventDetail == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrEmpty(eventDetail.SortOn))
            {
                eventDetail.SortOn = "ordinal";
            }

            return View(eventDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EventId,CourseId,PlayFormat,NumberOfHoles,IsShotgunStart,Sponsor,PlayListId,OrgId,StartHoleId,NumGroups,NumPerGroup,SortOn")] GolfDB2.EventDetail eventDetail)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (Request.Form["EditDetail"].ToString() == "true")
                return RedirectToAction("../EventDetails/Edit/" + eventDetail.EventId.ToString());

            //string action = Request["ActionType"].ToString();

            //if (ModelState.IsValid)
            //{
            //}

            if (string.IsNullOrEmpty(eventDetail.SortOn))
            {
                eventDetail.SortOn = "ordinal";
            }

            return View(eventDetail);
        }

        public string GetFormValue(NameValueCollection form, string format, int id, int index)
        {
            return form[string.Format(format, id, index)];
        }
    }
}