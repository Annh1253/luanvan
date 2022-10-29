using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using UserService.Models;

namespace UserService.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IndetnifyFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var requestBody = context.HttpContext.Request.Body;
            
        }
    }
}