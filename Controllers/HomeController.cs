using Microsoft.AspNetCore.Mvc;
using okumatakibisanalkutuphane.Data;
using okumatakibisanalkutuphane.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace okumatakibisanalkutuphane.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync();
            if (profile == null || !profile.OnboardingCompleted)
            {
                return RedirectToAction("Welcome", "Onboarding");
            }
            ViewBag.UserName = profile.Name;
            
            // Dashboard verilerini hazırla
            var books = await _context.Books.Include(b => b.Sessions).ToListAsync();
            ViewBag.TotalBooks = books.Count;
            ViewBag.FinishedBooks = books.Count(b => b.Status == "Finished");
            ViewBag.ReadingBooks = books.Count(b => b.Status == "Reading");
            
            var goals = await _context.Goals.OrderByDescending(g => g.EndDate).ToListAsync();
            ViewBag.Goals = goals;

            // Son 7 günlük okuma verisi
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i))
                .OrderBy(d => d)
                .ToList();
                
            var sessions = await _context.ReadingSessions.Where(s => s.Date >= last7Days.First()).ToListAsync();
            
            ViewBag.Last7DaysLabels = last7Days.Select(d => d.ToString("dd MMM")).ToList();
            ViewBag.Last7DaysData = last7Days.Select(d => sessions.Where(s => s.Date.Date == d.Date).Sum(s => s.PagesRead)).ToList();

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> SnavFormu()
        {
            ViewBag.PersonnelList = await _context.PersonnelProfiles.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SnavFormu(PersonnelRegistration model)
        {
            // Eğer Yetenekler listesi boşsa model state'e hata ekle (Manuel zorunluluk denetimi)
            if (model.SelectedSkills == null || model.SelectedSkills.Count == 0)
            {
                ModelState.AddModelError("SelectedSkills", "En az bir yetenek seçilmelidir.");
            }

            // Ad ve Soyad ilk harf kontrolü ve otomatik düzeltilmesi
            if (!string.IsNullOrEmpty(model.FirstName) && model.FirstName.Length > 0)
            {
                if (!char.IsUpper(model.FirstName.Trim()[0]))
                {
                    ModelState.AddModelError("FirstName", "Ad alanının ilk harfi büyük olmalıdır.");
                }
            }

            if (!string.IsNullOrEmpty(model.LastName) && model.LastName.Length > 0)
            {
                if (!char.IsUpper(model.LastName.Trim()[0]))
                {
                    ModelState.AddModelError("LastName", "Soyad alanının ilk harfi büyük olmalıdır.");
                }
            }

            if (model.ProfilePhoto == null || model.ProfilePhoto.Length == 0)
            {
                ModelState.AddModelError("ProfilePhoto", "Profil fotoğrafı yüklemek zorunludur.");
            }
            else
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(model.ProfilePhoto.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProfilePhoto", "Sadece resim dosyaları (.jpg, .jpeg, .png, .gif, .webp) yüklenebilir.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Profil fotoğrafını kaydet
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePhoto!.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePhoto!.CopyToAsync(fileStream);
                    }

                    // Veritabanına kaydet
                    var profile = new PersonnelProfile
                    {
                        FirstName = char.ToUpper(model.FirstName.Trim()[0]) + model.FirstName.Trim().Substring(1),
                        LastName = char.ToUpper(model.LastName.Trim()[0]) + model.LastName.Trim().Substring(1),
                        Email = model.Email.Trim(),
                        Phone = model.Phone.Trim(),
                        Password = model.Password,
                        BirthDate = model.BirthDate!.Value,
                        Gender = model.Gender,
                        MaritalStatus = model.MaritalStatus,
                        City = model.City,
                        Department = model.Department,
                        Position = model.Position,
                        Salary = model.Salary!.Value,
                        HireDate = model.HireDate!.Value,
                        WorkType = model.WorkType,
                        Skills = string.Join(", ", model.SelectedSkills),
                        ProfilePhotoPath = "/uploads/" + uniqueFileName,
                        Description = model.Description?.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    _context.PersonnelProfiles.Add(profile);
                    await _context.SaveChangesAsync();

                    // Başarı durumunu ve verileri arayüze ilet
                    ViewBag.Success = true;
                    ViewBag.UploadedPhotoPath = "/uploads/" + uniqueFileName;
                    ViewBag.SubmittedData = model;
                    ViewBag.PersonnelList = await _context.PersonnelProfiles.OrderByDescending(p => p.CreatedAt).ToListAsync();

                    // Modeli sıfırlayarak yeni kayıt için formu boşaltalım
                    return View(new PersonnelRegistration());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Kayıt işlemi gerçekleştirilirken bir hata oluştu: " + ex.Message);
                }
            }

            ViewBag.PersonnelList = await _context.PersonnelProfiles.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Portfolio()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
