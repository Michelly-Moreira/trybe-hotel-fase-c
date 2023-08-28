using System.Net;
using System.Net.Http;
using TrybeHotel.Dto;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Services
{
    public class GeoService : IGeoService
    {
         private readonly HttpClient _client;
        public GeoService(HttpClient client)
        {
            _client = client;
        }

        // 11. Desenvolva o endpoint GET /geo/status
        public async Task<object> GetGeoStatus()
        {
            // Fazendo requisição
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://nominatim.openstreetmap.org/status.php?format=json");
            
            // adicionando o header
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("User-Agent", "nome-do-software-que-faz-a-requisição");

            // Recebendo a resposta
            var response = await _client.SendAsync(requestMessage);
            var result = await response.Content.ReadFromJsonAsync<object>();
            
            switch (response.IsSuccessStatusCode)
            {
                case true:
                return result!;
                default:
                return default(Object)!;
            }      
        }
        
        // 12. Desenvolva o endpoint GET /geo/address
        // deve trazer latitude e longitude de um determinado endereço;
        public async Task<GeoDtoResponse> GetGeoLocation(GeoDto geoDto)
        {
            // Fazendo requisição
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://nominatim.openstreetmap.org/search?street={geoDto.Address}&city={geoDto.City}&country=Brazil&state={geoDto.State}&format=json&limit=1");
            
            // adicionando o header
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("User-Agent", "aspnet-user-agent");

            // Recebendo a resposta
            var response = await _client.SendAsync(requestMessage);
            var result = await response.Content.ReadFromJsonAsync<List<GeoDtoResponse>>();
            
            if (result!.Count() > 0)
            {
                    var geoResponse = new GeoDtoResponse
                    {
                        lat = result![0].lat,
                        lon = result[0].lon,
                    };
                    return geoResponse;
            }
                    return default(GeoDtoResponse)!;

        }

        // 12. Desenvolva o endpoint GET /geo/address
        // obtem a distância entre o endereço informado e cada hotel registrado no sistema;
        public async Task<List<GeoDtoHotelResponse>> GetHotelsByGeo(GeoDto geoDto, IHotelRepository repository)
        {
            //usar a função GetGeoLocation para pegar a longitude e latitude do geoDto(endereço recebido através do usuário)
            var geoLocationUser = await GetGeoLocation(geoDto);

            //usar a função GetGeoLocation para pegar a longitude e latitude de cada hotel
            var hotelsData = new List<GeoDto>();
             var hotelsWithDistance = new List<GeoDtoHotelResponse>();
            var hotels = repository.GetHotels();
            foreach (var hotel in hotels)
            {
                var addressHotel = new GeoDto
                {
                    Address = hotel.Address,
                    City = hotel.CityName,
                    State = hotel.State,
                };
                        var geoLocationHotel = await GetGeoLocation(addressHotel);

                        //calcular a distância entre o usuário e cada hotel
                        int distance = CalculateDistance(
                        geoLocationUser.lat!,
                        geoLocationUser.lon!,
                        geoLocationHotel.lat!,
                        geoLocationHotel.lon!
                    );
                    //traz todos os hoteis e suas distâncias (do usuario até cada hotel)
                   
                    var hotelsResponse = new GeoDtoHotelResponse
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityName = hotel.CityName,
                    State = hotel.State,
                    Distance = distance,
                };
                hotelsWithDistance.Add(hotelsResponse);
            }
             //ordena os hoteis por ordem crescente da distância
                var sortedHotels = hotelsWithDistance.OrderBy(h => h.Distance).ToList();
            //Não sei porque não retorna o resultado!
            return sortedHotels;
        }

       
        // calcula a distância entre duas posições
        // cada posição terá sua latitude e longitude
        public int CalculateDistance (string latitudeOrigin, string longitudeOrigin, string latitudeDestiny, string longitudeDestiny) {
            double latOrigin = double.Parse(latitudeOrigin.Replace('.',','));
            double lonOrigin = double.Parse(longitudeOrigin.Replace('.',','));
            double latDestiny = double.Parse(latitudeDestiny.Replace('.',','));
            double lonDestiny = double.Parse(longitudeDestiny.Replace('.',','));
            double R = 6371;
            double dLat = radiano(latDestiny - latOrigin);
            double dLon = radiano(lonDestiny - lonOrigin);
            double a = Math.Sin(dLat/2) * Math.Sin(dLat/2) + Math.Cos(radiano(latOrigin)) * Math.Cos(radiano(latDestiny)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
            double distance = R * c;
            return int.Parse(Math.Round(distance,0).ToString());
        }

        public double radiano(double degree) {
            return degree * Math.PI / 180;
        }

    }
}