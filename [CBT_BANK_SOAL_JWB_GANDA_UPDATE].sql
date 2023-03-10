USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_JWB_GANDA_UPDATE]    Script Date: 12-Mar-23 5:59:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_JWB_GANDA_UPDATE](
	@Kode	varchar(50),	
	@Jawaban	varchar(1000),	
	@Urut	varchar(10),	
	@user_id varchar(500)
)
AS
BEGIN
	UPDATE [dbo].CBT_BankSoalJwbGanda
   SET       
    Jawaban = @Jawaban,
	Urut = @Urut

 WHERE Kode = @Kode

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Update data cbt_soal_jwb_ganda',
		@Kode
	;
END