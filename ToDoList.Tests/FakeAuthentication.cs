using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Services;

namespace ToDoList.Tests
{
    public class FakeAuthentication : AuthenticationHelper
    {
        public string Return { get; set; }
        public override string CurrentUserName(HttpContext context)
        {
            return Return;
        }
    }
}
