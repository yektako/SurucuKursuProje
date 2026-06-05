using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class SinifKapsarSinif
{
    public string KapsayiciSinif { get; set; } = null!;

    public string KapsananSinif { get; set; } = null!;

    public virtual SertifikaSiniflari KapsananSinifNavigation { get; set; } = null!;

    public virtual SertifikaSiniflari KapsayiciSinifNavigation { get; set; } = null!;
}
