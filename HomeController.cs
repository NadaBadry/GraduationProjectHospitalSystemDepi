//using DepiProject.Repository;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace DepiProject.Controllers
//{
//    public class HomeController : Controller
//    {

//        public IActionResult Index()
//        {
//            ViewData["Title"] = "Dashboard";

//            return View();
//        }
//    }
//}

using DepiProject.Interfaces;
using DepiProject.Models;
using DepiProject.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DepiProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<Nurse> _nurseRepository;

        public HomeController(IGenericRepository<Appointment> appointmentRepository, IGenericRepository<Doctor> doctorRepository, IGenericRepository<Patient> patientRepository, IGenericRepository<Nurse> nurseRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _nurseRepository = nurseRepository;
        }

        public async Task<IActionResult> Index()
        {
            var appointmentsCount = await _appointmentRepository.GetAllAsync();
            var doctorsCount = await _doctorRepository.GetAllAsync();
            var patientsCount = await _patientRepository.GetAllAsync();
            var nursesCount = await _nurseRepository.GetAllAsync();

            var viewModel = new DashboardViewModel
            {
                AppointmentsCount = appointmentsCount.Count(),
                DoctorsCount = doctorsCount.Count(),
                PatientsCount = patientsCount.Count(),
                NursesCount = nursesCount.Count()
            };

            return View(viewModel);
        }

        
    }

    public class DashboardViewModel
    {
        public int AppointmentsCount { get; set; }
        public int DoctorsCount { get; set; }
        public int PatientsCount { get; set; }
        public int NursesCount { get; set; }
    }
}
