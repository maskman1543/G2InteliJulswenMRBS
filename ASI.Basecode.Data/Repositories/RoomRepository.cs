using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;
using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ASI.Basecode.Data.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        public void AddRoom(Room model)
        {
            this.GetDbSet<Room>().Add(model);
            UnitOfWork.SaveChanges();
        }
        public bool RoomNameExists(string roomName)
        {
            return this.GetDbSet<Room>().Any(x => x.RoomName == roomName);
        }

        public bool LocationExists(string location)
        {
            return this.GetDbSet<Room>().Any(x => x.Location == location);
        }

        public IEnumerable<Room> RetrieveAll()
        {
            return this.GetDbSet<Room>();
        }
        public void UpdateRoom(Room model)
        {
            this.GetDbSet<Room>().Update(model);
            UnitOfWork.SaveChanges();
        }
        public void DeleteRoom(Room model)
        {
            model.IsDeleted = true;
            this.GetDbSet<Room>().Update(model);
            UnitOfWork.SaveChanges();
        }

        // New implementation for searching rooms - Camus 
        
        public IEnumerable<Room> SearchRooms(string term)
        {
            return this.GetDbSet<Room>()
                       .Where(r => !r.IsDeleted &&
                                   (r.RoomName.Contains(term) || r.Equipment.Contains(term)))
                       .ToList();
        }
    }
}