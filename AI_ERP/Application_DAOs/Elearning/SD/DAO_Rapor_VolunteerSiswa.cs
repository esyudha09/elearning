using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SD;

namespace AI_ERP.Application_DAOs.Elearning.SD
{
    public class DAO_Rapor_VolunteerSiswa
    {
        public const string SP_SELECT_BY_HEADER = "SD_Rapor_VolunteerSiswa_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_HEADER_BY_SISWA = "SD_Rapor_VolunteerSiswa_SELECT_BY_HEADER_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_VSETTINGS_BY_SISWA =
                            "SD_Rapor_VolunteerSiswa_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_VSETTINGS_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET =
                            "SD_Rapor_Volunteersiswa_SELECT_BY_TA_BY_SM_BY_KELASDET";

        public const string SP_INSERT = "SD_Rapor_VolunteerSiswa_INSERT";

        public const string SP_UPDATE = "SD_Rapor_VolunteerSiswa_UPDATE";

        public const string SP_DELETE = "SD_Rapor_VolunteerSiswa_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Volunteer = "Rel_Rapor_Volunteer";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Rapor_Volunteer_Settings_Det = "Rel_Rapor_Volunteer_Settings_Det";
            public const string Nilai = "Nilai";
        }

        private static Rapor_VolunteerSiswa GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_VolunteerSiswa
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Volunteer = new Guid(row[NamaField.Rel_Rapor_Volunteer].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Rapor_Volunteer_Settings_Det = row[NamaField.Rel_Rapor_Volunteer_Settings_Det].ToString(),
                Nilai = row[NamaField.Nilai].ToString()
            };
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

        public static void Insert(Rapor_VolunteerSiswa m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Volunteer, m.Rel_Rapor_Volunteer));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Volunteer_Settings_Det, m.Rel_Rapor_Volunteer_Settings_Det));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));

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

        public static void Update(Rapor_VolunteerSiswa m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Volunteer, m.Rel_Rapor_Volunteer));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Volunteer_Settings_Det, m.Rel_Rapor_Volunteer_Settings_Det));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));
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

        public static List<Rapor_VolunteerSiswa> GetAllByHeader_Entity(
                string rel_Rapor_Volunteer
            )
        {
            List<Rapor_VolunteerSiswa> hasil = new List<Rapor_VolunteerSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Volunteer, rel_Rapor_Volunteer);

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

        public static List<Rapor_VolunteerSiswa> GetAllByHeaderBySiswa_Entity(
                string rel_Rapor_Volunteer,
                string rel_siswa
            )
        {
            List<Rapor_VolunteerSiswa> hasil = new List<Rapor_VolunteerSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER_BY_SISWA;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Volunteer, rel_Rapor_Volunteer);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);

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

        public static List<Rapor_VolunteerSiswa> GetAllByTABySMByKelasDetByVSetBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_rapor_volunteer_settings_det,
                string rel_siswa
            )
        {
            List<Rapor_VolunteerSiswa> hasil = new List<Rapor_VolunteerSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_VSETTINGS_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Rapor_Volunteer_Settings_Det", rel_rapor_volunteer_settings_det);
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
    }
}