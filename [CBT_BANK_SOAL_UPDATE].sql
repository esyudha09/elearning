USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_UPDATE]    Script Date: 3/2/2023 12:01:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_UPDATE](
	@Kode	varchar(50),	
	@Soal	varchar(1000),
	@Jenis Varchar(20),
	@JwbEssay varchar(1000),
	@Rel_JwbGanda varchar(50),	
	@user_id varchar(50)
)
AS
BEGIN
	UPDATE [dbo].CBT_BankSoal
   SET       
      [Soal] = @Soal
	  ,Jenis = @Jenis
      ,[JwbEssay] = @JwbEssay
	  ,[Rel_JwbGanda] = @Rel_JwbGanda     
      ,[Tanggal_Update] = GETDATE()
 WHERE Kode = @Kode

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Update data cbt_soal',
		@Kode
	;
END