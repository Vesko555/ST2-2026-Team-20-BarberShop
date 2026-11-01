using BarberShop.Models;

namespace BarberShop.Services
{
    public sealed class BarberDatabaseSingleton
    {
        private static BarberDatabaseSingleton? _instance;
        private static readonly object _lock = new object();

        public List<Appointment> Appointments { get; private set; }

        private BarberDatabaseSingleton()
        {
            // Начални примерни данни, за да не е празна таблицата
            Appointments = new List<Appointment>
            {
                new Appointment { Id = 1, ClientName = "Иван Иванов", ClientPhone = "0888123456", ServiceType = "Подстригване", DateTime = DateTime.Now.AddDays(1), Notes = "Без забележки" },
                new Appointment { Id = 2, ClientName = "Георги Петров", ClientPhone = "0877654321", ServiceType = "Оформяне на брада", DateTime = DateTime.Now.AddDays(2), Notes = "Скъсен контур" }
            };
        }

        public static BarberDatabaseSingleton Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BarberDatabaseSingleton();
                    }
                    return _instance;
                }
            }
        }
    }
}
