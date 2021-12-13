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
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private IRepository db;
        private AuthenticationHelper authentication;

        public HomeController(IRepository db, AuthenticationHelper authentication)
        {
            this.db = db;
            this.authentication = authentication;
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditPeriodTasks()
        {
            EditPeriodTaskViewModel model = new EditPeriodTaskViewModel()
            {
                PeriodTasks = db.GetAllPeriodTasks().Where(t => t.User.Email == authentication.CurrentUserName(HttpContext)).ToList(),
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
            var dayTasks = db.GetAllDayTasks();
            return dayTasks
                .Where(task =>
                    task.User.Email == authentication.CurrentUserName(HttpContext) &&
                    task.Date.Year == DateTime.Now.Year &&
                    task.Date.Month == DateTime.Now.Month &&
                    task.Date.Day == DateTime.Now.Day)
                .ToList();
        }
        private List<PeriodTaskLineInfo> GetPeriodTasksForToday()
        {
            List<PeriodTask> periodTasks = db.GetAllPeriodTasks()
                .Where(t => t.User.Email == authentication.CurrentUserName(HttpContext))
                .ToList()
                .Where(t => t.IsMatch(DateTime.Now))
                .ToList();
            List<PeriodTaskRecord> records = db.GetAllPeriodTaskRecords()
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