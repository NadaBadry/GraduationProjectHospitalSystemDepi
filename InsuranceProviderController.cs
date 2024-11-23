using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DepiProject.Controllers
{
   
    
    




    [ApiController]
    [Route("InsuranceProvider")]
    /*[Authorize(Roles = "Admin")]*/ // Add this attribute  
    public class InsuranceProviderController : Controller
    {
        private readonly IInsuranceProviderRepository _insuranceProviderRepository;
        public InsuranceProviderController(IInsuranceProviderRepository insuranceProviderRepository)
        {
            _insuranceProviderRepository = insuranceProviderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var nurses = await _insuranceProviderRepository.GetAllAsync();
            if (id.HasValue)
            {
                nurses = nurses.Where(n => n.ID == id);
            }
            return View(nurses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var nurse = await _insuranceProviderRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> Create([FromForm] InsuranceProvider insuranceProvider)
        {
            if (ModelState.IsValid)
            {
                await _insuranceProviderRepository.AddAsync(insuranceProvider);
                return RedirectToAction("Index");
            }
            return View(insuranceProvider);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var insuranceProvider = await _insuranceProviderRepository.GetByIdAsync(id);
            if (insuranceProvider == null) return NotFound();
            return View(insuranceProvider);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] InsuranceProvider insuranceProvider)
        {
            insuranceProvider.ID = id;

            if (ModelState.IsValid)
            {
                await _insuranceProviderRepository.UpdateAsync(insuranceProvider);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var insuranceProvider = await _insuranceProviderRepository.GetByIdAsync(id);
            if (insuranceProvider == null) return NotFound();
            return View(insuranceProvider);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _insuranceProviderRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

