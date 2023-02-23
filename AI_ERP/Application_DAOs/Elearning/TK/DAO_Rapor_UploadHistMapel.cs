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
    public static class DAO_Rapor_UploadHistMapel
    {
        public static class Kelas
        {
            public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL = "TK_Rapor_UploadHistKelasMapel_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL";
            public const string SP_SELECT_BY_ID = "TK_Rapor_UploadHistKelasMapel_SELECT_BY_ID";

            public const string SP_INSERT = "TK_Rapor_UploadHistKelasMapel_INSERT";

            public const string SP_DELETE = "TK_Rapor_UploadHistKelasMapel_DELETE";

            public static class NamaField
            {
                public const string Kode = "Kode";
                public const string Tanggal = "Tanggal";
                public const string PosisiUpload = "PosisiUpload";
                public const string TahunAjaran = "TahunAjaran";
                public const string Semester = "Semester";
                public const string Rel_KelasDet = "Rel_KelasDet";
                public const string Rel_Mapel = "Rel_Mapel";
            }

            private static Rapor_UploadHistKelasMapel GetEntityFromDataRow(DataRow row)
            {
                return new Rapor_UploadHistKelasMapel
                {
                    Kode = new Guid(row[NamaField.Kode].ToString()),
                    Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                    PosisiUpload = row[NamaField.PosisiUpload].ToString(),
                    TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                    Semester = row[NamaField.Semester].ToString(),
                    Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                    Rel_Mapel = row[NamaField.Rel_Mapel].ToString()
                };
            }

            public static Rapor_UploadHistKelasMapel GetByID_Entity(string kode)
            {
                Rapor_UploadHistKelasMapel hasil = new Rapor_UploadHistKelasMapel();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

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

            public static List<Rapor_UploadHistKelasMapel> GetByTABySMByKelasDet_Entity(string tahun_ajaran, string semester, string rel_kelasdet, string rel_mapel)
            {
                List<Rapor_UploadHistKelasMapel> hasil = new List<Rapor_UploadHistKelasMapel>();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

                try
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                    comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

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

            public static void Insert(Rapor_UploadHistKelasMapel m)
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
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PosisiUpload, m.PosisiUpload));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
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

        public static class Siswa
        {
            public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA_BY_MAPEL = "TK_Rapor_UploadHistSiswaMapel_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA_BY_MAPEL";
            public const string SP_SELECT_BY_ID = "TK_Rapor_UploadHistSiswaMapel_SELECT_BY_ID";
            public const string SP_SELECT_UPLOAD_LOG_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL = "TK_Rapor_UploadHistKelasMapel_SELECT_UPLOAD_LOG_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL";

            public const string SP_INSERT = "TK_Rapor_UploadHistSiswaMapel_INSERT";

            public const string SP_DELETE = "TK_Rapor_UploadHistSiswaMapel_DELETE";

            public static class NamaField
            {
                public const string Kode = "Kode";
                public const string Tanggal = "Tanggal";
                public const string PosisiUpload = "PosisiUpload";
                public const string TahunAjaran = "TahunAjaran";
                public const string Semester = "Semester";
                public const string Rel_KelasDet = "Rel_KelasDet";
                public const string Rel_Mapel = "Rel_Mapel";
                public const string Rel_Siswa = "Rel_Siswa";
            }

            private static Rapor_UploadHistSiswaMapel GetEntityFromDataRow(DataRow row)
            {
                return new Rapor_UploadHistSiswaMapel
                {
                    Kode = new Guid(row[NamaField.Kode].ToString()),
                    Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                    PosisiUpload = row[NamaField.PosisiUpload].ToString(),
                    TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                    Semester = row[NamaField.Semester].ToString(),
                    Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                    Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                    Rel_Siswa = row[NamaField.Rel_Siswa].ToString()
                };
            }

            public static Rapor_UploadHistSiswaMapel GetByID_Entity(string kode)
            {
                Rapor_UploadHistSiswaMapel hasil = new Rapor_UploadHistSiswaMapel();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

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

            public static List<Rapor_UploadHistSiswaMapel> GetByTABySMByKelasDetBySiswaByMapel_Entity(string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa, string rel_mapel)
            {
                List<Rapor_UploadHistSiswaMapel> hasil = new List<Rapor_UploadHistSiswaMapel>();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

                try
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                    comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

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

            public static string GetStatusUpload(string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa, string rel_mapel)
            {
                string hasil = "";

                var lst_log_kelas = GetLogByTABySMByKelasDetByMapel_Entity(tahun_ajaran, semester, rel_kelasdet, rel_mapel).FindAll(m0 => m0.Rel_Siswa.Trim() == "");
                var lst_log_siswa = GetLogByTABySMByKelasDetByMapel_Entity(tahun_ajaran, semester, rel_kelasdet, rel_mapel).FindAll(m0 => m0.Rel_Siswa.Trim().ToUpper() == rel_siswa.Trim().ToUpper());

                var m_log_kelas = lst_log_kelas.FirstOrDefault();
                var m_log_siswa = lst_log_siswa.FirstOrDefault();

                Rapor_UploadHistSiswaMapel m_log = null;

                if (m_log_kelas != null && m_log_siswa != null)
                {
                    if (m_log_kelas.Rel_KelasDet != null && m_log_siswa.Rel_Siswa != null)
                    {
                        if (m_log_kelas.Tanggal > m_log_siswa.Tanggal)
                        {
                            m_log = m_log_kelas;
                        }
                        else if (m_log_kelas.Tanggal < m_log_siswa.Tanggal)
                        {
                            m_log = m_log_siswa;
                        }
                    }
                }
                else if (m_log_kelas != null && m_log_siswa == null)
                {
                    if (m_log_kelas.Rel_KelasDet != null)
                    {
                        m_log = m_log_kelas;
                    }
                }
                else if (m_log_siswa != null && m_log_kelas == null)
                {
                    if (m_log_siswa.Rel_KelasDet != null)
                    {
                        m_log = m_log_siswa;
                    }
                }

                if (m_log != null)
                {
                    if (m_log.TahunAjaran != null)
                    {
                        return m_log.PosisiUpload;
                    }
                }

                return hasil;
            }

            public static List<Rapor_UploadHistSiswaMapel> GetLogByTABySMByKelasDetByMapel_Entity(string tahun_ajaran, string semester, string rel_kelasdet, string rel_mapel)
            {
                List<Rapor_UploadHistSiswaMapel> hasil = new List<Rapor_UploadHistSiswaMapel>();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

                try
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SELECT_UPLOAD_LOG_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                    comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

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

            public static void Insert(Rapor_UploadHistSiswaMapel m)
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
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PosisiUpload, m.PosisiUpload));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
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
}