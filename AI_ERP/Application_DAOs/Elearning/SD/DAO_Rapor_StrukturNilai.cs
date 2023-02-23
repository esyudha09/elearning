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
    public static class DAO_Rapor_StrukturNilai
    {
        public const string SP_SELECT_ALL = "SD_Rapor_StrukturNilai_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SD_Rapor_StrukturNilai_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_TA_BY_SM = "SD_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_SM";
        public const string SP_SELECT_ALL_BY_TA_BY_SM_FOR_SEARCH = "SD_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_SM_FOR_SEARCH";

        public const string SP_SELECT_TOP1_BY_TA_BY_SM_BY_KELAS_BY_MAPEL = "SD_Rapor_StrukturNilai_SELECT_TOP1_BY_TA_BY_SM_BY_KELAS_BY_MAPEL";

        public const string SP_SELECT_BY_ID = "SD_Rapor_StrukturNilai_SELECT_BY_ID";
        public const string SP_SELECT_BY_TA_BY_SM = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_2KELAS_BY_MAPEL = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_2KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_3KELAS_BY_MAPEL = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_3KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_4KELAS_BY_MAPEL = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_4KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_5KELAS_BY_MAPEL = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_5KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_6KELAS_BY_MAPEL = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_6KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_GURU_KELAS = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_KELAS_BY_GURU_KELAS";                
        public const string SP_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS";
        public const string SP_SELECT_BY_TA_BY_KELAS_BY_KELASDET_BY_GURU_KELAS = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_KELAS_BY_KELASDET_BY_GURU_KELAS";
        public const string SP_SELECT_BY_TA_BY_KELAS_BY_KELASDET_AS_GURU_MAPEL = "SD_Rapor_StrukturNilai_SELECT_BY_TA_BY_KELAS_BY_KELASDET_AS_GURU_MAPEL";
        public const string SP_SELECT_EKSKUL_BY_TA_BY_KELAS_BY_GURU_KELAS = "SD_Rapor_StrukturNilai_SELECT_EKSKUL_BY_TA_BY_KELAS_BY_GURU_KELAS";
        public const string SP_SELECT_KD_BY_TA_BY_SM_KELAS = "SD_Rapor_StrukturNilai_SELECT_KD_BY_TA_BY_SM_KELAS";

        public const string SP_SELECT_NILAI_SIKAP_BY_TA_BY_KELAS = "SD_Rapor_StrukturNilai_SELECT_NILAI_SIKAP_BY_TA_BY_KELAS";
        public const string SP_SELECT_NILAI_SIKAP_BY_TA_BY_SM_BY_KELAS = "SD_Rapor_StrukturNilai_SELECT_NILAI_SIKAP_BY_TA_BY_SM_BY_KELAS";
        public const string SP_SELECT_NILAI_EKSKUL_BY_TA_BY_KELAS = "SD_Rapor_StrukturNilai_SELECT_NILAI_EKSKUL_BY_TA_BY_KELAS";

        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN = "SD_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_PERIODE = "SD_Rapor_StrukturNilai_SELECT_PERIODE";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER = "SD_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS = "SD_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS";
        public const string SP_SELECT_DISTINCT_KD_BY_ID = "SD_Rapor_StrukturNilai_SELECT_DISTINCT_KD_BY_ID";
        public const string SP_SELECT_DISTINCT_KP_BY_ID = "SD_Rapor_StrukturNilai_SELECT_DISTINCT_KP_BY_ID";

        public const string SP_SELECT_NILAI_EKSKUL = "SD_Rapor_StrukturNilai_SELECT_NILAI_EKSKUL";
        public const string SP_SELECT_NILAI_EKSKUL_FOR_SEARCH = "SD_Rapor_StrukturNilai_SELECT_NILAI_EKSKUL_FOR_SEARCH";
        public const string SP_SELECT_MAX_PERIODE = "SD_Rapor_StrukturNilai_SELECT_MAX_PERIODE";

        public const string SP_INSERT = "SD_Rapor_StrukturNilai_INSERT";

        public const string SP_UPDATE = "SD_Rapor_StrukturNilai_UPDATE";
        public const string SP_UPDATE_BOBOT_SIKAP = "SD_Rapor_StrukturNilai_UPDATE_BOBOT_SIKAP";

        public const string SP_DELETE = "SD_Rapor_StrukturNilai_DELETE";

        public static string[] Arr_KP_Quran = {
            "FA55E41A-5916-4723-ABC3-32CE3A9E2112",
            "E9E293B0-81F4-460D-AA78-67BD0FE905F0",
            "40C95591-D755-4BB4-87C7-BBF4E03FE4A3"
        };

        public class DistinctKD
        {
            public string Kode { get; set; }
            public int Urutan { get; set; }
        }

        public class DistinctKP
        {
            public string Kode { get; set; }
            public int Urutan { get; set; }
        }

        public class TahunAjaranSemester
        {
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
        }

        public static string GetKurikulumByKelas(string tahun_ajaran, string semester, string rel_kelas_det)
        {
            AI_ERP.Application_Entities.Kelas m_kelas = DAO_Kelas.GetByID_Entity(DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(rel_kelas_det).Rel_Kelas.ToString()).Kode.ToString());
            Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester
                ).FirstOrDefault();
            if (m != null)
            {
                if (m.KepalaSekolah != null)
                {
                    switch (m_kelas.Nama.Trim().ToUpper())
                    {
                        case "I":
                        case "1":
                            return m.KurikulumRaporLevel1;
                        case "II":
                        case "2":
                            return m.KurikulumRaporLevel2;
                        case "III":
                        case "3":
                            return m.KurikulumRaporLevel3;
                        case "IV":
                        case "4":
                            return m.KurikulumRaporLevel4;
                        case "V":
                        case "5":
                            return m.KurikulumRaporLevel5;
                        case "VI":
                        case "6":
                            return m.KurikulumRaporLevel6;
                        default:
                            break;
                    }
                }
            }

            return "";
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Kelas2 = "Rel_Kelas2";
            public const string Rel_Kelas3 = "Rel_Kelas3";
            public const string Rel_Kelas4 = "Rel_Kelas4";
            public const string Rel_Kelas5 = "Rel_Kelas5";
            public const string Rel_Kelas6 = "Rel_Kelas6";
            public const string Kurikulum = "Kurikulum";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string KKM = "KKM";
            public const string JenisPerhitungan = "JenisPerhitungan";
            public const string IsKelompokanKP = "IsKelompokanKP";
            public const string IsKelompokanKPNoLTS = "IsKelompokanKPNoLTS";
            public const string BobotSikapGuruKelas = "BobotSikapGuruKelas";
            public const string BobotSikapGuruMapel = "BobotSikapGuruMapel";
        }

        private static Rapor_StrukturNilai GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = new Guid(row[NamaField.Rel_Kelas].ToString()),
                Rel_Kelas2 = row[NamaField.Rel_Kelas2].ToString(),
                Rel_Kelas3 = row[NamaField.Rel_Kelas3].ToString(),
                Rel_Kelas4 = row[NamaField.Rel_Kelas4].ToString(),
                Rel_Kelas5 = row[NamaField.Rel_Kelas5].ToString(),
                Rel_Kelas6 = row[NamaField.Rel_Kelas6].ToString(),
                Kurikulum = row[NamaField.Kurikulum].ToString(),
                Rel_Mapel = new Guid(row[NamaField.Rel_Mapel].ToString()),
                KKM = Convert.ToDecimal(row[NamaField.KKM]),
                JenisPerhitungan = row[NamaField.JenisPerhitungan].ToString(),
                IsKelompokanKP = (row[NamaField.IsKelompokanKP] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsKelompokanKP])),
                IsKelompokanKPNoLTS = (row[NamaField.IsKelompokanKPNoLTS] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsKelompokanKPNoLTS])),
                BobotSikapGuruKelas = (row[NamaField.BobotSikapGuruKelas] == DBNull.Value ? 0 : Convert.ToDecimal(row[NamaField.BobotSikapGuruKelas])),
                BobotSikapGuruMapel = (row[NamaField.BobotSikapGuruMapel] == DBNull.Value ? 0 : Convert.ToDecimal(row[NamaField.BobotSikapGuruMapel]))
            };
        }
        
        public static TahunAjaranSemester GetMaxPeriode_Entity()
        {
            TahunAjaranSemester hasil = new TahunAjaranSemester();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_MAX_PERIODE;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = (
                            new TahunAjaranSemester
                            {
                                TahunAjaran = row["TahunAjaran"].ToString(),
                                Semester = row["Semester"].ToString()
                            }
                        );
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

        public static TahunAjaranSemester GetNextPeriode_Entity()
        {
            TahunAjaranSemester hasil = new TahunAjaranSemester();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_MAX_PERIODE;
                
                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = (
                            new TahunAjaranSemester {
                                TahunAjaran = row["TahunAjaran"].ToString(),
                                Semester = row["Semester"].ToString()
                            }
                        );
                }

                if (hasil != null)
                {
                    if (hasil.TahunAjaran != null)
                    {
                        if (hasil.Semester.ToString() == "2")
                        {
                            hasil.Semester = "1";
                            hasil.TahunAjaran = Application_Libs.Libs.GetTahunAjaranPlus(hasil.TahunAjaran, 1);
                        }
                        else if (hasil.Semester.ToString() == "1")
                        {
                            hasil.Semester = "2";
                        }
                    }
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

        public static List<Rapor_StrukturNilai> GetAllByTABySMByKelasByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_mapel
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
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

        public static List<Rapor_StrukturNilai> GetAllByTABySM_Entity(
                string tahun_ajaran,
                string semester
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);

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

        public static Rapor_StrukturNilai GetATop1ByTABySMByKelasByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_mapel
            )
        {
            Rapor_StrukturNilai hasil = new Rapor_StrukturNilai();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_TOP1_BY_TA_BY_SM_BY_KELAS_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = GetEntityFromDataRow(row);
                    break;
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
        
        public static List<Rapor_StrukturNilai> GetAllByTABySMByKelasByMapel_Entity(
                string tahun_ajaran,
                string semester,
                List<string> rel_kelas,
                string rel_mapel
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            if (rel_kelas.Count == 0) return hasil;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);                
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

                if (rel_kelas.Count == 1)
                {
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas[0]);
                }                
                else if (rel_kelas.Count == 2)
                {
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_2KELAS_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas[0]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas2, rel_kelas[1]);
                }
                else if (rel_kelas.Count == 3)
                {
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_3KELAS_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas[0]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas2, rel_kelas[1]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas3, rel_kelas[2]);
                }
                else if (rel_kelas.Count == 4)
                {
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_4KELAS_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas[0]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas2, rel_kelas[1]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas3, rel_kelas[2]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas4, rel_kelas[3]);
                }
                else if (rel_kelas.Count == 5)
                {
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_5KELAS_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas[0]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas2, rel_kelas[1]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas3, rel_kelas[2]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas4, rel_kelas[3]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas5, rel_kelas[4]);
                }
                else if (rel_kelas.Count == 6)
                {
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_6KELAS_BY_MAPEL;
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas[0]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas2, rel_kelas[1]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas3, rel_kelas[2]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas4, rel_kelas[3]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas5, rel_kelas[4]);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas6, rel_kelas[5]);
                }

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

        public static List<Rapor_StrukturNilai> GetAllByTABySMByKelasByGuru_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_guru
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_GURU_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
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

        public static List<Rapor_StrukturNilai> GetMapelSikapByTAByKelas_Entity(
                string tahun_ajaran,
                string rel_kelas
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_NILAI_SIKAP_BY_TA_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

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

        public static List<Rapor_StrukturNilai> GetMapelSikapByTABySMByKelas_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_NILAI_SIKAP_BY_TA_BY_SM_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

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

        public static List<Rapor_StrukturNilai> GetMapelEkskulByTABySMByKelas_Entity(
                string tahun_ajaran,
                string rel_kelas
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_NILAI_EKSKUL_BY_TA_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

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

        public static List<DistinctKD> GetDistinctKD_Entity(
                string kode
            )
        {
            List<DistinctKD> hasil = new List<DistinctKD>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_KD_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new DistinctKD {
                                Kode = row["Kode"].ToString(),
                                Urutan = Convert.ToInt16(row["Urutan"])
                            }
                        );
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

        public static List<string> GetDistinctTahunAjaran_Entity()
        {
            List<string> hasil = new List<string>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN;
                
                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            row[0].ToString()
                        );
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

        public static List<TahunAjaranSemester> GetDistinctTahunAjaranPeriode_Entity()
        {
            List<TahunAjaranSemester> hasil = new List<TahunAjaranSemester>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN_PERIODE;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new TahunAjaranSemester {
                                TahunAjaran = row[0].ToString(),
                                Semester = row[1].ToString()
                            }                            
                        );
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

        public static List<TahunAjaranSemester> GetDistinctTahunAjaranSemester_Entity(
                string tahun_ajaran,
                string rel_kelas
            )
        {
            List<TahunAjaranSemester> hasil = new List<TahunAjaranSemester>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new TahunAjaranSemester {
                                TahunAjaran = row["TahunAjaran"].ToString(),
                                Semester = row["Semester"].ToString()
                            }
                        );
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

        public static List<DistinctKP> GetDistinctKP_Entity(
                string kode
            )
        {
            List<DistinctKP> hasil = new List<DistinctKP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_KP_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new DistinctKP
                            {
                                Kode = row["Kode"].ToString(),
                                Urutan = Convert.ToInt16(row["Urutan"])
                            }
                        );
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

        public static List<Rapor_StrukturNilai> GetAllByTAByKelasByGuru_Entity(
                string tahun_ajaran,
                string rel_kelas,
                string rel_guru
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
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

        public static List<Rapor_StrukturNilai> GetAllByTAByKelasByKelasDetByGuru_Entity(
                string tahun_ajaran,
                string rel_kelas,
                string rel_kelas_det,
                string rel_guru
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_KELAS_BY_KELASDET_BY_GURU_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);
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

        public static List<Rapor_StrukturNilai> GetAllByTAByKelasByKelasDetASGuruMapel_Entity(
                string tahun_ajaran,
                string rel_kelas,
                string rel_kelas_det,
                string rel_guru
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_KELAS_BY_KELASDET_AS_GURU_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);
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

        public static List<Rapor_StrukturNilai> GetAllEkskulByTAByKelasByGuru_Entity(
                string tahun_ajaran,
                string rel_kelas,
                string rel_guru
            )
        {
            List<Rapor_StrukturNilai> hasil = new List<Rapor_StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_EKSKUL_BY_TA_BY_KELAS_BY_GURU_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
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

        public static Rapor_StrukturNilai GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai hasil = new Rapor_StrukturNilai();
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

        public static void UpdateBobotSikap(string Kode, decimal bobot_guru_kelas, decimal bobot_guru_mapel)
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
                comm.CommandText = SP_UPDATE_BOBOT_SIKAP;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotSikapGuruKelas, bobot_guru_kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotSikapGuruMapel, bobot_guru_mapel));
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

        public static void Insert(Rapor_StrukturNilai m, string user_id)
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

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas2, m.Rel_Kelas2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas3, m.Rel_Kelas3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas4, m.Rel_Kelas4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas5, m.Rel_Kelas5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas6, m.Rel_Kelas6));
                //comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KKM, m.KKM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelompokanKP, m.IsKelompokanKP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelompokanKPNoLTS, m.IsKelompokanKPNoLTS));
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

        public static void Update(Rapor_StrukturNilai m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas2, m.Rel_Kelas2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas3, m.Rel_Kelas3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas4, m.Rel_Kelas4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas5, m.Rel_Kelas5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas6, m.Rel_Kelas6));
                //comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KKM, m.KKM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelompokanKP, m.IsKelompokanKP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKelompokanKPNoLTS, m.IsKelompokanKPNoLTS));
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

        public static bool IsNilaiCanEdit(string kode, string rel_guru, string rel_kelas_det)
        {
            bool hasil = false;

            Rapor_StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(kode);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    //by formasi guru mata pelajaran
                    var lst_guru_mapel = DAO_FormasiGuruMapelDet.GetByGuruByTABySMByKelasDetByMapelAsSelf_Entity(
                            rel_guru, m.TahunAjaran, m.Semester, rel_kelas_det, m.Rel_Mapel.ToString()
                        );

                    if (lst_guru_mapel.FindAll(m0 => m0.Rel_KelasDet == new Guid(rel_kelas_det)).Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        lst_guru_mapel = DAO_FormasiGuruMapelDet.GetByGuruByTABySMByKelasDetByMapelAsOther_Entity(
                            rel_guru, m.TahunAjaran, m.Semester, rel_kelas_det, m.Rel_Mapel.ToString()
                        );
                    }


                    //by formasi guru kelas
                    var lst_guru_kelas = DAO_FormasiGuruKelas.GetByGuruByTABySM_Entity(
                            rel_guru, m.TahunAjaran, m.Semester
                        );

                    lst_guru_mapel = DAO_FormasiGuruMapelDet.GetByGuruByTABySMByKelasDetByMapel_Entity(
                            rel_guru, m.TahunAjaran, m.Semester, rel_kelas_det, m.Rel_Mapel.ToString()
                        );

                    if (
                        lst_guru_kelas.FindAll(m0 => new Guid(m0.Rel_KelasDet) == new Guid(rel_kelas_det)).Count > 0 &&
                        lst_guru_mapel.Count == 0 && 
                        DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString()).Jenis != Application_Libs.Libs.JENIS_MAPEL.EKSKUL &&
                        DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString()).Jenis != Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER
                    ) return true;
                }
            }

            return hasil;
        }
    }
}