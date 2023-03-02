USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_INSERT]    Script Date: 3/2/2023 9:05:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_INSERT](

	@Kode	varchar(50),		
	@Rel_Guru	varchar(50),
	@Rel_Mapel	varchar(50),
	@Soal	varchar(1000),	
	@Jenis Varchar(20),
	@JwbEssay varchar(1000) = '',
	@Rel_JwbGanda varchar(50) = '',
	@user_id varchar(50)

)
AS
BEGIN
	
	
	INSERT INTO [dbo].CBT_BankSoal
           ([Kode]
           ,[Rel_Mapel]
           ,[Rel_Guru]
           ,[Soal]  
		   ,Jenis
		   ,Rel_JwbGanda
           ,JwbEssay
           ,[Tanggal_Buat])
     VALUES
           (@Kode
           ,@Rel_Mapel
           ,@Rel_Guru
           ,@Soal
		   ,@Jenis
		   ,@Rel_JwbGanda
           ,@JwbEssay
           ,GETDATE())


	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END