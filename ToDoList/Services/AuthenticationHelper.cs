using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Services
{
    public class AuthenticationHelper
    {
        public virtual string CurrentUserName(HttpContext context)
        {
            return context.User.Identity.Name;
        }
    }
}
