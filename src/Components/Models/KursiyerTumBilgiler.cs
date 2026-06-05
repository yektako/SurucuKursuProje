using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class KursiyerTumBilgiler
{
    public int KursiyerID { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string TCKN { get; set; } = null!;

    public DateOnly DogumTarihi { get; set; }

    public DateOnly KayitTarihi { get; set; }

    public string SertifikaSinifi { get; set; } = null!;

    public decimal OdenecekMiktar { get; set; }

    public decimal OdenenMiktar { get; set; }

    public bool OdendiMi { get; set; }

    public bool? BasariliMi { get; set; }

    public bool BitirdiMi { get; set; }
}
