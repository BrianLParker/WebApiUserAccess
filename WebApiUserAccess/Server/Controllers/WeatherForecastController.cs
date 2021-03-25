using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiUserAccess.Server.Models;
using WebApiUserAccess.Shared;

namespace WebApiUserAccess.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserManager<ApplicationUser> userManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var user = await GetUser();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = user.Email
            })
            .ToArray();
        }

        private async Task<ApplicationUser> GetUser()
        {
            ApplicationUser user = default;

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                user = await userManager.FindByIdAsync(userId);
            }

            return user;
        }
    }
}
