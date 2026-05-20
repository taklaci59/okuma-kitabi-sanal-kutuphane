using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace okumatakibisanalkutuphane.Models
{
    public class PersonnelRegistration
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        [FirstLetterCapital(ErrorMessage = "Ad alanının ilk harfi büyük olmalıdır.")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        [FirstLetterCapital(ErrorMessage = "Soyad alanının ilk harfi büyük olmalıdır.")]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Telefon alanı sadece sayı kabul edebilir.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; } = default!;

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "Doğum tarihi gelecek bir zaman olamaz.")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Medeni durum seçimi zorunludur.")]
        [Display(Name = "Medeni Durum")]
        public string MaritalStatus { get; set; } = default!;

        [Required(ErrorMessage = "Şehir seçimi zorunludur.")]
        [Display(Name = "Şehir")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Departman seçimi zorunludur.")]
        [Display(Name = "Departman")]
        public string Department { get; set; } = default!;

        [Required(ErrorMessage = "Maaş alanı zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Maaş negatif olamaz.")]
        [Display(Name = "Maaş")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Pozisyon seçimi zorunludur.")]
        [Display(Name = "Pozisyon")]
        public string Position { get; set; } = default!;

        [Required(ErrorMessage = "Çalışma şekli seçimi zorunludur.")]
        [Display(Name = "Çalışma Şekli")]
        public string WorkType { get; set; } = default!;

        [Required(ErrorMessage = "İşe giriş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "İşe giriş tarihi gelecek bir zaman olamaz.")]
        [Display(Name = "İşe Giriş Tarihi")]
        public DateTime? HireDate { get; set; }

        [Required(ErrorMessage = "En az bir yetenek seçilmelidir.")]
        [Display(Name = "Yetenekler")]
        public List<string> SelectedSkills { get; set; } = new();

        [Required(ErrorMessage = "Profil fotoğrafı zorunludur.")]
        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile ProfilePhoto { get; set; } = default!;

        [StringLength(200, ErrorMessage = "Açıklama alanı en fazla 200 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur.")]
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; } = default!;
    }

    public class PastOrPresentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Date > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage ?? "Tarih gelecek bir zaman olamaz.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class FirstLetterCapitalAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                if (!char.IsUpper(str[0]))
                {
                    return new ValidationResult(ErrorMessage ?? "İlk harf büyük olmalıdır.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
