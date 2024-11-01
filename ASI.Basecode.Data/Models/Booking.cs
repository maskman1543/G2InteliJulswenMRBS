using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public string UserId { get; set; }
        public int? RoomId { get; set; }
        public string BookingName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
