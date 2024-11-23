using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    [ApiController]
    [Route("api/Medication")]
    public class MedicationController : Controller
    {
        private readonly IMedicationRepository _medicationRepository;

        public MedicationController(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? name,int? id)
        {
            var d = await _medicationRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(name))
            {
                d = d.Where(n => n.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            if (id.HasValue)
            {
                d = d.Where(n => n.ID == id);
            }
            return View(d);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var d = await _medicationRepository.GetByIdAsync(id);
            if (d == null) return NotFound();
            return View(d);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Medication m)
        {
            if (ModelState.IsValid)
            {
                await _medicationRepository.AddAsync(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var m = await _medicationRepository.GetByIdAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Medication m)
        {
            m.ID = id;

            if (ModelState.IsValid)
            {
                await _medicationRepository.UpdateAsync(m);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _medicationRepository.GetByIdAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _medicationRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}