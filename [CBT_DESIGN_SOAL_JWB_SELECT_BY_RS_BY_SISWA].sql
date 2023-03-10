USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_DESIGN_SOAL_SELECT_BY_RS]    Script Date: 05-Mar-23 6:17:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[CBT_DESIGN_SOAL_JWB_SELECT_BY_RS_BY_SISWA](
	@Rel_RumahSoal	varchar (50),
	@Rel_Siswa	varchar (50)
)
AS
BEGIN
	SELECT 
	a.Kode,a.Rel_BankSoal,a.Rel_RumahSoal,
	c.Kode Rel_Jwb,c.Rel_Siswa
	FROM CBT_DesignSoal a
	inner join CBT_BankSoal b on b.Kode = a.Rel_BankSoal
	left join CBT_Jwb c on c.Rel_DesignSoal = a.Kode and c.Rel_Siswa = @Rel_Siswa
	WHERE a.Rel_RumahSoal = @Rel_RumahSoal 
	--order by a.Urut, a.Tanggal_Buat desc
END


