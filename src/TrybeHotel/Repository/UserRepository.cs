using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var allUsers = _context.Users;
            var existingLogin = from user in allUsers
            where user.Email == login.Email
            where user.Password == login.Password
            select new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
            switch(existingLogin.FirstOrDefault())
            {
                case null:
                return null;
                default:return existingLogin.First();
            }
        }
        public UserDto Add(UserDtoInsert user)
        {
            var allUsers = _context.Users;
            
            var newClient = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client",
            };
            allUsers.Add(newClient);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = newClient.UserId,
                Name = newClient.Name,
                Email = newClient.Email,
                UserType = newClient.UserType,
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
             var allUsers = _context.Users;
            var registeredEmail = from user in allUsers
            where user.Email == userEmail
            select new UserDto
            {
                Email = user.Email,
            };
            switch(registeredEmail.Count())
            {
                case 0:
                return null;
                default:return registeredEmail.First();
            } 
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var allUsers = _context.Users;

           var getAllUsers = from user in allUsers
           select new UserDto
           {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            UserType = user.UserType,
           };
           return getAllUsers;
        }

    }
}