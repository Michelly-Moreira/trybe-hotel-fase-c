using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{HotelId}")]
        public IActionResult GetRoom(int HotelId){
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult PostRoom([FromBody] Room room){
            throw new NotImplementedException();
        }

        [HttpDelete("{RoomId}")]
        public IActionResult Delete(int RoomId)
        {
             throw new NotImplementedException();
        }
    }
}