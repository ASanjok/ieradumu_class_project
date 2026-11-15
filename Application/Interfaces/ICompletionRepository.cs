using class_project_ieradumi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICompletionRepository
    {
        Task MarkAsCompletedAsync(CompletionRecord record);
        Task<List<CompletionRecord>> GetByHabitIdAsync(string habitId);
    }
}
