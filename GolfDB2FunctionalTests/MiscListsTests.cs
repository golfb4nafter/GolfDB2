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

namespace GolfDB2FunctionalTests
{
    [TestClass]
    public class MiscListsTests
    {
        [TestMethod]
        public void GetLabelsByCourseIdAndType()
        {
            string connectionString = "data source=DESKTOP-S7JQFF1\\SQLEXPRESS;initial catalog=GolfDB20161207-01;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

            string json = MiscLists.GetNineLabelsByCourseIdAndType(1, "Nine", connectionString);
            Console.Out.WriteLine(json);
            string expected = "[{\"key\":\"1\",\"val\":\"Front\"},{\"key\":\"2\",\"val\":\"Back\"},{\"key\":\"3\",\"val\":\"New\"}]";
            Assert.AreNotEqual(json, expected);
        }

        public void GetNineNameByCourseIdAndZeroBasedOrdinalTest()
        {
            int id = 0;

            string resp = MiscLists.GetNineNameByCourseIdAndZeroBasedOrdinal(1, 0, ref id);

            Assert.AreNotEqual("Front", resp);
        }

    }
}
