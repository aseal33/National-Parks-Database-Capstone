using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampsiteDAL
    {
        IList<Campsite> GetCampsitesFromCampground(Campground newCampground);
        IList<Campsite> GetCampsitesFromPark(Park newPark);
    }
}
