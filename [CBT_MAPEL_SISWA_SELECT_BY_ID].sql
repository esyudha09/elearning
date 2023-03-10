USE [SCHEMA_ERP]
GO
/****** Object:  StoredProcedure [dbo].[CBT_BANK_SOAL_SELECT_BY_ID]    Script Date: 3/2/2023 3:16:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[CBT_MAPEL_SISWA_SELECT_BY_ID](
	@Rel_Siswa	varchar(50),
	@TahunAjaran varchar(20),
	@Semester int
)
AS
BEGIN
Select 
a.Nama as NamaSiswa
,c.Nama as NamaMapel,c.Alias,c.Keterangan
,e.Nama as Kelas
,f.Nama as NamaGuru
,d.Kode as Rel_RumahSoal
,d.Rel_Rapor_StrukturNilai_KP
,d.Rel_Mapel
      ,d.Rel_Guru
      ,d.Kurikulum
      ,d.Nama as NamaKP
      ,d.Deskripsi
      ,d.StartDatetime
      ,d.EndDatetime
      ,d.LimitTime
      ,d.LimitSatuan
      ,d.CreatedDate
      ,d.UpdateDate
      ,d.Rel_Kelas
      ,d.Semester
      ,d.TahunAjaran

from siswa a
	inner join FormasiGuruMapel b on b.Rel_Kelas = a.Rel_Kelas
	inner join Mapel c on c.Kode = b.Rel_Mapel
	inner join CBT_RumahSoal d on b.Rel_Mapel =d.Rel_Mapel and b.TahunAjaran = d.TahunAjaran and b.Semester = d.Semester and d.Rel_Kelas =  b.Rel_Kelas
	inner join Kelas e on e.kode = a.Rel_Kelas
	inner join Pegawai f on f.Kode = d.Rel_Guru


where a.Kode = @Rel_Siswa and b.TahunAjaran = @TahunAjaran and b.Semester = @Semester

END