using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class Menu
    {
        public string Type;
        public string Title;
        public MenuOption Options;
    }

    public class MenuOption
    {
        public string Title;
        public string ScheduleFilter;
        public string Tabs;
    }
}