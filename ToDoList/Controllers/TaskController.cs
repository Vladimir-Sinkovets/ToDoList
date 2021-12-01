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
        public async Task<RedirectToActionResult> AddTodayTasks(string description)
        {
            int count = db.Users.ToList().Count();
            User user = await db.Users.FirstOrDefaultAsync(user => user.Email == User.Identity.Name);
            List<User> users = db.Users.Where(user => user.Email == User.Identity.Name).ToList();
            DayTask dayTask = new DayTask()
            {
                Date = DateTime.Now,
                Description = description,
                IsDone = false,
                User = user,
            };
            db.DayTasks.Add(dayTask);

            await db.SaveChangesAsync();

            return RedirectToActionPermanent("TodayTaskList", "Home");
        }
        
        public RedirectToActionResult ChangeTaskValue(int taskId)
        {
            DayTask task = db.DayTasks.FirstOrDefault(task => task.Id == taskId);
            task.IsDone = !task.IsDone;
            db.SaveChanges();
            return RedirectToAction("TodayTaskList", "Home");
        }

        public RedirectToActionResult DeleteTask(int taskId)
        {
            DayTask task = db.DayTasks.FirstOrDefault(task => task.Id == taskId);
            db.DayTasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("TodayTaskList", "Home");
        }


    }
}
