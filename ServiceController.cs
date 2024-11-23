using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace DepiProject.Controllers
{
    [ApiController]
    [Route("Service")]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var d = await _serviceRepository.GetAllAsync();
            
            if (id.HasValue)
            {
                d = d.Where(n => n.ID == id);
            }
            return View(d);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var d = await _serviceRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> Create([FromForm] Service d)
        {
            if (ModelState.IsValid)
            {
                await _serviceRepository.AddAsync(d);
                return RedirectToAction("Index");
            }
            return View(d);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var d = await _serviceRepository.GetByIdAsync(id);
            if (d == null) return NotFound();
            return View(d);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Service s)
        {
            s.ID = id;

            if (ModelState.IsValid)
            {
                await _serviceRepository.UpdateAsync(s);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _serviceRepository.GetByIdAsync(id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}