using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IRoomService
    {
        void AddRoom(RoomViewModel model, string userId);
        List<RoomViewModel> RetrieveAll();
        RoomViewModel RetrieveRoom(int RoomId);
        void UpdateRoom(RoomViewModel model, string userId);
        void DeleteRoom(int RoomId);
    }
}
