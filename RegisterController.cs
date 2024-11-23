using DepiProject.DbContextLayer;
using DepiProject.Migrations;
using DepiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace DepiProject.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HospitalContext _context;

        public RegisterController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users { Username = model.Username, Password = model.Password, Email = model.Email, Role = model.Role };
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "Login");
            }
            return View(model);
        }

    }
}
