USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_INSERT]    Script Date: 2/28/2023 10:38:53 AM ******/
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
	@JwbGanda varchar(1) = '',
	@JwbGanda1 varchar(1000) = '',
	@JwbGanda2 varchar(1000) = '',
	@JwbGanda3 varchar(1000) = '',
	@JwbGanda4 varchar(1000) = '',
	@JwbGanda5 varchar(1000) = '',
	@user_id varchar(50)

)
AS
BEGIN
	--DECLARE @Kode uniqueidentifier;
	--SET @Kode = NEWID();

	INSERT INTO [dbo].CBT_BankSoal
           ([Kode]
           ,[Rel_Mapel]
           ,[Rel_Guru]
           ,[Soal]  
		   ,Jenis
           ,JwbEssay
		   ,[JwbGanda]
           ,[JwbGanda1]
           ,[JwbGanda2]
           ,[JwbGanda3]
           ,[JwbGanda4]
           ,[JwbGanda5]
           ,[Tanggal_Buat])
     VALUES
           (@Kode
           ,@Rel_Mapel
           ,@Rel_Guru
           ,@Soal
		   ,@Jenis
           ,@JwbEssay
		   ,@JwbGanda
           ,@JwbGanda1
           ,@JwbGanda2
           ,@JwbGanda3
           ,@JwbGanda4
           ,@JwbGanda5
           ,GETDATE())


	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END