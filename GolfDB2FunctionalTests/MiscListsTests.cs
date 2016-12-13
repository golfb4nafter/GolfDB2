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
            string expected = "[{\"Key\":\"2\",\"Value\":\"Front\"},{\"Key\":\"3\",\"Value\":\"Back\"},{\"Key\":\"4\",\"Value\":\"Third\"}]";
            Assert.AreEqual(json, expected);
        }

        [TestMethod]
        public void GetNineNameByCourseIdAndZeroBasedOrdinal()
        {
            int id = 0;
            string resp = MiscLists.GetNineNameByCourseIdAndZeroBasedOrdinal(1, 1, ref id, connectionString);
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

            Assert.AreEqual(json, "[{\"Key\":\"1\",\"Value\":\"Airport National\"},{\"Key\":\"2\",\"Value\":\"twin pines\"}]");

        }

        [TestMethod]
        public void GetHoleListByCourseId()
        {
            string json = MiscLists.GetHoleListByCourseId(1, connectionString);
            Assert.AreEqual(json, "[{\"Key\":\"1\",\"Value\":\"1\"},{\"Key\":\"2\",\"Value\":\"2\"},{\"Key\":\"3\",\"Value\":\"3\"},{\"Key\":\"4\",\"Value\":\"4\"}]");
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
            Assert.AreEqual(json, "[{\"Key\":\"1\",\"Value\":\"Test Point 1\"},{\"Key\":\"2\",\"Value\":\"Test Point 2\"}]");
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


    }
}
