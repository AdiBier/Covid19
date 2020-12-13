using Covid19.Model;
using Covid19.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using PR.Notifications.Model;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Covid19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DpDataContext _context;
        private readonly ServiceBusSender _sender;

        public MovieController(DpDataContext context, ServiceBusSender sender)
        {
            _context = context;
            _sender = sender;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_context.Movies.ToList());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();

            await _sender.SendMessage(new MessagePayLoad()
            {
                EventName = "NewMovies",
                UserEmail = "noveMovies2k20@gmail.com"
            });

            return Created("/api/movies/", movie);
        }
    }
}
