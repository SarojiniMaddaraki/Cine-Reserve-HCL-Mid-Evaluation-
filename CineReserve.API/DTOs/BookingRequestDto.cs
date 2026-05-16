namespace CineReserve.API.DTOs
{
    public class BookingRequestDto
    {
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public List<SeatDto> Seats { get; set; }
    }

    public class SeatDto
    {
        public string Row { get; set; }
        public int Number { get; set; }
    }
}