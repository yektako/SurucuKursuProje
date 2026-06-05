using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class EgitmenleriGoster
{
    public int EgitmenID { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public decimal Maas { get; set; }

    public DateOnly IseAlmaTarihi { get; set; }

    public DateOnly? DogumTarihi { get; set; }
}
