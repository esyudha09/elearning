USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_INSERT]    Script Date: 3/2/2023 9:05:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[CBT_BANK_SOAL_JWB_GANDA_INSERT](

	@Kode	varchar(50),
	@Rel_BankSoal varchar(50),
	@Jawaban	varchar(1000),		
	@user_id varchar(50)

)
AS
BEGIN
	
	INSERT INTO [dbo].CBT_BankSoalJwbGanda
           ([Kode]
           ,[Rel_BankSoal]
           ,[Jawaban])
           
     VALUES
           (@Kode
		   ,@Rel_BankSoal
           ,@Jawaban)
           


	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END