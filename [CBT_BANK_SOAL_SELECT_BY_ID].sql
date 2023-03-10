USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_SELECT_BY_ID]    Script Date: 3/1/2023 4:16:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_BANK_SOAL_SELECT_BY_ID](
	@Kode	uniqueidentifier
)
AS
BEGIN
	SELECT * FROM CBT_BankSoal a
	inner join CBT_BankSoalJwbGanda b on a.Kode = b.Rel_BankSoal
	WHERE a.Kode = @Kode
END