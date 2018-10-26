using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;

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
        private int ChosenPark = 0;

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
                    this.ChosenPark = CLIHelper.GetInteger("Which park would you like to visit?");

                    // see that park
                    this.GetPark_View(this.ChosenPark);

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
                            this.Level_Current = Level_Parks;
                            continue;
                        case Command_BackToParks:
                            this.Level_Current = Level_Parks;
                            continue;
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
                    if (this.ChosenPark == NoChosenPark)
                    {
                        Console.WriteLine("Something went wrong.");
                        this.Level_Current = Level_Parks;
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Park Campgrounds");
                        this.GetAllCampgrounds_View(this.ChosenPark);
                        this.Campgrounds_View_AskNext();
                        command = Console.ReadLine();

                        switch (command)
                        {
                            case Command_GetCampgroundAvailability:
                                this.Level_Current = Level_Campground;
                                continue;
                            case Command_GetCampsitesFromCampground:
                                
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
        private void PrintOption (string choice, string text)
        {
            Console.WriteLine($" {choice} - ".PadRight(8) + text );
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
