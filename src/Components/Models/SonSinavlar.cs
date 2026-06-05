using System;
using System.Collections.Generic;

namespace SurucuKursu.Components.Models;

public partial class SonSinavlar
{
    public DateOnly SinavTarihi { get; set; }

    public string Kursiyer_Adı { get; set; } = null!;

    public string Kursiyer_Soyadı { get; set; } = null!;

    public bool SinavYapildiMi { get; set; }

    public bool BasariliMi { get; set; }

    public string SertifikaSinifi { get; set; } = null!;

    public string? Eğitmen_Adı { get; set; }

    public string? Eğitmen_Soyadı { get; set; }

    public string? Marka { get; set; }

    public string? AracModeli { get; set; }
}
