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
    public class HtmlFactoryTests
    {
        [TestMethod]
        public void MakeTeeTimeTable()
        {
            List<GolfDB2.TeeTime> ttList = TeeTimeTools.GetTeeTimeTimesByEventId(13, "");
            string html = HtmlFactory.MakeTeeTimeTable(ttList);
            Assert.IsNull(html);
        }

        [TestMethod]
        public void MakeTeeTimeHeaderRow()
        {
            string html = HtmlFactory.MakeTeeTimeHeaderRow();
            Assert.IsNull(html);
        }
    }
}
