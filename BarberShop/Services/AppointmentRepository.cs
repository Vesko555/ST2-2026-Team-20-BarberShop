using BarberShop.Models;

namespace BarberShop.Services
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAll();
        Appointment? GetById(int id);
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(int id);
    }

    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BarberDatabaseSingleton _db = BarberDatabaseSingleton.Instance;

        public IEnumerable<Appointment> GetAll() => _db.Appointments;

        public Appointment? GetById(int id) => _db.Appointments.FirstOrDefault(a => a.Id == id);

        public void Add(Appointment appointment)
        {
            appointment.Id = _db.Appointments.Any() ? _db.Appointments.Max(a => a.Id) + 1 : 1;
            _db.Appointments.Add(appointment);
        }

        public void Update(Appointment appointment)
        {
            var existing = GetById(appointment.Id);
            if (existing != null)
            {
                existing.ClientName = appointment.ClientName;
                existing.ClientPhone = appointment.ClientPhone;
                existing.ServiceType = appointment.ServiceType;
                existing.DateTime = appointment.DateTime;
                existing.Notes = appointment.Notes;
            }
        }

        public void Delete(int id)
        {
            var appointment = GetById(id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
            }
        }
    }
}
