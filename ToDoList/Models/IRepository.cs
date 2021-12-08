using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public interface IRepository
    {
        DbSet<User> Users { get; set; }
        DbSet<DayTask> DayTasks { get; set; }
        DbSet<PeriodTask> PeriodTasks { get; set; }
        DbSet<PeriodTaskRecord> PeriodTaskRecords { get; set; }

        void SaveChangesAsync();
    }
}