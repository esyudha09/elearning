USE [SCHEMA_RAPOR]
GO
/****** Object:  StoredProcedure [dbo].[SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_MAPEL]    Script Date: 2/14/2023 8:41:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_MAPEL_FOR_SEARCH](
	@TahunAjaran varchar(10),
	@Rel_Mapel varchar(50),
	@nama varchar(500)
)
AS
SELECT x.* FROM (
	SELECT a.Kode, a.TahunAjaran, a.Semester, a.Rel_Mapel, CONVERT(varchar(50), a.Rel_Kelas) AS Rel_Kelas, a.KKM,
		ISNULL(a.IsNilaiAkhir, 0) AS IsNilaiAkhir, b.Nama AS Kelas, c.Nama AS Mapel, b.UrutanLevel, 'KTSP' AS Kurikulum, c.Jenis AS JenisMapel,
		a.DeskripsiSikapSpiritual,
		a.DeskripsiSikapSosial
	FROM SMA_Rapor_StrukturNilai_KTSP a
		LEFT JOIN SCHEMA_ERP.dbo.Kelas b ON CONVERT(varchar(50), b.Kode) = CONVERT(varchar(50), a.Rel_Kelas)
		LEFT JOIN SCHEMA_ERP.dbo.Mapel c ON CONVERT(varchar(50), c.Kode) = CONVERT(varchar(50), a.Rel_Mapel)
	WHERE (
			  a.TahunAjaran LIKE '%' + @nama + '%' OR
			  a.Semester LIKE '%' + @nama + '%' OR
			  b.Nama LIKE '%' + @nama + '%' OR
			  c.Nama LIKE '%' + @nama + '%'
		  ) AND
		  a.TahunAjaran = @TahunAjaran AND
		  a.Rel_Mapel = @Rel_Mapel
	UNION
	SELECT a.Kode, a.TahunAjaran, a.Semester, a.Rel_Mapel, CONVERT(varchar(50), a.Rel_Kelas) AS Rel_Kelas, a.KKM,
		ISNULL(a.IsNilaiAkhir, 0) AS IsNilaiAkhir, b.Nama AS Kelas, c.Nama AS Mapel, b.UrutanLevel, 'KURTILAS' AS Kurikulum, c.Jenis AS JenisMapel,
		a.DeskripsiSikapSpiritual,
		a.DeskripsiSikapSosial
	FROM SMA_Rapor_StrukturNilai_KURTILAS a
		LEFT JOIN SCHEMA_ERP.dbo.Kelas b ON CONVERT(varchar(50), b.Kode) = CONVERT(varchar(50), a.Rel_Kelas)
		LEFT JOIN SCHEMA_ERP.dbo.Mapel c ON CONVERT(varchar(50), c.Kode) = CONVERT(varchar(50), a.Rel_Mapel)
	WHERE (
			  a.TahunAjaran LIKE '%' + @nama + '%' OR
			  a.Semester LIKE '%' + @nama + '%' OR
			  b.Nama LIKE '%' + @nama + '%' OR
			  c.Nama LIKE '%' + @nama + '%'
		  ) AND
		  a.TahunAjaran = @TahunAjaran AND
		  a.Rel_Mapel = @Rel_Mapel AND
		  c.Jenis <> 'SIKAP'
	UNION
	SELECT a.Kode, a.TahunAjaran, a.Semester, a.Rel_Mapel, CONVERT(varchar(50), a.Rel_Kelas) AS Rel_Kelas, a.KKM,
		ISNULL(a.IsNilaiAkhir, 0) AS IsNilaiAkhir, b.Nama AS Kelas, c.Nama AS Mapel, b.UrutanLevel, 'KURTILAS-SIKAP' AS Kurikulum, c.Jenis AS JenisMapel,
		a.DeskripsiSikapSpiritual,
		a.DeskripsiSikapSosial
	FROM SMA_Rapor_StrukturNilai_KURTILAS a
		LEFT JOIN SCHEMA_ERP.dbo.Kelas b ON CONVERT(varchar(50), b.Kode) = CONVERT(varchar(50), a.Rel_Kelas)
		LEFT JOIN SCHEMA_ERP.dbo.Mapel c ON CONVERT(varchar(50), c.Kode) = CONVERT(varchar(50), a.Rel_Mapel)
	WHERE (
			  a.TahunAjaran LIKE '%' + @nama + '%' OR
			  a.Semester LIKE '%' + @nama + '%' OR
			  b.Nama LIKE '%' + @nama + '%' OR
			  c.Nama LIKE '%' + @nama + '%'
		  ) AND
		  a.TahunAjaran = @TahunAjaran AND
		  a.Rel_Mapel = @Rel_Mapel AND
		  c.Jenis = 'SIKAP'
) x
ORDER BY x.TahunAjaran DESC, x.Semester DESC, x.UrutanLevel ASC, x.Mapel ASC