using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class Harclar
{
    public int KursiyerID { get; set; }

    public decimal OdenecekMiktar { get; set; }

    public decimal OdenenMiktar { get; set; }

    public bool OdendiMi { get; set; }

    public virtual Kursiyerler Kursiyer { get; set; } = null!;
}
