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

        public int ReserveCampsite(Campsite campsiteRequested, DateTime start_date, DateTime end_date, string partyName)
        {
            int id = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand($"INSERT INTO reservation VALUES (site_id, '{partyName} Family Reservation', {start_date}, {end_date}, CURRENT_TIMESTAMP)", conn);
                    SqlCommand sql = new SqlCommand("DECLARE @reservationID int = (SELECT @@IDENITY)", conn);

                    SqlDataReader reader = sql.ExecuteReader();

                    id = Convert.ToInt32(reader["@reservationID"]);
                }

                return id;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("I'm sorry friend, you can't reserve greatness");
                throw;
            }
        }
    }
}
