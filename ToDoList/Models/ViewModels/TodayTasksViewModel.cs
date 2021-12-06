using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models.ViewModels;

namespace ToDoList.Models
{
    public class TodayTasksViewModel
    {
        public List<DayTask> Tasks { get; set; }
        public List<PeriodTaskLineInfo> PeriodTasks { get; set; }

        public double Progress()
        {
            float countOfAllTasks = Tasks.Count + PeriodTasks.Count;
            if (countOfAllTasks == 0)
                return 0;
            float countOfCompletedTasks = Tasks.Where(task => task.IsDone).Count() + PeriodTasks.Where(task => task.IsDone).Count();
            return Math.Round(countOfCompletedTasks / countOfAllTasks * 100);
        }
    }
}
