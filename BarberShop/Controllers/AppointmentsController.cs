using BarberShop.Models;
using BarberShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _repository = new AppointmentRepository();

        // 1. SELECT WITH FILTER
        public IActionResult Index(string? searchName, string? serviceType)
        {
            var appointments = _repository.GetAll();

            if (!string.IsNullOrEmpty(searchName))
            {
                appointments = appointments.Where(a => a.ClientName.Contains(searchName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(serviceType))
            {
                appointments = appointments.Where(a => a.ServiceType == serviceType);
            }

            ViewData["CurrentNameFilter"] = searchName;
            ViewData["CurrentServiceFilter"] = serviceType;

            return View(appointments.ToList());
        }

        // 2. INSERT - GET
        public IActionResult Create()
        {
            return View();
        }

        // 2. INSERT - POST С интегриран Gemini AI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(appointment);

                var aiService = new GeminiAiService();
                var aiTip = await aiService.GetHaircutRecommendationAsync(appointment.ServiceType);

                
                TempData["AiSuccessMessage"] = $"Часът е записан успешно! AI съвет: {aiTip}";

                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // 3. UPDATE (EDIT) - GET
        public IActionResult Edit(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // 3. UPDATE (EDIT) - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(appointment);
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // 4. DELETE - GET Потвърждение
        public IActionResult Delete(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // 4. DELETE - POST Самото изтриване
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}