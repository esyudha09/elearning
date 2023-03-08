USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_STATUS_SISWA_INSERT]    Script Date: 3/8/2023 1:42:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_STATUS_SISWA_INSERT](
    @Kode varchar(50),
	@Rel_RumahSoal	varchar(50),
	@Rel_Siswa	varchar(50)
)
AS
BEGIN
	
	
	INSERT INTO [dbo].CBT_StatusSiswa
           ([Kode]
           ,Rel_RumahSoal
           ,Rel_Siswa
           ,StartDate  )
		   
     VALUES
           (@Kode
           ,@Rel_RumahSoal
           ,@Rel_Siswa
           ,GETDATE())
		  
	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END