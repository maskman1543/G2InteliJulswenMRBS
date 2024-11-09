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

        [Required(ErrorMessage = "Purpose is required.")]
        public string Purpose { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
