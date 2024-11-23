using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;
namespace DepiProject.Controllers
{
    [ApiController]
    [Route("Department")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var departments = await _departmentRepository.GetAllAsync();
        //    return Ok(departments);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var department = await _departmentRepository.GetByIdAsync(id);
        //    if (department == null) return NotFound();
        //    return Ok(department);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(Department department)
        //{
        //    await _departmentRepository.AddAsync(department);
        //    return CreatedAtAction(nameof(GetById), new { id = department.ID }, department);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, Department department)
        //{
        //    if (id != department.ID) return BadRequest();
        //    await _departmentRepository.UpdateAsync(department);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _departmentRepository.DeleteAsync(id);
        //    return NoContent();
        //}
        [HttpGet]
        public async Task<IActionResult> Index(string? name,int? id)
        {
            var d = await _departmentRepository.GetAllAsync();
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
            var d = await _departmentRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> Create([FromForm] Department d)
        {
            if (ModelState.IsValid)
            {
                await _departmentRepository.AddAsync(d);
                return RedirectToAction("Index");
            }
            return View(d);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var d = await _departmentRepository.GetByIdAsync(id);
            if (d == null) return NotFound();
            return View(d);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Department d)
        {
            d.ID = id;

            if (ModelState.IsValid)
            {
                await _departmentRepository.UpdateAsync(d);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nurse = await _departmentRepository.GetByIdAsync(id);
            if (nurse == null) return NotFound();
            return View(nurse);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}