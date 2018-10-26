using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectCLI cli = new ProjectCLI();
            cli.RunCLI();
        }
    }
}
