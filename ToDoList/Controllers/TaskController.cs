using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{ 
    public class TaskController : Controller
    {
        private ApplicationDbContext db;

        public TaskController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddTodayTasks(string taskDescription)
        {
            int count = db.Users.ToList().Count();
            User user = await db.Users.FirstOrDefaultAsync(user => user.Email == User.Identity.Name);
            List<User> users = db.Users.Where(user => user.Email == User.Identity.Name).ToList();
            DayTask dayTask = new DayTask()
            {
                Date = DateTime.Now,
                Description = taskDescription,
                IsDone = false,
                User = user,
            };
            db.DayTasks.Add(dayTask);

            await db.SaveChangesAsync();

            return RedirectToActionPermanent("TodayTaskList", "Home");
        }
        public RedirectToActionResult DeleteTask(int taskId)
        {
            DayTask task = db.DayTasks.FirstOrDefault(task => task.Id == taskId);
            db.DayTasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("TodayTaskList", "Home");
        }
        public RedirectToActionResult ChangeTaskRecord(int taskId)
        {
            DayTask task = db.DayTasks.FirstOrDefault(task => task.Id == taskId);
            task.IsDone = !task.IsDone;
            db.SaveChanges();
            return RedirectToAction("TodayTaskList", "Home");
        }

        public RedirectToActionResult AddPeriodTask(string description, string value, string type)
        {
            PeriodTask task = new PeriodTask() 
            { 
                Type = type, 
                User = db.Users.FirstOrDefault(user => user.Email == HttpContext.User.Identity.Name),
                Value = value,
                Description = description,
            };
            db.PeriodTasks.Add(task);
            db.SaveChanges();
            return RedirectToAction("EditPeriodTasks", "Home");
        }
        public RedirectToActionResult DeletePeriodTask(int periodTaskId)
        {
            PeriodTask task = db.PeriodTasks.FirstOrDefault(task => task.Id == periodTaskId);
            db.PeriodTasks.Remove(task);
            List<PeriodTaskRecord> records = db.PeriodTaskRecords
                .Where(rec => rec.PeriodTask.Id == periodTaskId)
                .ToList();
            db.PeriodTaskRecords.RemoveRange(records);
            db.SaveChanges();
            return RedirectToAction("EditPeriodTasks", "Home");
        }
        public RedirectToActionResult ChangePeriodTaskRecord(int taskId)
        {
            //DayTask task = db.DayTasks.FirstOrDefault(task => task.Id == taskId);
            //task.IsDone = !task.IsDone;
            //db.SaveChanges();
            PeriodTask periodTask = db.PeriodTasks.FirstOrDefault(task => task.Id == taskId);
            if(periodTask == null)
                return RedirectToAction("TodayTaskList", "Home");
            PeriodTaskRecord taskRecord = db.PeriodTaskRecords.FirstOrDefault(task => task.PeriodTask.Id == periodTask.Id);
            if(taskRecord == null)
            {
                User user = db.Users.FirstOrDefault(user => user.Email == HttpContext.User.Identity.Name);
                db.PeriodTaskRecords.Add(new PeriodTaskRecord()
                {
                    Date = DateTime.Now,
                    PeriodTask = periodTask,
                    User = user,
                });
            }
            else
            {
                db.PeriodTaskRecords.Remove(taskRecord);
            }
            db.SaveChanges();
            return RedirectToAction("TodayTaskList", "Home");
        }
    }
}
