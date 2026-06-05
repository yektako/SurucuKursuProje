using Microsoft.EntityFrameworkCore;
using SurucuKursu.Components.Models;

namespace SurucuKursu.Isimler;

public static class Sozluk
{
    public static readonly string[] Tablolar = {
        "KursiyerTumBilgiler",
        "AktifKursiyerler",
        "BitirenKursiyerler",
        "HarcDurumlari",
        "SonOdemeler",
        "SonSinavlar",
        "KursUcretleri",
        "MaasOdemeleriniGoster",
        "EgitmenleriGoster",
    };
    public static readonly string[] TabloIsimleri = {
        "Tüm Kursiyerler",
        "Aktif Kursiyerler",
        "Bitiren Kursiyerler",
        "Harçlar",
        "Son Ödemeler",
        "Son Sınavlar",
        "Sınıflar",
        "Maaş Ödemeleri",
        "Eğitmenler",
        };
    private static readonly Dictionary<string,string> terimler = new Dictionary<string,string>
    {
        {"False", "Hayır"},
        {"True", "Evet"},
        {"SertifikaSinifi", "Sertifika Sınıfı"},
        {"DersSaati", "Ders Saati"},
        {"SaatUcreti", "Saat Ücreti"},
        {"ToplamUcret", "Toplam Ücret"},
        {"AracModeli", "Araç Modeli"},
        {"Yil", "Yıl"},
        {"VitesCesidi", "Vites Çeşidi"},
        {"AracKilometresi", "Kilometre"},
        {"OdendiMi", "Ödendi Mi"},
        {"BasariliMi", "Başarılı Mı"},
        {"BitirdiMi", "Bitirdi Mi"},
        {"SinavYapildiMi", "Sınav Yapıldı Mı"},
        {"OdenecekMiktar", "Ödenecek Miktar"},
        {"OdenenMiktar", "Ödenen Miktar"},
        {"KayitTarihi", "Kayıt Tarihi"},
        {"SinavTarihi", "Sınav Tarihi"},
        {"DogumTarihi", "Doğum Tarihi"},
        {"YasSiniri", "Yaş Sınırı"},
        {"KursiyerID", "ID"},
        {"EgitmenID", "ID"},
    };
    public static string Donustur(string yazi)
    {
        if (terimler.ContainsKey(yazi)) return terimler[yazi];
        else return yazi;
    }
}

