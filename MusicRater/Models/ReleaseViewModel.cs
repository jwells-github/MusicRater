using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicRater.Models
{
    public class ReleaseViewModel
    {
        public ReleaseViewModel()
        {

        }
        public ReleaseViewModel(Release release)
        {
            this.ReleaseID = release.ReleaseID;
            this.Title = release.Title;
            this.ReleaseDay = release.ReleaseDay;
            this.ReleaseMonth = release.ReleaseMonth;
            this.ReleaseYear = release.ReleaseYear;
            this.Type = release.Type;
            this.Artist = release.Artist;
            this.ReleaseGenres = release.ReleaseGenres;
        }

        public long ReleaseID { get; set; }
        public string Title { get; set; }
        [Range(0, 31)]
        public int ReleaseDay { get; set; }
        [Range(0, 12)] 
        public int ReleaseMonth { get; set; }
        public int ReleaseYear { get; set; }
        public ReleaseType Type { get; set; }
        public Artist Artist { get; set; }

        public ReleaseRating UserRating { get; set; }

        public int NumberOfRatings { get; set; }
        public double AverageRating { get; set;}
        
        public ICollection<ReleaseGenre> ReleaseGenres { get; set; }

        public List<SelectListItem> Months { get; } = new List<SelectListItem>
        {
            new SelectListItem  { Value = "1", Text = "January" },
            new SelectListItem  { Value = "2", Text = "February" },
            new SelectListItem  { Value = "3", Text = "March" },
            new SelectListItem  { Value = "4", Text = "April" },
            new SelectListItem  { Value = "5", Text = "May" },
            new SelectListItem  { Value = "6", Text = "June" },
            new SelectListItem  { Value = "7", Text = "July" },
            new SelectListItem  { Value = "8", Text = "August" },
            new SelectListItem  { Value = "9", Text = "September" },
            new SelectListItem  { Value = "10", Text = "October" },
            new SelectListItem  { Value = "11", Text = "November" },
            new SelectListItem  { Value = "12", Text = "December" },
        };
        
        public String MonthString()
        {
            switch (this.ReleaseMonth)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";
            }
        }
    }
}
