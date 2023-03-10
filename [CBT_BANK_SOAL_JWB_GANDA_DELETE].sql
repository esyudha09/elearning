USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_DELETE]    Script Date: 3/2/2023 11:44:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CBT_BANK_SOAL_JWB_GANDA_DELETE](
	@Rel_BankSoal	varchar(50),
	@user_id varchar(500)
)
AS
BEGIN
	DELETE CBT_BankSoalJwbGanda WHERE Rel_BankSoal = @Rel_BankSoal;

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Hapus data JWB GANDA',
		@Rel_BankSoal
	;
END