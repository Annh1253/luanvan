using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ExamService.Models;

namespace ExamService.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {

        private readonly IExamRepository _examRepository;

        public AuthorizeFilter(IExamRepository examRepository)
        {
            this._examRepository = examRepository;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (string)context.HttpContext.Items["User"];
            bool isAuthorized = _examRepository.GetByEmail(user) != null;
            
            
            if(!isAuthorized)
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
            }
        }
    }
}