using System.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Capstone_project_WorkerService.Models;
using Dapper;

namespace Capstone_project_WorkerService.Contracts
{
    public class DueDateHandler : IDueDateHandler
    {
        private const string GET_ALL = "select Id, BranchId, SalesOfMonth, OriginalSales, Profit, PlatformFee, Total, Date, DueDate, Status from Dashboard;";
        private const string UPDATE_STATUS = "update Dashboard set Status = @status where DueDate <= @dateNow and Status = @statusToCompare";
        private const string DEACTIVATE_STORE = "update b set b.IsActive = 'false' from Branches b inner join Dashboard d on b.Id = d.BranchId where b.IsActive = 'true' and d.Status = 'UNPAID' and dateadd(day, 5, d.DueDate) <= @dateNow";
        private readonly IDbConnection _db;
        public DueDateHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> DeActivate5DaysUnpaidStore()
        {
            var paramsValue = new
            {
                dateNow = DateTime.Now.Date
            };

            int affectedRows = await _db.ExecuteAsync(DEACTIVATE_STORE, paramsValue);
            return affectedRows;
        }

        public async Task<IEnumerable<Dashboard>> GetAll()
        {
            var dashboards = await _db.QueryAsync<Dashboard>(GET_ALL);
            return dashboards;
        }

        public async Task<int> UpdateStatus()
        {
            var paramsValue = new
            {
                status = Keyword.UN_PAID,
                dateNow = DateTime.Now.Date,
                statusToCompare = Keyword.ON_GOING
            };

            var rowsAffected = await _db.ExecuteAsync(UPDATE_STATUS, paramsValue);
            return rowsAffected;
        }
    }

    public class Keyword
    {
        public const string ON_GOING = "ONGOING";
        public const string PAID = "PAID";
        public const string UN_PAID = "UNPAID";
        public const string DELAY = "DELAYED";
    }
}