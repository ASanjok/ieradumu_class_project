using Application.Interfaces;
using class_project_ieradumi;
using Microsoft.AspNetCore.Mvc;
using shared_object_ieradumi;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitController : ControllerBase
    {
        private readonly IHabitRepository _habitRepository;

        public HabitController(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        [HttpPost]
        public async Task<ActionResult<SharedHabit.FullHabitData>> SetHabit([FromBody] SharedHabit.FullHabitData request)
        {
            if (request is null)
                return BadRequest("Тело запроса не может быть пустым.");

            if (string.IsNullOrWhiteSpace(request.HabitName))
                return BadRequest("Название привычки не может быть пустым.");

            if (string.IsNullOrWhiteSpace(request.AuthorID))
                return BadRequest("Не указан ID пользователя (AuthorID).");

            var habitId = string.IsNullOrWhiteSpace(request.ID)
                ? Guid.NewGuid().ToString()
                : request.ID;

            var habit = new Habit
            {
                ID = habitId,
                HabitName = request.HabitName.Trim(),
                HabitDescription = request.HabitDescription?.Trim() ?? string.Empty,
                AuthorID = request.AuthorID,
                Archived = request.Archived,
                HabitCreationDate = request.HabitCreationDate == default ? DateTime.UtcNow : request.HabitCreationDate,
                HabitLastChangeDate = DateTime.UtcNow,
            };

            bool created = await _habitRepository.AddHabitAsync(habit);
            if (!created)
            {
                return Conflict("Не удалось создать привычку — возможно, запись с таким ID уже существует.");
            }

            var response = MapToFullHabitData(habit);
            return CreatedAtAction(nameof(GetHabit), new { id = habit.ID }, response);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<SharedHabit.FullHabitData>>> GetHabits(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("Не указан ID пользователя.");

            var habits = await _habitRepository.GetHabitsAsync(userId);
            var sharedHabits = habits.Select(MapToFullHabitData).ToList();

            return Ok(sharedHabits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SharedHabit.FullHabitData>> GetHabit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Не указан ID привычки.");

            Habit? habit = await _habitRepository.GetHabitAsync(id);
            if (habit is null)
            {
                return NotFound("Привычка с таким ID не найдена.");
            }

            return Ok(MapToFullHabitData(habit));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHabit(string id, [FromBody] SharedHabit.UpdateHabitData request)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Не указан ID привычки.");

            if (request is null || string.IsNullOrWhiteSpace(request.HabitName))
                return BadRequest("Название привычки не может быть пустым.");

            bool updated = await _habitRepository.UpdateHabitAsync(
                id,
                request.HabitName.Trim(),
                request.HabitDescription?.Trim() ?? string.Empty,
                request.AuthorID,
                request.Archived);

            if (!updated)
            {
                return NotFound("Привычка с таким ID не найдена.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHabit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Не указан ID привычки.");

            bool deleted = await _habitRepository.DeleteHabitAsync(id);
            if (!deleted)
            {
                return NotFound("Привычка с таким ID не найдена.");
            }

            return NoContent();
        }

        private static SharedHabit.FullHabitData MapToFullHabitData(Habit habit) => new()
        {
            ID = habit.ID,
            HabitName = habit.HabitName,
            HabitDescription = habit.HabitDescription,
            AuthorID = habit.AuthorID,
            Archived = habit.Archived,
            HabitCreationDate = habit.HabitCreationDate,
            HabitLastChangeDate = habit.HabitLastChangeDate,
        };
    }
}