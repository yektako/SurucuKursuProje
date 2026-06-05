using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class Kursiyerler
{
    public int KursiyerID { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string SertifikaSinifi { get; set; } = null!;

    public string TCKN { get; set; } = null!;

    public DateOnly DogumTarihi { get; set; }

    public DateOnly KayitTarihi { get; set; }

    public bool BitirdiMi { get; set; }

    public virtual ICollection<HarcOdemeleri> HarcOdemeleri { get; set; } = new List<HarcOdemeleri>();

    public virtual Harclar? Harclar { get; set; }

    public virtual SertifikaSiniflari SertifikaSinifiNavigation { get; set; } = null!;

    public virtual ICollection<Sinavlar> Sinavlar { get; set; } = new List<Sinavlar>();
}
