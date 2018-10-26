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
    }
}
