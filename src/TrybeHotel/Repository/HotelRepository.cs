using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        //  5. Refatore o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            var listHotels = _context.Hotels;
            var listCities = _context.Cities;

            var allHotels = from hotel in listHotels
            from city in listCities
            where hotel.CityId == city.CityId
            select new HotelDto {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = city.Name,
            };
            return allHotels;
        }

        // 6. Refatore o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
           var listCities = _context.Cities;
            var listHotels = _context.Hotels;
            listHotels.Add(hotel);
            _context.SaveChanges();
            
            var newHotel = from oneHotel in listHotels
            join city in listCities on oneHotel.CityId equals city.CityId
            where oneHotel.HotelId == hotel.HotelId
            select new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = city.CityId,
                CityName = city.Name
            };
            return newHotel.Last();
        }
    }
}