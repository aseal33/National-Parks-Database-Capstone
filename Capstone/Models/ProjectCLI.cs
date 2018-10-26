using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class ProjectCLI
    {

        // CampgroundAvailability returns ILIST of Camp Sites
        // Keep date range
        // reserve campsite - changing

        // PARKS
        public IList<Park> AllParks;
        //const string Level_Top = "P1";
        const string Level_Parks = "A";
        const string Command_GetAllParks = "1"; // in park // park id and park name 
        const string Command_GetParkInfo = "2"; // in park // returns park - give int park id

        // CAMPGROUNDS 
        const string Level_Campgrounds = "B";
        const string Command_GetAllCampgroundsFromPark = "1"; // in campground 
        const string Command_GetCampgroundAvailability = "2"; // in campground

        // CAMPGROUND
        const string Level_Campground = "C";
        const string Command_GetCampsitesFromCampground = "1"; // in campsite
        const string Command_ChooseCampground = "2";
        const string Command_BackToCampgrounds = "3";

        // RESERVATION
        const string Level_Reservation = "D";
        const string Command_ChooseCampsite = "1";
        const string Command_ReserveCampsite = "2";

        const string Command_Quit = "Q";
        const string DatabaseConnection = @"Data Source =.\sqlexpress;Initial Catalog = NPCampsite; Integrated Security = True";

        private string Level_Current;

        public void RunCLI()
        {
            this.Level_Current = Level_Parks;
            string command;

            while (true)
            {
                if (this.Level_Current == Level_Parks)
                {
                    this.PrintHeader();
                    this.GetAllParks_View();

                    // See which park they want to see
                    int chosenPark = CLIHelper.GetInteger("Which park would you like to visit?");

                    // see that park
                    this.GetPark_View(chosenPark);

                    // Ask what they want to do next
                    this.Park_View_AskNext();
                    command = Console.ReadLine();

                    switch (command)
                    {
                        case Command_GetAllCampgroundsFromPark:
                            this.Level_Current = Level_Campgrounds;
                            continue;
                        case Command_GetCampgroundAvailability:
                            this.Level_Current = Level_Campground;
                            continue;
                        case "3":
                            this.Level_Current = Level_Parks;
                            continue;
                    }

                    if(this.Level_Current == Level_Campgrounds)
                    {
                        this.PrintHeader();

                    }
                }
            }
        }

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

        private void Park_View_AskNext()
        {
            Console.WriteLine("Select a Command:");
            this.PrintOption("1", "View Campgrounds");
            this.PrintOption("2", "Search for a Reservation.");
            this.PrintOption("3", "Return to previous screen.");
        }

        private void PrintOption (string choice, string text)
        {
            Console.WriteLine($" {choice} - ".PadRight(8) + text );
        }

        private void PrintInfo(string text, string description)
        {
            Console.WriteLine($" {text}:".PadRight(20) + description);
        }

        private void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine("Welcome to our scenic reservation system");
            Console.WriteLine();
            switch (this.Level_Current)
            {
                case Level_Parks:
                    Console.WriteLine("Select a park for further details");
                    break;
                case Level_Campgrounds:
                    Console.WriteLine("Park Campgrounds");
                    break;
                case Level_Campground:
                    Console.WriteLine(" 3 - Employee search by first and last name");
                    break;
                case Level_Reservation:
                    Console.WriteLine(" 4 - Get employees without projects");
                    break;
            }
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
