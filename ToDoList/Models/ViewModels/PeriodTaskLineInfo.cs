using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models.ViewModels
{
    public class PeriodTaskLineInfo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
    }
}
