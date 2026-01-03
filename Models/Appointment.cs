using System.ComponentModel.DataAnnotations;

namespace HealthMate.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientId { get; set; } = "";
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = "";
        public string Reason { get; set; } = "";
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}