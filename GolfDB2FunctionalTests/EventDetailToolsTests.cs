using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GolfDB2.Models;
using GolfDB2.Tools;
using GolfDB2.Controllers;
using Newtonsoft.Json;

namespace GolfDB2FunctionalTests
{

    [TestClass]
    public class EventDetailToolsTests
    {
        private readonly string connectionString = "data source=DESKTOP-S7JQFF1\\SQLEXPRESS;initial catalog=GolfDB20161207-01;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        [TestMethod]
        public void EventDetailUpdate()
        {
            GolfDB2.EventDetail eventDetail = new GolfDB2.EventDetail();

            eventDetail.Id = 13;
            eventDetail.EventId = 13;
            eventDetail.CourseId = 1;
            eventDetail.PlayFormat = 8;
            eventDetail.NumberOfHoles = 13;
            eventDetail.IsShotgunStart = true;
            eventDetail.Sponsor = "Men's Association";
            eventDetail.PlayListId = 6;
            eventDetail.OrgId = 2;
            eventDetail.StartHoleId = 1;
            eventDetail.NumGroups = 14;
            eventDetail.NumPerGroup = 6;

            bool res = EventDetailTools.EventDetailUpdate(eventDetail, connectionString);
        }
    }
}
