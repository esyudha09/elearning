USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_RUMAH_SOAL_UPDATE]    Script Date: 3/1/2023 10:23:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CBT_RUMAH_SOAL_UPDATE](
	@Kode	varchar(50),
	@Rel_Rapor_StrukturNilai_KP	varchar(50),
	@Rel_Mapel	varchar(50),
	@Rel_Kelas	varchar(50),
	@Rel_Guru	varchar(1000),	
	@Kurikulum varchar(20),
	@Nama varchar(100),
	@Deskripsi varchar(1000),
	@StartDatetime varchar(100) = '1900-01-01 00:00:00',
	@EndDatetime varchar(100) = '1900-01-01 00:00:00',
	@LimitTime int,
	@LimitSatuan varchar(10) = null,	
	@user_id varchar(500)
)
AS
BEGIN
	UPDATE [dbo].[CBT_RumahSoal]
   SET 
      [Rel_Rapor_StrukturNilai_KP] = @Rel_Rapor_StrukturNilai_KP
      ,[Rel_Mapel] = @Rel_Mapel
	  ,Rel_Kelas = @Rel_Kelas
      ,[Rel_Guru] = @Rel_Guru
      ,[Kurikulum] = @Kurikulum
      ,[Nama] = @Nama
      ,[Deskripsi] = @Deskripsi
      ,[StartDatetime] = @StartDatetime
      ,[EndDatetime] = @EndDatetime
      ,[LimitTime] = @LimitTime
      ,[LimitSatuan] = @LimitSatuan     
      ,[UpdateDate] = GETDATE()
 WHERE Kode = @Kode
      

	--EXEC SCHEMA_ERP.dbo.UserActivities_INSERT 
	--	@user_id,
	--	'Tambah data soal',
	--	@Kode
	--;
END