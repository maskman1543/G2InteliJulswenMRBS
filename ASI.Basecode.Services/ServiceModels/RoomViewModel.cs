using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Roomname is required.")]
        public string RoomName { get; set; }
        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Equipment is required.")]
        public string Equipment { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public string Price { get; set; }
        
    }
}
