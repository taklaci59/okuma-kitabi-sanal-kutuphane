using System.ComponentModel.DataAnnotations;

namespace okumatakibisanalkutuphane.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MonthlyGoalPages { get; set; }
        public bool OnboardingCompleted { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kitap adı zorunludur.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        public string Author { get; set; }
        [Range(1, 10000, ErrorMessage = "Sayfa sayısı 1-10000 arası olmalıdır.")]
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string Genre { get; set; }
        public string Status { get; set; } = "ToRead"; // ToRead, Reading, Finished
        public string ImageUrl { get; set; }
        
        public List<ReadingSession> Sessions { get; set; } = new();

        public int ProgressPercentage => TotalPages > 0 ? (CurrentPage * 100 / TotalPages) : 0;
    }

    public class ReadingSession
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PagesRead => EndPage - StartPage;
        public int DurationMinutes { get; set; }
        
        public Book Book { get; set; }
    }

    public class Goal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TargetPages { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrentProgress { get; set; }
        public bool IsAchieved => CurrentProgress >= TargetPages;
        public int ProgressPercentage => TargetPages > 0 ? (CurrentProgress * 100 / TargetPages) : 0;
    }
}
