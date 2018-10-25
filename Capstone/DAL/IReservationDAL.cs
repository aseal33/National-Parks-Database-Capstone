﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAL
    {
        int ReserveCampsite(Campsite campsiteRequested, DateTime start_date, DateTime end_date, string partyName);
    }
}
