using DepiProject.DbContextLayer;
using DepiProject.Interfaces;
using DepiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepiProject.Controllers
{
    public class HomePageController : Controller
    {
		private readonly HospitalContext _context;

		private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<Nurse> _nurseRepository;
        private readonly IGenericRepository<Feedback> _feedbackRepository;
		private readonly IGenericRepository<Department> _departmentRepository;

		private readonly IGenericRepository<Service> _serviceRepository;



		public HomePageController(IGenericRepository<Appointment> appointmentRepository, 
            IGenericRepository<Doctor> doctorRepository, 
            IGenericRepository<Patient> patientRepository, 
            IGenericRepository<Nurse> nurseRepository,
			IGenericRepository<Feedback> feedbackRepository,
			IGenericRepository<Department> departmentRepository,
			IGenericRepository<Service> serviceRepository,
			HospitalContext context
			)
		{
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _nurseRepository = nurseRepository;
            _feedbackRepository = feedbackRepository;
			_departmentRepository = departmentRepository;
			_serviceRepository = serviceRepository;
			_context = context;


		}
		public IActionResult Index()
        {
            return View();
        }

       

		public async Task<IActionResult> Doctors()
        {
            var doctor = await _doctorRepository.GetAllAsync();

            return View(doctor);
		}


		[HttpGet]
		public IActionResult Feedback()
		{
			ViewBag.Patients = _patientRepository.GetAllAsync().Result;
			ViewBag.Doctors = _doctorRepository.GetAllAsync().Result;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Feedback(Feedback feedback)
		{
			if (ModelState.IsValid)
			{
				await _feedbackRepository.AddAsync(feedback);
				return RedirectToAction("Index");
			}
			ViewBag.Patients = _patientRepository.GetAllAsync().Result;
			ViewBag.Doctors = _doctorRepository.GetAllAsync().Result;

			return View(feedback);
		}






		public async Task<IActionResult> FeedbackSidebar()
		{
			var feedbacks = await _feedbackRepository.GetAllAsync();
			return PartialView("FeedbackSidebar", feedbacks);
		}


		public async Task<IActionResult> Departments()
		{
			var departments = await _departmentRepository.GetAllAsync();
			return View(departments);
		}


		public async Task<IActionResult> Services()
		{
			var services = await _serviceRepository.GetAllAsync();
			return View(services);
		}



        [HttpGet]
        public IActionResult CreateAppointment()
        {
            ViewBag.Doctors = _doctorRepository.GetAllAsync().Result;
            ViewBag.Patients = _patientRepository.GetAllAsync().Result;
            ViewBag.Nurses = _nurseRepository.GetAllAsync().Result;
            var appointment = new Appointment { Status = "Unscheduled" };
            return View(appointment);
        }

        [HttpPost]
		public async Task<IActionResult> CreateAppointment(Appointment appointment)
		{
			if (ModelState.IsValid)
			{
				await _appointmentRepository.AddAsync(appointment);
				return RedirectToAction("Index");
			}
			ViewBag.Doctors = _doctorRepository.GetAllAsync().Result;
			ViewBag.Patients = _patientRepository.GetAllAsync().Result;
			ViewBag.Nurses = _nurseRepository.GetAllAsync().Result;
			return View(appointment);
		}


		
		
		public IActionResult ContactUs()
		{
			
			return View();
		}

		[HttpGet]
		public IActionResult AboutUs()
		{
			return View();
		}



		public IActionResult GetPatientview()
		{
			return View();
		}

		[HttpPost]
		public IActionResult GetPatientview(int id)
		{
			// Retrieve the patient data from the database  
			var patient = _context.Patients
			 .Include(p => p.Appointments)
			 .Include(p => p.MedicalRecords)
			 .Include(p => p.Billings)
			 .Include(p => p.Feedbacks)
			 .Include(p => p.Medications)
			 .FirstOrDefault(p => p.ID == id);

			if (patient == null)
			{
				ViewBag.ErrorMessage = "No patient found with ID " + id;
				return View();
			}

			// Create a view model to hold the patient data  
			var viewModel = new PatientDataViewModel
			{
				Patient = patient,
				Appointments = patient.Appointments,
				MedicalRecords = patient.MedicalRecords,
				Billings = patient.Billings,
				Feedbacks = patient.Feedbacks,
				Medications = patient.Medications
			};

			return View(viewModel);
		}





	}
}
