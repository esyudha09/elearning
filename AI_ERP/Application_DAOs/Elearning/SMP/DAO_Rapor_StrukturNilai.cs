using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_Rapor_StrukturNilai
    {
        public const string SP_SELECT_ALL = "SMP_Rapor_StrukturNilai_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMP_Rapor_StrukturNilai_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_GURU = "SMP_Rapor_StrukturNilai_SELECT_ALL_BY_GURU";
        public const string SP_SELECT_ALL_BY_GURU_FOR_SEARCH = "SMP_Rapor_StrukturNilai_SELECT_ALL_BY_GURU_FOR_SEARCH";

        public const string SP_SELECT_ALL_BY_TA_BY_SM = "SMP_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_SM";
        public const string SP_SELECT_ALL_BY_TA_BY_SM_FOR_SEARCH = "SMP_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_SM_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_GURU_BY_TA_BY_SM = "SMP_Rapor_StrukturNilai_SELECT_ALL_BY_GURU_BY_TA_BY_SM";
        public const string SP_SELECT_ALL_BY_GURU_BY_TA_BY_SM_FOR_SEARCH = "SMP_Rapor_StrukturNilai_SELECT_ALL_BY_GURU_BY_TA_BY_SM_FOR_SEARCH";

        public const string SP_SELECT_SIKAP_BY_TA_BY_SM_BY_KELAS = "SMP_Rapor_StrukturNilai_SIKAP_BY_TA_BY_SM_BY_KELAS";

        public const string SP_SELECT_TOP1_BY_TA_BY_SM_BY_KELAS_BY_MAPEL = "SMP_Rapor_StrukturNilai_SELECT_TOP1_BY_TA_BY_SM_BY_KELAS_BY_MAPEL";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN = "SMP_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_PERIODE = "SMP_Rapor_StrukturNilai_SELECT_PERIODE";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER = "SMP_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS = "SMP_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS";
        public const string SP_SELECT_BY_ID = "SMP_Rapor_StrukturNilai_SELECT_BY_ID";        
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS = "SMP_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_KELAS";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL = "SMP_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS = "SMP_Rapor_StrukturNilai_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS";
        public const string SP_SELECT_BY_TA_BY_KELAS_BY_MAPEL = "SMP_Rapor_StrukturNilai_SELECT_BY_TA_BY_KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM = "SMP_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM";
        public const string SP_SELECT_MENGAJAR = "SMP_Rapor_StrukturNilai_SELECT_MENGAJAR";        

        public const string SP_INSERT = "SMP_Rapor_StrukturNilai_INSERT";
        public const string SP_CREATE_BUKA_SEMESTER_EKSKUL = "SMP_Rapor_StrukturNilai_CREATE_BUKA_SEMESTER_EKSKUL";
        public const string SP_SELECT_MAX_PERIODE = "SMP_Rapor_StrukturNilai_SELECT_MAX_PERIODE";

        public const string SP_UPDATE = "SMP_Rapor_StrukturNilai_UPDATE";
        public const string SP_UPDATE_DESKRIPSI_KTSP = "SMP_Rapor_StrukturNilai_UPDATE_DESKRIPSI_KTSP";
        public const string SP_UPDATE_DESKRIPSI_KURTILAS = "SMP_Rapor_StrukturNilai_UPDATE_DESKRIPSI_KURTILAS";

        public const string SP_SELECT_PREDIKAT_BY_HEADER = "SMP_Rapor_StrukturNilai_SELECT_PREDIKAT_BY_HEADER";

        public const string SP_DELETE = "SMP_Rapor_StrukturNilai_DELETE";

        public class TahunAjaranSemester
        {
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
        }
        public class StrukturNilaiPredikat
        {
            public Guid Kode { get; set; }
            public Guid Rel_Rapor_StrukturNilai { get; set; }
            public decimal Minimal { get; set; }
            public decimal Maksimal { get; set; }
            public string Predikat { get; set; }
            public int Urutan { get; set; }
            public string Deskripsi { get; set; }
        }
        public class StrukturNilai
        {
            public Guid Kode { get; set; }
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
            public Guid Rel_Kelas { get; set; }
            public Guid Rel_Mapel { get; set; }
            public string Kurikulum { get; set; }
            public decimal KKM { get; set; }
            public bool IsNilaiAkhir { get; set; }
            public string DeskripsiSikapSpiritual { get; set; }
            public string DeskripsiSikapSosial { get; set; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Kelas2 = "Rel_Kelas2";
            public const string Rel_Kelas3 = "Rel_Kelas3";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Kurikulum = "Kurikulum";
            public const string KKM = "KKM";
            public const string JenisPerhitungan = "JenisPerhitungan";
            public const string DeskripsiUmum = "DeskripsiUmum";
            public const string DeskripsiPengetahuan = "DeskripsiPengetahuan";
            public const string DeskripsiKeterampilan = "DeskripsiKeterampilan";
            public const string Is_PH_PTS_PAS = "Is_PH_PTS_PAS";
            public const string BobotPH = "BobotPH";
            public const string BobotPTS = "BobotPTS";
            public const string BobotPAS = "BobotPAS";

            public const string IsNilaiAkhir = "IsNilaiAkhir";
            public const string DeskripsiSikapSpiritual = "DeskripsiSikapSpiritual";
            public const string DeskripsiSikapSosial = "DeskripsiSikapSosial";
        }

        public static class NamaFieldPredikat
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai = "Rel_Rapor_StrukturNilai";
            public const string Minimal = "Minimal";
            public const string Maksimal = "Maksimal";
            public const string Predikat = "Predikat";
            public const string Urutan = "Urutan";
            public const string Deskripsi = "Deskripsi";
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
                Rel_Mapel = new Guid(row[NamaField.Rel_Mapel].ToString()),
                KKM = Convert.ToDecimal(row[NamaField.KKM]),
                JenisPerhitungan = row[NamaField.JenisPerhitungan].ToString(),
                DeskripsiUmum = row[NamaField.DeskripsiUmum].ToString(),
                DeskripsiPengetahuan = row[NamaField.DeskripsiPengetahuan].ToString(),
                DeskripsiKeterampilan = row[NamaField.DeskripsiKeterampilan].ToString(),
                Is_PH_PTS_PAS = (row[NamaField.Is_PH_PTS_PAS] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.Is_PH_PTS_PAS])),
                BobotPH = (row[NamaField.BobotPH] == DBNull.Value ? 0 : Convert.ToDecimal(row[NamaField.BobotPH])),
                BobotPTS = (row[NamaField.BobotPTS] == DBNull.Value ? 0 : Convert.ToDecimal(row[NamaField.BobotPTS])),
                BobotPAS = (row[NamaField.BobotPAS] == DBNull.Value ? 0 : Convert.ToDecimal(row[NamaField.BobotPAS])),
                IsNilaiAkhir = (row[NamaField.IsNilaiAkhir] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsNilaiAkhir])),
                DeskripsiSikapSpiritual = row[NamaField.DeskripsiSikapSpiritual].ToString(),
                DeskripsiSikapSosial = row[NamaField.DeskripsiSikapSosial].ToString()
            };
        }

        private static DAO_Rapor_StrukturNilai.StrukturNilai GetEntityFromDataRowStrukturNilai(DataRow row)
        {
            return new StrukturNilai
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = new Guid(row[NamaField.Rel_Kelas].ToString()),
                Rel_Mapel = new Guid(row[NamaField.Rel_Mapel].ToString()),
                Kurikulum = row[NamaField.Kurikulum].ToString(),
                KKM = Convert.ToDecimal(row[NamaField.KKM]),
                IsNilaiAkhir = (row[NamaField.IsNilaiAkhir] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsNilaiAkhir])),
                DeskripsiSikapSpiritual = row[NamaField.DeskripsiSikapSpiritual].ToString(),
                DeskripsiSikapSosial = row[NamaField.DeskripsiSikapSosial].ToString()
            };
        }

        private static StrukturNilaiPredikat GetEntityFromDataRowPredikat(DataRow row)
        {
            return new StrukturNilaiPredikat
            {
                Kode = new Guid(row[NamaFieldPredikat.Kode].ToString()),
                Rel_Rapor_StrukturNilai = new Guid(row[NamaFieldPredikat.Rel_Rapor_StrukturNilai].ToString()),
                Minimal = Convert.ToInt16(row[NamaFieldPredikat.Minimal]),
                Maksimal = Convert.ToInt16(row[NamaFieldPredikat.Maksimal]),
                Predikat = row[NamaFieldPredikat.Predikat].ToString(),
                Urutan = Convert.ToInt16(row[NamaFieldPredikat.Urutan]),
                Deskripsi = row[NamaFieldPredikat.Deskripsi].ToString()
            };
        }

        public static List<string> GetMengajar_Entity(string tahun_ajaran, string semester, string rel_guru, string rel_mapel, string rel_kelas)
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
                comm.CommandText = SP_SELECT_MENGAJAR;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);                
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add("OK");
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

        public static List<DAO_Rapor_StrukturNilai.StrukturNilai> GetAllByTABySMByKelasByMapelForSikap_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_mapel
            )
        {
            List<DAO_Rapor_StrukturNilai.StrukturNilai> hasil = new List<DAO_Rapor_StrukturNilai.StrukturNilai>();
            if (rel_mapel == "") return hasil;

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
                    hasil.Add(new StrukturNilai
                    {
                        Kode = new Guid(row["Kode"].ToString()),
                        TahunAjaran = row["TahunAjaran"].ToString(),
                        Semester = row["Semester"].ToString(),
                        Rel_Kelas = new Guid(row["Rel_Kelas"].ToString()),
                        Rel_Mapel = new Guid(row["Rel_Mapel"].ToString()),
                        Kurikulum = Libs.JenisKurikulum.SMP.KURTILAS_SIKAP,
                        KKM = Convert.ToDecimal(row["KKM"])
                    });
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

        public static List<Rapor_StrukturNilai> GetAllByTABySMByKelas_Entity(
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
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS;
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

        public static string GetNamaKelasEkskul(string kode)
        {
            Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(
                    kode
                );
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {
                    string kelas_1 = "";
                    string kelas_2 = "";
                    string kelas_3 = "";

                    if (m_sn.Rel_Kelas.ToString() != Constantas.GUID_NOL && m_sn.Rel_Kelas.ToString() != "")
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_sn.Rel_Kelas.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                kelas_1 = m_kelas.Nama;
                            }
                        }
                    }

                    if (m_sn.Rel_Kelas2.ToString() != Constantas.GUID_NOL && m_sn.Rel_Kelas.ToString() != "")
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_sn.Rel_Kelas2.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                kelas_2 = m_kelas.Nama;
                            }
                        }
                    }

                    if (m_sn.Rel_Kelas3.ToString() != Constantas.GUID_NOL && m_sn.Rel_Kelas.ToString() != "")
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_sn.Rel_Kelas3.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                kelas_3 = m_kelas.Nama;
                            }
                        }
                    }

                    return (
                            kelas_1 +
                            (
                                kelas_1.Trim() != "" &&
                                kelas_2.Trim() != ""
                                ? ", "
                                : ""
                            ) +
                            kelas_2 +
                            (
                                (
                                    kelas_2.Trim() != "" || kelas_1.Trim() != ""
                                ) && kelas_3.ToString().Trim() != ""
                                ? ", "
                                : ""
                            ) +
                            kelas_3
                        );
                }
            }

            return "";
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

        public static List<Rapor_StrukturNilai> GetAllByTAByKelasByMapel_Entity(
                string tahun_ajaran,
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
                comm.CommandText = SP_SELECT_BY_TA_BY_KELAS_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
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
                            new TahunAjaranSemester
                            {
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KKM, m.KKM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_PH_PTS_PAS, m.Is_PH_PTS_PAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotPH, m.BobotPH));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotPTS, m.BobotPTS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotPAS, m.BobotPAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNilaiAkhir, m.IsNilaiAkhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSosial, m.DeskripsiSikapSosial));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSpiritual, m.DeskripsiSikapSpiritual));
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

        public static void CreateBukaSemesterEkskul(string tahun_ajaran, string semester, string tahun_ajaran_sc, string semester_sc)
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
                comm.CommandText = SP_CREATE_BUKA_SEMESTER_EKSKUL;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@TahunAjaranSC", tahun_ajaran_sc));
                comm.Parameters.Add(new SqlParameter("@SemesterSC", semester_sc));
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

        public static List<StrukturNilaiPredikat> GetPredikatByHeader_Entity(
                string rel_rapor_strukturnilai
            )
        {
            List<StrukturNilaiPredikat> hasil = new List<StrukturNilaiPredikat>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_PREDIKAT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaFieldPredikat.Rel_Rapor_StrukturNilai, rel_rapor_strukturnilai);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowPredikat(row));
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

        public static List<DAO_Rapor_StrukturNilai.StrukturNilai> GetAllMapelSikapByTAByKelas_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas
            )
        {
            List<DAO_Rapor_StrukturNilai.StrukturNilai> hasil = new List<DAO_Rapor_StrukturNilai.StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SIKAP_BY_TA_BY_SM_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowStrukturNilai(row));
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
                            new TahunAjaranSemester
                            {
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KKM, m.KKM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_PH_PTS_PAS, m.Is_PH_PTS_PAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotPH, m.BobotPH));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotPTS, m.BobotPTS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotPAS, m.BobotPAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNilaiAkhir, m.IsNilaiAkhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSosial, m.DeskripsiSikapSosial));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSpiritual, m.DeskripsiSikapSpiritual));
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

        public static void UpdateDeskripsiKTSP(string kode, string deskripsi_umum)
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
                comm.CommandText = SP_UPDATE_DESKRIPSI_KTSP;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiUmum, deskripsi_umum));
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

        public static void UpdateDeskripsiKURTILAS(string kode, string deskripsi_pengetahuan, string deskripsi_keterampilan)
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
                comm.CommandText = SP_UPDATE_DESKRIPSI_KURTILAS;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiPengetahuan, deskripsi_pengetahuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiKeterampilan, deskripsi_keterampilan));
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

                    if (
                        (
                            lst_guru_mapel.Count > 0
                        )
                    ) return true;
                }
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

        public static string GetKurikulumByLevel(string rel_kelas, string tahun_ajaran, string semester)
        {
            AI_ERP.Application_Entities.Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            m_kelas.Nama = m_kelas.Nama + "-";

            string jenis_kurikulum = Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS;
            Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester).FirstOrDefault();
            if (m != null)
            {
                if (m_kelas.Nama.Length >= 4)
                {
                    if (m_kelas.Nama.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KURTILAS)
                    {
                        jenis_kurikulum = Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS;
                    }
                    else if (m_kelas.Nama.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KTSP)
                    {
                        jenis_kurikulum = Application_Libs.Libs.JenisKurikulum.SMP.KTSP;
                    }
                }
                if (m_kelas.Nama.Length >= 5)
                {
                    if (m_kelas.Nama.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KURTILAS)
                    {
                        jenis_kurikulum = Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS;
                    }
                    else if (m_kelas.Nama.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KTSP)
                    {
                        jenis_kurikulum = Application_Libs.Libs.JenisKurikulum.SMP.KTSP;
                    }
                }
                if (m_kelas.Nama.Length >= 3)
                {
                    if (m_kelas.Nama.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KURTILAS)
                    {
                        jenis_kurikulum = Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS;
                    }
                    else if (m_kelas.Nama.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KTSP)
                    {
                        jenis_kurikulum = Application_Libs.Libs.JenisKurikulum.SMP.KTSP;
                    }
                }
            }

            return jenis_kurikulum;
        }

        public static string GetKurikulumByKelas(string rel_kelas_det, string tahun_ajaran, string semester)
        {
            AI_ERP.Application_Entities.Kelas m_kelas = DAO_Kelas.GetByID_Entity(DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(rel_kelas_det).Rel_Kelas.ToString()).Kode.ToString());            
            return GetKurikulumByLevel(m_kelas.Kode.ToString(), tahun_ajaran, semester);
        }
    }
}