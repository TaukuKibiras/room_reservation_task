using Reservation.ServiceLibrary.Entities;
using Reservation.ServiceLibrary.Repositories;

namespace Reservation.ServiceLibrary.Service
{
    public class MeetingRoomService
    {
        private readonly MeetingRoomRepository _meetingRoomRepository;

        public MeetingRoomService(MeetingRoomRepository meetingRoomRepository)
        {
            _meetingRoomRepository = meetingRoomRepository;
        }

        public async Task<List<MeetingRoomEntity>> GetMeetingRooms()
        {
            return await _meetingRoomRepository.GetAll();
        }

        public async Task<MeetingRoomEntity> AddMeetingRoom(MeetingRoomEntity meetingRoomPost)
        {
            var meetingRoom = new MeetingRoomEntity
            {
                Name = meetingRoomPost.Name,
                Capacity = meetingRoomPost.Capacity,
                Location = meetingRoomPost.Location,
                Equipment = meetingRoomPost.Equipment,
                IsActive = meetingRoomPost.IsActive
            };

            return await _meetingRoomRepository.Add(meetingRoom);
        }
        public async Task<MeetingRoomEntity?> UpdateMeetingRoom(MeetingRoomEntity meetingRoomPost, int id)
        {
            var meetingRoom = await _meetingRoomRepository.GetById(id);

            if(meetingRoom == null){
                throw new Exception("Meeting room not found.");
            }

            meetingRoom.Name = meetingRoomPost.Name;
            meetingRoom.Capacity = meetingRoomPost.Capacity;
            meetingRoom.Location = meetingRoomPost.Location;
            meetingRoom.Equipment = meetingRoomPost.Equipment;
            meetingRoom.IsActive = meetingRoomPost.IsActive;

            await _meetingRoomRepository.Update();
            return meetingRoom;
        }

        public async Task<MeetingRoomEntity?> DeleteMeetingRoom(int id)
        {
            var meetingRoom = await _meetingRoomRepository.GetById(id);

            if (meetingRoom == null)
            {
                throw new Exception("Meeting room not found.");
            }

            await _meetingRoomRepository.Delete(meetingRoom);
            return meetingRoom;

        }
    }
}
