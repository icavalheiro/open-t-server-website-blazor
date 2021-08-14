using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace TibiaWebsite.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return Json(new { status = "ok" });
        }

        public IActionResult Custom()
        {
            return Json(new { status = "ok" });
        }

        [Route("/test")]
        public IActionResult Test()
        {
            return Json(new { working = "yes" });
        }
    }
}
