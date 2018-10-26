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

        public void LevelMaster()
        {
            // We start off on the parks level
            Level_Current = Level_Parks;

            do
            {
                // Which level are we on of the system?
                switch (Level_Current)
                {
                    case Level_Parks:

                        break;
                    case Level_Campgrounds:

                        break;
                    case Level_Campground:

                        break;
                    case Level_Reservation:

                        break;
                    case Command_Quit:
                        Console.WriteLine("Thank you for using the campground system.");
                        return;
                }
                string command = Console.ReadLine();
            } while (true);
        }


        public void RunCLI()
        {
            Level_Current = Level_Parks;
            string command;
            while (true)
            {
                PrintHeader();

                if (Level_Current == Level_Parks)
                {
                    GetAllParks_View();
                    int chosenPark = CLIHelper.GetInteger("Which park would you like to visit?");
                    Console.Clear();
                    GetPark_View(chosenPark);
                    Console.WriteLine("Select a Command:");
                    PrintOption("1", "View Campgrounds");
                    PrintOption("2", "Search for a Reservation.");
                    PrintOption("3", "Return to previous screen.");
                    command = Console.ReadLine();
                    switch(command)
                    {
                        case Command_GetAllCampgroundsFromPark:

                            break;
                        case Command_GetCampgroundAvailability:

                            break;
                        case "3":
                            Level_Current = Level_Parks;
                            continue;
                    }

                }
                
                switch ()
                {
                    
                    

                    case Command_Quit:
                        Console.WriteLine("Thank you for using the campground system.");
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;

                }

                PrintFooter();
            }
        }

        private void GetPark_View(int chosenPark)
        {
            ParkSqlDAL dal = new ParkSqlDAL(DatabaseConnection);
            Park park = dal.GetParkInfo(chosenPark);
            Console.WriteLine(park.Name);
            PrintInfo("Location", park.Location);
            PrintInfo("Established", park.EstablishedDate.ToShortDateString());
            PrintInfo("Area", park.Area.ToString("#,# sq km"));
            PrintInfo("Annual Visitors", park.AnnualVisitorCount.ToString("#,#"));
            Console.WriteLine();
            Console.WriteLine(park.Description);
        }

        private void GetAllParks_View()
        {
            ParkSqlDAL dal = new ParkSqlDAL(DatabaseConnection);
            this.AllParks = dal.GetAllParks();

            if (this.AllParks.Count > 0)
            {
                foreach (Park park in this.AllParks)
                {
                    PrintOption(park.Park_Id.ToString(), park.Name);
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }
        
        private void PrintOption (string choice, string text)
        {
            Console.WriteLine($" {choice} - ".PadRight(8) + text );
        }
        
        private void PrintInfo(string text, string description)
        {
            Console.WriteLine($" {text}:".PadRight(15) + description);
        }

        private void PrintHeader()
        {
            Console.WriteLine("Welcome to our scenic reservation system");
            Console.WriteLine();
            switch (this.Level_Current)
            {
                case Level_Parks:
                    Console.WriteLine("Select a park for further details");
                    break;
                case Level_Campgrounds:
                    Console.WriteLine(" 2 - Show all employees");
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
            PrintOption("Q", "Quit");
        }

        private void PrintMenu(int level)
        {
            

        }

    }
}
