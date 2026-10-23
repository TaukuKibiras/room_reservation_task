using Microsoft.AspNetCore.Mvc;
using Reservation.ServiceLibrary.Entities;
using Reservation.ServiceLibrary.Service;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserEntity>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserEntity>> Add(UserEntity userPost)
        {
            if (userPost == null)
            {
                return BadRequest();
            }

            var addedUser = await _userService.AddUser(userPost);

            return Created($"/api/User", addedUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserEntity>> Update(int id, UserEntity userPost)
        {
            var updatedUser = await _userService.UpdateUser(userPost, id);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserEntity>> Delete(int id)
        {
            var deletedUser = await _userService.DeleteUser(id);

            if (deletedUser == null)
            {
                return NotFound();
            }

            return Ok(deletedUser);
        }
    }
}


