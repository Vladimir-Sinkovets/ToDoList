using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class TodayTasksViewModel
    {
        public List<DayTask> Tasks { get; set; }

        public double Progress()
        {
            if (Tasks.Count == 0)
                return 0;
            float countOfCompletedTasks = Tasks.Where(task => task.IsDone).Count();
            return Math.Round(countOfCompletedTasks / (float)Tasks.Count() * 100);
        }
    }
}
