using Microsoft.AspNetCore.Mvc;
using okumatakibisanalkutuphane.Data;
using okumatakibisanalkutuphane.Models;
using Microsoft.EntityFrameworkCore;

namespace okumatakibisanalkutuphane.Controllers
{
    public class OnboardingController : Controller
    {
        private readonly AppDbContext _context;

        public OnboardingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Welcome()
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync();
            if (profile != null && profile.OnboardingCompleted)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Welcome(string name, int monthlyGoal)
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync();
            if (profile == null)
            {
                profile = new UserProfile
                {
                    Name = name,
                    MonthlyGoalPages = monthlyGoal,
                    OnboardingCompleted = true
                };
                _context.UserProfiles.Add(profile);
            }
            else
            {
                profile.Name = name;
                profile.MonthlyGoalPages = monthlyGoal;
                profile.OnboardingCompleted = true;
                _context.UserProfiles.Update(profile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
