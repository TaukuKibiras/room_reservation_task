using Microsoft.EntityFrameworkCore;
using Reservation.ServiceLibrary.Entities;

namespace Reservation.ServiceLibrary.Repositories
{
    public class MeetingRoomRepository
    {
        private readonly AppDbContext _context;

        public MeetingRoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MeetingRoomEntity>> GetAll()
        {
            return await _context.MeetingRoom.ToListAsync();
        }

        public async Task<MeetingRoomEntity?> GetById(int id)
        {
            return await _context.MeetingRoom.FindAsync(id);
        }

        public async Task<MeetingRoomEntity> Add(MeetingRoomEntity meetingRoom)
        {
            _context.MeetingRoom.Add(meetingRoom);
            await _context.SaveChangesAsync();
            return meetingRoom;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(MeetingRoomEntity meetingRoom)
        {
            _context.MeetingRoom.Remove(meetingRoom);
            await _context.SaveChangesAsync();
        }
    }
}
