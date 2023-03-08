USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_STATUS_SISWA_UPDATE]    Script Date: 3/8/2023 1:56:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_STATUS_SISWA_UPDATE](

	@Rel_RumahSoal	varchar(50),
	@Rel_Siswa	varchar(50)
)
AS
BEGIN
	
	UPDATE CBT_StatusSiswa
	SET EndDate = GETDATE()
	WHERE Rel_RumahSoal = @Rel_RumahSoal AND Rel_Siswa = @Rel_Siswa


		  
	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END