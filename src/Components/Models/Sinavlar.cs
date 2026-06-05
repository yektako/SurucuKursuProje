using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class Sinavlar
{
    public int SinavID { get; set; }

    public int? KursiyerID { get; set; }

    public int? EgitmenID { get; set; }

    public int? AracID { get; set; }

    public DateOnly SinavTarihi { get; set; }

    public bool SinavYapildiMi { get; set; }

    public bool BasariliMi { get; set; }

    public virtual Araclar? Arac { get; set; }

    public virtual Egitmenler? Egitmen { get; set; }

    public virtual Kursiyerler? Kursiyer { get; set; }
}
