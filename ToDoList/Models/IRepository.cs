using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public interface IRepository
    {
        IQueryable<User> GetAllUsers();
        IQueryable<DayTask> GetAllDayTasks();
        IQueryable<PeriodTask> GetAllPeriodTasks();
        IQueryable<PeriodTaskRecord> GetAllPeriodTaskRecords();

        void AddUser(User user);
        void AddDayTask(DayTask dayTask);
        void AddPeriodTask(PeriodTask periodTask);
        void AddPeiodTaskRecord(PeriodTaskRecord periodTaskRecord);

        void RemoveDayTask(DayTask dayTask);
        void RemovePeriodTask(PeriodTask periodTask);
        void RemovePeriodTaskRecord(PeriodTaskRecord record);

        void RemovePeriodTaskRecordsRange(IEnumerable<PeriodTaskRecord> range);

        void ChangeDayTask(Action<DayTask> func, int taskId);
    }
}