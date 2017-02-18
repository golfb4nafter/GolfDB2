using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Description;
using Newtonsoft.Json;
using GolfDB2.Models;
using GolfDB2.Tools;

namespace GolfDB2.Controllers
{
    public class SelectListApiController : ApiController
    {
        [ResponseType(typeof(SelectListItem))]
        // GET: api/SelectListApi
        public IHttpActionResult Get()
        {
            // make explicit calls to get parameters from the request object
            string action = Request.RequestUri.ParseQueryString().Get("action"); // need error logic!

            if (string.IsNullOrEmpty(action))
                return null;

            if (!string.IsNullOrEmpty(action) && action.ToLower(new CultureInfo("en-US", false)).Trim() == "updatecardselect")
            {
                int eventId = int.Parse(Request.RequestUri.ParseQueryString().Get("eventId"));
                string resp = MobileScoresHtmlFactory.makeTeamSelectOptions(eventId, null);
                List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(resp);
                return Json(items);
            }

            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "teetime")
            {
                string date = Request.RequestUri.ParseQueryString().Get("date"); // need error logic!
                Logger.LogDebug("get", date);                                                                         // Now use id and customer
                int holeId = int.Parse(Request.RequestUri.ParseQueryString().Get("holeId"));
                int eventId = int.Parse(Request.RequestUri.ParseQueryString().Get("eventId"));
                List<SelectListItem> items = MiscLists.MakeListOfAvailableTeeTimes(
                    int.Parse(date.Split('-')[1]),
                    int.Parse(date.Split('-')[2]),
                    int.Parse(date.Split('-')[0]),
                    holeId,
                    eventId, null);
                return Json(items);
            }

            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "labelslist")
            {
                int courseId = int.Parse(Request.RequestUri.ParseQueryString().Get("courseId"));
                string type = Request.RequestUri.ParseQueryString().Get("type");
                List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(MiscLists.GetLabelsByCourseIdAndType(courseId, type, null));
                return Json(items);
            }

            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "coursenameslist")
            {
                return Json(MiscLists.GetCourseNamesList(null));
            }


            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "objecttypelist")
            {
                return Json(MiscLists.GetObjectTypeList(null));
            }

            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "gpspointslist")
            {
                int courseId = int.Parse(Request.RequestUri.ParseQueryString().Get("courseId"));
                return Json(MiscLists.GetGeoSpatialDataPointsByCourseId(courseId));
            }


            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "holelist")
            {
                int courseId = int.Parse(Request.RequestUri.ParseQueryString().Get("courseId"));
                return Json(MiscLists.GetHoleListByCourseId(courseId));
            }

            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "startingholeselectlist")
            {
                bool isShotgunStart = false;

                int courseId = int.Parse(Request.RequestUri.ParseQueryString().Get("courseId"));
                int playlistId = int.Parse(Request.RequestUri.ParseQueryString().Get("playlistId"));

                string strShotgun = Request.RequestUri.ParseQueryString().Get("isShotgunStart");

                if (strShotgun.ToLower(new CultureInfo("en-US", false)) == "true" || strShotgun == "1")
                    isShotgunStart = true;

                return Json(MiscLists.StartingHoleSelectList(courseId, playlistId, isShotgunStart, null));
            }

            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "holelistselectlist")
            {
                int courseId = int.Parse(Request.RequestUri.ParseQueryString().Get("courseId"));
                return Json(MiscLists.GetHoleListSelectListByCourseId(courseId, null));
            }

            if (action.ToLower(new CultureInfo("en-US", false)).Trim() == "numholesselectlist")
            {
                int courseId = int.Parse(Request.RequestUri.ParseQueryString().Get("courseId"));
                int selection = int.Parse(Request.RequestUri.ParseQueryString().Get("selection"));
                return Json(MiscLists.GetNumberOfHolesSelectListByCourseIdAndType(courseId, selection, null));
            }

            return null;
        }
    }
}
