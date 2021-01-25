using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public class FormattedDateTime
    {
        public static DateTime GetFormattedDate(int releaseDay, int releaseMonth, int releaseYear)
        {
            if (releaseDay == 0 && releaseMonth == 0 && releaseYear == 0)
            {
                return DateTime.MaxValue;
            }
            releaseDay = releaseDay == 0 ? 1 : releaseDay;
            releaseMonth = releaseMonth == 0 ? 1 : releaseMonth;
            releaseYear = releaseYear == 0 ? 1 : releaseYear;
            return new DateTime(releaseYear, releaseMonth, releaseDay);
        }
    }
}
