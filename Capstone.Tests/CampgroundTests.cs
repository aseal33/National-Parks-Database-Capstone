using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundTests : ParkDB_Tests
    {
        [TestMethod]
        public void GetCampgroundsFromPark_ReturnsCampgrounds()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);

            int ID = 0;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT park_id FROM park", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ID = Convert.ToInt32(reader["park_id"]);
                }
            }

            // Act
            Campground test = new Campground();
           IList<Campground> campgrounds = dal.GetCampgroundsFromPark(ID);
           

            // Assert
            Assert.AreEqual(1, campgrounds.Count);

        }
        [TestMethod]
        public void GetAvailability_ReturnsCount()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);

            int iD = 0;
            DateTime start = new DateTime(0000, 00, 00);
            DateTime end = new DateTime(0000, 00, 00);
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM site INNER JOIN campground ON site.campground_id = campground.campground_id INNER JOIN reservation ON site.site_id = reservation.site_id WHERE from_date = '2018-06-30' AND to_date = '2018-07-21'", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    iD = Convert.ToInt32(reader["park_id"]);
                    start = Convert.ToDateTime(reader["from_date"]);
                    end = Convert.ToDateTime(reader["to_date"]);
                    
                }
            }

            // Act
            // Campground test = new Campground();
            IList<Campsite> campsite = dal.CampgroundAvailability(iD, start, end);


            // Assert
            Assert.AreEqual(1, campsite.Count);
        }
    }
}
