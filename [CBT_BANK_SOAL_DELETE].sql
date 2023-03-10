USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_DELETE]    Script Date: 2/21/2023 2:51:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_DELETE](
	@Kode	uniqueidentifier,
	@user_id varchar(500)
)
AS
BEGIN
	DELETE CBT_BankSoal WHERE Kode = @Kode;

	EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
		@user_id,
		'Hapus data mapel',
		@Kode
	;
END