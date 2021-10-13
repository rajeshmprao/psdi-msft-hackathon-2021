using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PSDIPortal.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace PSDIPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        private readonly IUserProcessor _userProcessor;


        public HomeController(ILogger<HomeController> logger, IConfiguration config, IUserProcessor userProcessor)
        {
            _logger = logger;
            _config = config;
            _userProcessor = userProcessor;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("CurrentUser", "jerryme@microsoft.com");
            if (await this._userProcessor.checkIsCurrentUserNew())
            {
                await this._userProcessor.AddDefaultCurrentUserDetails();
            }
            // Check in the PSDIPortalUsers if a record exists. If yes, continue. If no, put a record with default values.
            return View();
        }
    }
}
