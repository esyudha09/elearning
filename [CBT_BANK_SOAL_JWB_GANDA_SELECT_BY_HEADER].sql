USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_SELECT_BY_ID]    Script Date: 3/2/2023 9:41:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CBT_BANK_SOAL_JWB_GANDA_SELECT_BY_HEADER](
	@Rel_BankSoal	varchar(50)
)
AS
BEGIN
	SELECT * FROM CBT_BankSoalJwbGanda a	
	WHERE a.Rel_BankSoal = @Rel_BankSoal
END