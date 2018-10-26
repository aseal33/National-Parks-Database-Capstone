using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ParkDB_Tests
    {
        public const string ConnectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=NPCampsite; Integrated Security=True";
        TransactionScope transaction;

        public int ParkId;
        public int CampgroundId;
        public int CampsiteId;
        public int ReservationId;

        [TestInitialize]
        public void Initialize()
        {
            // BEGIN TRANSACTION
            transaction = new TransactionScope();

            // Read SQL from database.sql
            string sql = File.ReadAllText("database.sql");

            // Execute SQL against live database
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ParkId = Convert.ToInt32(reader["park"]);
                    CampgroundId = Convert.ToInt32(reader["campground"]);
                    CampsiteId = Convert.ToInt32(reader["campsite"]);
                    ReservationId = Convert.ToInt32(reader["reservation"]);
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // ROLLBACK TRANSACTION
            transaction.Dispose();

            // COMMIT TRANSACTION
            // transaction.Complete();
        }

        public int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }
    }
}
