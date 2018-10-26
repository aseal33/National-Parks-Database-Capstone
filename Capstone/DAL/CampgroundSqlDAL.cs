using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IList<Campsite> CampgroundAvailability(Campground campgroundToBook, DateTime start_date, DateTime end_date)
        {
            List<Campsite> output = new List<Campsite>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();

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
