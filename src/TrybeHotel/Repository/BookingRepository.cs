using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
           var allBookings = _context.Bookings;
            var allUsers = _context.Users;
            var allRooms = _context.Rooms;
            var allHotels = _context.Hotels;
            var allCities = _context.Cities;
            
            var room = allRooms.Find(booking.RoomId);
            if(room?.Capacity >= booking.GuestQuant)
            {
                var user = allUsers.Where(u => u.Email == email).FirstOrDefault();
                Booking bookForAdd = new Booking
                {
                    CheckIn = booking.CheckIn,
                    CheckOut = booking.CheckOut,
                    GuestQuant = booking.GuestQuant,
                    RoomId = booking.RoomId,
                    UserId = user.UserId,
                };

                allBookings.Add(bookForAdd);
                _context.SaveChanges();

                var newBooking = from b in allBookings
                join r in allRooms on b.RoomId equals r.RoomId
                join h in allHotels on r.HotelId equals h.HotelId
                join c in allCities on h.CityId equals c.CityId
                orderby b.BookingId
                select new BookingResponse
                {
                    BookingId = b.BookingId,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    GuestQuant = b.GuestQuant,
                    Room = new RoomDto
                    {
                        RoomId = b.RoomId,
                        Name = r.Name,
                        Capacity = r.Capacity,
                        Image = r.Image,
                        Hotel = new HotelDto
                        {
                            HotelId = r.HotelId,
                            Name = h.Name,
                            Address = h.Address,
                            CityId = h.CityId,
                            CityName = c.Name,
                        }
                    }
                };
                return newBooking.Last();
            }
            else
            {
                return null;
            }
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
            var allBookings = _context.Bookings;
            var allRooms = _context.Rooms;
            var allHotels = _context.Hotels;
            var allCities = _context.Cities;

            var getBooking = from b in allBookings
            join r in allRooms on b.RoomId equals r.RoomId
            join h in allHotels on r.HotelId equals h.HotelId
            join c in allCities on h.CityId equals c.CityId
            where b.BookingId == bookingId
            where b.User.Email == email
            select new BookingResponse
            {
                BookingId = b.BookingId,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                GuestQuant = b.GuestQuant,
                RoomId = b.RoomId,
                Room = new RoomDto
                {
                    RoomId = r.RoomId,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    Image = r.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = h.HotelId,
                        Name = h.Name,
                        Address = h.Address,
                        CityId = h.CityId,
                        CityName = c.Name,
                    }
                }
            };
            switch (getBooking.FirstOrDefault())
            {
                case null:
                return null;
                default:
                return getBooking.First();
            }
        }

        public Room GetRoomById(int RoomId)
        {
             throw new NotImplementedException();
        }

    }

}