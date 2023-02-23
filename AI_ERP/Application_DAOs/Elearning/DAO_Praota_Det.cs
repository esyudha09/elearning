using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_Praota_Det
    {
        public const string SP_SELECT_BY_TA_BY_KELAS_BY_GURU_BY_MAPEL = "Praota_Det_SELECT_BY_TA_BY_KELAS_BY_GURU_BY_MAPEL";
        public const string SP_SELECT_BY_ID = "Praota_Det_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "Praota_Det_SELECT_BY_HEADER";
        
        public const string SP_INSERT = "Praota_Det_INSERT";

        public const string SP_UPDATE = "Praota_Det_UPDATE";

        public const string SP_DELETE = "Praota_Det_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Praota = "Rel_Praota";
            public const string Semester = "Semester";
            public const string MateriPokok = "MateriPokok";
            public const string DeskripsiMateriPokok = "DeskripsiMateriPokok";
            public const string AlokasiWaktu = "AlokasiWaktu";
            public const string EstimasiWaktuDari = "EstimasiWaktuDari";
            public const string EstimasiWaktuSampai = "EstimasiWaktuSampai";
            public const string Keterangan = "Keterangan";
            public const string JenisFile = "JenisFile";
            public const string URLEmbed = "URLEmbed";
            public const string Urutan = "Urutan";
        }

        private static Praota_Det GetEntityFromDataRow(DataRow row)
        {
            return new Praota_Det
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Praota = new Guid(row[NamaField.Rel_Praota].ToString()),
                Semester = row[NamaField.Semester].ToString(),
                MateriPokok = row[NamaField.MateriPokok].ToString(),
                DeskripsiMateriPokok = row[NamaField.DeskripsiMateriPokok].ToString(),
                AlokasiWaktu = row[NamaField.AlokasiWaktu].ToString(),
                EstimasiWaktuDari = row[NamaField.EstimasiWaktuDari].ToString(),
                EstimasiWaktuSampai = row[NamaField.EstimasiWaktuSampai].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                JenisFile = row[NamaField.JenisFile].ToString(),
                URLEmbed = row[NamaField.URLEmbed].ToString(),
                Urutan = Convert.ToUInt16(row[NamaField.Urutan])
            };
        }

        public static List<Praota_Det> GetByTAByUnitByKelasByGuru_Entity(
                string tahun_ajaran,
                string rel_kelas,
                string rel_guru,
                string rel_mapel
            )
        {
            List<Praota_Det> hasil = new List<Praota_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_KELAS_BY_GURU_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + DAO_Praota.NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + DAO_Praota.NamaField.Rel_Kelas, rel_kelas);
                comm.Parameters.AddWithValue("@" + DAO_Praota.NamaField.Rel_Guru, rel_guru);
                comm.Parameters.AddWithValue("@" + DAO_Praota.NamaField.Rel_Mapel, rel_mapel);

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

        public static void Delete(string Kode, string user_id)
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

        public static Praota_Det GetByID_Entity(string kode)
        {
            Praota_Det hasil = new Praota_Det();
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

        public static void Insert(Praota_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Praota, m.Rel_Praota));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.MateriPokok, m.MateriPokok));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiMateriPokok, m.DeskripsiMateriPokok));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlokasiWaktu, m.AlokasiWaktu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EstimasiWaktuDari, m.EstimasiWaktuDari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EstimasiWaktuSampai, m.EstimasiWaktuSampai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisFile, m.JenisFile));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.URLEmbed, m.URLEmbed));
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

        public static void Update(Praota_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Praota, m.Rel_Praota));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.MateriPokok, m.MateriPokok));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiMateriPokok, m.DeskripsiMateriPokok));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlokasiWaktu, m.AlokasiWaktu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EstimasiWaktuDari, m.EstimasiWaktuDari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EstimasiWaktuSampai, m.EstimasiWaktuSampai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisFile, m.JenisFile));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.URLEmbed, m.URLEmbed));
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
    }
}