namespace Reservation.ServiceLibrary.Entities
{
    public class ReservationEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int MeetingRoomId { get; set; }
        public MeetingRoomEntity? MeetingRoom { get; set; }

        public int UserId { get; set; }
        public UserEntity? User { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int AttendeesCount { get; set; }

        public ReservationStatus Status { get; set; }
    }

    public enum ReservationStatus
    {
        Booked,
        Completed,
        Cancelled
    }
}
