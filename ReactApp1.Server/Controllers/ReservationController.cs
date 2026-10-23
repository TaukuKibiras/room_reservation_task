using Microsoft.AspNetCore.Mvc;
using Reservation.ServiceLibrary.Entities;
using Reservation.ServiceLibrary.Service;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationEntity>>> GetReservations()
        {
            var reservations = await _reservationService.GetReservations();
            return Ok(reservations);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationEntity>> AddReservation(ReservationEntity reservationPost)
        {
            if (reservationPost == null)
            {
                return BadRequest();
            }

            var addedUser = await _reservationService.AddReservation(reservationPost);

            return Created($"/api/Reservation", reservationPost);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationEntity>> UpdateReservation(int id, ReservationEntity reservationPost)
        {
            var reservation = await _reservationService.UpdateReservation(id, reservationPost);

            return Ok(reservation);
        }
 
    }
}
