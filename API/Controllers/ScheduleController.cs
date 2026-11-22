using Application.Interfaces;
using class_project_ieradumi;
using Microsoft.AspNetCore.Mvc;
using shared_object_ieradumi;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IHabitScheduleRepository _habitScheduleRepository;
        private readonly IHabitRepository _habitRepository;

        public ScheduleController(
            IScheduleRepository scheduleRepository,
            IHabitScheduleRepository habitScheduleRepository,
            IHabitRepository habitRepository)
        {
            _scheduleRepository = scheduleRepository;
            _habitScheduleRepository = habitScheduleRepository;
            _habitRepository = habitRepository;
        }

        // GET: api/Schedule/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<SharedSchedule.FullScheduleData>> GetSchedule(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("Не указан ID пользователя.");

            var schedule = await _scheduleRepository.GetScheduleAsync(userId);

            if (schedule is null)
            {
                return NotFound("Расписание для пользователя не найдено.");
            }
            SharedSchedule.FullScheduleData sharedschedule = new SharedSchedule.FullScheduleData
            {
                ID = schedule.ID,
                UserID = schedule.UserID,
                Archived = schedule.Archived,
                ScheduleCreationDate = schedule.ScheduleCreationDate,
                ScheduleLastChangeDate = schedule.ScheduleLastChangeDate,
            };
            return Ok(sharedschedule);
        }

        // POST: api/Schedule
        [HttpPost]
        public async Task<ActionResult<SharedSchedule.FullScheduleData>> CreateSchedule([FromBody] SharedSchedule.FullScheduleData request)
        {
            if (request is null)
                return BadRequest("Тело запроса не может быть пустым.");

            if (string.IsNullOrWhiteSpace(request.UserID))
                return BadRequest("Не указан ID пользователя (UserID).");

            var schedule = new Schedule
            {
                ID = string.IsNullOrWhiteSpace(request.ID)
                    ? "schedule_ID_" + Guid.NewGuid().ToString("N")[..8]
                    : request.ID,
                UserID = request.UserID,
                Archived = request.Archived,
                ScheduleCreationDate = DateTime.Now,
                ScheduleLastChangeDate = DateTime.Now
            };

            bool created = await _scheduleRepository.SetScheduleAsync(schedule);
            if (!created)
            {
                return Conflict("Не удалось создать расписание.");
            }
            SharedSchedule.FullScheduleData sharedschedule = new SharedSchedule.FullScheduleData
            {
                ID = schedule.ID,
                UserID = schedule.UserID,
                Archived = schedule.Archived,
                ScheduleCreationDate = schedule.ScheduleCreationDate,
                ScheduleLastChangeDate = schedule.ScheduleLastChangeDate,
            };

            return Ok(sharedschedule);
        }

        // POST: api/Schedule/{scheduleId}/items
        [HttpPost("{scheduleId}/items")]
        public async Task<ActionResult<SharedHabitSchedule.FullHabitScheduleData>> AddHabitScheduleItem(
            string scheduleId,
            [FromBody] SharedHabitSchedule.FullHabitScheduleData request)
        {
            if (string.IsNullOrWhiteSpace(scheduleId))
                return BadRequest("Не указан ID расписания.");

            if (request is null || string.IsNullOrWhiteSpace(request.HabitID))
                return BadRequest("Не указан ID привычки (HabitID).");

            if (string.IsNullOrWhiteSpace(request.DayOfWeek))
                return BadRequest("Не указан день недели (DayOfWeek).");

            var schedule = await _scheduleRepository.GetScheduleAsync(scheduleId);
            if (schedule is null)
                return NotFound("Расписание с таким ID не найдено.");

            var habit = await _habitRepository.GetHabitAsync(request.HabitID);
            if (habit is null)
                return NotFound("Привычка с таким ID не найдена.");

            var item = new HabitSchedule
            {
                ID = string.IsNullOrWhiteSpace(request.ID)
                    ? "habit_schedule_ID_" + Guid.NewGuid().ToString("N")[..8]
                    : request.ID,
                ScheduleID = scheduleId,
                HabitID = request.HabitID,
                DayOfWeek = request.DayOfWeek,
                ScheduledTime = request.ScheduledTime,
                CreatedAt = DateTime.Now,
                DeletedAt = null
            };

            bool added = await _habitScheduleRepository.SetHabitScheduleAsync(item);
            if (!added)
            {
                return BadRequest("Не удалось добавить элемент в расписание.");
            }

            schedule.ScheduleLastChangeDate = DateTime.Now;
            await _scheduleRepository.UpdateScheduleAsync(schedule);
            SharedHabitSchedule.FullHabitScheduleData sharedhabitschedule = new SharedHabitSchedule.FullHabitScheduleData
            {
                ID = item.ID,
                ScheduleID = item.ScheduleID,
                HabitID = item.HabitID,
                DayOfWeek = item.DayOfWeek,
                ScheduledTime = item.ScheduledTime,
                CreatedAt = item.CreatedAt,
                DeletedAt = item.DeletedAt,
                IsActive = item.IsActive,
            };
            return Ok(sharedhabitschedule);
        }

        // DELETE: api/Schedule/items/{habitScheduleId}
        [HttpDelete("items/{habitScheduleId}")]
        public async Task<ActionResult> SoftDeleteHabitScheduleItem(string habitScheduleId)
        {
            if (string.IsNullOrWhiteSpace(habitScheduleId))
                return BadRequest("Не указан ID элемента расписания.");

            bool deleted = await _habitScheduleRepository.SoftDeleteHabitScheduleAsync(habitScheduleId);
            if (!deleted)
            {
                return NotFound("Элемент расписания с таким ID не найден.");
            }

            return NoContent();
        }
    }
}