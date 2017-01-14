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
        private readonly string connectionString = "data source=DESKTOP-S7JQFF1\\SQLEXPRESS;initial catalog=GolfDB20161207-01;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

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

        [TestMethod]
        public void MakeTotalsTable()
        {
            string html = HtmlFactory.MakeTotalsTable("Senile", "7", "", "", "1");
            Assert.IsNotNull(html);
        }

        //[TestMethod]
        //public void MakeNineTable()
        //{
        //    List<string> scoresList = new List<string>();
        //    scoresList.Add("4");
        //    scoresList.Add("3");
        //    scoresList.Add("3");
        //    scoresList.Add("4");
        //    scoresList.Add("4");
        //    scoresList.Add("3");
        //    scoresList.Add("4");
        //    scoresList.Add("4");
        //    scoresList.Add("3");

        //    string html = HtmlFactory.MakeNineTable(0, "Jim Smith", "1", scoresList.ToArray(), "32");
        //    Assert.IsNotNull(html);
        //}

        //[TestMethod]
        //public void MakeNineTable2()
        //{
        //    List<string> scoresList = new List<string>();
        //    scoresList.Add("4");
        //    scoresList.Add("3");
        //    scoresList.Add("3");
        //    scoresList.Add("4");
        //    scoresList.Add("3");
        //    scoresList.Add("3");
        //    scoresList.Add("3");
        //    scoresList.Add("4");
        //    scoresList.Add("4");

        //    string html = HtmlFactory.MakeNineTable(9, null, "1", scoresList.ToArray(), "31");
        //    Assert.IsNotNull(html);
        //}

    }
}
