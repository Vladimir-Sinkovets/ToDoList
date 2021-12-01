using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class PeriodTask
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public User User { get; set; }
    }
}
