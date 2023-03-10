USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_UPDATE]    Script Date: 2/28/2023 8:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[CBT_DESIGN_SOAL_UPDATE_SKOR](
	@Kode	varchar(50),	
	@Skor	int,
	@user_id varchar(50)
)
AS
BEGIN
	UPDATE [dbo].CBT_DesignSoal
   SET       
      Skor = @Skor	
      ,[Tanggal_Update] = GETDATE()
 WHERE Kode = @Kode

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Update data skor cbt_design_soal',
		@Kode
	;
END