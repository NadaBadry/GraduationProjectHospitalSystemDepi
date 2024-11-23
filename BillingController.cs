using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
	[ApiController]
	[Route("Billing")]
	public class BillingController : Controller
	{
		private readonly IBillingRepository _billingRepository;

		public BillingController(IBillingRepository billingRepository)
		{
			_billingRepository = billingRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int? id)
		{
			var billings = await _billingRepository.GetAllAsync();
            if (id.HasValue)
            {
                billings = billings.Where(n => n.ID == id);
            }
            return View(billings);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Details(int id)
		{
			var b = await _billingRepository.GetByIdAsync(id);
			if (b == null) return NotFound();
			return View(b);
		}

		[HttpGet("Create")]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] Billing b)
		{
			if (ModelState.IsValid)
			{
				await _billingRepository.AddAsync(b);
				return RedirectToAction(nameof(Index));
			}
			return View(b);
		}



        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var b = await _billingRepository.GetByIdAsync(id);
            if (b == null) return NotFound();
            return View(b);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Billing billing)
        {
            billing.ID = id;

            if (ModelState.IsValid)
            {
                await _billingRepository.UpdateAsync(billing);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }



        [HttpGet("Delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var b = await _billingRepository.GetByIdAsync(id);
			if (b == null) return NotFound();
			return View(b);
		}

		[HttpPost("Delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _billingRepository.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
