using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SurucuKursu.Components.Models;

public partial class SurucuKursuContext : DbContext
{
    public SurucuKursuContext()
    {
    }

    public SurucuKursuContext(DbContextOptions<SurucuKursuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AktifKursiyerler> AktifKursiyerler { get; set; }

    public virtual DbSet<Araclar> Araclar { get; set; }

    public virtual DbSet<BitirenKursiyerler> BitirenKursiyerler { get; set; }

    public virtual DbSet<EgitmenOgretirSinif> EgitmenOgretirSinif { get; set; }

    public virtual DbSet<Egitmenler> Egitmenler { get; set; }

    public virtual DbSet<EgitmenleriGoster> EgitmenleriGoster { get; set; }

    public virtual DbSet<HarcDurumlari> HarcDurumlari { get; set; }

    public virtual DbSet<HarcOdemeleri> HarcOdemeleri { get; set; }

    public virtual DbSet<Harclar> Harclar { get; set; }

    public virtual DbSet<KursUcretleri> KursUcretleri { get; set; }

    public virtual DbSet<KursiyerTumBilgiler> KursiyerTumBilgiler { get; set; }

    public virtual DbSet<Kursiyerler> Kursiyerler { get; set; }

    public virtual DbSet<MaasOdemeleri> MaasOdemeleri { get; set; }

    public virtual DbSet<MaasOdemeleriniGoster> MaasOdemeleriniGoster { get; set; }

    public virtual DbSet<SertifikaSiniflari> SertifikaSiniflari { get; set; }

    public virtual DbSet<Sinavlar> Sinavlar { get; set; }

    public virtual DbSet<SinifKapsarSinif> SinifKapsarSinif { get; set; }

    public virtual DbSet<SonOdemeler> SonOdemeler { get; set; }

    public virtual DbSet<SonSinavlar> SonSinavlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SurucuKursu");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AktifKursiyerler>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AktifKursiyerler");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.OdenecekMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdenenMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.TCKN)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Araclar>(entity =>
        {
            entity.HasKey(e => e.AracID).HasName("PK__Araclar__1E09A830729786F9");

            entity.HasIndex(e => e.Plaka, "UQ__Araclar__830E30F7E3931FB3").IsUnique();

            entity.HasIndex(e => e.Plaka, "ix_Araclar_Plaka");

            entity.Property(e => e.AracKilometresi).HasDefaultValue(0);
            entity.Property(e => e.AracModeli).HasMaxLength(50);
            entity.Property(e => e.Marka).HasMaxLength(20);
            entity.Property(e => e.Plaka).HasMaxLength(10);
            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.VitesCesidi).HasMaxLength(20);

            entity.HasOne(d => d.SertifikaSinifiNavigation).WithMany(p => p.Araclar)
                .HasForeignKey(d => d.SertifikaSinifi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Araclar__Sertifi__46E78A0C");
        });

        modelBuilder.Entity<BitirenKursiyerler>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BitirenKursiyerler");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.OdenenMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.TCKN)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<EgitmenOgretirSinif>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.Egitmen).WithMany()
                .HasForeignKey(d => d.EgitmenID)
                .HasConstraintName("FK__EgitmenOg__Egitm__5EBF139D");

            entity.HasOne(d => d.SertifikaSinifiNavigation).WithMany()
                .HasForeignKey(d => d.SertifikaSinifi)
                .HasConstraintName("FK__EgitmenOg__Serti__5FB337D6");
        });

        modelBuilder.Entity<Egitmenler>(entity =>
        {
            entity.HasKey(e => e.EgitmenID).HasName("PK__Egitmenl__A3C1A3F35CC0BDEB");

            entity.HasIndex(e => e.TCKN, "UQ__Egitmenl__B7734003DB410615").IsUnique();

            entity.HasIndex(e => e.TCKN, "ix_Egitmenler_TCKN");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Maas).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.TCKN)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<EgitmenleriGoster>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("EgitmenleriGoster");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.EgitmenID).ValueGeneratedOnAdd();
            entity.Property(e => e.Maas).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Soyad).HasMaxLength(50);
        });

        modelBuilder.Entity<HarcDurumlari>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HarcDurumlari");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.OdenecekMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdenenMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Soyad).HasMaxLength(50);
        });

        modelBuilder.Entity<HarcOdemeleri>(entity =>
        {
            entity.HasKey(e => e.HarcOdemesiID).HasName("PK__HarcOdem__1AD359319A88DFB7");

            entity.ToTable(tb => tb.HasTrigger("trg_HarcGuncelle"));

            entity.HasIndex(e => e.OdemeTarihi, "ix_HarcOdemeleri_OdemeTarihi").IsDescending();

            entity.Property(e => e.Miktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdemeTarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Kursiyer).WithMany(p => p.HarcOdemeleri)
                .HasForeignKey(d => d.KursiyerID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__HarcOdeme__Kursi__59063A47");
        });

        modelBuilder.Entity<Harclar>(entity =>
        {
            entity.HasKey(e => e.KursiyerID).HasName("PK__Harclar__5E6C70B9904A41F8");

            entity.ToTable(tb => tb.HasTrigger("trg_KursiyerBitirdiMiHarc"));

            entity.Property(e => e.KursiyerID).ValueGeneratedNever();
            entity.Property(e => e.OdenecekMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdenenMiktar).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Kursiyer).WithOne(p => p.Harclar)
                .HasForeignKey<Harclar>(d => d.KursiyerID)
                .HasConstraintName("FK__Harclar__Kursiye__5535A963");
        });

        modelBuilder.Entity<KursUcretleri>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("KursUcretleri");

            entity.Property(e => e.SaatUcreti).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ToplamUcret).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<KursiyerTumBilgiler>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("KursiyerTumBilgiler");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.OdenecekMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdenenMiktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.TCKN)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Kursiyerler>(entity =>
        {
            entity.HasKey(e => e.KursiyerID).HasName("PK__Kursiyer__5E6C70B91C1013EC");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_HarcOlustur");
                    tb.HasTrigger("trg_KursiyerYasiUygunMu");
                });

            entity.HasIndex(e => e.TCKN, "UQ__Kursiyer__B773400391274016").IsUnique();

            entity.HasIndex(e => e.TCKN, "ix_Kursiyerler_TCKN");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.KayitTarihi).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.TCKN)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.SertifikaSinifiNavigation).WithMany(p => p.Kursiyerler)
                .HasForeignKey(d => d.SertifikaSinifi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Kursiyerl__Serti__3C69FB99");
        });

        modelBuilder.Entity<MaasOdemeleri>(entity =>
        {
            entity.HasKey(e => e.HarcOdemesiID).HasName("PK__MaasOdem__1AD35931EC8B9B8E");

            entity.HasIndex(e => e.OdemeTarihi, "ix_MaasOdemeleri_OdemeTarihi").IsDescending();

            entity.Property(e => e.Miktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdemeTarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Egitmen).WithMany(p => p.MaasOdemeleri)
                .HasForeignKey(d => d.EgitmenID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__MaasOdeme__Egitm__5CD6CB2B");
        });

        modelBuilder.Entity<MaasOdemeleriniGoster>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MaasOdemeleriniGoster");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Miktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdemeTarihi).HasColumnType("datetime");
            entity.Property(e => e.Soyad).HasMaxLength(50);
        });

        modelBuilder.Entity<SertifikaSiniflari>(entity =>
        {
            entity.HasKey(e => e.SertifikaSinifi).HasName("PK__Sertifik__C06EB7F66FD79526");

            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.SaatUcreti).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.ToplamUcret)
                .HasComputedColumnSql("(CONVERT([decimal](10,2),[DersSaati]*[SaatUcreti]))", true)
                .HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Sinavlar>(entity =>
        {
            entity.HasKey(e => e.SinavID).HasName("PK__Sinavlar__E089B78692EE146F");

            entity.ToTable(tb => tb.HasTrigger("trg_KursiyerBitirdiMiSinav"));

            entity.HasOne(d => d.Arac).WithMany(p => p.Sinavlar)
                .HasForeignKey(d => d.AracID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Sinavlar__AracID__4E88ABD4");

            entity.HasOne(d => d.Egitmen).WithMany(p => p.Sinavlar)
                .HasForeignKey(d => d.EgitmenID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Sinavlar__Egitme__4D94879B");

            entity.HasOne(d => d.Kursiyer).WithMany(p => p.Sinavlar)
                .HasForeignKey(d => d.KursiyerID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Sinavlar__Kursiy__4CA06362");
        });

        modelBuilder.Entity<SinifKapsarSinif>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.KapsananSinif)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.KapsayiciSinif)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.KapsananSinifNavigation).WithMany()
                .HasForeignKey(d => d.KapsananSinif)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SinifKaps__Kapsa__628FA481");

            entity.HasOne(d => d.KapsayiciSinifNavigation).WithMany()
                .HasForeignKey(d => d.KapsayiciSinif)
                .HasConstraintName("FK__SinifKaps__Kapsa__619B8048");
        });

        modelBuilder.Entity<SonOdemeler>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SonOdemeler");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Miktar).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OdemeTarihi).HasColumnType("datetime");
            entity.Property(e => e.Soyad).HasMaxLength(50);
        });

        modelBuilder.Entity<SonSinavlar>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SonSinavlar");

            entity.Property(e => e.AracModeli).HasMaxLength(50);
            entity.Property(e => e.Eğitmen_Adı)
                .HasMaxLength(50)
                .HasColumnName("Eğitmen Adı");
            entity.Property(e => e.Eğitmen_Soyadı)
                .HasMaxLength(50)
                .HasColumnName("Eğitmen Soyadı");
            entity.Property(e => e.Kursiyer_Adı)
                .HasMaxLength(50)
                .HasColumnName("Kursiyer Adı");
            entity.Property(e => e.Kursiyer_Soyadı)
                .HasMaxLength(50)
                .HasColumnName("Kursiyer Soyadı");
            entity.Property(e => e.Marka).HasMaxLength(20);
            entity.Property(e => e.SertifikaSinifi)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
