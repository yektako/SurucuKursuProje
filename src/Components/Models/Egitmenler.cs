using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class Egitmenler
{
    public int EgitmenID { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public decimal Maas { get; set; }

    public string TCKN { get; set; } = null!;

    public DateOnly IseAlmaTarihi { get; set; }

    public DateOnly? DogumTarihi { get; set; }

    public virtual ICollection<MaasOdemeleri> MaasOdemeleri { get; set; } = new List<MaasOdemeleri>();

    public virtual ICollection<Sinavlar> Sinavlar { get; set; } = new List<Sinavlar>();
}
