USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[Mapel_SELECT_BY_GURU_BY_TA_FOR_SEARCH]    Script Date: 2/14/2023 10:31:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Mapel_SELECT_BY_GURU_BY_TA_FOR_SEARCH](
	@Rel_Guru varchar(50),	
	@TahunAjaran varchar(50),
	@nama varchar(50)
)
AS
BEGIN
	SELECT x1.Kode,
		   x2.Nama,
		   x2.Alias,
		   x2.Jenis,
		   x2.Keterangan,
		   x2.Rel_Sekolah,
		   x3.Nama AS Unit
	FROM (
		SELECT DISTINCT x0.Rel_Mapel AS Kode FROM
		(
			--SELECT 
			--	a.Kode AS Rel_Mapel
			--FROM Mapel a
			--WHERE 
			--	  ISNULL((
			--		SELECT COUNT(b.Kode) FROM FormasiGuruKelas b 
			--		WHERE b.Rel_GuruKelas = @Rel_Guru OR b.Rel_GuruPendamping = @Rel_Guru and b.TahunAjaran = @TahunAjaran
			--	  ),0) > 0

			--UNION

			SELECT 
				a.Rel_Mapel
			FROM FormasiGuruMapel a
			LEFT JOIN FormasiGuruMapelDet b ON b.Rel_FormasiGuruMapel = a.Kode
			LEFT JOIN Mapel c ON c.Kode = a.Rel_Mapel			
			WHERE b.Rel_Guru = @Rel_Guru and a.TahunAjaran = @TahunAjaran
		)x0
	)x1
	LEFT JOIN Mapel x2 ON x2.Kode = x1.Kode
	LEFT JOIN Sekolah  x3 ON CONVERT(varchar(50), x3.Kode) = x2.Rel_Sekolah
	WHERE
		  (
			x2.Nama LIKE '%' + @nama + '%' OR
			x2.Keterangan LIKE '%' + @nama + '%' OR
			x2.Alias LIKE '%' + @nama + '%' OR
			x3.Nama LIKE '%' + @nama + '%' OR
			x2.Jenis LIKE '%' + @nama + '%'
		  )
	ORDER BY x2.Nama
END
