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
            Campsite site = new Campsite();
            site.Campground_Id = CampgroundId;
            site.HasUtilities = true;
            site.IsAccessible = true;
            site.Max_Occupancy = 20;
            site.Max_RV_Length = 0;
            site.Site_Id = CampsiteId;
            site.Site_Number = 2;

            List<Campsite> campsites = new List<Campsite>();
            int initialCount = campsites.Count;

            // Act
            dal.ReserveCampsite(site, new DateTime(2018, 09, 01), new DateTime(2018, 09, 08), "Cray-Smith");
            List<Campsite> remaining = new List<Campsite>();
            int remainingCount = remaining.Count;


            // Assert
            Assert.AreEqual(initialCount + 1, remainingCount);
        }
    }
}
