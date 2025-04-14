# MusicApp - Müzik Uygulaması
# Projenin tasarım kısımları daha modern bir yapıya dönüştürülecektir.Yapıldıktan sonra Bu kısım güncellenecek.

MusicApp, kullanıcıların müzik dinleme, favorilere ekleme ve çalma listeleri oluşturma gibi işlemleri yapabildiği bir web uygulamasıdır. Kullanıcılar, şarkıları dinleyebilir, şarkı detaylarına göz atabilir ve şarkıları favorilerine ekleyebilir. Ayrıca, kullanıcılar farklı çalma listeleri oluşturup bu listelere şarkılar ekleyebilir.

## Proje Hakkında

Bu proje, müzik dinleme uygulaması geliştirmenin yanı sıra, kullanıcı etkileşimini artırmak için çeşitli özelliklere sahiptir:
- Şarkıları dinleyin
- Favorilere ekleyin
- Çalma listeleri oluşturun ve şarkıları ekleyin
- Şarkıları başlık, sanatçı veya albüm adına göre arayın
- Şarkı dinlendiğinde dinlenme sayısını artırma özelliği

## Kullanılan Araçlar ve Teknolojiler

Bu projede aşağıdaki araçlar ve teknolojiler kullanılmıştır:

- **ASP.NET Core MVC**: Web uygulamasının temel yapısını oluşturmak için kullanıldı.
- **Entity Framework Core**: Veritabanı işlemleri ve modelleme için kullanıldı.
- **SQL Server**: Veritabanı olarak kullanıldı.
- **HTML/CSS**: Kullanıcı arayüzünü geliştirmek için temel web teknolojileri.
- **jQuery**: Dinlenme sayısını artırmak için Ajax ile etkileşim sağlamak amacıyla kullanıldı.
- **Bootstrap**: UI bileşenlerini hızlıca geliştirmek için kullanıldı.
- **JavaScript**: Dinlenme sayısının artırılması için Ajax çağrıları ve dinleme kontrolleri için kullanıldı.

## Proje Özellikleri

### 1. **Kullanıcı Girişi ve Kayıt**
- Kullanıcılar sisteme giriş yapabilir veya yeni bir hesap oluşturabilir.
- Giriş yaptıktan sonra kullanıcı, profil bilgilerini görüntüleyebilir ve düzenleyebilir.

### 2. **Şarkı Dinleme**
- Şarkılar listelenebilir ve kullanıcılar şarkıları dinleyebilir.
- Dinlenen şarkıların **dinlenme sayısı** artırılır.

### 3. **Favorilere Ekleme**
- Kullanıcılar beğendikleri şarkıları favorilerine ekleyebilir.

### 4. **Çalma Listeleri**
- Kullanıcılar birden fazla **çalma listesi** oluşturabilir ve şarkıları bu listelere ekleyebilir.

### 5. **Arama**
- Kullanıcılar şarkıları **başlık, sanatçı, albüm** adıyla arayabilir.

## Kurulum ve Çalıştırma

Projeyi yerel bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz

1. **Depoyu Klonlayın**:
   Projeyi kendi bilgisayarınıza klonlamak için aşağıdaki komutu kullanın:
   ```bash
   git clone https://github.com/murattyildiz/musicapp.git

Veritabanı Bağlantısını Yapılandırın: MusicAppDbContext bağlantı dizesi (Connection String) ve veritabanı yapılandırmasını appsettings.json dosyasında yapmanız gerekmektedir.
appsettings.json dosyasını açın ve veritabanı bağlantı dizesini kendi ortamınıza göre düzenleyin. Örnek bir bağlantı dizesi şu şekilde olabilir:

{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=MusicAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
}

Bağlantı dizesini ve veritabanı yapılandırmalarını kendi SQL Server veya veritabanı sunucunuz ile uyumlu olacak şekilde değiştirin.

## Uygulanacak Komutlar
 dotnet ef migrations add mig

dotnet ef database update
komutlarını uygulayın.Bu adımlarla proje kolayca kurulup çalıştırılabilirsiniz.

# Proje Görselleri

# Kullanıcı Kısmı

## Şarkılar Listesi
![resim](https://github.com/user-attachments/assets/60110102-d525-47a9-b2b8-2e6ea1cf6444)

## Şarkı detay Sayfası
![resim5](https://github.com/user-attachments/assets/cffae9cf-4bbb-4ed3-b95c-761888d4a054)

## Favori Şarkılarım
![resim2](https://github.com/user-attachments/assets/3078b0f0-3e76-4fa5-9723-d4fe1716b901)

## Çalma Listelerim
![resim3](https://github.com/user-attachments/assets/969cc946-d6be-423d-a89f-ac051ba728a4)

## En çok dinlenen Şarkılar Sayfası
![resim10](https://github.com/user-attachments/assets/bdc44c40-f848-4ca8-8d7a-49c1bbd20d3a)

# Admin Kısmı

## Şarkıları düzenleme,ekleme sayfaları
![resim6](https://github.com/user-attachments/assets/ad690286-577d-44bd-bc56-752658ba95c4)

![resim9](https://github.com/user-attachments/assets/4637ac9b-ae9b-4769-bcf1-217df2d854f7)


## Profilim Sayfası
![resim8](https://github.com/user-attachments/assets/f263e4c5-7236-475e-af78-31df65e0812d)

## Kullanıcılar Bilgisi
![resim7](https://github.com/user-attachments/assets/2944a744-2938-40d1-9bda-b50cc36eaef9)
