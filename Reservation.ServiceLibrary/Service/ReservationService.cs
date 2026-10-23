using Reservation.ServiceLibrary.Entities;
using Reservation.ServiceLibrary.Repositories;

namespace Reservation.ServiceLibrary.Service
{
    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly UserRepository _userRepository;
        private readonly MeetingRoomRepository _meetingRoomRepository;

        public ReservationService(
            ReservationRepository reservationRepository, 
            UserRepository userRepository, 
            MeetingRoomRepository meetingRoomRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _meetingRoomRepository = meetingRoomRepository;
        }

        public async Task<List<ReservationEntity>> GetReservations()
        {
            return await _reservationRepository.GetAll();
        }

        public async Task<ReservationEntity> AddReservation(ReservationEntity reservationPost)
        {
            var user = await _userRepository.GetById(reservationPost.UserId);

            if (user == null)
                throw new Exception("User not found.");

            var room = await _meetingRoomRepository.GetById(reservationPost.MeetingRoomId);

            if (!room.IsActive)
                throw new Exception("Inactive meeting rooms cannot be booked.");

            if (reservationPost.StartTime <= DateTime.UtcNow)
                throw new Exception("Start time must be in the future.");

            if (reservationPost.EndTime <= reservationPost.StartTime)
                throw new Exception("End time must be later than start time.");

            var hasConflict = await _reservationRepository.HasOverlappingReservation(
                reservationPost.MeetingRoomId,
                reservationPost.StartTime,
                reservationPost.EndTime);

            if (hasConflict)
                throw new Exception("This meeting room is already reserved for this time.");

            var reservation = new ReservationEntity
            {
                Title = reservationPost.Title,
                MeetingRoomId = reservationPost.MeetingRoomId,
                UserId = reservationPost.UserId,
                StartTime = reservationPost.StartTime,
                EndTime = reservationPost.EndTime,
                AttendeesCount = reservationPost.AttendeesCount,
                Status = ReservationStatus.Booked
            };          

            user.Reservations++;

            await _userRepository.Update();

            return await _reservationRepository.Add(reservation);

        }

        public async Task<ReservationEntity> UpdateReservation(int id, ReservationEntity reservationPost)
        {
            var existingReservation = await _reservationRepository.GetById(id);

            var room = await _meetingRoomRepository.GetById(reservationPost.MeetingRoomId);

            if (!room.IsActive)
                throw new Exception("Inactive meeting rooms cannot be booked.");

            if (reservationPost.StartTime <= DateTime.UtcNow)
                throw new Exception("Start time must be in the future."); 

            if (reservationPost.EndTime <= reservationPost.StartTime)
                throw new Exception("End time must be later than start time.");

            if (existingReservation.Status != ReservationStatus.Booked)
                throw new Exception("Completed or cancelled reservations cannot be updated.");


            if (reservationPost.Status == ReservationStatus.Booked)
            {
                var hasConflict = await _reservationRepository.HasOverlappingReservation(
                    reservationPost.MeetingRoomId,
                    reservationPost.StartTime,
                    reservationPost.EndTime,
                    id);

                if (hasConflict)
                    throw new Exception("This meeting room is already reserved for this time.");
            }

            var user = await _userRepository.GetById(existingReservation.UserId);

            if (user == null)
                throw new Exception("User not found.");


            if (existingReservation.Status == ReservationStatus.Booked && reservationPost.Status != ReservationStatus.Booked)
            {
                user.Reservations--;
            }

            existingReservation.Title = reservationPost.Title;
            existingReservation.StartTime = reservationPost.StartTime;
            existingReservation.EndTime = reservationPost.EndTime;
            existingReservation.Status = reservationPost.Status;

            await _reservationRepository.Update();
            await _userRepository.Update();

            return existingReservation;
        }
    }
}
