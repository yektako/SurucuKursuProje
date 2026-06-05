using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class HarcOdemeleri
{
    public int HarcOdemesiID { get; set; }

    public int? KursiyerID { get; set; }

    public decimal Miktar { get; set; }

    public DateTime OdemeTarihi { get; set; }

    public virtual Kursiyerler? Kursiyer { get; set; }
}
