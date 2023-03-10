USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_RUMAH_SOAL_SELECT_BY_ID]    Script Date: 3/3/2023 1:06:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_RUMAH_SOAL_SELECT_BY_ID](
	@Kode	uniqueidentifier
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
	
	WHERE a.Kode = @Kode
END


