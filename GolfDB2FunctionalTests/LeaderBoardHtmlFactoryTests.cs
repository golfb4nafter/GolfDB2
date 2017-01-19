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
    public class LeaderBoardHtmlFactoryTests
    {
        private readonly string connectionString = "data source=DESKTOP-S7JQFF1\\SQLEXPRESS;initial catalog=GolfDB20161207-01;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        //[TestMethod]
        //public void MakeNineTableTest()
        //{
        //    GolfDB2.Event evt = EventDetailTools.GetEventRecord(8, connectionString);

        //    GolfDB2.EventDetail eventDetail = EventDetailTools.GetEventDetailRecord(evt.id, connectionString);

        //    string html = LeaderBoardHtmlFactory.MakeNineTable(0, 1, evt, eventDetail, connectionString);

        //    Assert.IsNull(html);
        //}
    }
}
