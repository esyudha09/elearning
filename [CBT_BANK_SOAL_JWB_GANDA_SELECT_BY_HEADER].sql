USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_JWB_GANDA_SELECT_BY_HEADER]    Script Date: 3/9/2023 11:29:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_JWB_GANDA_SELECT_BY_HEADER](
	@Rel_BankSoal	varchar(50)
)
AS
BEGIN
	SELECT * FROM CBT_BankSoalJwbGanda a	
	WHERE a.Rel_BankSoal = @Rel_BankSoal
	--order by NEWID()
END