USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_UPDATE]    Script Date: 3/2/2023 11:52:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[CBT_BANK_SOAL_JWB_GANDA_UPDATE](
	@Kode	varchar(50),	
	@Jawaban	varchar(1000),	
	@user_id varchar(500)
)
AS
BEGIN
	UPDATE [dbo].CBT_BankSoalJwbGanda
   SET       
    Jawaban = @Jawaban

 WHERE Kode = @Kode

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Update data cbt_soal_jwb_ganda',
		@Kode
	;
END