using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

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
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult TodayTaskList()
        {
            List<DayTask> tasks = db.DayTasks
                .Where(task => 
                    task.User.Email == User.Identity.Name && 
                    task.Date.Year == DateTime.Now.Year &&
                    task.Date.Month == DateTime.Now.Month &&
                    task.Date.Day == DateTime.Now.Day)
                .ToList();
            TodayTasksViewModel model = new TodayTasksViewModel()
            {
                Tasks = tasks,
            };
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public IActionResult AddPeriodTasks()
        {
            return View();
        }

    }
}
