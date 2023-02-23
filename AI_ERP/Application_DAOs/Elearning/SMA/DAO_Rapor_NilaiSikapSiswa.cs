using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMA;

namespace AI_ERP.Application_DAOs.Elearning.SMA
{
    public static class DAO_Rapor_NilaiSikapSiswa
    {
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet = "SMA_Rapor_NilaiSikapSiswa_SELECT_BY_TA_BY_SM_BY_KelasDet";
        public const string SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET_BY_SISWA = "SMA_Rapor_NilaiSikapSiswa_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET_BY_SISWA";        

        public const string SP_SELECT_SIKAP_SPIRITUAL_BY_TA_BY_SM_BY_MAPEL = "SMA_Rapor_NilaiSikapSiswa_SELECT_SIKAP_SPIRITUAL_BY_TA_BY_SM_BY_MAPEL";
        public const string SP_SELECT_SIKAP_SPIRITUAL_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiSikapSiswa_SELECT_SIKAP_SPIRITUAL_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";

        public const string SP_SELECT_SIKAP_SOSIAL_BY_TA_BY_SM_BY_MAPEL = "SMA_Rapor_NilaiSikapSiswa_SELECT_SIKAP_SOSIAL_BY_TA_BY_SM_BY_MAPEL";        
        public const string SP_SELECT_SIKAP_SOSIAL_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiSikapSiswa_SELECT_SIKAP_SOSIAL_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";

        public const string SP_SELECT_BY_ID = "SMA_Rapor_NilaiSikapSiswa_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER_BY_SISWA = "SMA_Rapor_NilaiSikapSiswa_SELECT_BY_HEADER_BY_SISWA";

        public const string SP_INSERT = "SMA_Rapor_NilaiSikapSiswa_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_NilaiSikapSiswa_UPDATE";
        public const string SP_UPDATE_NILAI_AKHIR = "SMA_Rapor_NilaiSikapSiswa_UPDATE_NILAI_AKHIR";

        public const string SP_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiSikapSiswa_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";
        public const string SP_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET_BY_SISWA = "SMA_Rapor_NilaiSikapSiswa_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET_BY_SISWA";

        public const string SP_DELETE = "SMA_Rapor_NilaiSikapSiswa_DELETE";

        public class Rapor_NilaiSikapSiswa_Lengkap : Rapor_NilaiSikapSiswa
        {
            public string Rel_Mapel { get; set; }
            public string PredikatSikapSpiritual { get; set; }
            public string PredikatSikapSosial { get; set; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_NilaiSikap = "Rel_Rapor_NilaiSikap";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string SikapSpiritual = "SikapSpiritual";
            public const string SikapSosial = "SikapSosial";

            public const string Rel_Mapel = "Rel_Mapel";
            public const string PredikatSikapSpiritual = "PredikatSikapSpiritual";
            public const string PredikatSikapSosial = "PredikatSikapSosial";

            public const string DeskripsiSikapSpiritual = "DeskripsiSikapSpiritual";
            public const string DeskripsiSikapSosial = "DeskripsiSikapSosial";
            public const string SikapSpiritualAkhir = "SikapSpiritualAkhir";
            public const string SikapSosialAkhir = "SikapSosialAkhir";
        }

        private static Rapor_NilaiSikapSiswa GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiSikapSiswa
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSikap = row[NamaField.Rel_Rapor_NilaiSikap].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                SikapSpiritual = row[NamaField.SikapSpiritual].ToString(),
                SikapSosial = row[NamaField.SikapSosial].ToString(),
                DeskripsiSikapSpiritual = row[NamaField.DeskripsiSikapSpiritual].ToString(),
                DeskripsiSikapSosial = row[NamaField.DeskripsiSikapSosial].ToString(),
                SikapSpiritualAkhir = row[NamaField.SikapSpiritualAkhir].ToString(),
                SikapSosialAkhir = row[NamaField.SikapSosialAkhir].ToString()
            };
        }

        private static Rapor_NilaiSikapSiswa_Lengkap GetEntityFromDataRow_Lengkap(DataRow row)
        {
            return new Rapor_NilaiSikapSiswa_Lengkap
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSikap = row[NamaField.Rel_Rapor_NilaiSikap].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                SikapSpiritual = row[NamaField.SikapSpiritual].ToString(),
                SikapSosial = row[NamaField.SikapSosial].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                PredikatSikapSpiritual = row[NamaField.PredikatSikapSpiritual].ToString(),
                PredikatSikapSosial = row[NamaField.PredikatSikapSosial].ToString(),
                DeskripsiSikapSpiritual = row[NamaField.DeskripsiSikapSpiritual].ToString(),
                DeskripsiSikapSosial = row[NamaField.DeskripsiSikapSosial].ToString(),
                SikapSpiritualAkhir = row[NamaField.SikapSpiritualAkhir].ToString(),
                SikapSosialAkhir = row[NamaField.SikapSosialAkhir].ToString()
            };
        }

        public static List<Rapor_NilaiSikapSiswa> GetSikapSpiritualByTABySMByMapel_Entity(string tahun_ajaran, string semester, string rel_mapel)
        {
            List<Rapor_NilaiSikapSiswa> hasil = new List<Rapor_NilaiSikapSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SIKAP_SPIRITUAL_BY_TA_BY_SM_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);

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

        public static List<Rapor_NilaiSikapSiswa_Lengkap> GetByTABySMByMapelByKelasDet_Entity(string tahun_ajaran, string semester, string rel_kelas_det)
        {
            List<Rapor_NilaiSikapSiswa_Lengkap> hasil = new List<Rapor_NilaiSikapSiswa_Lengkap>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KelasDet;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_Lengkap(row));
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

        public static List<Rapor_NilaiSikapSiswa> GetByTABySMByMapelByKelasDetBySiswa_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_kelas_det, string rel_siswa)
        {
            List<Rapor_NilaiSikapSiswa> hasil = new List<Rapor_NilaiSikapSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

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

        public static List<Rapor_NilaiSikapSiswa> GetSikapSpiritualByTABySMByMapelBySiswa_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_siswa)
        {
            List<Rapor_NilaiSikapSiswa> hasil = new List<Rapor_NilaiSikapSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SIKAP_SPIRITUAL_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

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

        public static List<Rapor_NilaiSikapSiswa> GetSikapSosialByTABySMByMapel_Entity(string tahun_ajaran, string semester, string rel_mapel)
        {
            List<Rapor_NilaiSikapSiswa> hasil = new List<Rapor_NilaiSikapSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SIKAP_SOSIAL_BY_TA_BY_SM_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);

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

        public static List<Rapor_NilaiSikapSiswa> GetSikapSosialByTABySMByMapelBySiswa_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_siswa)
        {
            List<Rapor_NilaiSikapSiswa> hasil = new List<Rapor_NilaiSikapSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SIKAP_SOSIAL_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

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

        public static List<Rapor_NilaiSikapSiswa> GetByHeaderBySiswa_Entity(string rel_rapor_nilaisikap, string rel_siswa)
        {
            List<Rapor_NilaiSikapSiswa> hasil = new List<Rapor_NilaiSikapSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER_BY_SISWA;
                comm.Parameters.AddWithValue("@Rel_Rapor_NilaiSikap", rel_rapor_nilaisikap);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

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

        public static Rapor_NilaiSikapSiswa GetByID_Entity(string kode)
        {
            Rapor_NilaiSikapSiswa hasil = new Rapor_NilaiSikapSiswa();
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

        public static void Insert(Rapor_NilaiSikapSiswa m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                Guid kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSikap, m.Rel_Rapor_NilaiSikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SikapSpiritual, m.SikapSpiritual));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SikapSosial, m.SikapSosial));
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

        public static void SaveNilaiSikap(
                string tahun_ajaran,
                string semester,
                string rel_mapel,
                string rel_kelasdet,
                string rel_siswa,
                string sikap_spiritual,
                string sikap_sosial
            )
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET_BY_SISWA;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_KelasDet", rel_kelasdet));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@SikapSpiritual", sikap_spiritual));
                comm.Parameters.Add(new SqlParameter("@SikapSosial", sikap_sosial));
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
        
        public static void Update(Rapor_NilaiSikapSiswa m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSikap, m.Rel_Rapor_NilaiSikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SikapSpiritual, m.SikapSpiritual));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SikapSosial, m.SikapSosial));
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

        public static void UpdateNilaiAkhir(
            string kode, 
            string deskripsi_sikap_spiritual, 
            string deskripsi_sikap_sosial, 
            string sikap_spiritual_akhir,
            string sikap_sosial_akhir
        )
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE_NILAI_AKHIR;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSpiritual, deskripsi_sikap_spiritual));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSosial, deskripsi_sikap_sosial));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SikapSpiritualAkhir, sikap_spiritual_akhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SikapSosialAkhir, sikap_sosial_akhir));
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
    }
}