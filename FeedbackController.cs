using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DepiProject.Controllers
{
	[ApiController]
	[Route("Feedback")]
	public class FeedbackController : Controller
	{
		private readonly INurseRepository _nurseRepository;

		private readonly IFeedbackRepository _feedbackRepository;
		public FeedbackController(IFeedbackRepository feedbackRepository)
		{
			_feedbackRepository = feedbackRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var feedbacks = await _feedbackRepository.GetAllAsync();
			return View(feedbacks);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Details(int id)
		{
			var feedback = await _feedbackRepository.GetByIdAsync(id);
			if (feedback == null) return NotFound();
			return View(feedback);
		}

		[HttpGet("Create")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] Feedback feedback)
		{
			if (ModelState.IsValid)
			{
				await _feedbackRepository.AddAsync(feedback);
				return RedirectToAction("Index");
			}
			return View(feedback);
		}

		[HttpGet("Edit/{id}")]
		public async Task<IActionResult> Edit(int id)
		{
			var feedback = await _feedbackRepository.GetByIdAsync(id);
			if (feedback == null) return NotFound();
			return View(feedback);
		}

		[HttpPost("Edit/{id}")]
		public async Task<IActionResult> Edit(int id, [FromForm] Feedback feedback)
		{
			feedback.ID = id;

			if (ModelState.IsValid)
			{
				await _feedbackRepository.UpdateAsync(feedback);
				return RedirectToAction(nameof(Index));
			}
			return BadRequest();
		}


		[HttpGet("Delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var feedback = await _feedbackRepository.GetByIdAsync(id);
			if (feedback == null) return NotFound();
			return View(feedback);
		}

		[HttpPost("Delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _feedbackRepository.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
