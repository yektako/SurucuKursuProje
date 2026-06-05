CREATE DATABASE SurucuKursu
GO

USE SurucuKursu

CREATE TABLE SertifikaSiniflari (
	SertifikaSinifi varchar(3) PRIMARY KEY,
	DersSaati tinyint NOT NULL,
	SaatUcreti decimal(6,2) NOT NULL,
	ToplamUcret AS CONVERT( decimal(10,2), (DersSaati*SaatUcreti) ) PERSISTED NOT NULL,
	YasSiniri tinyint NOT NULL
)
GO


CREATE TABLE Kursiyerler (
	KursiyerID int IDENTITY(1,1) PRIMARY KEY,
	Ad nvarchar(50) NOT NULL,
	Soyad nvarchar(50) NOT NULL,
	SertifikaSinifi varchar(3) NOT NULL,
	TCKN char(11) NOT NULL UNIQUE,
	DogumTarihi date NOT NULL,
	KayitTarihi date NOT NULL DEFAULT CONVERT(DATE, GETDATE()),
	BitirdiMi bit NOT NULL DEFAULT 0,
	FOREIGN KEY (SertifikaSinifi) REFERENCES SertifikaSiniflari(SertifikaSinifi),
	CONSTRAINT ck_TCKN_Uzunluk CHECK (LEN(TCKN) = 11)
)
GO

CREATE TABLE Egitmenler (
	EgitmenID int IDENTITY(1,1) PRIMARY KEY,
	Ad nvarchar(50) NOT NULL,
	Soyad nvarchar(50) NOT NULL,
	Maas decimal(10,2) NOT NULL,
	TCKN char(11) NOT NULL UNIQUE,
	IseAlmaTarihi date NOT NULL,
	DogumTarihi date NULL
)
GO

CREATE TABLE Araclar (
	AracID int IDENTITY(1,1) PRIMARY KEY,
	Plaka nvarchar(10) NOT NULL UNIQUE,
	SertifikaSinifi varchar(3) NOT NULL,
	VitesCesidi nvarchar(20) NOT NULL CHECK (VitesCesidi IN ('Manuel','Otomatik')),
	Marka nvarchar(20) NULL,
	AracModeli nvarchar(50) NULL,
	Yil smallint NULL CHECK ( Yil BETWEEN 1970 AND YEAR(GETDATE()) ),
	AracKilometresi int NULL DEFAULT 0,
	FOREIGN KEY (SertifikaSinifi) REFERENCES SertifikaSiniflari(SertifikaSinifi),
	CONSTRAINT ck_PlakaGecerli CHECK (
	(Plaka LIKE '[0-8][0-9] [a-z][a-z] [0-9][0-9]') OR
	(Plaka LIKE '[0-8][0-9] [a-z][a-z] [0-9][0-9][0-9]') OR
	(Plaka LIKE '[0-8][0-9] [a-z][a-z][a-z] [0-9][0-9]') OR
	(Plaka LIKE '[0-8][0-9] [a-z][a-z][a-z] [0-9][0-9][0-9]') )
)
GO

CREATE TABLE Sinavlar (
	SinavID int IDENTITY(1,1) PRIMARY KEY,
	KursiyerID int NULL,
	EgitmenID int NULL,
	AracID int NULL,
	SinavTarihi date NOT NULL,
	SinavYapildiMi bit NOT NULL DEFAULT 0,
	BasariliMi bit NOT NULL DEFAULT 0,
	FOREIGN KEY (KursiyerID) REFERENCES Kursiyerler(KursiyerID) ON DELETE SET NULL,
	FOREIGN KEY (EgitmenID) REFERENCES Egitmenler(EgitmenID) ON DELETE SET NULL,
	FOREIGN KEY (AracID) REFERENCES Araclar(AracID) ON DELETE SET NULL,
	CONSTRAINT ck_SinavYapilmadi CHECK (BasariliMi <= SinavYapildiMi)
)
GO

CREATE TABLE Harclar (
	KursiyerID int PRIMARY KEY,
	OdenecekMiktar decimal(10,2) NOT NULL CHECK (OdenecekMiktar > 0),
	OdenenMiktar decimal(10,2) NOT NULL DEFAULT 0,
	OdendiMi bit NOT NULL DEFAULT 0,
	FOREIGN KEY (KursiyerID) REFERENCES Kursiyerler(KursiyerID) ON DELETE CASCADE
)
GO

CREATE TABLE HarcOdemeleri (
	HarcOdemesiID int IDENTITY(1,1) PRIMARY KEY,
	KursiyerID int NULL,
	Miktar decimal(10,2) NOT NULL,
	OdemeTarihi datetime NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (KursiyerID) REFERENCES Kursiyerler(KursiyerID) ON DELETE SET NULL
)
GO

CREATE TABLE MaasOdemeleri (
	MaasOdemesiID int IDENTITY(1,1) PRIMARY KEY,
	EgitmenID int NULL,
	Miktar decimal(10,2) NOT NULL,
	OdemeTarihi datetime NOT NULL DEFAULT GETDATE(),
	FOREIGN KEY (EgitmenID) REFERENCES Egitmenler(EgitmenID) ON DELETE SET NULL
)
GO

CREATE TABLE EgitmenOgretirSinif (
	EgitmenID int NOT NULL,
	SertifikaSinifi varchar(3) NOT NULL,
	FOREIGN KEY (EgitmenID) REFERENCES Egitmenler(EgitmenID) ON DELETE CASCADE,
	FOREIGN KEY (SertifikaSinifi) REFERENCES SertifikaSiniflari(SertifikaSinifi) ON DELETE CASCADE
)
GO

CREATE TABLE SinifKapsarSinif (
	KapsayiciSinif varchar(3) NOT NULL,
	KapsananSinif varchar(3) NOT NULL,
	FOREIGN KEY (KapsayiciSinif) REFERENCES SertifikaSiniflari(SertifikaSinifi) ON DELETE CASCADE,
	FOREIGN KEY (KapsananSinif) REFERENCES SertifikaSiniflari(SertifikaSinifi) ON DELETE NO ACTION
)
GO


CREATE PROCEDURE usp_HarcFazlasiniIadeEt AS
	INSERT INTO HarcOdemeleri (KursiyerID, Miktar)
	SELECT Harclar.KursiyerID, Harclar.OdenecekMiktar - Harclar.OdenenMiktar
	FROM Harclar
	WHERE Harclar.OdenenMiktar > Harclar.OdenecekMiktar

	UPDATE Harclar
	SET OdenenMiktar = OdenecekMiktar
	WHERE OdenenMiktar > OdenecekMiktar
GO

CREATE PROCEDURE usp_EgitmenMaaslariniOde AS
	INSERT INTO MaasOdemeleri (EgitmenID, Miktar)
	SELECT EgitmenID, Maas
	FROM Egitmenler
GO

--https://stackoverflow.com/a/49129796
CREATE PROCEDURE usp_YasSinirindanBuyukMu
	@KursiyerID int,
	@SertifikaSinifi varchar(3),
	@BuyukMu bit OUTPUT
AS
BEGIN
	DECLARE @DogumTarihi date
	DECLARE @YasSiniri tinyint
	SET @DogumTarihi = (SELECT DogumTarihi FROM Kursiyerler AS K WHERE K.KursiyerID = @KursiyerID)
	SET @YasSiniri = (SELECT YasSiniri FROM SertifikaSiniflari AS S WHERE S.SertifikaSinifi = @SertifikaSinifi)
	IF DATEDIFF(year,@DogumTarihi,GETDATE()) >= @YasSiniri
	SET @BuyukMu = 1
	ELSE SET @BuyukMu = 0
END
GO


--https://stackoverflow.com/a/68076339
--https://stackoverflow.com/a/60232533
CREATE VIEW SonOdemeler AS
SELECT OdemeTarihi, Miktar, Ad, Soyad
FROM Kursiyerler AS K
JOIN Harclar AS H
ON K.KursiyerID = H.KursiyerID
JOIN (SELECT OdemeTarihi, Miktar, KursiyerID FROM HarcOdemeleri ORDER BY OdemeTarihi DESC OFFSET 0 ROWS) AS O
ON H.KursiyerID = O.KursiyerID
GO

CREATE VIEW SonSinavlar AS
SELECT DISTINCT SinavTarihi, K.Ad AS [Kursiyer Adı], K.Soyad AS [Kursiyer Soyadı], SinavYapildiMi, BasariliMi,
K.SertifikaSinifi, E.Ad AS [Eğitmen Adı], E.Soyad AS [Eğitmen Soyadı], Marka, AracModeli
FROM (SELECT * FROM Sinavlar ORDER BY SinavTarihi DESC OFFSET 0 ROWS) AS S
JOIN Kursiyerler AS K
ON S.KursiyerID = K.KursiyerID
JOIN SertifikaSiniflari AS SS
ON K.SertifikaSinifi = SS.SertifikaSinifi
LEFT JOIN Egitmenler AS E
ON S.EgitmenID = E.EgitmenID
LEFT JOIN Araclar AS A
ON S.AracID = A.AracID
GO

CREATE VIEW HarcDurumlari AS
SELECT K.KursiyerID, Ad, Soyad, KayitTarihi, OdenecekMiktar, OdenenMiktar, OdendiMi
FROM Kursiyerler AS K
JOIN Harclar AS H
ON K.KursiyerID = H.KursiyerID
GO

CREATE VIEW KursiyerTumBilgiler AS
SELECT K.KursiyerID, Ad, Soyad, TCKN, DogumTarihi, KayitTarihi, SertifikaSinifi, OdenecekMiktar, OdenenMiktar, OdendiMi, BasariliMi, BitirdiMi
FROM Kursiyerler AS K
JOIN Harclar AS H
ON K.KursiyerID = H.KursiyerID
LEFT JOIN Sinavlar AS S
ON K.KursiyerID = S.KursiyerID AND S.BasariliMi=1
GO

--https://stackoverflow.com/q/79864966
CREATE VIEW AktifKursiyerler AS
SELECT K.KursiyerID, Ad, Soyad, TCKN, KayitTarihi, SertifikaSinifi, OdenecekMiktar, OdenenMiktar, OdendiMi, S.SinavYapildiMi, S.BasariliMi
FROM KursiyerTumBilgiler AS K
OUTER APPLY (
SELECT TOP 1 * FROM Sinavlar AS S WHERE S.KursiyerID=K.KursiyerID ORDER BY SinavTarihi DESC
) AS S
WHERE BitirdiMi=0
GO

CREATE VIEW BitirenKursiyerler AS
SELECT K.KursiyerID, Ad, Soyad, TCKN, DogumTarihi, KayitTarihi, SertifikaSinifi, OdenenMiktar
FROM
(SELECT * FROM Kursiyerler AS K WHERE BitirdiMi = 1) AS K
JOIN Harclar AS H
ON K.KursiyerID = H.KursiyerID
GO

CREATE VIEW EgitmenleriGoster AS
SELECT EgitmenID, Ad, Soyad, Maas, IseAlmaTarihi, DogumTarihi
FROM Egitmenler
GO

CREATE VIEW KursUcretleri AS
SELECT SertifikaSinifi, DersSaati, SaatUcreti, ToplamUcret
FROM SertifikaSiniflari
GO

CREATE VIEW MaasOdemeleriniGoster AS
SELECT OdemeTarihi, Miktar, Ad, Soyad
FROM MaasOdemeleri AS M
LEFT JOIN Egitmenler AS E
ON M.EgitmenID = E.EgitmenID
GO


CREATE TRIGGER trg_HarcOlustur
ON Kursiyerler AFTER INSERT AS
BEGIN
	INSERT INTO Harclar (KursiyerID, OdenecekMiktar)
	SELECT KursiyerID, ToplamUcret
	FROM inserted JOIN SertifikaSiniflari
	ON inserted.SertifikaSinifi = SertifikaSiniflari.SertifikaSinifi
END
GO

--https://stackoverflow.com/a/68802175
CREATE TRIGGER trg_HarcGuncelle
ON HarcOdemeleri AFTER INSERT AS
BEGIN
	UPDATE Harclar
	SET
	OdendiMi = (CASE WHEN Harclar.OdenenMiktar + inserted.Miktar >= Harclar.OdenecekMiktar THEN 1 ELSE 0 END),
	Harclar.OdenenMiktar += inserted.Miktar
	FROM Harclar JOIN inserted
	ON Harclar.KursiyerID = inserted.KursiyerID
END
GO

CREATE TRIGGER trg_KursiyerBitirdiMiHarc
ON Harclar AFTER UPDATE AS
BEGIN
	UPDATE Kursiyerler
	SET BitirdiMi = 1
	FROM Sinavlar AS S
	JOIN inserted AS I
	ON S.KursiyerID = I.KursiyerID
	JOIN Kursiyerler AS K
	ON K.KursiyerID = I.KursiyerID
	WHERE S.BasariliMi=1 AND I.OdendiMi=1
END
GO

CREATE TRIGGER trg_KursiyerBitirdiMiSinav
ON Sinavlar AFTER INSERT AS
BEGIN
	UPDATE Kursiyerler
	SET BitirdiMi = 1
	FROM Harclar AS H
	JOIN inserted AS I
	ON H.KursiyerID = I.KursiyerID
	JOIN Kursiyerler AS K
	ON K.KursiyerID = I.KursiyerID
	WHERE I.BasariliMi=1 AND H.OdendiMi=1
END
GO

CREATE TRIGGER trg_KursiyerYasiUygunMu
ON Kursiyerler AFTER INSERT AS
BEGIN
	DECLARE @sonuc bit
	DECLARE @id int
	DECLARE @sinif varchar(3)
	SELECT @id = KursiyerID FROM inserted
	SELECT @sinif = SertifikaSinifi FROM inserted
	EXEC usp_YasSinirindanBuyukMu
	@KursiyerID=@id,
	@SertifikaSinifi=@sinif,
	@BuyukMu=@sonuc OUTPUT
	IF @sonuc = 0
	THROW 55555, 'Kursiyerin yaşı bu sertifika sınıfı için yeterli değil', 1
END
GO

CREATE NONCLUSTERED INDEX ix_HarcOdemeleri_OdemeTarihi
ON HarcOdemeleri (OdemeTarihi DESC)

CREATE NONCLUSTERED INDEX ix_MaasOdemeleri_OdemeTarihi
ON MaasOdemeleri (OdemeTarihi DESC)

CREATE NONCLUSTERED INDEX ix_Kursiyerler_TCKN
ON Kursiyerler (TCKN)

CREATE NONCLUSTERED INDEX ix_Egitmenler_TCKN
ON Egitmenler (TCKN)

CREATE NONCLUSTERED INDEX ix_Araclar_Plaka
ON Araclar (Plaka)
GO

INSERT INTO SertifikaSiniflari (SertifikaSinifi,DersSaati,SaatUcreti,YasSiniri) VALUES
('A',50,900,20),
('A1',50,600,16),
('A2',50,800,18),
('B',50,700,18),
('B1',50,1000,18),
('C',20,2000,21),
('C1',20,1500,21),
('D',15,2000,24),
('E',20,1600,24)
GO

INSERT INTO SinifKapsarSinif (KapsayiciSinif, KapsananSinif) VALUES
('A','A1'),
('A','A2'),
('A2','A1'),
('B','B1'),
('C','C1')
GO

INSERT INTO Araclar (Plaka,SertifikaSinifi,VitesCesidi,Marka,AracModeli,Yil,AracKilometresi) VALUES
('41 KOU 001','B','Manuel','Hyundai','i20',2020,1000),
('41 KOU 41','B','Manuel','Renault','Clio',2015,2000),
('41 ABC 10','B','Otomatik','Renault','Clio E-Tech',2025,500),
('41 VT 55','C','Manuel','Isuzu','NPR',2010,5000),
('41 TR 101','B','Manuel','Opel','Astra',2013,10000),
('41 KOU 14','B','Manuel','Renault','Clio',2013,12000),
('41 CK 87','A2','Manuel','Yamaha','XMAX 300',2013,28000),
('41 TR 102','B','Otomatik','Ford','Fiesta',2013,10000),
('41 TR 10','A','Manuel','Suzuki','SV650',2011,3000),
('41 KR 34','A1','Manuel','Honda','Dio',2012,19000),
('41 TR 33','D','Otomatik','Isuzu','Citibus',2014,20000),
('41 YS 44','D','Otomatik','Otokar','Sultan',2014,30000),
('41 CB 11','B1','Otomatik','Mondial','Venture 200',2020,14000),
('41 FD 62','C1','Otomatik','Iveco','Daily',2010,22000)
GO

INSERT INTO Egitmenler (Ad,Soyad,Maas,TCKN,IseAlmaTarihi) VALUES
('Tolga','Şahin',28000,'69858435352','2018-03-12'),
('Berk','Güneş',35000,'79023351465','2022-12-30'),
('Metehan','Çolak',35000,'15752516567','2022-02-20'),
('Can','Demir',40000,'42175424911','2021-05-10'),
('Mehmet','Yıldız',33000,'44382599320','2024-04-15'),
('İbrahim','Gün',33000,'36537530012','2023-02-11')
GO
INSERT INTO Egitmenler (Ad,Soyad,Maas,TCKN,IseAlmaTarihi,DogumTarihi) VALUES
('Arif','Altun',48000,'51737235235','2016-10-01','1985-01-11'),
('Arda','Çetin',50000,'56789011234','2015-10-28','1979-08-21'),
('Hüsnü','Ay',50000,'38570560014','2011-11-01','1999-10-07'),
('Mustafa','Su',40000,'16643671546','2013-09-20','1992-04-23')
GO

INSERT INTO EgitmenOgretirSinif (EgitmenID, SertifikaSinifi) VALUES
(1,'B'),
(1,'C'),
(1,'D'),
(2,'A'),
(3,'B'),
(4,'C'),
(5,'A'),
(5,'B'),
(5,'C'),
(5,'D'),
(6,'B'),
(7,'A'),
(7,'B'),
(8,'B'),
(9,'D'),
(10,'C')
GO

INSERT INTO Kursiyerler (Ad,Soyad,SertifikaSinifi,TCKN,DogumTarihi,KayitTarihi) VALUES
('Hasan','Yılmaz','B','35352698584','1996-03-12','2026-02-01'),
('Hakan','Abalıoğlu','B','35146579023','2006-12-30','2026-03-14'),
('Alara','Altun','A','69665365426','2000-10-28','2026-02-22'),
('Berk','Yılmaz','D','12345678901','2005-10-28','2026-03-30'),
('Mete','Yıldırım','C','25165671575','1998-02-20','2026-02-08'),
('Ahmet','Çobanoğlu','A','54249142171','2001-05-10','2026-02-01'),
('Ayşe','Yıldız','B','48993204436','2004-04-15','2026-03-23'),
('Tolga','Gök','D','37530012365','2003-02-11','2026-02-11'),
('Mert','Demir','A','56001438570','2007-11-01','2026-02-21'),
('Gülnur','Kartal','A','67154616501','2006-09-25','2026-04-10'),
('Efe','Çoban','A1','46161515607','2005-08-19','2026-03-27'),
('Hüseyin','Nalbur','C1','43014616185','1990-03-10','2026-03-19'),
('Yakup','Sevinç','A2','78285900125','1995-03-10','2026-04-20'),
('Muzaffer','Genç','B1','61756827894','2005-12-23','2026-04-11'),
('Hasan','Yiğit','A1','72354525288','2004-11-10','2026-02-11')
GO

INSERT INTO Sinavlar (KursiyerID, EgitmenID, AracID, SinavTarihi, SinavYapildiMi, BasariliMi) VALUES
(1,1,1,'2026-03-24',1,1),
(2,1,2,'2026-03-24',1,0),
(2,3,3,'2026-03-26',1,1),
(3,2,9,'2026-03-28',1,1),
(4,5,11,'2026-05-15',1,1),
(5,5,4,'2026-04-18',1,0),
(6,5,9,'2026-04-20',1,0),
(6,5,9,'2026-05-05',1,1),
(8,1,12,'2026-04-01',0,0),
(8,1,12,'2026-04-24',1,1),
(10,7,9,'2026-04-10',1,1),
(11,2,10,'2026-04-19',1,1),
(13,1,9,'2026-04-20',1,1),
(13,2,9,'2026-05-10',1,0),
(15,2,9,'2026-04-20',0,0),
(15,2,9,'2026-04-25',1,0),
(15,2,9,'2026-04-30',1,1)
GO

INSERT INTO HarcOdemeleri (KursiyerID, Miktar, OdemeTarihi) VALUES
(1,1200,'2026-03-22 09:00:50'),
(2,1600,'2026-03-20 14:51:39'),
(2,3500,'2026-04-11 12:03:28'),
(2,11000,'2026-04-21 16:33:20'),
(2,5000,'2026-05-01 08:10:44'),
(2,4000,'2026-05-22 12:13:14'),
(4,1600,'2026-03-01 12:08:17'),
(6,15000,'2026-04-01 16:50:52'),
(6,45000,'2026-05-22 23:40:11'),
(7,40000,'2026-05-28 05:28:20'),
(8,60000,'2026-05-29 18:45:14'),
(9,10000,'2026-05-04 15:20:55'),
(9,30000,'2026-05-18 11:00:44'),
(10,8000,'2026-04-29 10:29:20'),
(10,28000,'2026-05-11 13:12:00'),
(10,19000,'2026-05-31 16:21:30'),
(12,20000,'2026-05-12 00:25:09'),
(12,20000,'2026-05-29 10:42:41'),
(15,30000,'2026-02-15 10:42:41')
GO

EXEC usp_EgitmenMaaslariniOde
GO