using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundDAL
    {
        IList<Campground> GetCampgroundsFromPark(int park_Id);

        IList<Campsite> CampgroundAvailability(int campground_Id, DateTime start_date, DateTime end_date);
    }
}
