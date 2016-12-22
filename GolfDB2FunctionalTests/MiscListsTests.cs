﻿using System;
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
    public class MiscListsTests
    {
        private readonly string connectionString = "data source=DESKTOP-S7JQFF1\\SQLEXPRESS;initial catalog=GolfDB20161207-01;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        [TestMethod]
        public void GetNineLabelsByCourseIdAndType()
        {
            string json = MiscLists.GetNineLabelsByCourseIdAndType(1, "Nine", connectionString);
            Console.Out.WriteLine(json);
            string expected = "[{\"Value\":\"2\",\"Text\":\"Front\"},{\"Value\":\"3\",\"Text\":\"Back\"},{\"Value\":\"4\",\"Text\":\"Third\"}]";
            Assert.AreEqual(json, expected);
        }

        [TestMethod]
        public void GetNineNameByCourseIdAndZeroBasedOrdinal()
        {
            string resp = MiscLists.GetNineNameByCourseIdAndZeroBasedOrdinal(1, 1, connectionString);
            Assert.AreEqual("Back", resp);
        }

        [TestMethod]
        public void GetCourseNameById()
        {
            string resp = MiscLists.GetCourseNameById(1, connectionString);

            Assert.AreEqual("Airport National", resp);
        }

        [TestMethod]
        public void GetCourseNamesList()
        {
            string json = MiscLists.GetCourseNamesList(connectionString);

            Assert.AreEqual(json, "[{\"Value\":\"1\",\"Text\":\"Airport National\"},{\"Value\":\"2\",\"Text\":\"twin pines\"}]");

        }

        [TestMethod]
        public void GetHoleListByCourseId()
        {
            string json = MiscLists.GetHoleListByCourseId(1, connectionString);
            Assert.AreEqual(json, "[{\"Value\":\"1\",\"Text\":\"1\"},{\"Value\":\"2\",\"Text\":\"2\"},{\"Value\":\"3\",\"Text\":\"3\"},{\"Value\":\"4\",\"Text\":\"4\"}]");
        }

        [TestMethod]
        public void GetHoleNumberByHoleId()
        {
            int resp = MiscLists.GetHoleNumberByHoleId(2, connectionString);
            Assert.AreEqual(2, resp);
        }

        [TestMethod]
        public void GetGeoSpatialDataPointDescriptionById()
        {
            string json = MiscLists.GetGeoSpatialDataPointDescriptionById(2, connectionString);
            Assert.AreEqual(json, "Test Point 2");
        }

        [TestMethod]
        public void GetGeoSpatialDataPointsByCourseId()
        {
            string json = MiscLists.GetGeoSpatialDataPointsByCourseId(1, connectionString);
            Assert.AreEqual(json, "[{\"Value\":\"2\",\"Text\":\"Test Point 2\"},{\"Value\":\"1\",\"Text\":\"unknown\"}]");
        }

        [TestMethod]
        public void GetObjectTypeList()
        {
            string json = MiscLists.GetObjectTypeList(connectionString);
            Assert.IsTrue(json.Length > 5);
        }

        [TestMethod]
        public void GetObjectTypeById()
        {
            string json = MiscLists.GetObjectTypeById(1, connectionString);

            Assert.AreEqual(json, "TeeBox");
        }

        [TestMethod]
        public void GetCourseRatings()
        {
            string json = MiscLists.GetCourseRatings("WHERE CourseId=1 AND Gender='M'", connectionString);

            Assert.IsTrue(json.Length > 5);
        }

        [TestMethod]
        public void GetCourseRatingsList()
        {
            CourseRatingsCache crCache = new CourseRatingsCache(connectionString);
            Assert.IsTrue(crCache.RatingsList.Count > 3);
        }

        [TestMethod]
        public void GetHoleHandicap()
        {
            int handicap = MiscLists.GetHoleHandicap(1, "white", "f", "10-27", 12, connectionString);

            Assert.IsTrue(handicap == 4);
        }

        [TestMethod]
        public void GetSetting()
        {
            GlobalSettingsApi gs = new GlobalSettingsApi(connectionString);

            string val = gs.GetSetting(0, "Test1", "YYYY");

            Assert.IsTrue(val == "XXXX");
        }

    }
}
