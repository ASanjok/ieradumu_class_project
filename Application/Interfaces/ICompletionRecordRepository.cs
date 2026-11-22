using class_project_ieradumi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICompletionRecordRepository
    {
        Task<List<CompletionRecord>> GetCompletionRecordsAsync(string userid, DateTime date);
        Task<bool> SetCompletionRecordAsync(CompletionRecord record);
        Task<bool> DeleteCompletionRecordAsync(string id);
    }
}
