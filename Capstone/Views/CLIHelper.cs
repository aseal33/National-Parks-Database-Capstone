﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class CLIHelper
    {
        public static DateTime GetDateTime(string message)
        {
            string userInput = string.Empty;
            DateTime dateValue = DateTime.MinValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid date format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                CheckQuit(userInput);
                numberOfAttempts++;
            }
            while (!DateTime.TryParse(userInput, out dateValue));

            return dateValue;
        }

        public static int GetInteger(string message)
        {
            string userInput = string.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                Console.Write(userInput);
                CheckQuit(userInput);
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;
        }

        public static double GetDouble(string message)
        {
            string userInput = string.Empty;
            double doubleValue = 0.0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                CheckQuit(userInput);
                numberOfAttempts++;
            }
            while (!double.TryParse(userInput, out doubleValue));

            return doubleValue;
        }

        public static bool GetBool(string message)
        {
            string userInput = string.Empty;
            bool boolValue = false;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                CheckQuit(userInput);
                numberOfAttempts++;
            }
            while (!bool.TryParse(userInput, out boolValue));

            return boolValue;
        }

        public static string GetString(string message)
        {
            string userInput = string.Empty;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                CheckQuit(userInput);
                numberOfAttempts++;
            }
            while (string.IsNullOrEmpty(userInput));

            return userInput;
        }

        private static void CheckQuit(string userInput)
        {
            if (userInput.ToUpper() == "q")
            {
                Console.WriteLine("Thank you for using the campground system.");
                Environment.Exit(0);
            }
        }
    }
}
