using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProjectCLI cli = new ProjectCLI();
            cli.RunCLI();
        }
    }
}
