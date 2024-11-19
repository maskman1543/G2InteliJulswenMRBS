using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IRoomRepository
    {
        void AddRoom(Room model);
        IEnumerable<Room> RetrieveAll();
        void UpdateRoom(Room model);
        void DeleteRoom(Room room);

        // Add this new method for searching rooms
        IEnumerable<Room> SearchRooms(string term);
    }
}
