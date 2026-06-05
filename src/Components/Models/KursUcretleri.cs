using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class KursUcretleri
{
    public string SertifikaSinifi { get; set; } = null!;

    public byte DersSaati { get; set; }

    public decimal SaatUcreti { get; set; }

    public decimal? ToplamUcret { get; set; }
}
