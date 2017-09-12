using System.Collections.Generic;
using System.Linq;
using CamReports.Models;

namespace CamReports.Services.Database
{
    public class Issues
    {
        private LocalDatabaseService _LocalDatabaseService;

        public Issues(LocalDatabaseService localDatabaseService)
        {
            _LocalDatabaseService = localDatabaseService;
        }

        public IEnumerable<Issue> GetIssues(int scheduleId)
        {
             return _LocalDatabaseService.DatabaseConnection.Table<Issue>().Where(item => item.ScheduleId == scheduleId);
        }

        public IEnumerable<int> GetInProgressReportScheduleIds()
        {
            return _LocalDatabaseService.DatabaseConnection.Table<Issue>()
                .GroupBy(item => item.ScheduleId)
                .Select(item => item.Key);
        }

        public int InsertIssue(Issue issue)
        {
            var result = _LocalDatabaseService.DatabaseConnection.Insert(issue);
            return result;
        }

        public int UpdateIssue(Issue newIssue)
        {
            var result = _LocalDatabaseService.DatabaseConnection.Update(newIssue);
            return result;
        }

        public int DeleteIssue(Issue issue)
        {
            var result = _LocalDatabaseService.DatabaseConnection.Delete(issue);
            return result;
        }
    }
}
