using DepiProject.DbContextLayer;
using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepiProject.Controllers
{

    [ApiController]
    [Route("Patient")]
    public class PatientController : Controller
    {
        private readonly HospitalContext _context;
        private readonly IPatientRepository _patientRepository;

        public PatientController( HospitalContext context ,IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
            _context = context;
        }




        [HttpGet]
        public async Task<IActionResult> Index(string? name, int? id)
        {
            var patients = await _patientRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(name))
            {
                patients = patients.Where(n => n.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            if (id.HasValue)
            {
                patients = patients.Where(n => n.ID == id);
            }
            return View(patients);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Patient patient)
        {
            if (ModelState.IsValid)
            {
                await _patientRepository.AddAsync(patient);
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _patientRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _patientRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Patient patient)
        {
            patient.ID = id;
            if (ModelState.IsValid)
            {
                await _patientRepository.UpdateAsync(patient);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();


        }

		


	}
}
