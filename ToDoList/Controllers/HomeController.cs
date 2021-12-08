using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Models.ViewModels;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditPeriodTasks()
        {
            EditPeriodTaskViewModel model = new EditPeriodTaskViewModel()
            {
                PeriodTasks = db.PeriodTasks.Where(t => t.User.Email == HttpContext.User.Identity.Name).ToList(),
            };
            return View(model);
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult TodayTaskList()
        {
            List<DayTask> dayTasks = GetTaskForToday();
            List<PeriodTaskLineInfo> periodTasksInfo = GetPeriodTasksForToday();

            TodayTasksViewModel model = new TodayTasksViewModel()
            {
                Tasks = dayTasks,
                PeriodTasks = periodTasksInfo,
            };
            return View(model);
        }
        private List<DayTask> GetTaskForToday()
        {
            return db.DayTasks
                .Where(task =>
                    task.User.Email == User.Identity.Name &&
                    task.Date.Year == DateTime.Now.Year &&
                    task.Date.Month == DateTime.Now.Month &&
                    task.Date.Day == DateTime.Now.Day)
                .ToList();
        }
        private List<PeriodTaskLineInfo> GetPeriodTasksForToday()
        {
            List<PeriodTask> periodTasks = db.PeriodTasks
                .Where(t => t.User.Email == HttpContext.User.Identity.Name)
                .ToList()
                .Where(t => t.IsMatch(DateTime.Now))
                .ToList();
            List<PeriodTaskRecord> records = db.PeriodTaskRecords
                .Where(rec => 
                    rec.Date.Year == DateTime.Now.Year &&
                    rec.Date.Month == DateTime.Now.Month &&
                    rec.Date.Day == DateTime.Now.Day)
                .ToList();
            List<PeriodTaskLineInfo> periodTasksInfo = periodTasks
                .ToList()
                .Select(pTask => new PeriodTaskLineInfo() 
                { 
                    Id = pTask.Id, 
                    Description = pTask.Description, 
                    IsDone = records.Contains(
                        records.FirstOrDefault(rec => rec.PeriodTask.Id == pTask.Id)) 
                })
                .ToList();
            return periodTasksInfo;
        }
    }
}