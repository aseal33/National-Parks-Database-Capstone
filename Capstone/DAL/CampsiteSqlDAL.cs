using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Capstone.DAL
{

    public class CampsiteSqlDAL : ICampsiteDAL
    {
        private string ConnectionString;

        public CampsiteSqlDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public IList<Campsite> GetCampsitesFromCampground(Campground newCampground)
        {
            throw new NotImplementedException();
        }

        public IList<Campsite> GetCampsitesFromPark(Park newPark)
        {
            throw new NotImplementedException();
        }
    }
}
