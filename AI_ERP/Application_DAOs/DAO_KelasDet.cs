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
    public static class DAO_KelasDet
    {
        public const string SP_SELECT_ALL = "KelasDet_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "KelasDet_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "KelasDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_KELAS = "KelasDet_SELECT_BY_KELAS";
        public const string SP_SELECT_BY_SEKOLAH = "KelasDet_SELECT_BY_SEKOLAH";

        public const string SP_INSERT = "KelasDet_INSERT";

        public const string SP_UPDATE = "KelasDet_UPDATE";

        public const string SP_DELETE = "KelasDet_DELETE";
        public const string SP_DELETE_BY_KELAS = "KelasDet_DELETE_BY_KELAS";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Nama = "Nama";
            public const string UrutanKelas = "UrutanKelas";
            public const string Keterangan = "Keterangan";
            public const string IsKelasJurusan = "IsKelasJurusan";
            public const string IsKelasSosialisasi = "IsKelasSosialisasi";
            public const string IsAktif = "IsAktif";
        }

        public static KelasDet GetEntityFromDataRow(DataRow row)
        {
            return new KelasDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Kelas = new Guid(row[NamaField.Rel_Kelas].ToString()),
                Nama = row[NamaField.Nama].ToString(),
                UrutanKelas = Convert.ToInt16(row[NamaField.UrutanKelas]),
                IsKelasJurusan = (row[NamaField.IsKelasJurusan] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsKelasJurusan])),
                IsKelasSosialisasi = (row[NamaField.IsKelasSosialisasi] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsKelasSosialisasi])),
                IsAktif = (row[NamaField.IsAktif] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsAktif])),
                Keterangan = row[NamaField.Keterangan].ToString()
            };
        }

        public static List<KelasDet> GetByKelas_Entity(string rel_kelas)
        {
            List<KelasDet> hasil = new List<KelasDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (rel_kelas == null || rel_kelas.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_KELAS;
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);

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

        public static List<KelasDet> GetBySekolah_Entity(string rel_sekolah)
        {
            List<KelasDet> hasil = new List<KelasDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (rel_sekolah == null || rel_sekolah.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SEKOLAH;
                comm.Parameters.AddWithValue("@Rel_Sekolah", rel_sekolah);

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

        public static List<KelasDet> GetAll_Entity()
        {
            List<KelasDet> hasil = new List<KelasDet>();
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

        public static KelasDet GetByID_Entity(string kode)
        {
            KelasDet hasil = new KelasDet();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
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
        public static void DeleteByKelas(string rel_kelas)
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
                comm.CommandText = SP_DELETE_BY_KELAS;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, rel_kelas));
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

        public static void Insert(KelasDet kelas_det)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, kelas_det.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, kelas_det.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanKelas, kelas_det.UrutanKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, kelas_det.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelasJurusan, kelas_det.IsKelasJurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelasSosialisasi, kelas_det.IsKelasSosialisasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsAktif, kelas_det.IsAktif));
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

        public static void Update(KelasDet kelas_det)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kelas_det.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, kelas_det.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, kelas_det.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanKelas, kelas_det.UrutanKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, kelas_det.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelasJurusan, kelas_det.IsKelasJurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelasSosialisasi, kelas_det.IsKelasSosialisasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsAktif, kelas_det.IsAktif));
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

        public static string GetKelasForRapor(string rel_kelas_det)
        {
            string hasil = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    string[] arr_kelas = m_kelas_det.Nama.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {

                            if (arr_kelas.Length == 2)
                            {
                                switch (m_kelas.Nama.ToUpper().Trim())
                                {
                                    case "I":
                                        return arr_kelas[0] + " (satu) " + arr_kelas[1];
                                    case "II":
                                        return arr_kelas[0] + " (dua) " + arr_kelas[1];
                                    case "III":
                                        return arr_kelas[0] + " (tiga) " + arr_kelas[1];
                                    case "IV":
                                        return arr_kelas[0] + " (empat) " + arr_kelas[1];
                                    case "V":
                                        return arr_kelas[0] + " (lima) " + arr_kelas[1];
                                    case "VI":
                                        return arr_kelas[0] + " (enam) " + arr_kelas[1];
                                    default:
                                        return arr_kelas[0] + " (tujuh) " + arr_kelas[1];
                                }
                            }

                        }
                    }
                }
            }

            return hasil;
        }
    }
}