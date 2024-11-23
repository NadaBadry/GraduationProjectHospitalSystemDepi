using DepiProject.DbContextLayer;
using DepiProject.Models;
using Microsoft.AspNetCore.Mvc;


using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DepiProject.Migrations;
using Microsoft.AspNetCore.Authentication;


namespace DepiProject.Controllers
{
	public class LoginController : Controller
	{
		private readonly HospitalContext _context;

		public LoginController(HospitalContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
				if (user != null)
				{
					var token = GenerateToken(user);
					if (user.Role == "Admin")
					{
						return RedirectToAction("Index", "Home", new { token });
					}
					else
					{
						return RedirectToAction("Index", "HomePage", new { token });
					}
				}
				else
				{
					ModelState.AddModelError("", "Invalid username or password");
				}
			}
			return View(model);
		}


		private string GenerateToken(Users user)
		{
			var claims = new[]
			{
	new Claim(ClaimTypes.Name, user.Username),
	new Claim(ClaimTypes.Email, user.Email),
	new Claim(ClaimTypes.Role, user.Role) // Add this claim  
   };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(30), signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		[HttpPost]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync();
			return RedirectToAction("Index", "HomePage");
		}

	}
}
