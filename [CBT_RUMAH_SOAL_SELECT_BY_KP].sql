USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_RUMAH_SOAL_SELECT_BY_KP]    Script Date: 3/3/2023 3:36:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_RUMAH_SOAL_SELECT_BY_KP](
	@Rel_Rapor_StrukturNilai_KP	uniqueidentifier
)
AS
BEGIN
	SELECT top 1
	a.*
	  ,b.Nama as NamaKelas
	  ,c.Nama as NamaMapel


FROM CBT_RumahSoal a
inner join kelas b on b.Kode = a.Rel_Kelas
inner join Mapel c on c.Kode = a.Rel_Mapel

	WHERE Rel_Rapor_StrukturNilai_KP = @Rel_Rapor_StrukturNilai_KP
END


