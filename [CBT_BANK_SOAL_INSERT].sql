USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_INSERT]    Script Date: 3/14/2023 9:46:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_INSERT](

	@Kode	varchar(50),		
	@Rel_Guru	varchar(50),
	@Rel_Mapel	varchar(50),
	@Rel_Kelas varchar(50),		
	@Rel_Rapor_AspekPenilaian varchar(50),
	@Nama varchar(100),
	@Soal	varchar(1000),	
	@Jenis Varchar(20),
	@JwbEssay varchar(1000) = '',
	@Rel_JwbGanda varchar(50) = '',
	@FileImage varchar(100) = '',
	@FileAudio varchar(100) = '',
	@FileVideo varchar(100) = '',
	@user_id varchar(50)

)
AS
BEGIN
	
	
	INSERT INTO [dbo].CBT_BankSoal
           ([Kode]
           ,[Rel_Mapel]
		   ,Rel_kelas
           ,[Rel_Guru]
		   ,Rel_Rapor_AspekPenilaian
		   ,Nama
           ,[Soal]  
		   ,Jenis
		   ,Rel_JwbGanda
           ,JwbEssay
		   ,FileImage
		   ,FileAudio
		   ,FileVideo
           ,[Tanggal_Buat])
     VALUES
           (@Kode
           ,@Rel_Mapel
		   ,@Rel_Kelas
           ,@Rel_Guru
		   ,@Rel_Rapor_AspekPenilaian
		   ,@Nama
           ,@Soal
		   ,@Jenis
		   ,@Rel_JwbGanda
           ,@JwbEssay
		   ,@FileImage
		   ,@FileAudio
		   ,@FileVideo
           ,GETDATE())


	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END