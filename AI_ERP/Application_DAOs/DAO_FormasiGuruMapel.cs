using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_FormasiGuruMapel
    {
        public const string SP_SELECT_ALL = "FormasiGuruMapel_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "FormasiGuruMapel_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_PERIODE_BY_UNIT = "FormasiGuruMapel_SELECT_ALL_BY_PERIODE_BY_UNIT";
        public const string SP_SELECT_ALL_BY_PERIODE_BY_UNIT_FOR_SEARCH = "FormasiGuruMapel_SELECT_ALL_BY_PERIODE_BY_UNIT_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "FormasiGuruMapel_SELECT_BY_ID";        
        public const string SP_SELECT_BY_UNIT = "FormasiGuruMapel_SELECT_BY_UNIT";
        public const string SP_SELECT_BY_Guru = "FormasiGuruMapel_SELECT_BY_GURU";
        public const string SP_SELECT_BY_UNIT_BY_TAHUN_AJARAN_BY_SEMESTER = "FormasiGuruMapel_SELECT_BY_UNIT_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_KELAS_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER = "FormasiGuruMapel_SELECT_KELAS_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL_BY_SISWA = "FormasiGuruMapel_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL_BY_SISWA";
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER = "FormasiGuruMapel_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS = "FormasiGuruMapel_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_KELASDET = "FormasiGuruMapel_SELECT_BY_TA_BY_SM_BY_KELAS_BY_KELAS_DET";

        public const string SP_INSERT = "FormasiGuruMapel_INSERT";

        public const string SP_UPDATE = "FormasiGuruMapel_UPDATE";

        public const string SP_DELETE = "FormasiGuruMapel_DELETE";

        public class FormasiGuruMapel_Ext
        {
            public string Rel_Sekolah { set; get; }
            public string Rel_Mapel { set; get; }
            public string TahunAjaran { set; get; }
            public string Semester { set; get; }
            public string Rel_Kelas { set; get; }
            public string Mapel { set; get; }
            public string Rel_KelasDet { set; get; }
            public string Kelas { set; get; }
            public string UrutanKelas { set; get; }
            public string JenisKelas { set; get; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
        }

        public static class NamaField_Ext
        {
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Mapel = "Mapel";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Kelas = "Kelas";
            public const string UrutanKelas = "UrutanKelas";
            public const string JenisKelas = "JenisKelas";
        }

        public static FormasiGuruMapel GetEntityFromDataRow(DataRow row)
        {
            return new FormasiGuruMapel
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Sekolah = new Guid(row[NamaField.Rel_Sekolah].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Kelas = row[NamaField.Rel_Kelas].ToString()
            };
        }

        public static FormasiGuruMapel_Ext GetEntityFromDataRow_Ext(DataRow row)
        {
            return new FormasiGuruMapel_Ext
            {
                Rel_Sekolah = row[NamaField_Ext.Rel_Sekolah].ToString(),
                Rel_Mapel = row[NamaField_Ext.Rel_Mapel].ToString(),
                TahunAjaran = row[NamaField_Ext.TahunAjaran].ToString(),
                Semester = row[NamaField_Ext.Semester].ToString(),                
                Rel_Kelas = row[NamaField_Ext.Rel_Kelas].ToString(),
                Mapel = row[NamaField_Ext.Mapel].ToString(),
                Rel_KelasDet = row[NamaField_Ext.Rel_KelasDet].ToString(),
                Kelas = row[NamaField_Ext.Kelas].ToString(),
                UrutanKelas = row[NamaField_Ext.UrutanKelas].ToString(),
                JenisKelas = row[NamaField_Ext.JenisKelas].ToString()
            };
        }

        public static List<FormasiGuruMapel> GetAll_Entity()
        {
            List<FormasiGuruMapel> hasil = new List<FormasiGuruMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static FormasiGuruMapel GetByID_Entity(string kode)
        {
            FormasiGuruMapel hasil = new FormasiGuruMapel();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = GetEntityFromDataRow(row);
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static List<FormasiGuruMapel> GetByUnit_Entity(string rel_sekolah)
        {
            List<FormasiGuruMapel> hasil = new List<FormasiGuruMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_UNIT;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static List<FormasiGuruMapel> GetByGuru_Entity(string rel_guru)
        {
            List<FormasiGuruMapel> hasil = new List<FormasiGuruMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_Guru;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static bool IsSebagaiGuru(string no_induk, string rel_sekolah)
        {
            bool hasil = false;

            List<FormasiGuruMapel> lst_formasi_guru_mapel = DAO_FormasiGuruMapel.GetByGuru_Entity(no_induk).FindAll(m => m.Rel_Sekolah == new Guid(rel_sekolah));
            foreach (FormasiGuruMapel formasi in lst_formasi_guru_mapel)
            {
                return true;
            }

            return hasil;
        }

        public static bool IsSebagaiGuru(string no_induk, int urut_jenjang)
        {
            bool hasil = false;

            List<FormasiGuruMapel> lst_formasi_guru_mapel = DAO_FormasiGuruMapel.GetByGuru_Entity(no_induk);
            foreach (FormasiGuruMapel formasi in lst_formasi_guru_mapel)
            {
                Sekolah m_sekolah_guru_mapel = DAO_Sekolah.GetByID_Entity(formasi.Rel_Sekolah.ToString());
                if (m_sekolah_guru_mapel != null)
                {
                    if (m_sekolah_guru_mapel.Nama != null)
                    {
                        if (m_sekolah_guru_mapel.UrutanJenjang == urut_jenjang)
                        {
                            return true;
                        }
                    }
                }
            }

            return hasil;
        }

        public static List<Kelas> GetKelasByGuruByTahunAjaranBySemester_Entity(
                string rel_guru,
                string tahun_ajaran,
                string semester
            )
        {
            List<Kelas> hasil = new List<Kelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_KELAS_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new Kelas {
                        Kode = new Guid(row[DAO_Kelas.NamaField.Kode].ToString()),
                        Rel_Sekolah = new Guid(row[DAO_Kelas.NamaField.Rel_Sekolah].ToString()),
                        Nama = row[DAO_Kelas.NamaField.Nama].ToString(),
                        UrutanLevel = Convert.ToInt16(row[DAO_Kelas.NamaField.UrutanLevel]),
                        Keterangan = row[DAO_Kelas.NamaField.UrutanLevel].ToString(),
                        IsAktif = (row[DAO_Kelas.NamaField.IsAktif] == DBNull.Value ? false : Convert.ToBoolean(row[DAO_Kelas.NamaField.IsAktif]))
                    });
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static List<FormasiGuruMapel_Ext> GetByTABySMByKelasByKelasDet_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_kelas_det
            )
        {
            List<FormasiGuruMapel_Ext> hasil = new List<FormasiGuruMapel_Ext>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_KELASDET;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_Ext(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static List<Siswa> GetSiswaByTABySMByKelasByMapelBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_mapel, 
                string rel_siswa
            )
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(DAO_Siswa.GetEntityFromDataRow(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static List<DAO_Siswa.SiswaByFormasiMapel> GetSiswaByTABySM_Entity(
                string tahun_ajaran,
                string semester
            )
        {
            List<DAO_Siswa.SiswaByFormasiMapel> hasil = new List<DAO_Siswa.SiswaByFormasiMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new DAO_Siswa.SiswaByFormasiMapel
                    {
                        Kode = new Guid(row["Kode"].ToString()),
                        Nama = row["Nama"].ToString(),
                        NISN = row["NISN"].ToString(),
                        NISSekolah = row["NISSekolah"].ToString(),
                        JenisKelamin = row["JenisKelamin"].ToString(),
                        JenisKelas = row["JenisKelas"].ToString(),
                        Rel_Kelas = row["Rel_Kelas"].ToString(),
                        Rel_KelasDet = row["Rel_KelasDet"].ToString(),
                        Rel_KelasDetJurusan = row["Rel_KelasDetJurusan"].ToString(),
                        Rel_KelasDetSosialisasi = row["Rel_KelasDetSosialisasi"].ToString(),
                        Rel_Mapel = row["Rel_Mapel"].ToString()
                    });
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static List<DAO_Siswa.SiswaByFormasiMapel> GetSiswaByTABySMByKelas_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas
            )
        {
            List<DAO_Siswa.SiswaByFormasiMapel> hasil = new List<DAO_Siswa.SiswaByFormasiMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new DAO_Siswa.SiswaByFormasiMapel
                    {
                        Kode = new Guid(row["Kode"].ToString()),
                        Nama = row["Nama"].ToString(),
                        NISN = row["NISN"].ToString(),
                        NISSekolah = row["NISSekolah"].ToString(),
                        JenisKelamin = row["JenisKelamin"].ToString(),
                        JenisKelas = row["JenisKelas"].ToString(),
                        Rel_Kelas = row["Rel_Kelas"].ToString(),
                        Rel_KelasDet = row["Rel_KelasDet"].ToString(),
                        Rel_KelasDetJurusan = row["Rel_KelasDetJurusan"].ToString(),
                        Rel_KelasDetSosialisasi = row["Rel_KelasDetSosialisasi"].ToString(),
                        Rel_Mapel = row["Rel_Mapel"].ToString()
                    });
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static void Delete(string Kode, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Kode));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static void Insert(FormasiGuruMapel formasi_guru, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                if (formasi_guru.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) formasi_guru.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, formasi_guru.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, formasi_guru.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, formasi_guru.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, formasi_guru.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, formasi_guru.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, formasi_guru.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));

                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void Update(FormasiGuruMapel formasi_guru, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, formasi_guru.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, formasi_guru.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, formasi_guru.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, formasi_guru.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, formasi_guru.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, formasi_guru.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));

                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static List<FormasiGuruMapel> GetByUnitByTABySM_Entity(string rel_sekolah, string tahun_ajaran, string semester)
        {
            List<FormasiGuruMapel> hasil = new List<FormasiGuruMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_UNIT_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@Rel_Sekolah", rel_sekolah);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }
    }
}