using Application.Interfaces;
using class_project_ieradumi;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using shared_object_ieradumi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompletionRecordController : ControllerBase
    {
        private readonly ICompletionRecordRepository _completionRecordRepository;

        public CompletionRecordController(ICompletionRecordRepository completionRecordRepository)
        {
            _completionRecordRepository = completionRecordRepository;
        }

        // GET: api/CompletionRecord/user/{userId}?date=2026-11-22
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<SharedCompletionRecord.FullCompletionRecordData>>> GetCompletionRecordsByUserAndDate(string userId, [FromQuery] DateTime? date)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("Не указан ID пользователя.");

            var targetDate = (date ?? DateTime.Today).Date;
            var records = await _completionRecordRepository.GetCompletionRecordsAsync(userId, targetDate);
            var response = records.Select(record => new SharedCompletionRecord.FullCompletionRecordData
            {
                ID = record.ID,
                HabitID = record.HabitID,
                UserID = record.UserID,
                CompletionDate = record.CompletionDate,
                InTime = record.InTime,
                HasTheRecordBeenUsed = record.HasTheRecordBeenUsed,
                ScheduleTime = record.ScheduleTime ?? new Dictionary<string, TimeOnly>()
            }).ToList();

            return Ok(response);
        }

        // POST: api/CompletionRecord
        [HttpPost]
        public async Task<ActionResult> CreateCompletionRecord([FromBody] SharedCompletionRecord.FullCompletionRecordData request)
        {
            if (request is null)
                return BadRequest("Тело запроса не может быть пустым.");

            if (string.IsNullOrWhiteSpace(request.HabitID))
                return BadRequest("Не указан ID привычки (HabitID).");

            if (string.IsNullOrWhiteSpace(request.UserID))
                return BadRequest("Не указан ID пользователя (UserID).");

            var record = new CompletionRecord
            {
                ID = string.IsNullOrWhiteSpace(request.ID)
                    ? "completion_record_ID_" + Guid.NewGuid().ToString("N")[..8]
                    : request.ID,
                HabitID = request.HabitID,
                UserID = request.UserID,
                CompletionDate = request.CompletionDate == default ? DateTime.Now : request.CompletionDate,
                InTime = request.InTime,
                HasTheRecordBeenUsed = request.HasTheRecordBeenUsed,
                ScheduleTime = request.ScheduleTime ?? new Dictionary<string, TimeOnly>()
            };
            bool created = await _completionRecordRepository.SetCompletionRecordAsync(record);
            if (!created)
            {
                return Conflict("Не удалось сохранить запись о выполнении (возможно, дубликат ID).");
            }

            return Ok();
        }

        // DELETE: api/CompletionRecord/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompletionRecord (string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Не указан ID записи.");

            bool deleted = await _completionRecordRepository.DeleteCompletionRecordAsync(id);
            if (!deleted)
            {
                return NotFound("Запись о выполнении с таким ID не найдена.");
            }

            return NoContent();
        }
    }
}
