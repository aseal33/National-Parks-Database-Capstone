using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ParkSqlDAL : IParkDAL
    {
        private string connectionString;

        // Single Parameter Constructor
        public ParkSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park> GetAllParks()
        {
            List<Park> output = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Park ORDER BY name", conn);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Description = Convert.ToString(reader["description"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.AnnualVisitorCount = Convert.ToInt32(reader["visitors"]);
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
    }
}
