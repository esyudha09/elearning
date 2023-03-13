USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_JWB_GANDA_INSERT]    Script Date: 12-Mar-23 5:44:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_JWB_GANDA_INSERT](

	@Kode	varchar(50),
	@Rel_BankSoal varchar(50),
	@Jawaban	varchar(1000),
	@Urut varchar(10),
	@user_id varchar(50)

)
AS
BEGIN
	
	INSERT INTO [dbo].CBT_BankSoalJwbGanda
           ([Kode]
           ,[Rel_BankSoal]
           ,[Jawaban]
		   ,Urut)
           
     VALUES
           (@Kode
		   ,@Rel_BankSoal
           ,@Jawaban
		   ,@Urut)
           


	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END