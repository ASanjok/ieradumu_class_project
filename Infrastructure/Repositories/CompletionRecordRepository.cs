using Application.Interfaces;
using class_project_ieradumi;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompletionRecordRepository : ICompletionRecordRepository
    {
        private readonly DBContext _dbContext;

        public CompletionRecordRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CompletionRecord>> GetCompletionRecordsAsync(string userid, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _dbContext.CompletionRecords
                .Where(c => c.UserID == userid &&
                            c.CompletionDate >= startOfDay &&
                            c.CompletionDate < endOfDay)
                .ToListAsync();
        }

        public async Task<bool> SetCompletionRecordAsync(CompletionRecord record)
        {
            if (record == null)
                return false;

            await _dbContext.CompletionRecords.AddAsync(record);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCompletionRecordAsync(string id)
        {
            var record = await _dbContext.CompletionRecords
                .FirstOrDefaultAsync(c => c.ID == id);

            if (record == null)
                return false;

            _dbContext.CompletionRecords.Remove(record);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}