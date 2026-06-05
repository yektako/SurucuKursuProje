using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class MaasOdemeleriniGoster
{
    public DateTime OdemeTarihi { get; set; }

    public decimal Miktar { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }
}
