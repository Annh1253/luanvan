using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserService.Models;

namespace UserService.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizeFilter(string role)
        {
            this._role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = false;
            var user = (User)context.HttpContext.Items["User"];
            
            foreach(Role role in user.Roles)
            {
                if(role.Name == this._role)
                {
                    isAuthorized = true;
                    break;
                }                    
            }
            if(!isAuthorized)
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };

            }
        }
    }
}