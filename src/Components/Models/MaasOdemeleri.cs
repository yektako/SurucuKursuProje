using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class MaasOdemeleri
{
    public int HarcOdemesiID { get; set; }

    public int? EgitmenID { get; set; }

    public decimal Miktar { get; set; }

    public DateTime OdemeTarihi { get; set; }

    public virtual Egitmenler? Egitmen { get; set; }
}
