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
            this.GetDbSet<Room>().Remove(model);
            UnitOfWork.SaveChanges();
        }
    }
}
