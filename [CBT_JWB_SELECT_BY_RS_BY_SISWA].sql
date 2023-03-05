USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_DESIGN_SOAL_SELECT_BY_RS_BY_SISWA]    Script Date: 05-Mar-23 3:39:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_JWB_SELECT_BY_RS_BY_SISWA](
	@Rel_RumahSoal varchar(50),
	@Rel_Siswa varchar(50)
)
AS
BEGIN
	select * from cbt_jwb a	
	where a.Rel_RumahSoal = @Rel_RumahSoal and a.Rel_Siswa = @Rel_Siswa
END


