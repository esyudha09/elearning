USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_BY_KELAS_FOR_SEARCH]    Script Date: 12-Mar-23 2:49:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





alter PROCEDURE [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_BY_KELAS_FOR_SEARCH]
(
	@Rel_Mapel varchar(50),
	@Rel_Kelas varchar(50),
	@nama varchar (500)
)
AS
BEGIN
	SELECT a.*, b.Nama AS Unit FROM CBT_BankSoal a
	LEFT JOIN Mapel  b ON CONVERT(varchar(50), b.Kode) = a.Rel_Mapel
	WHERE a.Rel_Mapel = @Rel_Mapel AND a.Rel_kelas = @Rel_Kelas AND
		  (
			a.Soal LIKE '%' + @nama + '%' 
			--x.Keterangan LIKE '%' + @nama + '%' OR
			--x.Alias LIKE '%' + @nama + '%' OR
			--x.Unit LIKE '%' + @nama + '%' OR
			--x.Jenis LIKE '%' + @nama + '%'
		  )
	ORDER BY a.Soal
END



