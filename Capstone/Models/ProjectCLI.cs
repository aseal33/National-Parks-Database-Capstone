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
        //const string Command_GetAllParks = "1"; // in park // park id and park name 
        //const string Command_GetParkInfo = "2"; // in park // returns park - give int park id

        const int Level_Top = 0;

        const int Level_Parks = 1;
        

        const int Level_Campgrounds = 2;
        // CAMPGROUNDS 
        const string Command_GetAllCampgroundsFromPark = "1"; // in campground 
        const string Command_GetCampgroundAvailability = "2"; // in campground


        const int Level_Campground = 3;
        // CAMPGROUND
        const string Command_GetCampsitesFromCampground = "1"; // in campsite
        const string Command_GetCampGroundAvailability = "2";
        const string Command_ChooseCampground = "3";
        const string Command_BackToCampgrounds = "4";

        const int Level_Reservation = 4;
        // RESERVATION
        const string Command_ChooseCampsite = "1";
        const string Command_ReserveCampsite = "2";

        const string Command_Quit = "q";
        const string DatabaseConnection = @"Data Source =.\sqlexpress;Initial Catalog = EmployeeDB; Integrated Security = True";

        public void RunCLI()
        {
            PrintHeader(Level_Top);

            while (true)
            {
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_AllDepartments:
                        GetAllDepartments();
                        break;

                    case Command_AllEmployees:
                        GetAllEmployees();
                        break;

                    case Command_EmployeeSearch:
                        EmployeeSearch();
                        break;

                    case Command_EmployeesWithoutProjects:
                        GetEmployeesWithoutProjects();
                        break;

                    case Command_ProjectList:
                        GetAllProjects();
                        break;

                    case Command_CreateDepartment:
                        CreateDepartment();
                        break;

                    case Command_UpdateDepartment:
                        UpdateDepartment();
                        break;

                    case Command_CreateProject:
                        CreateProject();
                        break;

                    case Command_AssignEmployeeToProject:
                        AssignEmployeeToProject();
                        break;

                    case Command_RemoveEmployeeFromProject:
                        RemoveEmployeeFromProject();
                        break;

                    case Command_Quit:
                        Console.WriteLine("Thank you for using the project organizer");
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;

                }

                PrintMenu();
            }
        }



        private void PrintHeader(int level)
        {
            Console.WriteLine("Welcome to our scenic reservation system");
            Console.WriteLine();
            switch (level)
            {
                case Level_Top:
                    Console.WriteLine("Select a park for further details");
                    break;
                case Level_Parks:
                    Console.WriteLine("Park information");
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
            Console.WriteLine(" Q - Quit");
            Console.WriteLine();
        }

        private void PrintMenu()
        {
            

        }

    }
}
