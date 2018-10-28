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
        public void GetAvailability_DateIsAvailable()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);

            //int iD = 0;
            DateTime start = new DateTime(2000, 10, 01);
            DateTime end = new DateTime(2000, 10, 10);
           
            IList<Campsite> campsite = dal.CampgroundAvailability(CampgroundId, start, end);


            // Assert
            Assert.AreEqual(1, campsite.Count);
        }

        [TestMethod]
        public void GetAvailability_ReturnsCount1()
        {       
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.CampgroundAvailability(CampgroundId, new DateTime(2018, 06, 30), new DateTime(2018, 07, 21));            

            // Assert
            Assert.AreEqual(1, campsite.Count);
        }

        [TestMethod]
        public void GetAvailability_ReturnsCount0()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.CampgroundAvailability(CampgroundId, new DateTime(2018, 07, 28), new DateTime(2018, 07, 30));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailability_ReturnsCount0Again()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.CampgroundAvailability(CampgroundId, new DateTime(2018, 07, 21), new DateTime(2018, 07, 23));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailability_ReturnsCount0Again1()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.CampgroundAvailability(CampgroundId, new DateTime(2018, 07, 21), new DateTime(2018, 07, 22));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailability_ReturnsCount0Again2()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.CampgroundAvailability(CampgroundId, new DateTime(2018, 07, 29), new DateTime(2018, 07, 30));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailability_ReturnsCount0Again3()
        {
            // Arrange
            CampgroundSqlDAL dal = new CampgroundSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.CampgroundAvailability(CampgroundId, new DateTime(2018, 07, 21), new DateTime(2018, 07, 30));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }       

    }
}
