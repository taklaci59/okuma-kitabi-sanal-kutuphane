# 📚 Okuma Kitabı Sanal Kütüphane — LibrisTrack Pro

Okuduğun kitapları takip et, okuma seansı ekle, hedef belirle ve istatistiklerini Chart.js grafiklerinde izle. Personel kayıt formu, karanlık/açık tema ve PDF/Excel raporlama özellikli kapsamlı bir platform.

## 🚀 Özellikler

- 📖 Kitap ekleme, düzenleme, silme (Durum: Okunuyor / Bitti / Bekliyor)
- ⏱️ Okuma seansı takibi (sayfa bazlı)
- 🎯 Okuma hedefi belirleme
- 📊 Chart.js ile son 7 günlük okuma grafiği
- 🌙 Karanlık / Açık tema (tek tıkla geçiş)
- 📥 Excel ve PDF çıktısı (kitap listesi)
- 👤 Personel Kayıt Formu (tam doğrulamalı, veritabanı destekli)
- 🗂️ Portfolyo sayfası (tüm projeler)

## 🛠️ Kullanılan Teknolojiler

| Katman | Teknoloji |
|--------|-----------|
| Backend | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Veritabanı | SQL Server LocalDB |
| Frontend | Bootstrap 5, Bootstrap Icons |
| Grafik | Chart.js |
| Excel | EPPlus |
| PDF | iText7 |

## ⚙️ Kurulum

### Gereksinimler
- .NET 8 SDK
- SQL Server LocalDB (Visual Studio ile birlikte gelir)

### Adımlar

```bash
# Repoyu klonla
git clone https://github.com/taklaci59/okuma-kitabi-sanal-kutuphane.git
cd okuma-kitabi-sanal-kutuphane

# Veritabanını oluştur ve migration'ları uygula
dotnet ef database update

# Uygulamayı çalıştır
dotnet run
```

Tarayıcıda `https://localhost:5001` adresini aç.

## 🗄️ Veritabanı

`appsettings.json` içindeki bağlantı dizesi:
```
Server=(localdb)\mssqllocaldb;Database=LibrisTrackProDb;Trusted_Connection=True
```

## 📋 Sayfalar

| Sayfa | Açıklama |
|-------|----------|
| Dashboard | İstatistikler, grafik, aktif kitaplar |
| Kütüphanem | Tüm kitaplar, filtre, Excel/PDF çıktı |
| Sınav Formu | Personel kayıt formu (veritabanı destekli) |
| Portfolyo | Tüm projeler ve GitHub linkleri |

## 👤 Geliştirici

**Kıvanç** — [GitHub](https://github.com/taklaci59)
