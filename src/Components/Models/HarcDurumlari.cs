using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class HarcDurumlari
{
    public int KursiyerID { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public DateOnly KayitTarihi { get; set; }

    public decimal OdenecekMiktar { get; set; }

    public decimal OdenenMiktar { get; set; }

    public bool OdendiMi { get; set; }
}
