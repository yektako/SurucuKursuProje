using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class SertifikaSiniflari
{
    public string SertifikaSinifi { get; set; } = null!;

    public byte DersSaati { get; set; }

    public decimal SaatUcreti { get; set; }

    public decimal ToplamUcret { get; set; }

    public byte YasSiniri { get; set; }

    public virtual ICollection<Araclar> Araclar { get; set; } = new List<Araclar>();

    public virtual ICollection<Kursiyerler> Kursiyerler { get; set; } = new List<Kursiyerler>();
}
