USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_UPDATE]    Script Date: 3/14/2023 9:48:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_UPDATE](
	@Kode	varchar(50),
	@Rel_Rapor_AspekPenilaian varchar(50),
	@Nama varchar(100),
	@Soal	varchar(1000),
	@Jenis Varchar(20),
	@JwbEssay varchar(1000)= '',
	@Rel_JwbGanda varchar(50) = '',	
	@FileImage varchar(100) = '',
	@FileAudio varchar(100) = '',
	@FileVideo varchar(100) = '',
	@user_id varchar(50)
)
AS
BEGIN
	UPDATE [dbo].CBT_BankSoal
   SET 
	  Rel_Rapor_AspekPenilaian   = @Rel_Rapor_AspekPenilaian
	  ,Nama = @Nama
      ,[Soal] = @Soal
	  ,Jenis = @Jenis
      ,[JwbEssay] = @JwbEssay
	  ,[Rel_JwbGanda] = @Rel_JwbGanda 
      ,[Tanggal_Update] = GETDATE()
 WHERE Kode = @Kode


 if(@FileAudio <> '' or @FileImage <> '' or @FileVideo <> '')
 begin
 UPDATE [dbo].CBT_BankSoal
 SET 	  
	   FileImage = @FileImage
	  ,FileAudio = @FileAudio
	  ,FileVideo = @FileVideo    
 WHERE Kode = @Kode
 end

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Update data cbt_soal',
		@Kode
	;
END