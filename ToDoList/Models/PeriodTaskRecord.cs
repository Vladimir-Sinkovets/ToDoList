using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class PeriodTaskRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public PeriodTask PeriodTask { get; set; }
    }
}
