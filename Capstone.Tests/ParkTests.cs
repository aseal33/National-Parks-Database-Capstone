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
    }
}
