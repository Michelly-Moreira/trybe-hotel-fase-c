using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
           var listCities = _context.Cities;
            var allCities = from city in listCities
            select new CityDto {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State,
            };
            return allCities;
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            var completsCities = _context.Cities;
            completsCities.Add(city);
            _context.SaveChanges();
            
            var newCity = new CityDto {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State,
            }; 
            return newCity;
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
           throw new NotImplementedException();
        }

    }
}