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
    public class TeeTimeToolsTests
    {
        private readonly string connectionString = "data source=DESKTOP-S7JQFF1\\SQLEXPRESS;initial catalog=GolfDB20161207-01;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";



        [TestMethod]
        public void MakeTeeTimes()
        {
            List<GolfDB2.TeeTime> d = TeeTimeTools.MakeTeeTimes(7, connectionString);
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void GetTeeTime()
        {
            GolfDB2.TeeTime d = TeeTimeTools.GetTeeTime(17, connectionString);
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void GetEventDetailTest()
        {
            GolfDB2.EventDetail d = TeeTimeTools.GetEventDetail(7, connectionString);
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void GetCalendarEventTest()
        {
            GolfDB2.Event d = TeeTimeTools.GetCalendarEvent(7, connectionString);
            Assert.IsNotNull(d);
        }

    }
}
