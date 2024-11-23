using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    [ApiController]
    [Route("Doctor")]  
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? name, int? id)
        {
            var doctor = await _doctorRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(name))
            {
                doctor = doctor.Where(n => n.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            if (id.HasValue)
            {
                doctor = doctor.Where(n => n.ID == id);
            }
            return View(doctor);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Doctor doctor)
        {
            await _doctorRepository.AddAsync(doctor);
            return RedirectToAction("Index");
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id,[FromForm] Doctor doctor)
        {
            if (id != doctor.ID) return BadRequest();
            if (ModelState.IsValid)
            {
                await _doctorRepository.UpdateAsync(doctor);
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _doctorRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
