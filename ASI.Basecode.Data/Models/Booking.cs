﻿using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public string Username { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

        public virtual Room Room { get; set; }
    }
}
