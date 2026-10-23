using Reservation.ServiceLibrary.Entities;
using Reservation.ServiceLibrary.Repositories;

namespace Reservation.ServiceLibrary.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserEntity>> GetUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<UserEntity> AddUser(UserEntity userPost)
        {
            var user = new UserEntity
            {
                Name = userPost.Name,
                Surname = userPost.Surname,
                Reservations = userPost.Reservations
            };

            return await _userRepository.Add(user);
        }
        public async Task<UserEntity?> UpdateUser(UserEntity userPost, int id)
        {
            var user = await _userRepository.GetById(id);

            if(user == null){
                throw new Exception("User not found.");
            }

            user.Name = userPost.Name;
            user.Surname = userPost.Surname;
            user.Reservations = userPost.Reservations;

            await _userRepository.Update();
            return user;
        }

        public async Task<UserEntity?> DeleteUser(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null){
                throw new Exception("User not found.");
            }

            await _userRepository.Delete(user);
            return user;

        }
    }
}
