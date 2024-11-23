using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

namespace DepiProject.Controllers
{
    [ApiController]
    [Route("MedicalRecord")]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordController(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var nurses = await _medicalRecordRepository.GetAllAsync();
            if (id.HasValue)
            {
                nurses = nurses.Where(n => n.PatientID == id);
            }
            return View(nurses);
        }















		[HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var nurse = await _medicalRecordRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> Create([FromForm] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                await _medicalRecordRepository.AddAsync(medicalRecord);
                return RedirectToAction("Index");
            }
            return View(medicalRecord);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
            if (medicalRecord == null) return NotFound();
            return View(medicalRecord);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] MedicalRecord medicalRecord)
        {
            medicalRecord.ID = id;

            if (ModelState.IsValid)
            {
                await _medicalRecordRepository.UpdateAsync(medicalRecord);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
            if (medicalRecord == null) return NotFound();
            return View(medicalRecord);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _medicalRecordRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
