using Covid19.Model;
using Covid19.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using PR.Notifications.Model;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Covid19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DpDataContext _context;
        private readonly ServiceBusSender _sender;

        public PatientsController(DpDataContext context, ServiceBusSender sender)
        {
            _context = context;
            _sender = sender;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Patients.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();

            await _sender.SendMessage(new MessagePayLoad()
            {
                EventName = "NewUserRegistered",
                UserEmail = "notifications2k19@gmail.com"
            });

            return Created("/api/patients/", patient);
        }
    }
}
