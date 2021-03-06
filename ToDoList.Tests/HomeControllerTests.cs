using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Controllers;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Services;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ToDoList.Models.ViewModels;

namespace ToDoList.Tests
{
    public class HomeControllerTests
    {
        private Mock<IRepository> CreateMockRepository()
        {
            List<User> userData = new List<User>
            {
                new User() { Id = 1, Email = "v1@mail.com", Password = "1111" },
                new User() { Id = 2, Email = "v2@mail.com", Password = "1111" },
                new User() { Id = 3, Email = "v3@mail.com", Password = "1111" },
                new User() { Id = 4, Email = "v4@mail.com", Password = "1111" },
            };
            List<DayTask> tasksData = new List<DayTask>
            {
                new DayTask(){ Id= 1, User = userData.First(), Date = DateTime.Now, Description ="1", IsDone = false},
                new DayTask(){ Id= 2, User = userData.First(), Date = DateTime.Now, Description ="1", IsDone = false},
                new DayTask(){ Id= 3, User = userData.First(), Date = DateTime.Now, Description ="1", IsDone = false},
                new DayTask(){ Id= 4, User = userData.First(), Date = DateTime.Now, Description ="1", IsDone = false},
                new DayTask(){ Id= 5, User = userData.First(), Date = DateTime.Now, Description ="1", IsDone = false},
            };
            List<PeriodTask> periodTasksData = new List<PeriodTask>
            {
                new PeriodTask() { Id = 1, Type = "daysInMonth", User = userData.First(),
                    Value = "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31" },
                new PeriodTask() { Id = 2, Type = "daysInMonth", User = userData.First(),
                    Value = "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31" },
            };
            List<PeriodTaskRecord> periodTasksRecordsData = new List<PeriodTaskRecord>()
            {
                //new PeriodTaskRecord()
                //{
                //    Id = 1,
                //    PeriodTask = periodTasksData.First(),
                //    Date = DateTime.Today,
                //    User = userData.First()
                //}
            };

            var fakeRepository = new Mock<IRepository>();
            fakeRepository.Setup(obj => obj.GetAllUsers()).Returns(userData.AsQueryable());
            fakeRepository.Setup(obj => obj.GetAllDayTasks()).Returns(tasksData.AsQueryable());
            fakeRepository.Setup(obj => obj.GetAllPeriodTasks()).Returns(periodTasksData.AsQueryable());
            fakeRepository.Setup(obj => obj.GetAllPeriodTaskRecords()).Returns(periodTasksRecordsData.AsQueryable());
            return fakeRepository;
        }
        [Fact]
        public void TodayTasksIsCorrectListOfTasksAndListOfPeriodTasks()
        {
            // arrange
            var fakeRepository = CreateMockRepository();
            HomeController home = new HomeController(fakeRepository.Object, new FakeAuthentication() { Return = "v1@mail.com" });
            // act
            ViewResult todayPage = home.TodayTaskList() as ViewResult;
            // assert
            var model = (TodayTasksViewModel) todayPage.Model;
            Assert.Equal(0, model.Progress());
            Assert.Equal(5, model.Tasks.Count);
            Assert.Equal(2, model.PeriodTasks.Count);
        }
        [Fact]
        public void EditPeriodTasksIsCorrectPeriodTaskaList()
        {
            //arrange
            var fakeRepository = CreateMockRepository();
            HomeController home = new HomeController(fakeRepository.Object, new FakeAuthentication() { Return = "v1@mail.com" });
            //act
            ViewResult todayPage = home.EditPeriodTasks() as ViewResult;
            //assert
            var model = (EditPeriodTaskViewModel) todayPage.Model;
            Assert.Equal(2, model.PeriodTasks.Count);
        }

    }
}
