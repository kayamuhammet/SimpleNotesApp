# SimpleNotesApp

SimpleNotesApp is a user-friendly note-taking and management application developed using ASP.NET Core MVC.

## 🚀 Features

- 📝 Create, edit and delete notes
- 📂 Organize notes by categories
- ⭐ Mark favorite notes
- 🖨️ Export notes as PDF
- 🔐 Secure user authentication
- 📧 Email verification system
- 🔑 Password reset
- 🌐 Turkish and English language support
- 📱 Responsive design
- 🎨 Modern and user-friendly interface

## 🛠️ Technologies

- ASP.NET Core MVC 7.0
- Entity Framework Core
- SQLite
- Bootstrap 5
- ASP.NET Core Identity
- JWT Authentication
- Localization
- SMTP Email service

## 📋 Requirements

- .NET 7.0 SDK
- Visual Studio 2022 or Visual Studio Code
- SQLite

## 🚀 Installation

1. Clone the project:
```bash
git clone https://github.com/kayamuhammet/SimpleNotesApp.git
```

2. Navigate to the project directory:
```bash
cd SimpleNotesApp
```

3. Install required packages:
```bash
dotnet restore
```

4. Edit `appsettings.json` file:
   - Database connection settings
   - JWT settings
   - Email service settings

5. Create Migrations and Database:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

6. Run the application:
```bash
dotnet run
```

## 🔧 Configuration

### Database
The application uses SQLite database by default. You can change database connection settings in `appsettings.json` file.

### Email Settings
To use email verification and password reset features, you need to configure SMTP settings in `appsettings.json` file.

### Language Support
The application supports Turkish and English languages by default. Language can be changed through the browser.

## 📝 Usage

1. Start the application
2. Register and verify your email
3. Log in
4. Start creating and editing your notes
5. Create categories to organize your notes
6. Add important notes to favorites
7. Export your notes as PDF when needed

## 🤝 Contributing

1. Fork this repository
2. Create your feature branch (`git checkout -b feature/feature-name`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/feature-name`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for more information.

## 📞 Contact

Project Owner - [@kayamuhammet](https://github.com/kayamuhammet)

Project Link: [https://github.com/kayamuhammet/SimpleNotesApp](https://github.com/kayamuhammet/SimpleNotesApp)

---

# SimpleNotesApp

SimpleNotesApp, ASP.NET Core MVC kullanılarak geliştirilmiş, kullanıcı dostu bir not alma ve yönetme uygulamasıdır.

## 🚀 Özellikler

- 📝 Not oluşturma, düzenleme ve silme
- 📂 Kategorilere göre notları organize etme
- ⭐ Favori notları işaretleme
- 🖨️ Notları pdf olarak yazdırma
- 🔐 Güvenli kullanıcı kimlik doğrulama
- 📧 E-posta doğrulama sistemi
- 🔑 Şifre sıfırlama
- 🌐 Türkçe ve İngilizce dil desteği
- 📱 Duyarlı tasarım
- 🎨 Modern ve kullanıcı dostu arayüz

## 🛠️ Teknolojiler

- ASP.NET Core MVC 7.0
- Entity Framework Core
- SQLite
- Bootstrap 5
- ASP.NET Core Identity
- JWT Authentication
- Localization
- SMTP E-posta servisi

## 📋 Gereksinimler

- .NET 7.0 SDK
- Visual Studio 2022 veya Visual Studio Code
- SQLite

## 🚀 Kurulum

1. Projeyi klonlayın:
```bash
git clone https://github.com/kayamuhammet/SimpleNotesApp.git
```

2. Proje dizinine gidin:
```bash
cd SimpleNotesApp
```

3. Gerekli paketleri yükleyin:
```bash
dotnet restore
```

4. `appsettings.json` dosyasını düzenleyin:
   - Veritabanı bağlantı ayarlarını
   - JWT ayarlarını
   - E-posta servisi ayarlarını

5. Migrations ve Veritabanını oluşturun:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

6. Uygulamayı çalıştırın:
```bash
dotnet run
```

## 🔧 Yapılandırma

### Veritabanı
Uygulama varsayılan olarak SQLite veritabanını kullanır. Veritabanı bağlantı ayarlarını `appsettings.json` dosyasından değiştirebilirsiniz.

### E-posta Ayarları
E-posta doğrulama ve şifre sıfırlama özelliklerini kullanmak için `appsettings.json` dosyasında SMTP ayarlarını yapılandırmanız gerekir.

### Dil Desteği
Uygulama varsayılan olarak Türkçe ve İngilizce dil desteği sunar. Dil değişikliği tarayıcı üzerinden yapılabilir.

## 📝 Kullanım

1. Uygulamayı başlatın
2. Kayıt olun ve e-posta adresinizi doğrulayın
3. Giriş yapın
4. Notlarınızı oluşturmaya ve düzenlemeye başlayın
5. Kategoriler oluşturarak notlarınızı organize edin
6. Önemli notları favorilere ekleyin
7. İhtiyaç duyduğunuzda notlarınızı PDF olarak dışa aktarın

## 🤝 Katkıda Bulunma

1. Bu depoyu fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/feature-name`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some feature'`)
4. Branch'inizi push edin (`git push origin feature/feature-name`)
5. Bir Pull Request oluşturun

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakın.

## 📞 İletişim

Proje Sahibi - [@kayamuhammet](https://github.com/kayamuhammet)

Proje Linki: [https://github.com/kayamuhammet/SimpleNotesApp](https://github.com/kayamuhammet/SimpleNotesApp) 