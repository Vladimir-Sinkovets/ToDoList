using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class PeriodTask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public User User { get; set; }
        
        public bool IsMatch(DateTime date)
        {
            try
            {
                switch (Type)
                {
                    case "daysInMonth":
                        int[] days = Value
                            .Split()
                            .Select(s => Int32.Parse(s))
                            .ToArray();
                        return days.Contains(date.Day);
                        break;
                    case "daysInWeek":
                        return date.DayOfWeek.ToString() == Value;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
