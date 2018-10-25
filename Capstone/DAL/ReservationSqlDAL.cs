using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace Capstone.DAL
{
    public class ReservationSqlDAL : IReservationDAL
    {
        private string ConnectionString;

        public ReservationSqlDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public int ReserveCampsite(Campsite newCampsite)
        {
            throw new NotImplementedException();
        }
    }
}
