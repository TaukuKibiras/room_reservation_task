using Microsoft.EntityFrameworkCore;
using Reservation.ServiceLibrary.Entities;

namespace Reservation.ServiceLibrary
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> User { get; set; }

        public DbSet<MeetingRoomEntity> MeetingRoom { get; set; }

        public DbSet<ReservationEntity> Reservation { get; set; }
    }
}
