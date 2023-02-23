using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.TK;

namespace AI_ERP.Application_DAOs.Elearning.TK
{
    public static class DAO_Rapor_Nilai
    {
        public const string SP_SELECT_ALL = "TK_Rapor_Nilai_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "TK_Rapor_Nilai_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "TK_Rapor_Nilai_SELECT_BY_TA_BY_SM_BY_KELASDET";
        public const string SP_SELECT_BY_ID = "TK_Rapor_Nilai_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT = "TK_Rapor_DesignDet_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT";
        public const string SP_SELECT_BY_HEADER_NO_EKSKUL = "TK_Rapor_DesignDet_SELECT_BY_HEADER_NO_EKSKUL";
        public const string SP_SELECT_PENILAIAN = "TK_Rapor_NilaiSiswa_SELECT_PENILAIAN";

        public const string SP_INSERT = "TK_Rapor_Nilai_INSERT";

        public const string SP_UPDATE = "TK_Rapor_Nilai_UPDATE";

        public const string SP_DELETE = "TK_Rapor_Nilai_DELETE";

        public const string FONT_SIZE = "@fontsize";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string IsPosted = "IsPosted";
            public const string IsLocked = "IsLocked";
        }

        public class ItemDesign
        {
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
            public string Kode { get; set; }
            public string UrutKategori { get; set; }
            public string PoinKategori { get; set; }
            public string NamaKategori { get; set; }
            public string UrutSubKategori { get; set; }
            public string PoinSubKategori { get; set; }
            public string NamaSubKategori { get; set; }
            public string PoinItemPenilaian { get; set; }
            public string NamaItemPenilaian { get; set; }
            public bool IsNewPage { get; set; }
            public string JenisKomponen { get; set; }
            public int Urut { get; set; }            
            public string Kriteria1 { get; set; }
            public string NamaKriteria1 { get; set; }
            public string Kriteria2 { get; set; }
            public string NamaKriteria2 { get; set; }
            public string Kriteria3 { get; set; }
            public string NamaKriteria3 { get; set; }
        }

        public class ItemNilaiRapor: ItemDesign
        {
            public string IDSiswa { get; set; }
            public string NIS { get; set; }
            public string Nama { get; set; }
            public string Kelas { get; set; }
            public string Rel_KelasDet { get; set; }
            public string Sakit { get; set; }
            public string Izin { get; set; }
            public string Alpa { get; set; }
            public string TinggiBadan { get; set; }
            public string BeratBadan { get; set; }
            public string TanggalRapor { get; set; }
            public string GuruKelas { get; set; }
            public string KepalaSekolah { get; set; }
            public string JenisNilai { get; set; }
            public string Nilai { get; set; }
            public bool IsNaik { get; set; }
            public string KeteranganKenaikan { get; set; }
        }

        public static List<ItemNilaiRapor> GetItemsRaporByDesignBySiswa(string rel_rapor_design, string rel_siswa)
        {
            List<ItemNilaiRapor> hasil = new List<ItemNilaiRapor>();

            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            Rapor_Design m_desain = DAO_Rapor_Design.GetByID_Entity(rel_rapor_design);
            if (m_desain != null)
            {
                if (m_desain.TahunAjaran != null)
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = DAO_Rapor_Nilai.SP_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT;
                    comm.Parameters.AddWithValue("@" + DAO_Rapor_DesignDet.NamaField.Rel_Rapor_Design, rel_rapor_design);
                    comm.Parameters.AddWithValue("@" + DAO_Rapor_NilaiSiswa.NamaField.Rel_Siswa, rel_siswa);

                    DataTable dtResult = new DataTable();
                    sqlDA = new SqlDataAdapter(comm);
                    sqlDA.Fill(dtResult);

                    foreach (DataRow row in dtResult.Rows)
                    {
                        hasil.Add(new ItemNilaiRapor {
                            TahunAjaran = row["TahunAjaran"].ToString(),
                            Semester = row["Semester"].ToString(),
                            UrutKategori = row["UrutKategori"].ToString(),
                            PoinKategori = row["PoinKategori"].ToString(),
                            NamaKategori = row["NamaKategori"].ToString(),
                            UrutSubKategori = row["UrutSubKategori"].ToString(),
                            PoinSubKategori = row["PoinSubKategori"].ToString(),
                            NamaSubKategori = row["NamaSubKategori"].ToString(),
                            PoinItemPenilaian = row["PoinItemPenilaian"].ToString(),
                            NamaItemPenilaian = row["NamaItemPenilaian"].ToString(),
                            IsNewPage = Convert.ToBoolean(row["IsNewPage"] == DBNull.Value ? false : row["IsNewPage"]),
                            Urut = Convert.ToInt32(row["Urut"] == DBNull.Value ? false : row["Urut"]),
                            Kriteria1 = row["Kriteria1"].ToString(),
                            NamaKriteria1 = row["NamaKriteria1"].ToString(),
                            Kriteria2 = row["Kriteria2"].ToString(),
                            NamaKriteria2 = row["NamaKriteria2"].ToString(),
                            Kriteria3 = row["Kriteria3"].ToString(),
                            NamaKriteria3 = row["NamaKriteria3"].ToString(),
                            IDSiswa = row["IDSiswa"].ToString(),
                            NIS = row["NIS"].ToString(),
                            Nama = row["Nama"].ToString(),
                            Kelas = row["Kelas"].ToString(),
                            Rel_KelasDet = row["Rel_KelasDet"].ToString(),
                            Sakit = row["Sakit"].ToString(),
                            Izin = row["Izin"].ToString(),
                            Alpa = row["Alpa"].ToString(),
                            TinggiBadan = row["TinggiBadan"].ToString(),
                            BeratBadan = row["BeratBadan"].ToString(),
                            TanggalRapor = row["TanggalRapor"].ToString(),
                            GuruKelas = row["GuruKelas"].ToString(),
                            KepalaSekolah = row["KepalaSekolah"].ToString(),
                            JenisNilai = row["JenisNilai"].ToString(),
                            JenisKomponen = row["JenisKomponen"].ToString(),
                            Nilai = row["Nilai"].ToString(),
                            IsNaik = Convert.ToBoolean(row["IsNaik"] == DBNull.Value ? false : row["IsNaik"]),
                            KeteranganKenaikan = row["KeteranganKenaikan"].ToString()
                        });
                    }
                }
            }

            return hasil;
        }


        public static List<ItemNilaiRapor> GetItemsRaporByDesign(string rel_rapor_design)
        {
            List<ItemNilaiRapor> hasil = new List<ItemNilaiRapor>();

            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            Rapor_Design m_desain = DAO_Rapor_Design.GetByID_Entity(rel_rapor_design);
            if (m_desain != null)
            {
                if (m_desain.TahunAjaran != null)
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = DAO_Rapor_Nilai.SP_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT;
                    comm.Parameters.AddWithValue("@" + DAO_Rapor_DesignDet.NamaField.Rel_Rapor_Design, rel_rapor_design);

                    DataTable dtResult = new DataTable();
                    sqlDA = new SqlDataAdapter(comm);
                    sqlDA.Fill(dtResult);

                    foreach (DataRow row in dtResult.Rows)
                    {
                        hasil.Add(new ItemNilaiRapor
                        {
                            TahunAjaran = row["TahunAjaran"].ToString(),
                            Semester = row["Semester"].ToString(),
                            UrutKategori = row["UrutKategori"].ToString(),
                            PoinKategori = row["PoinKategori"].ToString(),
                            NamaKategori = row["NamaKategori"].ToString(),
                            UrutSubKategori = row["UrutSubKategori"].ToString(),
                            PoinSubKategori = row["PoinSubKategori"].ToString(),
                            NamaSubKategori = row["NamaSubKategori"].ToString(),
                            PoinItemPenilaian = row["PoinItemPenilaian"].ToString(),
                            NamaItemPenilaian = row["NamaItemPenilaian"].ToString(),
                            IsNewPage = Convert.ToBoolean(row["IsNewPage"] == DBNull.Value ? false : row["IsNewPage"]),
                            Urut = Convert.ToInt32(row["Urut"] == DBNull.Value ? false : row["Urut"]),
                            Kriteria1 = row["Kriteria1"].ToString(),
                            NamaKriteria1 = row["NamaKriteria1"].ToString(),
                            Kriteria2 = row["Kriteria2"].ToString(),
                            NamaKriteria2 = row["NamaKriteria2"].ToString(),
                            Kriteria3 = row["Kriteria3"].ToString(),
                            NamaKriteria3 = row["NamaKriteria3"].ToString(),
                            IDSiswa = row["IDSiswa"].ToString(),
                            NIS = row["NIS"].ToString(),
                            Nama = row["Nama"].ToString(),
                            Kelas = row["Kelas"].ToString(),
                            Rel_KelasDet = row["Rel_KelasDet"].ToString(),
                            Sakit = row["Sakit"].ToString(),
                            Izin = row["Izin"].ToString(),
                            Alpa = row["Alpa"].ToString(),
                            TinggiBadan = row["TinggiBadan"].ToString(),
                            BeratBadan = row["BeratBadan"].ToString(),
                            TanggalRapor = row["TanggalRapor"].ToString(),
                            GuruKelas = row["GuruKelas"].ToString(),
                            KepalaSekolah = row["KepalaSekolah"].ToString(),
                            JenisNilai = row["JenisNilai"].ToString(),
                            JenisKomponen = row["JenisKomponen"].ToString(),
                            Nilai = row["Nilai"].ToString(),
                            IsNaik = Convert.ToBoolean(row["IsNaik"] == DBNull.Value ? false : row["IsNaik"]),
                            KeteranganKenaikan = row["KeteranganKenaikan"].ToString()
                        });
                    }
                }
            }

            return hasil;
        }

        private static Rapor_Nilai GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Nilai
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                IsPosted = (row[NamaField.IsPosted] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsPosted])),
                IsLocked = (row[NamaField.IsLocked] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsLocked]))
            };
        }

        public static List<Rapor_Nilai> GetAll_Entity()
        {
            List<Rapor_Nilai> hasil = new List<Rapor_Nilai>();
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

        public static Rapor_Nilai GetByID_Entity(string kode)
        {
            Rapor_Nilai hasil = new Rapor_Nilai();
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

        public static void Insert(Rapor_Nilai m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsPosted, m.IsPosted));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, m.IsLocked));
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

        public static void Update(Rapor_Nilai m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsPosted, m.IsPosted));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, m.IsLocked));
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

        public static List<Rapor_Nilai> GetAllByTABySMByKelasDet_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet
            )
        {
            List<Rapor_Nilai> hasil = new List<Rapor_Nilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);

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

        public static void SaveNilai(string tahun_ajaran, string semester, string rel_siswa, string rel_rapordesigndet, string rel_kelasdet, string rel_kriteria, string deskripsi, string berat_badan = "", string tinggi_badan = "")
        {
            List<string> hasil = new List<string>();
            Guid kode = Guid.NewGuid();

            if (
                tahun_ajaran.Trim() != "" &&
                semester.Trim() != "" &&
                rel_siswa.Trim() != "" &&
                rel_kelasdet.Trim() != ""
            )
            {
                //update header
                List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                        tahun_ajaran, semester, rel_kelasdet
                    );

                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_KelasDet = rel_kelasdet,
                        IsLocked = false,
                        IsPosted = false
                    });
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //update detail 1
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_NilaiSiswa> lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode.ToString());
                lst_nilai_siswa = lst_nilai_siswa.FindAll(m => m.Rel_Siswa == rel_siswa);

                if (lst_nilai_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa.Insert(new Rapor_NilaiSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        IsLocked = false,
                        IsPosted = false
                    });
                }
                else
                {
                    kode_nilai_siswa = lst_nilai_siswa.FirstOrDefault().Kode;
                }

                //update detail 2 (nilai)
                Guid kode_nilai_siswa_det = Guid.NewGuid();
                List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetPoinPenilaian_Entity(
                        kode_nilai_siswa.ToString(), rel_siswa, rel_rapordesigndet
                    );

                if (lst_nilai_siswa_det.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa_Det.Insert(new Rapor_NilaiSiswa_Det
                    {
                        Kode = kode_nilai_siswa_det,
                        Rel_Siswa = rel_siswa,
                        Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                        Rel_Rapor_DesignDet = rel_rapordesigndet,
                        Rel_Rapor_Kriteria = rel_kriteria,
                        Deskripsi = deskripsi
                    });
                }
                else
                {
                    kode_nilai_siswa_det = lst_nilai_siswa_det.FirstOrDefault().Kode;
                    DAO_Rapor_NilaiSiswa_Det.Update(new Rapor_NilaiSiswa_Det
                    {
                        Kode = kode_nilai_siswa_det,
                        Rel_Siswa = rel_siswa,
                        Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                        Rel_Rapor_DesignDet = rel_rapordesigndet,
                        Rel_Rapor_Kriteria = rel_kriteria,
                        Deskripsi = deskripsi
                    });
                }
            }
        }
    }
}