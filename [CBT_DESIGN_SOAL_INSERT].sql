USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_DESIGN_SOAL_INSERT]    Script Date: 3/13/2023 1:36:41 PM ******/
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
		   ,(select max(Urut)+1 From CBT_DesignSoal Where Rel_RumahSoal = @Rel_RumahSoal) 
           ,GETDATE())

	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END