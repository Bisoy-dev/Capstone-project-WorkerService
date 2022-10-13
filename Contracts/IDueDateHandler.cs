using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Capstone_project_WorkerService.Models;

namespace Capstone_project_WorkerService.Contracts
{
    public interface IDueDateHandler
    {
        Task<int> UpdateStatus();
        Task<IEnumerable<Dashboard>> GetAll();
        Task<int> DeActivate5DaysUnpaidStore();
    }
}