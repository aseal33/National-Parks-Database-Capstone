using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL : ICampgroundDAL
    {
        private string ConnectionString;

        public CampgroundSqlDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public IList<Campground> GetCampgroundsFromPark(int park_Id)
        {
            List<Campground> output = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();

                    string query = $"SELECT * FROM campground WHERE campground.park_id = {park_Id}";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        campground.Park_Id = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.Opening_Month = Convert.ToInt32(reader["open_from_mm"]);
                        campground.Closing_Month = Convert.ToInt32(reader["open_to_mm"]);
                        campground.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);
                        output.Add(campground);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("These aren't the grounds you're looking for.");
                throw;
            }
            return output;
        }

        public IList<Campsite> CampgroundAvailability(int campground_Id, DateTime startDate, DateTime endDate)
        {
            List<Campsite> output = new List<Campsite>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand($"SELECT * FROM campground WHERE campground_id = {campground_Id};", conn);
                    SqlDataReader read = command.ExecuteReader();
                    Campground campgroundToBook = new Campground();
                    while (read.Read())
                    {
                        campgroundToBook.Campground_Id = Convert.ToInt32(read["campground_id"]);
                        campgroundToBook.Park_Id = Convert.ToInt32(read["park_id"]);
                        campgroundToBook.Name = Convert.ToString(read["name"]);
                        campgroundToBook.Opening_Month = Convert.ToInt32(read["open_from_mm"]);
                        campgroundToBook.Closing_Month = Convert.ToInt32(read["open_to_mm"]);
                        campgroundToBook.Daily_Fee = Convert.ToDecimal(read["daily_fee"]);
                    }

                    read.Close();

                    if (startDate.Month < campgroundToBook.Opening_Month || endDate.Month > campgroundToBook.Closing_Month)
                    {
                        Console.WriteLine("GO AWAY! WE CLOSED!!!!");
                        return output;
                    }

                    string query = $" SELECT * FROM site "
                    + $" INNER JOIN campground ON "
                    + $" campground.campground_id = site.campground_id "
                    + $" WHERE site.site_id IN ("
                    + $" SELECT site_id FROM site"
                    + $" WHERE campground_id = {campground_Id}"
                    + $" EXCEPT"
                    + $" SELECT site.site_id FROM reservation INNER JOIN site ON site.site_id = reservation.site_id"
                    + $"	WHERE campground_id = {campground_Id} "
                    + $"	AND ((to_date BETWEEN @startDate AND @endDate)"
                    + $"		OR	(from_date BETWEEN @startDate AND @endDate)"
                    + $"		OR	(from_date >= @endDate AND to_date <= @startDate)))";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campsite campsite= new Campsite();
                        campsite.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        campsite.Site_Id = Convert.ToInt32(reader["site_id"]);
                        campsite.Site_Number = Convert.ToInt32(reader["site_number"]);
                        campsite.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        campsite.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        campsite.Max_RV_Length = Convert.ToInt32(reader["max_rv_length"]);
                        campsite.HasUtilities = Convert.ToBoolean(reader["utilities"]);
                        output.Add(campsite);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Creativity is in another castle; I have no idea.");
                throw;
            }

            return output;
        }
    }
}
