using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    [ApiController]
    [Route("Appointment")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? patientId, int? doctorId, DateTime? date)
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            if (patientId.HasValue)
            {
                appointments = appointments.Where(a => a.PatientID == patientId.Value);
            }

            if (doctorId.HasValue)
            {
                appointments = appointments.Where(a => a.DoctorID == doctorId.Value);
            }

            if (date.HasValue)
            {
                appointments = appointments.Where(a => a.Date.Date == date.Value.Date);
            }

            return View(appointments);
        }



        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentRepository.AddAsync(appointment);
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _appointmentRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Appointment appointment)
        {
            appointment.ID = id;
            if (ModelState.IsValid)
            {
                await _appointmentRepository.UpdateAsync(appointment);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nurse = await _appointmentRepository.GetByIdAsync(id);
            if (nurse == null) return NotFound();
            return View(nurse);
        }
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
