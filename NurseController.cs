using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DepiProject.Controllers
{
    [ApiController]
    [Route("Nurse")]
    public class NurseController : Controller
    {
        private readonly INurseRepository _nurseRepository;

        public NurseController(INurseRepository nurseRepository)
        {
            _nurseRepository = nurseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? name, int? id)
        {
            var nurses = await _nurseRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(name))
            {
                nurses = nurses.Where(n => n.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            if (id.HasValue)
            {
                nurses = nurses.Where(n => n.ID == id);
            }
            return View(nurses);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var nurse = await _nurseRepository.GetByIdAsync(id);
            if (nurse == null) return NotFound();
            return View(nurse);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Nurse nurse)
        {
            if (ModelState.IsValid)
            {
                await _nurseRepository.AddAsync(nurse);
                return RedirectToAction("Index");
            }
            return View(nurse); 
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var nurse = await _nurseRepository.GetByIdAsync(id);
            if (nurse == null) return NotFound();
            return View(nurse);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Nurse nurse)
        {
            nurse.ID = id;

            if (ModelState.IsValid)
            {
                await _nurseRepository.UpdateAsync(nurse);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nurse = await _nurseRepository.GetByIdAsync(id);
            if (nurse == null) return NotFound();
            return View(nurse);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _nurseRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
