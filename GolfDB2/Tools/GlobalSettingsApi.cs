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

        private static GlobalSettingsApi _instance = null;

        /// <summary>
        /// Get an instance of this Api
        /// </summary>
        /// <returns></returns>
        public static GlobalSettingsApi GetInstance()
        {
            if (_instance == null)
                _instance = new GlobalSettingsApi();

            return _instance;
        }

        /// <summary>
        /// Get an instance of globa settings api with connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static GlobalSettingsApi GetInstance(string connectionString)
        {
            if (connectionString == null)
                _instance = new GlobalSettingsApi();

            if (_instance == null)
                _instance = new GlobalSettingsApi(connectionString);

            return _instance;
        }

        private readonly string _connectionString = null;
        GolfDB2DataContext db = null;

        public int CourseId { get; set; }
        public int LogLevel { get; set; }

        public bool bInitDone = false;

        /// <summary>
        /// Initialization of global settings from repo.
        /// </summary>
        public void InitGlobalSettings()
        {
            Logger.LogDebug("GlobalSettingsApi.InitGlobalSettings", "");

            if (!bInitDone)
            {
                CourseId = int.Parse(GetSetting(0, "DefaultCourseId", "1"));
                LogLevel = int.Parse(GetSetting(0, "DefaultLogLevel", "5"));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GlobalSettingsApi()
        {
            Logger.LogDebug("GlobalSettingsApi.GlobalSettingsApi", "");

            InitGlobalSettings();
        }

        /// <summary>
        /// Constructor with connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public GlobalSettingsApi(string connectionString)
        {
            Logger.LogDebug("GlobalSettingsApi.GlobalSettingsApi", connectionString);

            _connectionString = connectionString;

            InitGlobalSettings();
        }

        /// <summary>
        /// Get global setting.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetSetting(int userId, string settingName, string defaultValue)
        {
            Logger.LogDebug("GlobalSettingsApi.GetSetting", 
                string.Format("userId={0}, settingName={1}, defaultValue{2}", userId, settingName, defaultValue));

            if (db == null)
                db = EventDetailTools.GetDB(_connectionString);

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

        /// <summary>
        /// Add new global setting.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool AddSetting(int userId, string settingName, string value, string defaultValue)
        {
            Logger.LogDebug("GlobalSettingsApi.AddSetting",
                string.Format("userId={0}, settingName={1}, value={2}, defaultValue{3}", userId, settingName, value, defaultValue));

            if (db == null)
                db = EventDetailTools.GetDB(_connectionString);

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
                Logger.LogError("GlobalSettingsApi.AddSetting", ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Update global setting.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool UpdateSetting(int userId, string settingName, string value, string defaultValue)
        {
            Logger.LogDebug("GlobalSettingsApi.UpdateSetting",
                string.Format("userId={0}, settingName={1}, value={2}, defaultValue{3}", userId, settingName, value, defaultValue));

            if (db == null)
                db = EventDetailTools.GetDB(_connectionString);

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
                Logger.LogError("GlobalSettingsApi.UpdateSetting", ex.ToString());
            }

            return false;
        }
    }
}