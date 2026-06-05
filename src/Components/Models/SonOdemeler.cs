using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class SonOdemeler
{
    public DateTime OdemeTarihi { get; set; }

    public decimal Miktar { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;
}
