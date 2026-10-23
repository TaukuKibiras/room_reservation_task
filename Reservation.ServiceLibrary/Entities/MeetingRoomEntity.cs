namespace Reservation.ServiceLibrary.Entities
{
    public class MeetingRoomEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public string Location { get; set; } = string.Empty;

        public string Equipment { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
