USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_DESIGN_SOAL_INSERT]    Script Date: 2/28/2023 4:06:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_DESIGN_SOAL_INSERT](
	@Kode	varchar(50),
	@Rel_RumahSoal	varchar(50),
	@Rel_BankSoal	varchar(50),
	@Skor int = 0,
	@Urut int = 1000,
	@user_id varchar(500)
)
AS
BEGIN
	--DECLARE @Kode uniqueidentifier;
	--SET @Kode = NEWID();

	INSERT INTO [dbo].CBT_DesignSoal
           ([Kode]
           ,[Rel_RumahSoal]
           ,[Rel_BankSoal] 
		   ,Skor
		   ,Urut
           ,[Tanggal_Buat])
     VALUES
           (@Kode
           ,@Rel_RumahSoal
           ,@Rel_BankSoal		   
		   ,@Skor
		   ,@Urut
           ,GETDATE())

	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END