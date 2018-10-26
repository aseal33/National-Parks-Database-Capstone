using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Tests
{
    [TestClass]
     public class ReservationTests : ParkDB_Tests
    {
        [TestMethod]
        public void MakeaReservation_ReturnsCount()
        {
            // Arrange
            ReservationSqlDAL dal = new ReservationSqlDAL(ConnectionString);           
            int initialCount = dal.CountReservations(CampsiteId);

            // Act
            dal.ReserveCampsite(CampsiteId, new DateTime(2018, 09, 01), new DateTime(2018, 09, 08), "Cray-Smith");
            int newCount = dal.CountReservations(CampsiteId);

            // Assert
            Assert.AreEqual(initialCount + 1, newCount);
        }
    }
}
