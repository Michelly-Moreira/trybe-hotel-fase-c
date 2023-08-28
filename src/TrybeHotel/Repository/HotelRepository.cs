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
                State = city.State,
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
            orderby oneHotel.HotelId
            select new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = city.CityId,
                CityName = city.Name,
                State = city.State,
            };
            return newHotel.Last();
        }

        /* public void DeleteHotel(int HotelId) {
           var hotel = _context.Hotels.Find(HotelId);
            if(hotel != null){
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
            }else{
                Console.WriteLine("This hotel does not exist");
            }
        } */
    }
}