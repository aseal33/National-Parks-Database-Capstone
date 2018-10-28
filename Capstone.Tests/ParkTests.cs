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
    public class ParkTests : ParkDB_Tests
    {
        [TestMethod]
        public void GetAllParks()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);

            // Act
            IList<Park> parks = dal.GetAllParks();

            // Assert
            Assert.AreEqual(1, parks.Count);
        }

        [TestMethod]
        public void GetParkInfo()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);
            int ID = 0;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT park_id FROM park", conn);
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    ID = Convert.ToInt32(reader["park_id"]);
                }
            }


                // Act
                 Park test = dal.GetParkInfo(ID);

            // Assert
            Assert.AreEqual(250000, test.AnnualVisitorCount);
        }

        // ----------------------------------------------------
        // FROM HERE DOWN IS NEW TESTS MADE BY ADAM ON SATURDAY
        // ----------------------------------------------------

        [TestMethod]
        public void GetAvailabilityFromPark_DateIsAvailable()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);

            int iD = 0;
            DateTime start = new DateTime(2000, 10, 01);
            DateTime end = new DateTime(2000, 10, 10);

            IList<Campsite> campsite = dal.ParkAvailability(iD, start, end);


            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailabilityFromPark_ReturnsCount1()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.ParkAvailability(0, new DateTime(2018, 06, 30), new DateTime(2018, 07, 21));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailabilityFromPark_ReturnsCount0()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.ParkAvailability(0, new DateTime(2018, 07, 28), new DateTime(2018, 07, 30));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailabilityFromPark_ReturnsCount0Again()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.ParkAvailability(0, new DateTime(2018, 07, 21), new DateTime(2018, 07, 23));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailabilityFromPark_ReturnsCount0Again1()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.ParkAvailability(0, new DateTime(2018, 07, 21), new DateTime(2018, 07, 22));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailabilityFromPark_ReturnsCount0Again2()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.ParkAvailability(0, new DateTime(2018, 07, 29), new DateTime(2018, 07, 30));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }

        [TestMethod]
        public void GetAvailabilityFromPark_ReturnsCount0Again3()
        {
            // Arrange
            ParkSqlDAL dal = new ParkSqlDAL(ConnectionString);
            IList<Campsite> campsite = dal.ParkAvailability(0, new DateTime(2018, 07, 21), new DateTime(2018, 07, 30));

            // Assert
            Assert.AreEqual(0, campsite.Count);
        }
    }
}
