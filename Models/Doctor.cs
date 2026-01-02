using System.ComponentModel.DataAnnotations;

namespace HealthMate.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Specialization { get; set; } = "";
        public string Qualification { get; set; } = "";
        public string Experience { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
    }
}