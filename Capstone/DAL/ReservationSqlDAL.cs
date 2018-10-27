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

        public int ReserveCampsite(int campsiteId, DateTime start_date, DateTime end_date, string partyName)
        {
            int id = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand($"INSERT INTO reservation VALUES (@site_id, @partyName, @start_date, @end_date, CURRENT_TIMESTAMP); DECLARE @reservationID int = (SELECT @@IDENTITY)", conn);
                    command.Parameters.AddWithValue("@partyName", partyName + " " + "Family Reservation");
                    command.Parameters.AddWithValue("@start_date", start_date);
                    command.Parameters.AddWithValue("@end_date", end_date);
                    command.Parameters.AddWithValue("@site_id", campsiteId);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["@reservationID"]);
                    }
                }

                return id;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("I'm sorry friend, you can't reserve greatness");
                throw;
            }
        }

        public int CountReservations(int campsiteID)
        {
            int output = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    string query = $"SELECT COUNT(reservation_id) AS res FROM reservation WHERE reservation.site_id = {campsiteID}";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        output = Convert.ToInt32(reader["res"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Couldn't find reservations - look elsewhere!");
                throw;
            }

            return output;
        }
    }
}
