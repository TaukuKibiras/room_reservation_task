using Microsoft.EntityFrameworkCore;
using Reservation.ServiceLibrary.Entities;

namespace Reservation.ServiceLibrary.Repositories
{
    public class ReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReservationEntity>> GetAll()
        {
            return await _context.Reservation
                .Include(r => r.MeetingRoom)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<ReservationEntity?> GetById(int id)
        {
            return await _context.Reservation
                .Include(r => r.MeetingRoom)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReservationEntity> Add(ReservationEntity reservation)
        {
            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(ReservationEntity reservation)
        {
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasOverlappingReservation(int meetingRoomId, DateTime startTime, DateTime endTime, int excludeReservationId = 0)
        {
            return await _context.Reservation.AnyAsync(r =>
                r.MeetingRoomId == meetingRoomId &&
                r.Id != excludeReservationId &&
                r.Status == ReservationStatus.Booked &&
                r.StartTime < endTime &&
                r.EndTime > startTime);
        }
    }
}
