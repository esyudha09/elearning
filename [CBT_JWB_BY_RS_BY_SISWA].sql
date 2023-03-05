USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_RUMAH_SOAL_SELECT_BY_ID]    Script Date: 04-Mar-23 5:54:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CBT_JWB_BY_RS_BY_SISWA](
	@Rel_RumahSoal varchar(50),
	@Rel_Siswa varchar(50)
)
AS
BEGIN
select * from CBT_DesignSoal a
left join cbt_jwb b on a.Kode = b.Rel_DesignSoal
where a.Rel_RumahSoal = @Rel_RumahSoal and b.Rel_Siswa = @Rel_Siswa
	
END


