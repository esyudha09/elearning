USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_BY_KELAS]    Script Date: 3/13/2023 2:11:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_BY_KELAS]
(
	@Rel_Mapel varchar(50),
	@Rel_Kelas varchar(50)
)
AS
BEGIN
	SELECT a.*, b.Nama AS Unit FROM CBT_BankSoal a
	LEFT JOIN Mapel  b ON CONVERT(varchar(50), b.Kode) = a.Rel_Mapel
	WHERE a.Rel_Mapel = @Rel_Mapel and Rel_kelas = @Rel_Kelas
	ORDER BY a.Tanggal_Buat desc
END



