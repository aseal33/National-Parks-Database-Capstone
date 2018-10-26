using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Capstone.Models
{
    public class ProjectCLI
    {

        // CampgroundAvailability returns ILIST of Camp Sites
        // Keep date range
        // reserve campsite - changing

        // PARKS
        private IList<Park> AllParks;
        private const string Level_Parks = "A";
        private const string Command_GetAllParks = "1"; // in park // park id and park name 
        // private const string Command_GetParkInfo = "2"; // in park // returns park - give int park id
        private const string Command_GetParkAvailability = "2";
        private const string Command_BackToParks = "3";
        private const int NoChosenPark = 0;
        private int ChosenParkID = 0;

        // CAMPGROUNDS 
        private const string Level_Campgrounds = "B";
        private const string Command_GetAllCampgroundsFromPark = "1"; // in campground

        // CAMPGROUND
        private const string Level_Campground = "C";
        private const string Command_GetCampgroundAvailability = "1"; // in campground
        private const string Command_GetCampsitesFromCampground = "2"; // in campsite
        //private const string Command_ChooseCampground = "2";
        private const string Command_BackToCampgrounds = "3";

        // RESERVATION
        private const string Level_Reservation = "D";
        private const string Command_ChooseCampsite = "1";
        private const string Command_ReserveCampsite = "2";

        private const string Command_Quit = "Q";
        public const string DatabaseConnection = @"Data Source =.\sqlexpress;Initial Catalog = NPCampsite; Integrated Security = True";

        private string Level_Current;

        public void RunCLI()
        {
            this.Level_Current = Level_Parks;
            string command;

            // THE LEVEL MASTER ///////////////////////////////////////////
            while (true)
            {
                // LEVEL: PARKS ///////////////////////////////////////////////
                if (this.Level_Current == Level_Parks)
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to our scenic reservation system");
                    Console.WriteLine("Select a park for further details");
                    this.GetAllParks_View();

                    // See which park they want to see
                    this.ChosenParkID = CLIHelper.GetInteger("Which park would you like to visit?");

                    // see that park
                    this.GetPark_View(this.ChosenParkID);

                    // Ask what they want to do next
                    this.Park_View_AskNext();
                    command = Console.ReadLine();

                    switch (command)
                    {
                        case Command_GetAllCampgroundsFromPark:
                            this.Level_Current = Level_Campgrounds;
                            continue;
                        case Command_GetParkAvailability:
                            this.GetParkAvailability_View();
                            Console.ReadLine();
                            continue;
                        case Command_BackToParks:
                            this.Level_Current = Level_Parks;
                            Console.ReadLine();
                            break;
                        default:
                            return;
                    }
                }

                // LEVEL: CAMPGROUNDS /////////////////////////////////////////
                if (this.Level_Current == Level_Campgrounds)
                {
                    // We should always have a chosen park when we get here.
                    // If we don't, kick us back up to parks
                    Console.Clear();
                    if (this.ChosenParkID == NoChosenPark)
                    {
                        this.Level_Current = Level_Parks;
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Park Campgrounds");
                        this.GetAllCampgrounds_View(this.ChosenParkID);
                        this.Campgrounds_View_AskNext();
                        command = Console.ReadLine();

                        switch (command)
                        {
                            case Command_GetCampgroundAvailability:
                                this.Level_Current = Level_Campground;
                                continue;
                            case Command_GetCampsitesFromCampground:

                                continue;
                            default:
                                return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        // LEVEL: PARKS ///////////////////////////////////////////////
        private void GetAllParks_View()
        {
            ParkSqlDAL dal = new ParkSqlDAL(DatabaseConnection);
            this.AllParks = dal.GetAllParks();

            if (this.AllParks.Count > 0)
            {
                foreach (Park park in this.AllParks)
                {
                    this.PrintOption(park.Park_Id.ToString(), park.Name);
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        private void GetPark_View(int chosenPark)
        {
            Console.Clear();
            ParkSqlDAL dal = new ParkSqlDAL(DatabaseConnection);
            Park park = dal.GetParkInfo(chosenPark);
            Console.WriteLine(park.Name);
            this.PrintInfo("Location", park.Location);
            this.PrintInfo("Established", park.EstablishedDate.ToShortDateString());
            this.PrintInfo("Area", park.Area.ToString("#,# sq km"));
            this.PrintInfo("Annual Visitors", park.AnnualVisitorCount.ToString("#,#"));
            Console.WriteLine();
            Console.WriteLine(park.Description);
            Console.WriteLine();
        }

        private void GetParkAvailability_View()
        {
            // ParkAvailability
            DateTime startDate = CLIHelper.GetDateTime("What is the arrival date? mm/dd/yyyy");
            DateTime endDate = CLIHelper.GetDateTime("What is the departure date? mm/dd/yyyy");

            ParkSqlDAL parkDAL = new ParkSqlDAL(DatabaseConnection);
            IList<Campsite> availableCampsites = parkDAL.ParkAvailability(this.ChosenParkID, startDate, endDate);
            int lengthOfStay = (int)(endDate - startDate).TotalDays;

            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL(DatabaseConnection);
<<<<<<< HEAD
            IList<Campground> campgroundsList = campgroundDAL.GetCampgroundsFromPark(this.ChosenParkID);
            Dictionary<int, Campground> campgroundDict = this.ListToDict(campgroundsList);

            if (availableCampsites.Count > 0)
            {
                decimal cost = 0;
                decimal fee = 0;
                Console.WriteLine(
                    "Campground".PadRight(30)
                    + "Site No.".PadRight(15)
                    + "Max Occup.".ToString().PadRight(15)
                    + "Accessible".PadRight(15)
                    + "RV Len".PadRight(15)
                    + "Utility".PadRight(15)
                    + "Cost".PadLeft(20));

                foreach (Campsite campsite in availableCampsites)
                {
                    fee = campgroundDict[campsite.Campground_Id].Daily_Fee;
                    cost = fee * lengthOfStay;
                    this.PrintCampsiteAvailability(
                        campgroundDict[campsite.Campground_Id].Name,
                        campsite.Site_Number,
                        campsite.Max_Occupancy,
                        campsite.IsAccessible,
                        campsite.Max_RV_Length,
                        campsite.HasUtilities,
                        cost);
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
=======
            IList<Campground> campgrounds = campgroundDAL.GetCampgroundsFromPark(this.ChosenParkID);
            
            //if (availableCampsites.Count > 0)
            //{
            //    decimal cost = 0;
            //    decimal fee = 0;
            //    foreach (Campsite campsite in availableCampsites)
            //    {
            //        fee = campgrounds[campground_id].Daily_Fee;
            //        cost = fee * lengthOfStay;
            //        this.PrintCampsiteAvailability(
            //            campgrounds[campsite.Campground_Id].Name,
            //            campsite.Site_Number,
            //            campsite.Max_Occupancy,
            //            campsite.IsAccessible,
            //            campsite.Max_RV_Length,
            //            campsite.HasUtilities,
            //            cost);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("**** NO RESULTS ****");
            //}
>>>>>>> 2e1a5c484ce8f40cd19d60864165190de89b3e1f
        }

        private Dictionary<int, Campground> ListToDict(IList<Campground> campgroundsList)
        {
            Dictionary<int, Campground> output = new Dictionary<int, Campground>();

            foreach (Campground campground in campgroundsList)
            {
                output.Add(campground.Campground_Id, campground);
            }

            return output;
        }

        private void PrintCampsiteAvailability(string campgroundName, int site_Id, int max_Occupancy, bool isAccessible, int max_RV_Length, bool hasUtilities, decimal cost)
        {
            string acessable = isAccessible ? "Yes" : "No";
            string rvLength = max_RV_Length > 0 ? max_RV_Length.ToString() : "N/A";
            string utilities = hasUtilities ? "Yes" : "N/A";

            Console.WriteLine(
                campgroundName.PadRight(30)
                + site_Id.ToString().PadRight(15)
                + max_Occupancy.ToString().PadRight(15)
                + acessable.PadRight(15)
                + rvLength.PadRight(15)
                + utilities.PadRight(15)
                + cost.ToString("C").PadLeft(20));
        }

        private void Park_View_AskNext()
        {
            Console.WriteLine("Select a Command:");
            this.PrintOption(Command_GetAllCampgroundsFromPark, "View Campgrounds");
            this.PrintOption(Command_GetParkAvailability, "Search for a Reservation.");
            this.PrintOption(Command_BackToParks, "Return to previous screen.");
        }

        // LEVEL: CAMPGROUNDS /////////////////////////////////////////
        private void GetAllCampgrounds_View(int parkID)
        {
            CampgroundSqlDAL dal = new CampgroundSqlDAL(DatabaseConnection);
            Console.WriteLine(
                " ".PadRight(10)
                + "Name".PadRight(40)
                + "Open".PadRight(15)
                + "Close".PadRight(15)
                + "Daily Fee".PadLeft(15));

            IList<Campground> campgrounds = dal.GetCampgroundsFromPark(parkID);

            if (campgrounds.Count > 0)
            {
                foreach (Campground campground in campgrounds)
                {
                    this.PrintCampground(campground.Campground_Id, campground.Name, campground.Opening_Month, campground.Closing_Month, campground.Daily_Fee);
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        private void Campgrounds_View_AskNext()
        {
            Console.WriteLine("Select a Command:");
            //this.PrintOption("1", "Print campground info again.");
            this.PrintOption("2", "Search for sites available to reserve.");
            this.PrintOption("3", "Return to previous screen.");
        }

        // Campground
        private void GetCampgroundAvailability_View()
        {

        }

        // Pretty Printing
        private void PrintOption(string choice, string text)
        {
            Console.WriteLine($" {choice} - ".PadRight(8) + text);
        }

        private void PrintInfo(string text, string description)
        {
            Console.WriteLine($" {text}:".PadRight(20) + description);
        }

        private void PrintCampground(int id, string name, int open, int close, decimal daily_fee)
        {
            string openMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(open);
            string closeMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(close);
            Console.WriteLine(
                $" #{id}:".PadRight(10)
                + name.PadRight(40)
                + openMonth.PadRight(15)
                + closeMonth.PadRight(15)
                + daily_fee.ToString("C").PadLeft(15));
        }

        private void PrintFooter()
        {
            this.PrintOption("Q", "Quit");
        }

        private void PrintMenu(int level)
        {

        }

    }
}
