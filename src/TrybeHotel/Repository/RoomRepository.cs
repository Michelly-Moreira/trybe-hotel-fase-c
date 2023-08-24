using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 7. Refatore o endpoint GET /room
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
           var listRooms = _context.Rooms;
            var listHotels = _context.Hotels;
            var listCities = _context.Cities;

            var allRooms = from room in listRooms
            join hotel in listHotels on room.HotelId equals hotel.HotelId
            join city in listCities on hotel.CityId equals city.CityId
            where hotel.HotelId == HotelId
            select new RoomDto {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = city.CityId,
                    CityName = city.Name
                }
            };
            return allRooms;
        }

        // 8. Refatore o endpoint POST /room
        public RoomDto AddRoom(Room room) {
            var allRooms = _context.Rooms;
            allRooms.Add(room);
            _context.SaveChanges();

            var listHotels = _context.Hotels;
            var listCities = _context.Cities;

            var newCity = from aroom in allRooms
            join hotel in listHotels on aroom.HotelId equals hotel.HotelId
            join city in listCities on hotel.CityId equals city.CityId
            where aroom.RoomId == room.RoomId
            select new RoomDto
            {
                RoomId = aroom.RoomId,
                Name = aroom.Name,
                Capacity = aroom.Capacity,
                Image = aroom.Image,
                Hotel = new HotelDto
                {
                    HotelId = room.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = hotel.CityId,
                    CityName = city.Name 
                } 
            };
            return newCity.Last();
        }

        public void DeleteRoom(int RoomId) {
           var room = _context.Rooms.Find(RoomId);
            if(room != null){
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }else{
                Console.WriteLine("room does not exist");
            }
        }
    }
}