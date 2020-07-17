using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{StatusCode}")]
        public IActionResult HttpStatusCodeHandler(int StatusCode)
        {
            var data = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
           
            switch(StatusCode){
                case 404: 
                         ViewBag.Message = "Requested page does not exist";
                    ViewBag.Path = data.OriginalPath;
                    ViewBag.QueryString = data.OriginalQueryString;
                    break;
            }
            return View("ErrorPage");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExMessage = exceptionDetails.Error.Message;
            ViewBag.ExStackTrace = exceptionDetails.Error.StackTrace;
            ViewBag.ExPath = exceptionDetails.Path;

            return View("ErrorPage");
        }
    }
}
