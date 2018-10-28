using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Capstone.Models
{
    public class ProjectCLI
    {
        public const string DatabaseConnection = @"Data Source =.\sqlexpress;Initial Catalog = NPCampsite; Integrated Security = True";

        // CampgroundAvailability returns ILIST of Camp Sites
        // Keep date range
        // reserve campsite - changing

        // PARKS
        private IList<Park> AllParks;
        private const string Level_Parks = "A";
        private const string Command_GetAllParks = "1";
        private const string Level_Park = "B";
        private const string Command_GetParkAvailability = "2";
        private const string Command_BackToParks = "3";
        private const string Command_BackToPark = "2";
        private const int NoChosenPark = -1;
        private int ChosenParkID = NoChosenPark;
        private const int NoChosenCampground = -1;
        private int ChosenCampgroundID = NoChosenCampground;

        // CAMPGROUNDS
        private const string Level_Campgrounds = "C";
        private const string Command_GetAllCampgroundsFromPark = "1";

        // CAMPGROUND
        private const string Level_Campground = "D";
        private const string Command_GetCampsitesFromCampground = "1";
        private const string Command_BackToCampgrounds = "2";

        // RESERVATION
        private const string Level_Reservation = "E";
        private const string Command_ChooseCampsite = "1";
        private const string Command_ReserveCampsite = "2";

        private const string Command_Quit = "Q";

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
                    this.Level_Current = Level_Park;
                }

                // LEVEL: PARK ////////////////////////////////////////////////
                if (this.Level_Current == Level_Park)
                {
                    if (this.ChosenParkID == NoChosenPark)
                    {
                        this.Level_Current = Level_Parks;
                        continue;
                    }

                    // see that park, if it's an invalid park, go back
                    if (this.GetPark_View(this.ChosenParkID).Name == null)
                    {
                        this.Level_Current = Level_Parks;
                        continue;
                    }

                    // Ask what they want to do next
                    this.Park_View_AskNext();
                    command = "0";
                    do
                    {
                        command = CLIHelper.GetString("> ");
                    }
                    while (command != Command_GetAllCampgroundsFromPark && command != Command_GetParkAvailability && command != Command_BackToParks);
                    switch (command)
                    {
                        case Command_GetAllCampgroundsFromPark:
                            this.Level_Current = Level_Campgrounds;
                            continue;
                        case Command_GetParkAvailability:
                            this.GetParkAvailability_View();
                            Console.ReadLine();
                            this.Level_Current = Level_Parks;
                            continue;
                        case Command_BackToParks:
                            this.Level_Current = Level_Parks;
                            continue;
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

                    Console.WriteLine("Park Campgrounds");
                    this.GetAllCampgrounds_View(this.ChosenParkID);
                    this.Campgrounds_View_AskNext();

                    command = "0";
                    do
                    {
                        command = CLIHelper.GetString("> ");
                    }
                    while (command != Command_GetCampsitesFromCampground && command != Command_BackToPark);
                    switch (command)
                    {
                        case Command_GetCampsitesFromCampground:
                            this.Level_Current = Level_Campground;
                            continue;
                        case Command_BackToPark:
                            this.Level_Current = Level_Park;
                            continue;
                    }
                }

                // LEVEL: CAMPGROUND //////////////////////////////////////////
                if (this.Level_Current == Level_Campground)
                {
                    // We should always have a chosen campground when we get here.
                    // If we don't, kick us back up to parks
                    Console.Clear();
                    if (this.ChosenParkID == NoChosenPark)
                    {
                        this.Level_Current = Level_Parks;
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Reserve a Campground");
                        int[] campgroundIDs = this.GetAllCampgrounds_View(this.ChosenParkID);
                        int selectedCampground = -1;
                        while (!campgroundIDs.Contains(selectedCampground) || selectedCampground == 0)
                        {
                            selectedCampground = CLIHelper.GetInteger("Which campground? (enter 0 to cancel) ");
                            if (selectedCampground == 0)
                            {
                                this.Level_Current = Level_Campgrounds;
                                break;
                            }
                            else
                            {
                                this.ChosenCampgroundID = selectedCampground;
                                this.Level_Current = Level_Reservation;
                            }
                        }
                    }
                }

                // LEVEL: RESERVATION /////////////////////////////////////////
                if (this.Level_Current == Level_Reservation)
                {
                    // We should always have a chosen campground when we get here.
                    // If we don't, kick us back up to parks
                    if (this.ChosenCampgroundID == NoChosenCampground)
                    {
                        Console.Clear();
                        this.Level_Current = Level_Parks;
                        continue;
                    }
                    else
                    {
                        DateTime startDate = CLIHelper.GetDateTime("What is the arrival date? mm/dd/yyyy");
                        DateTime endDate = CLIHelper.GetDateTime("What is the departure date? mm/dd/yyyy");

                        int[] campsiteNumbers = this.GetCampgroundAvailability_View(this.ChosenCampgroundID, startDate, endDate);
                        if (campsiteNumbers.Length > 0)
                        {
                            int chosenCampsite = -1;
                            while (!campsiteNumbers.Contains(chosenCampsite) || chosenCampsite == 0)
                            {
                                chosenCampsite = CLIHelper.GetInteger("Which campsite? (enter 0 to cancel) ");
                                if (chosenCampsite == 0)
                                {
                                    this.Level_Current = Level_Campground;
                                    break;
                                }
                            }

                            if (chosenCampsite != 0)
                            {
                                string familyName = CLIHelper.GetString("What name should the reservation be made under? ");
                                int reservationID = this.ReserveCampsite_View(chosenCampsite, startDate, endDate, familyName);
                                Console.WriteLine($"The reservation has been made and the confirmation id is {reservationID}.");
                                Console.Write("Press enter to book another reservation. ");
                                this.Level_Current = Level_Campground;
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            // Invalid campground choice
                            this.Level_Current = Level_Campgrounds;
                        }
                    }
                }

                if (Console.ReadLine().ToLower() == "q")
                {
                    Console.WriteLine("Thank you for using the park reservation system.");
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

        private Park GetPark_View(int chosenPark)
        {
            Console.Clear();
            ParkSqlDAL dal = new ParkSqlDAL(DatabaseConnection);
            Park park = dal.GetParkInfo(chosenPark);
            if (park.Name == string.Empty || park.Name == null)
            {
                return park;
            }

            Console.WriteLine($"[{park.Name}]");
            this.PrintInfo("Location", park.Location);
            this.PrintInfo("Established", park.EstablishedDate.ToShortDateString());
            this.PrintInfo("Area", park.Area.ToString("#,# sq km"));
            this.PrintInfo("Annual Visitors", park.AnnualVisitorCount.ToString("#,#"));
            Console.WriteLine();
            Console.WriteLine(park.Description);
            Console.WriteLine();
            return park;
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
        private int[] GetAllCampgrounds_View(int parkID)
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
                int[] output = new int[campgrounds.Count];
                int i = 0;
                foreach (Campground campground in campgrounds)
                {
                    this.PrintCampground(campground.Campground_Id, campground.Name, campground.Opening_Month, campground.Closing_Month, campground.Daily_Fee);
                    output[i] = campground.Campground_Id;
                    i++;
                }

                return output;
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
                return new int[0];
            }
        }

        private void Campgrounds_View_AskNext()
        {
            Console.WriteLine("Select a Command:");
            this.PrintOption(Command_GetCampsitesFromCampground, "Search for sites available to reserve.");
            this.PrintOption(Command_BackToCampgrounds, "Return to previous screen.");
        }

        // LEVEL: CAMPGROUND  /////////////////////////////////////////
        private int[] GetCampgroundAvailability_View(int campgroundID, DateTime startDate, DateTime endDate)
        {
            CampgroundSqlDAL campDAL = new CampgroundSqlDAL(DatabaseConnection);
            IList<Campsite> availableCampsites = campDAL.CampgroundAvailability(campgroundID, startDate, endDate);
            int lengthOfStay = (int)(endDate - startDate).TotalDays;

            Campground campground = campDAL.CampgroundDetails(campgroundID);

            if (availableCampsites.Count > 0)
            {
                int[] output = new int[availableCampsites.Count];
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

                int i = 0;
                foreach (Campsite campsite in availableCampsites)
                {
                    output[i] = campsite.Site_Number;
                    fee = campground.Daily_Fee;
                    cost = fee * lengthOfStay;
                    this.PrintCampsiteAvailability(
                        campground.Name,
                        campsite.Site_Number,
                        campsite.Max_Occupancy,
                        campsite.IsAccessible,
                        campsite.Max_RV_Length,
                        campsite.HasUtilities,
                        cost);
                    i++;
                }

                return output;
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
                return new int[0];
            }
        }

        // LEVEL: RESERVATION  /////////////////////////////////////////
        //
        //
        //
        private int ReserveCampsite_View(int chosenCampsite, DateTime startDate, DateTime endDate, string partyName)
        {
            int reservationID = 0;
            ReservationSqlDAL dal = new ReservationSqlDAL(DatabaseConnection);

            reservationID = dal.ReserveCampsite(chosenCampsite, startDate, endDate, partyName);

            return reservationID;
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
    }
}
