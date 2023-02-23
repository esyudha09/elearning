using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_FormasiEkskul
    {
        public const string SP_SELECT_ALL = "SMP_FormasiEkskul_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMP_FormasiEkskul_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_PERIODE = "SMP_FormasiEkskul_SELECT_ALL_BY_PERIODE";
        public const string SP_SELECT_ALL_BY_PERIODE_FOR_SEARCH = "SMP_FormasiEkskul_SELECT_ALL_BY_PERIODE_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SMP_FormasiEkskul_SELECT_BY_ID";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER = "SMP_FormasiEkskul_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN = "SMP_FormasiEkskul_SELECT_BY_GURU_BY_TAHUN_AJARAN";
        public const string SP_SELECT_MAPEL_BY_GURU_BY_TAHUN_AJARAN = "SMP_FormasiEkskul_SELECT_MAPEL_BY_GURU_BY_TAHUN_AJARAN";
        public const string SP_SELECT_KELAS_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN = "SMP_FormasiEkskul_SELECT_KELAS_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN";
        public const string SP_SELECT_KELAS_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER = "SMP_FormasiEkskul_SELECT_KELAS_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN = "SMP_FormasiEkskul_SELECT_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN";
        public const string SP_SELECT_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER = "SMP_FormasiEkskul_SELECT_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER";

        public const string SP_INSERT = "SMP_FormasiEkskul_INSERT";

        public const string SP_UPDATE = "SMP_FormasiEkskul_UPDATE";

        public const string SP_DELETE = "SMP_FormasiEkskul_DELETE";

        public class KelasFormasi
        {
            public string Rel_Mapel { get; set; }
            public string Rel_Kelas1 { get; set; }
            public string Rel_Kelas2 { get; set; }
            public string Rel_Kelas3 { get; set; }
        }

        public static class NamaField_KelasFormasi
        {
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Kelas1 = "Rel_Kelas";
            public const string Rel_Kelas2 = "Rel_Kelas2";
            public const string Rel_Kelas3 = "Rel_Kelas3";
        }

        public static KelasFormasi GetEntityFromDataRow_KelasFormasi(DataRow row)
        {
            return new KelasFormasi
            {
                Rel_Mapel = row[NamaField_KelasFormasi.Rel_Mapel].ToString(),
                Rel_Kelas1 = row[NamaField_KelasFormasi.Rel_Kelas1].ToString(),
                Rel_Kelas2 = row[NamaField_KelasFormasi.Rel_Kelas2].ToString(),
                Rel_Kelas3 = row[NamaField_KelasFormasi.Rel_Kelas3].ToString()
            };
        }

        public class FormasiMapel
        {
            public string Rel_Mapel { get; set; }
            public string Nama { get; set; }
            public string Alias { get; set; }
            public string Jenis { get; set; }
            public string Keterangan { get; set; }
            public string Rel_Sekolah { get; set; }
            public string KodeSN { get; set; }
            public string Rel_Kelas { get; set; }
            public string Rel_Kelas2 { get; set; }
            public string Rel_Kelas3 { get; set; }
            public string KodeFormasiEkskul { get; set; }
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
        }

        public static class NamaField_FormasiMapel
        {
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Nama = "Nama";
            public const string Alias = "Alias";
            public const string Jenis = "Jenis";
            public const string Keterangan = "Keterangan";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string KodeSN = "KodeSN";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Kelas2 = "Rel_Kelas2";
            public const string Rel_Kelas3 = "Rel_Kelas3";
            public const string KodeFormasiEkskul = "KodeFormasiEkskul";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
        }

        public static FormasiMapel GetEntityFromDataRow_FormasiMapel(DataRow row)
        {
            return new FormasiMapel
            {
                Rel_Mapel = row[NamaField_FormasiMapel.Rel_Mapel].ToString(),
                Nama = row[NamaField_FormasiMapel.Nama].ToString(),
                Alias = row[NamaField_FormasiMapel.Alias].ToString(),
                Jenis = row[NamaField_FormasiMapel.Jenis].ToString(),
                Keterangan = row[NamaField_FormasiMapel.Keterangan].ToString(),
                Rel_Sekolah = row[NamaField_FormasiMapel.Rel_Sekolah].ToString(),
                KodeSN = row[NamaField_FormasiMapel.KodeSN].ToString(),
                Rel_Kelas = row[NamaField_FormasiMapel.Rel_Kelas].ToString(),
                Rel_Kelas2 = row[NamaField_FormasiMapel.Rel_Kelas2].ToString(),
                Rel_Kelas3 = row[NamaField_FormasiMapel.Rel_Kelas3].ToString(),
                KodeFormasiEkskul = row[NamaField_FormasiMapel.KodeFormasiEkskul].ToString(),
                TahunAjaran = row[NamaField_FormasiMapel.TahunAjaran].ToString(),
                Semester = row[NamaField_FormasiMapel.Semester].ToString()
            };
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Guru = "Rel_Guru";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Rapor_StrukturNilai = "Rel_Rapor_StrukturNilai";
        }

        public static FormasiEkskul GetEntityFromDataRow(DataRow row)
        {
            return new FormasiEkskul
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Rapor_StrukturNilai = row[NamaField.Rel_Rapor_StrukturNilai].ToString()
            };
        }

        public static List<FormasiEkskul> GetAll_Entity()
        {
            List<FormasiEkskul> hasil = new List<FormasiEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static FormasiEkskul GetByID_Entity(string kode)
        {
            FormasiEkskul hasil = new FormasiEkskul();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static void Delete(string Kode)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static void Insert(FormasiEkskul m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, m.Rel_Rapor_StrukturNilai));

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

        public static void Update(FormasiEkskul m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, m.Rel_Rapor_StrukturNilai));

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

        public static List<FormasiEkskul> GetByTABySM_Entity(string tahun_ajaran, string semester)
        {
            List<FormasiEkskul> hasil = new List<FormasiEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER;
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
        
        public static List<FormasiEkskul> GetByGuruByTA_Entity(string rel_guru, string tahun_ajaran)
        {
            List<FormasiEkskul> hasil = new List<FormasiEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_TAHUN_AJARAN;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);                

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

        public static List<Mapel> GetMapelByGuruByTA_Entity(string rel_guru, string tahun_ajaran)
        {
            List<Mapel> hasil = new List<Mapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_MAPEL_BY_GURU_BY_TAHUN_AJARAN;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(DAO_Mapel.GetEntityFromDataRow(row));
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

        public static List<KelasFormasi> GetKelasFormasiByGuruByTA_Entity(string rel_guru, string tahun_ajaran)
        {
            List<KelasFormasi> hasil = new List<KelasFormasi>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_KELAS_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_KelasFormasi(row));
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

        public static List<KelasFormasi> GetKelasFormasiByGuruByTABySemester_Entity(string rel_guru, string tahun_ajaran, string semester)
        {
            List<KelasFormasi> hasil = new List<KelasFormasi>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_KELAS_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_KelasFormasi(row));
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

        public static List<FormasiMapel> GetFormasiMapelByGuruByTA_Entity(string rel_guru, string tahun_ajaran)
        {
            List<FormasiMapel> hasil = new List<FormasiMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_FormasiMapel(row));
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

        public static List<FormasiMapel> GetFormasiMapelByGuruByTABySemester_Entity(string rel_guru, string tahun_ajaran, string semester)
        {
            List<FormasiMapel> hasil = new List<FormasiMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_FORMASI_MAPEL_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_FormasiMapel(row));
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