using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_Sekolah
    {
        public const string SP_SELECT_ALL = "Sekolah_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "Sekolah_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "Sekolah_SELECT_BY_ID";

        public const string SP_INSERT = "Sekolah_INSERT";

        public const string SP_UPDATE = "Sekolah_UPDATE";

        public const string SP_DELETE = "Sekolah_DELETE";

        public static string GetTokenSekolah(Libs.UnitSekolah unit_sekolah)
        {
            switch (unit_sekolah)
            {
                case Libs.UnitSekolah.SALAH:
                    return "";
                case Libs.UnitSekolah.KB:
                    return "klerwpoCj0KCQjwvdXpBRCoARIsAMJSKqJ4J0ZiUcdCZIv68bM76HHKFkDTE_5UHPtIxoCPDtchnTyCRd6d56adfohineajgkAngSkwSx";
                case Libs.UnitSekolah.TK:
                    return "87fjghysdfkjhCj0KCQjwvdXpBRCoARIsAMJSKqJ4J0ZiUcdCZIv68bM76HHKFkDTE_5UHPtIxoCPDtchnTyCRdTyCRd6d56adfoj0KCQ";
                case Libs.UnitSekolah.SD:
                    return "eqweretbnkjhnbCj0KCQjwvdXpBRCoARIsAMJSKqJ4J0ZiUcdCZIv68bM76HHKFkDTE_5UHPtIxoCPDtchnTyCRdgt54fsdCPDtchnTkY";
                case Libs.UnitSekolah.SMP:
                    return "kjhrweuykjbmgCj0KCQjwvdXpBRCoARIsAMJSKqJ4J0ZiUcdCZIv68bM76HHKFkDTE_5UHPtIxoCPDtchnTyCRdjhjjhTyCRd6d56adfo";
                case Libs.UnitSekolah.SMA:
                    return "cj0KCQjwvdXpBRCoARIsAMJSKqJ4J0ZiUcdCZIv68bM76HHKFkDTE_5UHPtIxoCPDtchnTyCRd_ZAe4aAtpaEALw_wcBTyCRd6d56adfo";
                default:
                    return "";
            }
        }

        public static string GetKodeSekolah(Libs.UnitSekolah unit_sekolah)
        {
            Sekolah m_sekolah = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)unit_sekolah).FirstOrDefault();

            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    return m_sekolah.Kode.ToString();
                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        public static bool IsValidTokenUnit(string kode_unit, string token)
        {
            if (kode_unit.Trim() == "" && token == Constantas.TOKEN_ADMIN) return true;

            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(kode_unit);

            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    return (GetTokenSekolah((Libs.UnitSekolah)m_sekolah.UrutanJenjang) == token ? true : false);
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Nama = "Nama";
            public const string Keterangan = "Keterangan";
            public const string Email = "Email";
            public const string Website = "Website";
            public const string UrutanJenjang = "UrutanJenjang";
            public const string Rel_Divisi = "Rel_Divisi";
        }

        private static Sekolah GetEntityFromDataRow(DataRow row)
        {
            return new Sekolah
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Nama = row[NamaField.Nama].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Email = row[NamaField.Email].ToString(),
                Website = row[NamaField.Website].ToString(),
                UrutanJenjang = (row[NamaField.UrutanJenjang] != DBNull.Value ? Convert.ToInt16(row[NamaField.UrutanJenjang]) : 0),
                Rel_Divisi = new Guid(row[NamaField.Rel_Divisi].ToString())
            };
        }

        public static List<Sekolah> GetAll_Entity()
        {
            List<Sekolah> hasil = new List<Sekolah>();
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

        public static Sekolah GetByID_Entity(string kode)
        {
            Sekolah hasil = new Sekolah();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null || kode.Trim() == "") return hasil;
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

        public static void Insert(Sekolah sekolah, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, sekolah.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, sekolah.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, sekolah.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Website, sekolah.Website));                
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanJenjang, sekolah.UrutanJenjang));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Divisi, sekolah.Rel_Divisi));
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

        public static void Update(Sekolah sekolah, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, sekolah.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, sekolah.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, sekolah.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, sekolah.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Website, sekolah.Website));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanJenjang, sekolah.UrutanJenjang));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Divisi, sekolah.Rel_Divisi));
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