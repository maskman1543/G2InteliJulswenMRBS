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
        public RoomService(IRoomRepository bookRepository, IMapper mapper, IConfiguration configuration)
        {
            _roomRepository = bookRepository;
            _mapper = mapper;
            _config = configuration;
        }
        public void AddRoom(RoomViewModel model, string UserId)
        {
            //var coverImagePath = PathManager.DirectoryPath.CoverImagesDirectory;
            var newRoom = new Room();
            _mapper.Map(model, newRoom);
            newRoom.Createdby = UserId;
            newRoom.Updatedby = UserId;
            newRoom.CreatedTime = DateTime.Now;
            newRoom.UpdatedTime = DateTime.Now;
            //newRoom.RoomId = Guid.NewGuid().ToString();
            //var coverImageFileName = Path.Combine(coverImagePath, newBook.BookId  ".png");
            //using (var fileStream = new FileStream(coverImageFileName, FileMode.Create))
            //{
            //    model.ImageFile.CopyTo(fileStream);
            //}
            _roomRepository.AddRoom(newRoom);
        }
        public List<RoomViewModel> RetrieveAll()
        {
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _roomRepository.RetrieveAll().Select(s => new RoomViewModel
            {
                 Id = s.Id,
                 RoomName = s.RoomName,
                 Capacity = s.Capacity,
                 Location = s.Location,
                 Equipment = s.Equipment,
                 price = s.Price,


            }).ToList();

            return data;
        }
        public RoomViewModel RetrieveRoom(int RoomId)
        {
            var room = _roomRepository.RetrieveAll().Where(x => x.Id.Equals(RoomId)).Select(s => new RoomViewModel
            {
                Id = s.Id,
                RoomName = s.RoomName,
                Capacity = s.Capacity,
                Location = s.Location,
                Equipment = s.Equipment,
                price = s.Price,
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
            var room = _roomRepository.RetrieveAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            if (room != null)
            {
                _roomRepository.DeleteRoom(room);
            }
        }
    }
}
