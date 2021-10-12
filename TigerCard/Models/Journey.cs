using System;
using System.Globalization;

namespace TigerCard.Models
{
    public class Journey
    {
        public DateTime Date { get; set; }

        public int Week
        {
            get
            {
                var info = CultureInfo.CurrentCulture;
                var calendar = info.Calendar;
                return calendar.GetWeekOfYear(Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            }
        }

        public string Day => Date.DayOfWeek.ToString();

        public int Time { get; set; }
        public int FromZone { get; set; }
        public int ToZone { get; set; }
    }
}