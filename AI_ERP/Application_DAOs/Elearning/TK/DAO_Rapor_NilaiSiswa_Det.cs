using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.TK;

namespace AI_ERP.Application_DAOs.Elearning.TK
{
    public static class DAO_Rapor_NilaiSiswa_Det
    {
        public const string SP_SELECT_ALL = "TK_Rapor_NilaiSiswa_Det_SELECT_ALL";
        public const string SP_SELECT_BY_HEADER = "TK_Rapor_NilaiSiswa_Det_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "TK_Rapor_NilaiSiswa_Det_SELECT_BY_ID";
        public const string SP_SELECT_POIN_PENILAIAN = "TK_Rapor_NilaiSiswa_Det_SELECT_POIN_PENILAIAN";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET = "TK_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KELAS_DET";

        public const string SP_CREATE_NILAI_STANDAR = "TK_Rapor_NilaiSiswa_Det_CREATE_NILAI_STANDAR";
        public const string SP_CREATE_NILAI_STANDAR_EKSKUL = "TK_Rapor_NilaiSiswa_Det_CREATE_NILAI_STANDAR_EKSKUL";

        public const string SP_INSERT = "TK_Rapor_NilaiSiswa_Det_INSERT";

        public const string SP_UPDATE = "TK_Rapor_NilaiSiswa_Det_UPDATE";

        public const string SP_DELETE = "TK_Rapor_NilaiSiswa_Det_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_NilaiSiswa = "Rel_Rapor_NilaiSiswa";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Rapor_DesignDet = "Rel_Rapor_DesignDet";
            public const string Rel_Rapor_Kriteria = "Rel_Rapor_Kriteria";
            public const string Deskripsi = "Deskripsi";
        }

        private static Rapor_NilaiSiswa_Det GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiSiswa_Det
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSiswa = new Guid(row[NamaField.Rel_Rapor_NilaiSiswa].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Rapor_DesignDet = row[NamaField.Rel_Rapor_DesignDet].ToString(),
                Rel_Rapor_Kriteria = row[NamaField.Rel_Rapor_Kriteria].ToString(),
                Deskripsi = row[NamaField.Deskripsi].ToString()
            };
        }

        public static List<Rapor_NilaiSiswa_Det> GetAll_Entity()
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
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

        public static List<Rapor_NilaiSiswa_Det> GetByHeader_Entity(string rel_rapornilai)
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_NilaiSiswa, rel_rapornilai);

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

        public static List<Rapor_NilaiSiswa_Det> GetByTABySMByKelasDet_Entity(string tahun_ajaran, string semester, string rel_kelas_det)
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);

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

        public static List<Rapor_NilaiSiswa_Det> GetPoinPenilaian_Entity(string rel_rapornilaisiswa, string rel_siswa, string rel_rapor_design_det)
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_POIN_PENILAIAN;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_NilaiSiswa, rel_rapornilaisiswa);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_DesignDet, rel_rapor_design_det);

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

        public static Rapor_NilaiSiswa_Det GetByID_Entity(string kode)
        {
            Rapor_NilaiSiswa_Det hasil = new Rapor_NilaiSiswa_Det();
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

        public static void Insert(Rapor_NilaiSiswa_Det m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSiswa, m.Rel_Rapor_NilaiSiswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_DesignDet, m.Rel_Rapor_DesignDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Kriteria, m.Rel_Rapor_Kriteria));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
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

        public static void CreateNilaiStandar(string rel_rapor_nilaisiswa, string rel_siswa, string rel_rapor_kriteria, string rel_rapor_design)
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
                comm.CommandText = SP_CREATE_NILAI_STANDAR;

                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_NilaiSiswa", rel_rapor_nilaisiswa));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_Kriteria", rel_rapor_kriteria));
                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_Design", rel_rapor_design));
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

        public static void CreateNilaiStandarEkskul(string rel_rapor_nilaisiswa, string rel_siswa, string rel_rapor_kriteria, string rel_rapor_design, string rel_mapel)
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
                comm.CommandText = SP_CREATE_NILAI_STANDAR_EKSKUL;

                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_NilaiSiswa", rel_rapor_nilaisiswa));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_Kriteria", rel_rapor_kriteria));
                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_Design", rel_rapor_design));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));
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

        public static void Update(Rapor_NilaiSiswa_Det m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSiswa, m.Rel_Rapor_NilaiSiswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_DesignDet, m.Rel_Rapor_DesignDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Kriteria, m.Rel_Rapor_Kriteria));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));                
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