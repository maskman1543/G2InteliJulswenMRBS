using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Room
    {
        public string Id { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public string Equipment { get; set; }
        public string Price { get; set; }
        public string Createdby { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Updatedby { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
