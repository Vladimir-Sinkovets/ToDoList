using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{ 
    public class TaskController : Controller
    {
        private IRepository db;
        private AuthenticationHelper authentication;

        public TaskController(IRepository db, AuthenticationHelper authentication)
        {
            this.db = db;
            this.authentication = authentication;
        }

        [HttpPost]
        public RedirectToActionResult AddTodayTasks(string taskDescription)
        {
            var allUsers = db.GetAllUsers(); 
            int count = allUsers.ToList().Count();
            User user = allUsers.FirstOrDefault(user => user.Email == authentication.CurrentUserName(HttpContext));
            List<User> users = allUsers.Where(user => user.Email == authentication.CurrentUserName(HttpContext)).ToList();
            DayTask dayTask = new DayTask()
            {
                Date = DateTime.Now,
                Description = taskDescription,
                IsDone = false,
                User = user,
            };
            db.AddDayTask(dayTask);

            return RedirectToActionPermanent("TodayTaskList", "Home");
        }
        public RedirectToActionResult DeleteTask(int taskId)
        {
            DayTask task = db.GetAllDayTasks().FirstOrDefault(task => task.Id == taskId);
            db.RemoveDayTask(task);
            return RedirectToAction("TodayTaskList", "Home");
        }
        public RedirectToActionResult ChangeTaskRecord(int taskId)
        {
            db.ChangeDayTask(t => t.IsDone = !t.IsDone, taskId);
            return RedirectToAction("TodayTaskList", "Home");
        }

        public RedirectToActionResult AddPeriodTask(string description, string value, string type)
        {
            var allUsers = db.GetAllUsers();
            PeriodTask task = new PeriodTask()
            {
                Type = type,
                User = allUsers.FirstOrDefault(user => user.Email == authentication.CurrentUserName(HttpContext)),
                Value = value,
                Description = description,
            };
            db.AddPeriodTask(task);
            return RedirectToAction("EditPeriodTasks", "Home");
        }
        public RedirectToActionResult DeletePeriodTask(int periodTaskId)
        {
            var allPeriodTasks = db.GetAllPeriodTasks();
            var allPeriodTaskRecords = db.GetAllPeriodTaskRecords();
            PeriodTask task = allPeriodTasks.FirstOrDefault(task => task.Id == periodTaskId);
            db.RemovePeriodTask(task);
            List<PeriodTaskRecord> records = db.GetAllPeriodTaskRecords()
                .Where(rec => rec.PeriodTask.Id == periodTaskId)
                .ToList();
            db.RemovePeriodTaskRecordsRange(records);
            return RedirectToAction("EditPeriodTasks", "Home");
        }
        public RedirectToActionResult ChangePeriodTaskRecord(int taskId)
        {
            var allPeriodTasks = db.GetAllPeriodTasks();
            var allPeriodTaskRecords = db.GetAllPeriodTaskRecords();
            var allUsers = db.GetAllUsers();

            PeriodTask periodTask = allPeriodTasks.FirstOrDefault(task => task.Id == taskId);
            if(periodTask == null)
                return RedirectToAction("TodayTaskList", "Home");
            PeriodTaskRecord taskRecord = allPeriodTaskRecords.FirstOrDefault(task => task.PeriodTask.Id == periodTask.Id);
            if(taskRecord == null)
            {
                User user = allUsers.FirstOrDefault(user => user.Email == authentication.CurrentUserName(HttpContext));
                db.AddPeiodTaskRecord(new PeriodTaskRecord()
                {
                    Date = DateTime.Now,
                    PeriodTask = periodTask,
                    User = user,
                });
            }
            else
            {
                db.RemovePeriodTaskRecord(taskRecord);
            }
            return RedirectToAction("TodayTaskList", "Home");
        }
    }
}