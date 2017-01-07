using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Description;
using Newtonsoft.Json;
using GolfDB2.Models;
using GolfDB2.Tools;
using System.Net;

namespace GolfDB2.Controllers
{
    public class HtmlApiController : ApiController
    {

        [ResponseType(typeof(string))]
        // GET: api/HtmlApi
        public HttpResponseMessage Get()
        {
            // make explicit calls to get parameters from the request object
            string action = Request.RequestUri.ParseQueryString().Get("action"); // need error logic!

            string resp = "<p>Hello world!</p>";

            if (!string.IsNullOrEmpty(action) && action.ToLower().Trim() == "teetimes")
            {
                int eventId = int.Parse(Request.RequestUri.ParseQueryString().Get("eventId"));
                List<TeeTime> teeTimeList = TeeTimeTools.MakeTeeTimes(eventId, null);
                resp = HtmlFactory.MakeTeeTimeTable(teeTimeList);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(resp)
            };
        }
    }
}
