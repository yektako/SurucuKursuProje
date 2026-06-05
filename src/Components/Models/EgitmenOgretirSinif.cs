using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class EgitmenOgretirSinif
{
    public int EgitmenID { get; set; }

    public string SertifikaSinifi { get; set; } = null!;

    public virtual Egitmenler Egitmen { get; set; } = null!;

    public virtual SertifikaSiniflari SertifikaSinifiNavigation { get; set; } = null!;
}
