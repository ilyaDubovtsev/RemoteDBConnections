using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab3.Models;

namespace Lab3.Implementation
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> ReadAll();

        Task Create(Feedback item);

        Task<Feedback> Read(Guid id);

        Task<bool> Update(Feedback item);

        Task<bool> Delete(Guid id);

        Task<bool> DeleteAll();
    }
}