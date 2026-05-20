using System;

namespace okumatakibisanalkutuphane.Models
{
    public class PersonnelProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Password { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; } = default!;
        public string MaritalStatus { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Department { get; set; } = default!;
        public string Position { get; set; } = default!;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public string WorkType { get; set; } = default!;
        public string Skills { get; set; } = default!; // Comma separated list of skills
        public string ProfilePhotoPath { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
