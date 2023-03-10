USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_FOR_SEARCH]    Script Date: 2/21/2023 2:55:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_FOR_SEARCH]
(
	@Rel_Mapel varchar(50),
	@nama varchar (500)
)
AS
BEGIN
	SELECT a.*, b.Nama AS Unit FROM CBT_BankSoal a
	LEFT JOIN Mapel  b ON CONVERT(varchar(50), b.Kode) = a.Rel_Mapel
	WHERE a.Rel_Mapel = @Rel_Mapel AND
		  (
			a.Soal LIKE '%' + @nama + '%' 
			--x.Keterangan LIKE '%' + @nama + '%' OR
			--x.Alias LIKE '%' + @nama + '%' OR
			--x.Unit LIKE '%' + @nama + '%' OR
			--x.Jenis LIKE '%' + @nama + '%'
		  )
	ORDER BY a.Soal
END



