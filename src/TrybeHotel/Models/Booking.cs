namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking {
  [Key]
  public int BookingId {get; set;}
  public DateTime CheckIn {get; set;}
  public DateTime CheckOut {get; set;}
  public int GuestQuant {get; set;}

  [ForeignKey("UserId")]
  public int UserId {get; set;}
  public virtual User? User {get; set;}

  [ForeignKey("RoomId")]
  public int RoomId {get; set;}
  public virtual Room? Room {get; set;}
}