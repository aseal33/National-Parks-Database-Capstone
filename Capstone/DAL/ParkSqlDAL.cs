using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAL : IParkDAL
    {
        private string connectionString;

        // Single Parameter Constructor
        public ParkSqlDAL(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }

        public IList<Park> GetAllParks()
        {
            List<Park> output = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT park.park_id, park.name FROM Park ORDER BY name", conn);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.Name = Convert.ToString(reader["name"]);
                        park.Park_Id = Convert.ToInt32(reader["park_id"]);

                        output.Add(park);
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Ya played yourself, friend. Ain't no parks here.");
                throw;
            }

            return output;
        }

        public Park GetParkInfo(int Park_Id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM park WHERE park.park_id = @parkId", conn);
                    command.Parameters.AddWithValue("@parkId", Park_Id);

                    SqlDataReader reader = command.ExecuteReader();
                    Park park = new Park();
                    while (reader.Read())
                    {
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Description = Convert.ToString(reader["description"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.AnnualVisitorCount = Convert.ToInt32(reader["visitors"]);
                        park.Park_Id = Convert.ToInt32(reader["park_id"]);
                    }

                    return park;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("404: File Not Found");
                throw;
            }
        }

        public IList<Campsite> ParkAvailability(Park parkToBook, DateTime start_date, DateTime end_date)
        {
            List<Campsite> output = new List<Campsite>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand($"SELECT * FROM campground WHERE campground.park_id = {parkToBook.Park_Id}");
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground campgroundToBook = new Campground();
                        campgroundToBook.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        campgroundToBook.Park_Id = Convert.ToInt32(reader["park_id"]);
                        campgroundToBook.Name = Convert.ToString(reader["name"]);
                        campgroundToBook.Opening_Month = Convert.ToInt32(reader["open_from_mm"]);
                        campgroundToBook.Closing_Month = Convert.ToInt32(reader["open_to_mm"]);
                        campgroundToBook.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);

                        if (start_date.Month < campgroundToBook.Opening_Month || end_date.Month > campgroundToBook.Closing_Month)
                        {
                            Console.WriteLine("GO AWAY! WE CLOSED!!!!");
                            return output;
                        }

                        string query = "SELECT site.site_id, campground.daily_fee FROM site INNER JOIN campground " +
                            "ON campground.campground_id = site.campground_id WHERE site.site_id IN (SELECT site_id FROM site " +
                            $"WHERE campground_id = {campgroundToBook.Campground_Id} " +
                            "EXCEPT " +
                            "SELECT reservation.site_id FROM reservation " +
                            "INNER JOIN site ON site.site_id = reservation.site_id " +
                            $"WHERE campground_id = {campgroundToBook.Campground_Id} AND ((to_date BETWEEN \'{start_date}\' AND \'{end_date}\') " +
                            $"OR (from_date BETWEEN \'{start_date}\' AND \'{end_date}\') OR " +
                            $"((to_date >= '{start_date}') AND (from_date <= '{end_date}')))";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        SqlDataReader reader2 = cmd.ExecuteReader();

                        while (reader2.Read())
                        {
                            Campsite campsite = new Campsite();
                            campsite.Campground_Id = Convert.ToInt32(reader2["campground_id"]);
                            campsite.Site_Id = Convert.ToInt32(reader2["site_id"]);
                            campsite.Site_Number = Convert.ToInt32(reader2["site_number"]);
                            campsite.Max_Occupancy = Convert.ToInt32(reader2["max_occupancy"]);
                            campsite.IsAccessible = Convert.ToBoolean(reader2["accessible"]);
                            campsite.Max_RV_Length = Convert.ToInt32(reader2["max_rv_length"]);
                            campsite.HasUtilities = Convert.ToBoolean(reader2["utilities"]);
                            output.Add(campsite);
                        }
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
