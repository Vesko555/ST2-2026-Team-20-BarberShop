namespace BarberShop.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty; 
        public DateTime DateTime { get; set; }
        public string? Notes { get; set; }
    }
}
