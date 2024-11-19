 using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    

    public class RoomService: IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public RoomService(IRoomRepository roomRepository, IMapper mapper, IConfiguration configuration)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _config = configuration;
        }
        public void AddRoom(RoomViewModel model, string userId)
        {
            var newRoom = new Room();
            _mapper.Map(model, newRoom);
            newRoom.Createdby = userId;
            newRoom.Updatedby = userId;
            newRoom.CreatedTime = DateTime.Now;
            newRoom.UpdatedTime = DateTime.Now;
            newRoom.IsDeleted = false;

            _roomRepository.AddRoom(newRoom);
        }
   
        public IEnumerable<RoomViewModel> RetrieveActiveRooms()
        {
            // Retrieve all users and apply the necessary filtering
            var rooms = _roomRepository.RetrieveAll()
                                       .Where(room => room.IsDeleted != true)
                                       .Select(room => new RoomViewModel
                                       {
                                           Id = room.Id,
                                           RoomName = room.RoomName,
                                           Capacity = room.Capacity,
                                           Location = room.Location,
                                           Equipment = room.Equipment,
                                       });

            return rooms;
        }
        public RoomViewModel RetrieveRoom(int Id)
        {
            var room = _roomRepository.RetrieveAll().Where(x => x.Id.Equals(Id)).Select(s => new RoomViewModel
            {
                Id = s.Id,
                RoomName = s.RoomName,
                Capacity = s.Capacity,
                Location = s.Location,
                Equipment = s.Equipment,
            }).FirstOrDefault();

            return room;
        }
        public void UpdateRoom(RoomViewModel model, string userId)
        {
            var room = _roomRepository.RetrieveAll().Where(x => x.Id.Equals(model.Id)).FirstOrDefault(); 
            if (room != null)
            {
                _mapper.Map(model, room);
                room.UpdatedTime = DateTime.Now;
                room.Updatedby = userId;

                _roomRepository.UpdateRoom(room);
            }
        }
        public void DeleteRoom(int Id)
        {
            var room = _roomRepository.RetrieveAll().FirstOrDefault(x => x.Id == Id);
            if (room != null)
            {
                _roomRepository.DeleteRoom(room);
            }
        }
        public List<RoomViewModel> GetRoomsBySearchTerm(string term)
        {
            var rooms = _roomRepository.SearchRooms(term);

            return rooms.Select(r => new RoomViewModel
            {
                Id = r.Id,
                RoomName = r.RoomName,
                Capacity = r.Capacity,
                Equipment = r.Equipment,
            }).ToList();
        }
    }
}
