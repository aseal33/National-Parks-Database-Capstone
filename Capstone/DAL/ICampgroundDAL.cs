using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundDAL
    {
        IList<Campground> GetCampgroundsFromPark(Park newPark);
        IList<Campground> CampgroundAvailability(Campground campgroundToBook, DateTime start_date, DateTime end_date);
    }
}
