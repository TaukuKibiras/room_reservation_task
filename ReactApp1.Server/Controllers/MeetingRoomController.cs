using Microsoft.AspNetCore.Mvc;
using Reservation.ServiceLibrary.Entities;
using Reservation.ServiceLibrary.Service;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingRoomController : ControllerBase
    {
        private readonly MeetingRoomService _meetingRoomService;

        public MeetingRoomController(MeetingRoomService meetingRoomService)
        {
            _meetingRoomService = meetingRoomService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MeetingRoomEntity>>> GetMeetingRooms()
        {
            var meetingRooms = await _meetingRoomService.GetMeetingRooms();
            return Ok(meetingRooms);
        }

        [HttpPost]
        public async Task<ActionResult<MeetingRoomEntity>> Add(MeetingRoomEntity meetingRoomPost)
        {
            if (meetingRoomPost == null)
            {
                return BadRequest();
            }

            var addedMeetingRoom = await _meetingRoomService.AddMeetingRoom(meetingRoomPost);

            return Created($"/api/MeetingRoom", addedMeetingRoom);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MeetingRoomEntity>> Update(int id, MeetingRoomEntity meetingRoomPost)
        {
            var updatedMeetingRoom = await _meetingRoomService.UpdateMeetingRoom(meetingRoomPost, id);

            if (updatedMeetingRoom == null)
            {
                return NotFound();
            }

            return Ok(updatedMeetingRoom);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MeetingRoomEntity>> Delete(int id)
        {
            var deletedMeetingRoom = await _meetingRoomService.DeleteMeetingRoom(id);

            if (deletedMeetingRoom == null)
            {
                return NotFound();
            }

            return Ok(deletedMeetingRoom);
        }
    }
}
