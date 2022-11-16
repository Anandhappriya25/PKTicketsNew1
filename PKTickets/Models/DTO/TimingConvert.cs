﻿using System.Globalization;

namespace PKTickets.Models.DTO
{
    public static class TimingConvert
    {
        public static int ConvertToInt(string time)
        { 
            DateTime s=DateTime.Parse(time,System.Globalization.CultureInfo.InvariantCulture);
            int hour=Convert.ToInt32(s.ToString("HH"));
            int min = Convert.ToInt32(s.ToString("mm"));
            int hours = hour * 60;
            int timing = hours + min;
            return timing;
        }
        public static string ConvertToString(int time)
        {
            int hour = time / 60;
            int min = time % 60;
            string t = hour+":"+min;
            DateTime ss = DateTime.Parse(t, System.Globalization.CultureInfo.CurrentCulture);
            string tt = ss.ToString("hh:mm tt");
            return tt;
        }
    }
}
