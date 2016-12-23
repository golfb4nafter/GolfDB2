using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using log4net;

namespace GolfDB2.Tools
{
    public class GlobalSettingsApi
    {
        private string _connectionString = null;
        GlobalSettingsDataContext db = null;

        public int CourseId { get; set; }

        public bool bInitDone = false;

        public void InitGlobalSettings()
        {
            GolfDB2Logger.LogDebug("InitGlobalSettings", "Initialize GlobalSettingsApi");

            if (!bInitDone)
            {
                CourseId = int.Parse(GetSetting(0, "DefaultCourseId", "1"));
            }
        }

        public GlobalSettingsApi()
        {
            GolfDB2Logger.LogDebug("GlobalSettingsApi", "Constructor.");

            InitGlobalSettings();
        }

        public GlobalSettingsApi(string connectionString)
        {
            GolfDB2Logger.LogDebug("GlobalSettingsApi", "Constructor. connectionString=" + connectionString);

            _connectionString = connectionString;

            InitGlobalSettings();
        }

        public string GetSetting(int userId, string settingName, string defaultValue)
        {
            if (db == null)
            {
                if (!string.IsNullOrEmpty(_connectionString))
                    db = new GlobalSettingsDataContext(_connectionString);
                else
                    db = new GlobalSettingsDataContext();
            }

            var globalSettings = from ev in db.GlobalSettings.AsEnumerable()
                                    where ev.SettingName == settingName select ev;

            var result = globalSettings.Select(e => new GolfDB2.Models.GlobalSetting()
            {
                Id = e.Id,
                SettingName = e.SettingName,
                LastUserId = e.LastUserId,
                Value = e.Value
            }).ToList();
            
            if (result.Count < 1)
            {
                AddSetting(userId, settingName, defaultValue, defaultValue);
                return defaultValue;
            }

            return result[0].Value;
        }

        public bool AddSetting(int userId, string settingName, string value, string defaultValue)
        {
            if (db == null)
            {
                if (!string.IsNullOrEmpty(_connectionString))
                    db = new GlobalSettingsDataContext(_connectionString);
                else
                    db = new GlobalSettingsDataContext();
            }

            GlobalSetting obj = null;

            try
            {
                obj = new GlobalSetting() { LastUserId = userId, SettingName = settingName, Value = value };
                db.GlobalSettings.InsertOnSubmit(obj);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }

            return false;
        }


        public bool UpdateSetting(int userId, string settingName, string value, string defaultValue)
        {
            if (db == null)
            {
                if (!string.IsNullOrEmpty(_connectionString))
                    db = new GlobalSettingsDataContext(_connectionString);
                else
                    db = new GlobalSettingsDataContext();
            }

            GlobalSetting obj = null;

            try
            {
                obj = db.GlobalSettings.Single(setting => setting.SettingName == settingName);
                obj.Value = value;
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }

            return false;
        }
    }
}