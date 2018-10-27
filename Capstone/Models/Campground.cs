using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int Campground_Id { get; set; }

        public int Park_Id { get; set; }

        public string Name { get; set; }

        public decimal Daily_Fee { get; set; }

        public int Opening_Month { get; set; }

        public int Closing_Month { get; set; }
    }
}
