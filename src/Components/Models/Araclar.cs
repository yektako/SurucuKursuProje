using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class Araclar
{
    public int AracID { get; set; }

    public string Plaka { get; set; } = null!;

    public string SertifikaSinifi { get; set; } = null!;

    public string VitesCesidi { get; set; } = null!;

    public string? Marka { get; set; }

    public string? AracModeli { get; set; }

    public short? Yil { get; set; }

    public int? AracKilometresi { get; set; }

    public virtual SertifikaSiniflari SertifikaSinifiNavigation { get; set; } = null!;

    public virtual ICollection<Sinavlar> Sinavlar { get; set; } = new List<Sinavlar>();
}
