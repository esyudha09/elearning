USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_DESIGN_SOAL_SELECT_BY_RS]    Script Date: 3/6/2023 3:53:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_DESIGN_SOAL_SELECT_BY_RS](
	@Rel_RumahSoal	varchar (50)
)
AS
BEGIN
	SELECT * FROM CBT_DesignSoal a
	inner join CBT_RumahSoal b on b.Kode =  a.Rel_RumahSoal
	inner join CBT_BankSoal c on c.Kode = a.Rel_BankSoal
	WHERE a.Rel_RumahSoal = @Rel_RumahSoal
	order by a.Urut, a.Tanggal_Buat desc
END


