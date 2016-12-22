using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GolfDB2.Tools
{
    public class GlobalSettingsApi
    {
        GlobalSettingsDataContext db = null;

        //private static GlobalSettingsApi _instance = null;

        //public static GlobalSettingsApi getGlobalSettingsApi(string connectionString)
        //{
        //    if (_instance == null)
        //        _instance = new GlobalSettingsApi(connectionString);

        //    return _instance;
       // }

        public GlobalSettingsApi(string connectionString)
        {
            db = new GlobalSettingsDataContext(connectionString);
        }

        public string GetSetting(string settingName)
        {
            var globalSettings = from ev in db.GlobalSettings.AsEnumerable()
                                    where ev.SettingName == settingName select ev;

            var result = globalSettings.Select(e => new GolfDB2.Models.GlobalSetting()
            {
                Id = e.Id,
                SettingName = e.SettingName,
                LastUserId = e.LastUserId,
                Value = e.Value
            }).ToList();

            return result[0].Value;
        }

        public bool AddUpdateSetting(int userId, string settingName, string value)
        {
            string resp = GetSetting(settingName);

            if (resp == value)
                return true;

            if (resp == null)
            {
                GlobalSetting obj = new GlobalSetting() { LastUserId = userId, SettingName = settingName, Value = value };
                db.GlobalSettings.InsertOnSubmit(obj);
                db.SubmitChanges();
                return true;
            }





            return false;
        }

        private void UpdateCourse()
        {
            OperationDataContext OdContext = new OperationDataContext();
            //Get Single course which need to update
            COURSE objCourse = OdContext.COURSEs.Single(course => course.course_name == "B.Tech");
            //Field which will be update
            objCourse.course_desc = "Bachelor of Technology";
            // executes the appropriate commands to implement the changes to the database
            OdContext.SubmitChanges();
        }

        private void DeleteCourse()
        {
            OperationDataContext OdContext = new OperationDataContext();
            //Get Single course which need to Delete
            COURSE objCourse = OdContext.COURSEs.Single(course => course.course_name == "B.Tech");
            //Puts an entity from this table into a pending delete state and parameter is the entity which to be deleted.
            OdContext.COURSEs.DeleteOnSubmit(objCourse);
            // executes the appropriate commands to implement the changes to the database
            OdContext.SubmitChanges();
        }
    }
}