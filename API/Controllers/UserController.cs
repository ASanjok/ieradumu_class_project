using Application.Interfaces;
using class_project_ieradumi;
using shared_object_ieradumi;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userrepository)
        {
            _userRepository = userrepository;
        }






        public record LoginResponse(string id);

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] SharedUser.LoginUserData request)
        {
            if (string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Username))
            {
                return BadRequest("нету имени и/или пароля");
            }

            var userId = await _userRepository.LoginAsync(request.Username, request.Password);


            if (userId is null)
            {
                return Unauthorized("Неверное имя пользователя и/или пароль.");
            }

            return Ok(new LoginResponse(userId));
        }


        public record RegisterResponse(string id);


        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] SharedUser.RegisterUserData request)
        {
            if (string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Username))
            {
                return BadRequest("нету имени и/или пароля");
            }

            string? userId = await _userRepository.RegisterAsync(request.Username, request.Password);


            if (userId is null)
            {
                return Conflict("пользователь с таким именем уже существует");
            }

            return Ok(new RegisterResponse(userId));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SharedUser.FullUserData>> GetUser(string id)
        {
            User? user = await _userRepository.GetUserAsync(id);
            if (user is null)
            {
                return NotFound("Пользователь с таким ID не найден.");
            }
            SharedUser.FullUserData response = new SharedUser.FullUserData
            {
                ID = user.ID,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                Archived = user.Archived,
                UserCreationDate = user.UserCreationDate,
                UserLastChangeDate = user.UserLastChangeDate,
            };
            return response;
        }


        //public record UpdateUserRequest(string UserName, string? Password, string? Email, bool? archived);

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] SharedUser.UpdateUserData request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Имя пользователя и/или пароль не может быть пустым.");
            }

            var updated = await _userRepository.UpdateUserAsync(id, request.Username, request.Password, request.Email, request.Archived);
            if (!updated)
            {
                return NotFound("Пользователь с таким ID не найден.");
            }

            return NoContent();
        }
    }
}
