using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;
        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login){
           var verifyUser = _repository.Login(login);
            switch (verifyUser)
            {
                case null:
                return Unauthorized(new{message = "Incorrect e-mail or password"});
                default:
                return Ok(new{token = new TokenGenerator().Generate(verifyUser)});
            }
        }
    }
}