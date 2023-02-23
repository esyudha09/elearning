using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_SiswaAbsenRekapDet
    {
        public const string SP_SELECT_BY_ID = "SiswaAbsenRekapDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "SiswaAbsenRekapDet_SELECT_BY_HEADER";

        public const string SP_INSERT = "SiswaAbsenRekapDet_INSERT";

        public const string SP_UPDATE = "SiswaAbsenRekapDet_UPDATE";

        public const string SP_DELETE = "SiswaAbsenRekapDet_DELETE";
        public const string SP_DELETE_BY_HEADER = "SiswaAbsenRekapDet_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_SiswaAbsenRekap = "Rel_SiswaAbsenRekap";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Hadir = "Hadir";
            public const string JumlahPertemuan = "JumlahPertemuan";
            public const string Sakit = "Sakit";
            public const string Izin = "Izin";
            public const string Alpa = "Alpa";
            public const string Terlambat = "Terlambat";
        }

        private static SiswaAbsenRekapDet GetEntityFromDataRow(DataRow row)
        {
            return new SiswaAbsenRekapDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_SiswaAbsenRekap = row[NamaField.Rel_SiswaAbsenRekap].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Hadir = row[NamaField.Hadir].ToString(),
                JumlahPertemuan = row[NamaField.JumlahPertemuan].ToString(),
                Sakit = row[NamaField.Sakit].ToString(),
                Izin = row[NamaField.Izin].ToString(),
                Alpa = row[NamaField.Alpa].ToString(),
                Terlambat = row[NamaField.Terlambat].ToString()
            };
        }

        public static void Delete(string Kode)
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

        public static void DeleteByHeader(string rel_siswaabsenrekap)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_SiswaAbsenRekap, rel_siswaabsenrekap));
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

        public static void Insert(SiswaAbsenRekapDet m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_SiswaAbsenRekap, m.Rel_SiswaAbsenRekap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hadir, m.Hadir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahPertemuan, m.JumlahPertemuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sakit, m.Sakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Izin, m.Izin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alpa, m.Alpa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Terlambat, m.Terlambat));
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

        public static void Update(SiswaAbsenRekapDet m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_SiswaAbsenRekap, m.Rel_SiswaAbsenRekap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hadir, m.Hadir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahPertemuan, m.JumlahPertemuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sakit, m.Sakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Izin, m.Izin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alpa, m.Alpa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Terlambat, m.Terlambat));
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

        public static List<SiswaAbsenRekapDet> GetAllByHeader_Entity(
                string rel_siswaabsenrekap
            )
        {
            List<SiswaAbsenRekapDet> hasil = new List<SiswaAbsenRekapDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_SiswaAbsenRekap, rel_siswaabsenrekap);

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