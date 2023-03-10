USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL]    Script Date: 12-Mar-23 2:47:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL]
(
	@Rel_Mapel varchar(50),
	@Rel_Kelas varchar(50)
)
AS
BEGIN
	SELECT a.*, b.Nama AS Unit FROM CBT_BankSoal a
	LEFT JOIN Mapel  b ON CONVERT(varchar(50), b.Kode) = a.Rel_Mapel
	WHERE a.Rel_Mapel = @Rel_Mapel and Rel_kelas = @Rel_Kelas
	ORDER BY a.Tanggal_Buat
END



