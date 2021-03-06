﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GolfDB2.Tools;
using System.Web.Mvc;

namespace GolfDB2.Models
{
    public class SetupModel
    {
        public int CourseId { get; set; }
        public int LogLevel { get; set; }

        public SetupModel()
        {
            CourseId = GlobalSettingsApi.GetInstance().CourseId;
            LogLevel = GlobalSettingsApi.GetInstance().LogLevel;
        }
    }
}