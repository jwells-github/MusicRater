using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class FormattedDateTime
    {
        public static DateTime GetFormattedDate(int day, int month, int year)
        {
            if (day == 0 && month == 0 && year == 0)
            {
                return DateTime.MaxValue;
            }
            day = day == 0 ? 1 : day;
            month = month == 0 ? 1 : month;
            year = year == 0 ? 1 : year;
            return new DateTime(year, month, day);
        }
    }
}
