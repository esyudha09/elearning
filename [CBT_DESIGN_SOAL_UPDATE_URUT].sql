USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_DESIGN_SOAL_UPDATE_SKOR]    Script Date: 2/28/2023 3:37:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[CBT_DESIGN_SOAL_UPDATE_URUT](
	@Kode	varchar(50),	
	@Urut	int,
	@user_id varchar(50)
)
AS
BEGIN
	UPDATE [dbo].CBT_DesignSoal
   SET       
      Urut = @Urut	
      ,[Tanggal_Update] = GETDATE()
 WHERE Kode = @Kode

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Update data urut cbt_design_soal',
		@Kode
	;
END