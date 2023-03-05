USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_DESIGN_SOAL_SELECT_BY_RS]    Script Date: 04-Mar-23 6:13:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CBT_DESIGN_SOAL_SELECT_BY_RS_BY_SISWA](
	@Rel_RumahSoal varchar(50),
	@Rel_Siswa varchar(50)
)
AS
BEGIN
	select * from CBT_DesignSoal a
	left join cbt_jwb b on a.Kode = b.Rel_DesignSoal
	where a.Rel_RumahSoal = @Rel_RumahSoal and b.Rel_Siswa = @Rel_Siswa
END


