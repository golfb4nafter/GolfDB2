using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Description;
using System.Globalization;
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

            // url: '/api/HtmlApi?action=updatescore&Name=' + scoreElementId + '&score=' + val,
            if (!string.IsNullOrEmpty(action) && action.Trim() == "updatescore")
            {
                try
                {
                    int scoreCardId;
                    int ordinal;

                    string elementName = Request.RequestUri.ParseQueryString().Get("Name");
                    int score = int.Parse(Request.RequestUri.ParseQueryString().Get("score"));
                    int playListId = int.Parse(Request.RequestUri.ParseQueryString().Get("playListId"));

                    if (string.IsNullOrEmpty(elementName))
                    {
                        scoreCardId = int.Parse(Request.RequestUri.ParseQueryString().Get("scoreCardId"));
                        ordinal = int.Parse(Request.RequestUri.ParseQueryString().Get("ordinal"));
                    }
                    else
                    {
                        scoreCardId = int.Parse(elementName.Split('_')[1]);
                        ordinal = int.Parse(elementName.Split('_')[2]);
                    }

                    TeeTimeTools.UpdateScoreEntry(scoreCardId, ordinal, score, null);

                    ScoreCard card = TeeTimeTools.GetScoreCard(scoreCardId, null);
                    resp = LeaderBoardHtmlFactory.getRowTotals(scoreCardId, playListId, card.Handicap, null);
                }
                catch (Exception ex)
                {
                    GolfDB2Logger.LogError("HtmlApiController.Get", ex.ToString());
                    resp = "<p style=\"font-weight: bold!important; color: red; \">Error: " + ex.Message + "</p>";
                }
            }

            if (!string.IsNullOrEmpty(action) && action.Trim() == "scoreboard")
            {
                int eventId = int.Parse(Request.RequestUri.ParseQueryString().Get("eventId"));
                string sortType = Request.RequestUri.ParseQueryString().Get("sortType");

                if (sortType.ToLower(new CultureInfo("en-US", false)) == "division")
                    resp = LeaderBoardHtmlFactory.MakeLeaderBoardTable(eventId,
                        LeaderBoardHtmlFactory.SortOnColumn.Division, null);
                else if (sortType.ToLower(new CultureInfo("en-US", false)) == "name")
                    resp = LeaderBoardHtmlFactory.MakeLeaderBoardTable(eventId,
                        LeaderBoardHtmlFactory.SortOnColumn.Name, null);
                else if (sortType.ToLower(new CultureInfo("en-US", false)) == "startingholenumber")
                    resp = LeaderBoardHtmlFactory.MakeLeaderBoardTable(eventId,
                        LeaderBoardHtmlFactory.SortOnColumn.StartingHoleNumber, null);
                else if (sortType.ToLower(new CultureInfo("en-US", false)) == "totalscore")
                    resp = LeaderBoardHtmlFactory.MakeLeaderBoardTable(eventId,
                        LeaderBoardHtmlFactory.SortOnColumn.TotalScore, null);
                else    
                    resp = LeaderBoardHtmlFactory.MakeLeaderBoardTable(eventId,
                        LeaderBoardHtmlFactory.SortOnColumn.Ordinal, null);
            }

            if (!string.IsNullOrEmpty(action) && action.ToLower(new CultureInfo("en-US", false)).Trim() == "teetimes")
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
