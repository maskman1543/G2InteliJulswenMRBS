using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string Username { get; set; }

        [Required(ErrorMessage = "Purpose is required.")]
        public string Purpose { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        public TimeSpan EndTime { get; set; }

    }
}
