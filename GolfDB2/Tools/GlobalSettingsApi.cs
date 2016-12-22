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
        private string _connectionString = null;

        GlobalSettingsDataContext db = null;

        public GlobalSettingsApi()
        {
        }

        public GlobalSettingsApi(string connectionString)
        {
            _connectionString = connectionString;
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

            string value = result[0].Value;

            if (value == null)
            {
                AddUpdateSetting(userId, settingName, defaultValue, defaultValue);
                return defaultValue;
            }

            return value;
        }

        public bool AddUpdateSetting(int userId, string settingName, string value, string defaultValue)
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
                string resp = GetSetting(userId, settingName, defaultValue);

                if (resp == null)
                {
                    obj = new GlobalSetting() { LastUserId = userId, SettingName = settingName, Value = value };
                    db.GlobalSettings.InsertOnSubmit(obj);
                    db.SubmitChanges();
                    return true;
                }

                if (resp == value)
                    return true;

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