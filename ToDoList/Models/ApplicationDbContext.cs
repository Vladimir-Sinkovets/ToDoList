using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class ApplicationDbContext : DbContext, IRepository
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DayTask> DayTasks { get; set; }
        public DbSet<PeriodTask> PeriodTasks { get; set; }
        public DbSet<PeriodTaskRecord> PeriodTaskRecords { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public IQueryable<User> GetAllUsers() => Users.AsQueryable();
        public IQueryable<DayTask> GetAllDayTasks() => DayTasks.Where(dt => dt.User != null).AsQueryable();
        public IQueryable<PeriodTask> GetAllPeriodTasks() => PeriodTasks.AsQueryable();
        public IQueryable<PeriodTaskRecord> GetAllPeriodTaskRecords() => PeriodTaskRecords.AsQueryable();

        public void AddUser(User user)
        {
            Users.Add(user);
            SaveChanges();
        }
        public void AddDayTask(DayTask dayTask)
        {
            DayTasks.Add(dayTask);
            SaveChanges();
        }
        public void AddPeriodTask(PeriodTask periodTask)
        {
            PeriodTasks.Add(periodTask);
            SaveChanges();
        }
        public void AddPeiodTaskRecord(PeriodTaskRecord periodTaskRecord)
        {
            PeriodTaskRecords.Add(periodTaskRecord);
            SaveChanges();
        }

        public void RemoveDayTask(DayTask dayTask)
        {
            DayTasks.Remove(dayTask);
            SaveChanges();
        }
        public void RemovePeriodTask(PeriodTask periodTask)
        {
            PeriodTasks.Remove(periodTask);
            SaveChanges();
        }
        public void RemovePeriodTaskRecord(PeriodTaskRecord record)
        {
            PeriodTaskRecords.Remove(record);
            SaveChanges();
        }

        public void RemovePeriodTaskRecordsRange(IEnumerable<PeriodTaskRecord> range)
        {
            PeriodTaskRecords.RemoveRange(range);
            SaveChanges();
        }

        public void ChangeDayTask(Action<DayTask> func, int taskId)
        {
            DayTask task = DayTasks.FirstOrDefault(t => t.Id == taskId);
            func.Invoke(task);
            SaveChanges();
        }
    }
}
