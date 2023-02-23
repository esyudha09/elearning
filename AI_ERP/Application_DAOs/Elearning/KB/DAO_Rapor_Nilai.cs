using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.KB;

namespace AI_ERP.Application_DAOs.Elearning.KB
{
    public static class DAO_Rapor_Nilai
    {
        public const string SP_SELECT_ALL = "KB_Rapor_Nilai_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "KB_Rapor_Nilai_SELECT_ALL_FOR_SEARCH";        
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "KB_Rapor_Nilai_SELECT_BY_TA_BY_SM_BY_KELASDET";
        public const string SP_SELECT_BY_ID = "KB_Rapor_Nilai_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT = "KB_Rapor_DesignDet_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT";
        public const string SP_SELECT_PENILAIAN = "KB_Rapor_NilaiSiswa_SELECT_PENILAIAN";

        public const string SP_INSERT = "KB_Rapor_Nilai_INSERT";

        public const string SP_UPDATE = "KB_Rapor_Nilai_UPDATE";

        public const string SP_DELETE = "KB_Rapor_Nilai_DELETE";

        public const string FONT_SIZE = "@fontsize";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string IsPosted = "IsPosted";
            public const string IsLocked = "IsLocked";
            public const string JenisRapor = "JenisRapor";
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

        public static void SaveNilai(string tahun_ajaran, string semester, string rel_siswa, string rel_rapordesigndet, string rel_kelasdet, string rel_kriteria, string deskripsi, string rel_nilaisiswa, string berat_badan = "", string tinggi_badan = "", string lingkar_kepala = "")
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
                string jenis_rapor = "";
                Rapor_DesignDet m_rapor_design_det = DAO_Rapor_DesignDet.GetByID_Entity(rel_rapordesigndet);
                if (m_rapor_design_det != null)
                {
                    if (m_rapor_design_det.NamaKomponen != null)
                    {
                        Rapor_Design m_rapor_design = DAO_Rapor_Design.GetByID_Entity(m_rapor_design_det.Rel_Rapor_Design.ToString());
                        if (m_rapor_design != null)
                        {
                            if (m_rapor_design.TahunAjaran != null)
                            {
                                jenis_rapor = m_rapor_design.JenisRapor;
                            }
                        }
                    }
                }

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

                Guid kode_nilai_siswa = new Guid(rel_nilaisiswa);
                
                //update detail 2 (nilai)
                List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetPoinPenilaian_Entity(
                        rel_siswa, rel_rapordesigndet
                    );
                foreach (var item in lst_nilai_siswa_det)
                {
                    DAO_Rapor_NilaiSiswa_Det.Delete(item.Kode.ToString());
                }

                DAO_Rapor_NilaiSiswa_Det.Insert(new Rapor_NilaiSiswa_Det
                {
                    Kode = Guid.NewGuid(),
                    Rel_Siswa = rel_siswa,
                    Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                    Rel_Rapor_DesignDet = rel_rapordesigndet,
                    Rel_Rapor_Kriteria = rel_kriteria,
                    Deskripsi = deskripsi
                });
            }
        }

        public static string GetHTMLReport(
                System.Web.UI.Page page,
                string kode_desain_rapor,
                string tahun_ajaran,
                string semester,
                string rel_kelas_det,
                bool show_pagebreak,
                string rel_siswa = "",
                bool print_mode = false,
                bool show_qrcode = false
            )
        {
            string html = "";
            string qrcode = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null && kode_desain_rapor.Trim() != "")
            {
                if (m_kelas_det.Nama != null)
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());

                    if (m_kelas != null)
                    {

                        if (m_kelas.Nama != null)
                        {
                            List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                m_kelas.Rel_Sekolah.ToString(),
                                rel_kelas_det,
                                tahun_ajaran,
                                semester
                            );

                            if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode == new Guid(rel_siswa));

                            Rapor_Design m_desain = DAO_Rapor_Design.GetByID_Entity(kode_desain_rapor);
                            if (m_desain != null)
                            {
                                if (m_desain.TahunAjaran != null)
                                {

                                    List<Rapor_DesignKriteria> lst_desain_kriteria = DAO_Rapor_DesignKriteria.GetByHeader_Entity(m_desain.Kode.ToString());
                                    List<Rapor_DesignDet> lst_desain_det = DAO_Rapor_DesignDet.GetByHeader_Entity(m_desain.Kode.ToString());
                                    List<Rapor_DesignDetEkskul> lst_desain_det_ekskul = DAO_Rapor_DesignDetEkskul.GetByHeader_Entity(m_desain.Kode.ToString());

                                    foreach (var m_siswa in lst_siswa)
                                    {
                                        //header
                                        string s_usia = "";
                                        string s_tinggi_badan = "";
                                        string s_berat_badan = "";
                                        string s_bulan_rapor = "";
                                        string s_kelompok_header = "";
                                        string s_nama_kelompok = "";

                                        string html_header =
                                               "<table style=\"width: 100%; margin: 0px;\">" +
                                                    "<tr>" +
                                                        "<td style=\"width: 60px;\">" +
                                                            "<img src=\"" + page.ResolveUrl("~/Application_CLibs/images/logo.png") + "\" />" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; text-align: center;  padding: 5px;" + FONT_SIZE + "\">" +
                                                            "YAYASAN ANAKKU<br />" +
                                                            "PERGURUAN ISLAM AL-IZHAR PONDOK LABU<br />" +
                                                            "KELOMPOK BERMAIN AL-IZHAR PONDOK LABU<br />" +
                                                            "Jl. RS. Fatmawati Kav. 49 Jakarta 12450 Telp. (021) 7506128, 7695542" +
                                                        "</td>" +
                                                    "</tr>" +
                                               "</table>" +
                                               "<div style=\"margin: 0 auto; display: table; border-color: black; border-width: 1px; border-style: solid; font-size: 1pt; color: white; width: 100%; border-left-style: none; border-right: none; margin-bottom: 15px;\">l</div>" +
                                               "<div style=\"font-weight: bold; margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                                    "LAPORAN  PERKEMBANGAN  ANAK" +
                                               "</div>" +
                                               "<div style=\"font-weight: bold; margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                                    s_kelompok_header +
                                               "</div>" +
                                               "<div style=\"font-weight: bold; margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                                    "SEMESTER " + (semester == "1" ? "I (SATU)" : "II (DUA)") + "&nbsp;" +
                                                    "TAHUN PELAJARAN " + tahun_ajaran +
                                               "</div>" +
                                               "<table style=\"margin: 15px; width: 100%; margin-left: 0px; margin-right: 0px;\">" +
                                                    "<tr>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 120px;" + FONT_SIZE + "\">" +
                                                            "Nama" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 20px;" + FONT_SIZE + "\">" +
                                                            ":" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px;" + FONT_SIZE + "\">" +
                                                            Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 120px;" + FONT_SIZE + "\">" +
                                                            "Usia" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 20px;" + FONT_SIZE + "\">" +
                                                            ":" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px;" + FONT_SIZE + "\">" +
                                                            s_usia +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 120px;" + FONT_SIZE + "\">" +
                                                            "Berat Badan" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 20px;" + FONT_SIZE + "\">" +
                                                            ":" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px;" + FONT_SIZE + "\">" +
                                                            s_berat_badan +
                                                            "&nbsp;<span style=\"font-weight: normal;\">Kg/" + s_bulan_rapor + "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 120px;" + FONT_SIZE + "\">" +
                                                            "Tinggi Badan" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 20px;" + FONT_SIZE + "\">" +
                                                            ":" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px;" + FONT_SIZE + "\">" +
                                                            s_tinggi_badan +
                                                            "&nbsp;<span style=\"font-weight: normal;\">Cm/" + s_bulan_rapor + "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 120px;" + FONT_SIZE + "\">" +
                                                            "Kelompok" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px; width: 20px;" + FONT_SIZE + "\">" +
                                                            ":" +
                                                        "</td>" +
                                                        "<td style=\"font-weight: bold; padding: 2px;" + FONT_SIZE + "\">" +
                                                            s_nama_kelompok +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>";

                                        string css_table_header = "border-style: solid; border-width: 1px; padding: 2px; text-align: center; font-weight: bold;";
                                        string css_table_cell = "border-style: solid; border-width: 1px; padding: 2px;";
                                        string html_table_body = "";

                                        html_table_body += "<tr>" +
                                                                "<td rowspan=\"2\" style=\"" + css_table_header + " background-color: #FABF8F;\">NO</td>" +
                                                                "<td colspan=\"3\" rowspan=\"2\" style=\"" + css_table_header + " background-color: #FABF8F;\">TINGKAT PENCAPAIAN PERKEMBANGAN ANAK</td>" +
                                                                "<td colspan=\"" + lst_desain_kriteria.Count.ToString() + "\" style=\"" + css_table_header + " background-color: #FABF8F;\">KRITERIA</td>" +
                                                            "</tr>";

                                        string html_td_kriteria = "";
                                        foreach (var item_kriteria in lst_desain_kriteria)
                                        {
                                            string s_nama_kriteria = "";
                                            Rapor_Kriteria m_kriteria = DAO_Rapor_Kriteria.GetByID_Entity(item_kriteria.Rel_Rapor_Kriteria.ToString());
                                            if (m_kriteria != null)
                                            {
                                                if (m_kriteria.Nama != null) {
                                                    s_nama_kriteria = m_kriteria.Alias;
                                                }
                                            }
                                            html_td_kriteria += "<td style=\"" + css_table_header + " width: 30px; background-color: #FABF8F;\">" + s_nama_kriteria + "</td>";
                                        }
                                        if (html_td_kriteria.Trim() != "")
                                        {
                                            html_td_kriteria = "<tr>" +
                                                                    html_td_kriteria +
                                                               "</tr>";
                                        }
                                        html_table_body = "<thead>" +
                                                            html_table_body+
                                                            html_td_kriteria +
                                                          "</thead>";

                                        var m_rapor_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();
                                        List<Rapor_NilaiSiswa> lst_rapor_nilai_siswa = new List<Rapor_NilaiSiswa>();
                                        if (m_rapor_nilai != null)
                                        {
                                            if (m_rapor_nilai.TahunAjaran != null)
                                            {
                                                lst_rapor_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(
                                                        m_rapor_nilai.Kode.ToString()
                                                    );
                                            }
                                        }
                                        Rapor_NilaiSiswa m_rapor_nilai_siswa = lst_rapor_nilai_siswa.FindAll(m => m.Rel_Siswa.ToString() == m_siswa.Kode.ToString()).FirstOrDefault();
                                        List<Rapor_NilaiSiswa_Det> lst_rapor_siswa_det = new List<Rapor_NilaiSiswa_Det>();
                                        if (lst_rapor_nilai_siswa.Count > 0)
                                        {
                                            if (m_rapor_nilai_siswa != null)
                                            {
                                                if (m_rapor_nilai_siswa.Rel_Siswa != null)
                                                {
                                                    lst_rapor_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetByHeader_Entity(m_rapor_nilai_siswa.Kode.ToString());
                                                }
                                            }
                                        }

                                        //load desain det
                                        string html_td_item_desain = "";
                                        int row_span = 0;
                                        int id = 0;
                                        foreach (var item_desain in lst_desain_det)
                                        {
                                            bool ada_item = false;                                            
                                            switch (item_desain.JenisKomponen)
                                            {
                                                case JenisKomponenRapor.KategoriPencapaian:
                                                    if (row_span > 0)
                                                    {
                                                        html_td_item_desain = html_td_item_desain.Replace("@rowspan", "rowspan=\"" + row_span.ToString() + "\"");
                                                        row_span = 0;
                                                    }

                                                    html_td_item_desain += //"<td @rowspan style=\"" + css_table_cell + " text-align: center; width: 30px; font-weight: bold; vertical-align: top;\">" +
                                                                           "<td style=\"" + css_table_cell + " text-align: center; width: 30px; font-weight: bold; vertical-align: top;\">" +
                                                                             item_desain.Poin +
                                                                           "</td>" + 
                                                                           "<td colspan=\"3\" style=\"" + css_table_cell + " text-align: left; width: 30px; font-weight: bold;\">" +
                                                                             Libs.GetHTMLNoParagraphDiAwal(item_desain.NamaKomponen) +
                                                                           "</td>";
                                                    row_span++;
                                                    ada_item = true;
                                                    break;
                                                case JenisKomponenRapor.SubKategoriPencapaian:
                                                    html_td_item_desain += "<td style=\"" + css_table_cell + " text-align: center; width: 30px; font-weight: bold;\">" +
                                                                             "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_table_cell + " text-align: left; width: 10px; font-weight: bold; border-right-width: 0px; vertical-align: top;\">" +
                                                                             item_desain.Poin +
                                                                           "</td>" +
                                                                           "<td colspan=\"2\" style=\"" + css_table_cell + " font-weight: bold; border-left-width: 0px;\">" +
                                                                             Libs.GetHTMLNoParagraphDiAwal(item_desain.NamaKomponen) +
                                                                           "</td>";
                                                    row_span++;
                                                    ada_item = true;
                                                    break;
                                                case JenisKomponenRapor.PoinKategoriPencapaian:
                                                    html_td_item_desain += "<td style=\"" + css_table_cell + " text-align: center; width: 30px;\">" +
                                                                             "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_table_cell + " width: 10px; border-right-width : 0px;\">" +
                                                                             "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_table_cell + " border-left-style: none; border-right-width : 0px;\">" +
                                                                             item_desain.Poin +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_table_cell + " border-left-width : 0px;\">" +
                                                                             Libs.GetHTMLNoParagraphDiAwal(item_desain.NamaKomponen) +
                                                                           "</td>";
                                                    row_span++;
                                                    ada_item = true;
                                                    break;
                                                case JenisKomponenRapor.Rekomendasi:
                                                    string s_rekomendasi = "";
                                                    if (lst_rapor_siswa_det.FindAll(
                                                            m => m.Rel_Rapor_DesignDet.ToUpper() == item_desain.Kode.ToString().ToUpper()
                                                    ).Count > 0)
                                                    {
                                                        var m_nilai = lst_rapor_siswa_det.FindAll(
                                                            m => m.Rel_Rapor_DesignDet.ToUpper() == item_desain.Kode.ToString().ToUpper()
                                                        ).FirstOrDefault();
                                                        if (m_nilai != null)
                                                        {
                                                            if (m_nilai.Deskripsi != null)
                                                            {
                                                                s_rekomendasi = m_nilai.Deskripsi;
                                                            }
                                                        }
                                                    }

                                                    string s_keterangan = "";
                                                    foreach (var item_kriteria in lst_desain_kriteria)
                                                    {
                                                        Rapor_Kriteria m_kriteria = DAO_Rapor_Kriteria.GetByID_Entity(item_kriteria.Rel_Rapor_Kriteria.ToString());
                                                        if (m_kriteria != null)
                                                        {
                                                            if (m_kriteria.Nama != null)
                                                            {
                                                                s_keterangan += "<tr>" +
                                                                                    "<td style=\"width: 100px; border-style: none; padding: 2px;\">" + Libs.GetHTMLNoParagraphDiAwal(m_kriteria.Alias) + "</td>" +
                                                                                    "<td style=\"width: 30px; border-style: none; padding: 2px;\">&nbsp;:&nbsp;</td>" +
                                                                                    "<td style=\"border-style: none; padding: 2px;\">" + Libs.GetHTMLNoParagraphDiAwal(m_kriteria.Nama) + "</td>" +
                                                                                "</tr>";
                                                            }
                                                        }
                                                    }

                                                    html_td_item_desain += "<tr>" +
                                                                                "<td colspan=\"" + (lst_desain_kriteria.Count + 4).ToString() + "\" style=\"" + css_table_cell + " padding: 10px; border-width: 0px; padding-left: 0px; padding-bottom: 0px; font-weight: bold;\">" +
                                                                                    "Keterangan :   " +
                                                                                "</td>" +
                                                                           "</tr>" +
                                                                           "<tr>" +
                                                                                "<td colspan=\"" + (lst_desain_kriteria.Count + 4).ToString() + "\" style=\"" + css_table_cell + " padding: 10px; border-width: 0px; padding-left: 0px;\">" +
                                                                                    "<table style=\"margin: 0px; width: 100%; border-style: none;\">" +
                                                                                        s_keterangan +
                                                                                    "</table>" +
                                                                                "</td>" +
                                                                           "</tr>" +
                                                                           "<tr>" + 
                                                                                "<td colspan=\"" + (lst_desain_kriteria.Count + 4).ToString() + "\" style=\"" + css_table_cell + " padding: 10px;\">" +
                                                                                     "Rekomendasi untuk Orangtua :<br /><br />" +
                                                                                     Libs.GetHTMLNoParagraphDiAwal(s_rekomendasi) +
                                                                                "</td>" +
                                                                           "</tr>";
                                                    ada_item = false;
                                                    break;
                                            }

                                            if (ada_item)
                                            {
                                                html_td_item_desain = "<tr " + (item_desain.JenisKomponen == JenisKomponenRapor.PoinKategoriPencapaian || 
                                                                                item_desain.JenisKomponen == JenisKomponenRapor.KategoriPencapaian ||
                                                                                item_desain.JenisKomponen == JenisKomponenRapor.SubKategoriPencapaian 
                                                                                ? "class=\"display: table-row-group;\"" 
                                                                                : ""
                                                                               ) + ">" +
                                                                        html_td_item_desain;
                                                foreach (var item_kriteria in lst_desain_kriteria)
                                                {
                                                    if (lst_rapor_siswa_det.FindAll(
                                                            m => m.Rel_Rapor_DesignDet.ToUpper() == item_desain.Kode.ToString().ToUpper() && 
                                                                 m.Rel_Rapor_Kriteria.ToUpper() == item_kriteria.Kode.ToString().ToUpper()
                                                    ).Count > 0 && item_desain.JenisKomponen == JenisKomponenRapor.PoinKategoriPencapaian)
                                                    {
                                                        html_td_item_desain += "<td style=\"" + css_table_cell + " width: 30px; text-align: center; padding: 0px; font-weight: bold; font-size: large;\">" +
                                                                                    "&#x1f5f8;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        html_td_item_desain += "<td style=\"" + css_table_cell + " width: 30px;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                }
                                                html_td_item_desain += "</tr>";
                                            }
                                            id++;

                                            if (lst_desain_det.Count == id)
                                            {
                                                html_td_item_desain = html_td_item_desain.Replace("@rowspan", "rowspan=\"" + row_span.ToString() + "\"");
                                            }
                                        }

                                        html_table_body += html_td_item_desain;
                                        //end load desain det

                                        //load desain det ekskul
                                        foreach (var item_desain in lst_desain_det_ekskul)
                                        {

                                        }
                                        //end load desain det ekskul

                                        html_table_body = "<table class=\"print-friendly\" style=\"margin: 0px; margin-bottom: 15px; border-collapse: collapse; width: 100%;\">" +
                                                            html_table_body +
                                                          "</table>";

                                        html = html_header +
                                               html_table_body +
                                               (
                                                show_pagebreak
                                                ? "<div class=\"pagebreak\"></div>"
                                                : ""
                                               ) +
                                               "<div style=\"margin-bottom: 100px;\"></div>";

                                        if (print_mode)
                                        {
                                            html = html.Replace(FONT_SIZE, "font-size: 12pt;");
                                        }
                                        else
                                        {
                                            html = html.Replace(FONT_SIZE, "font-size: 12pt;");
                                        }

                                        string id_siswa = m_siswa.Kode.ToString().Replace("-", "_");

                                        if (show_qrcode)
                                        {
                                            qrcode += "<script type=\"text/javascript\">" +
                                                        "var qrcode_" + id_siswa + " = new QRCode(document.getElementById(\"qrcode_" + id_siswa + "\"), {" +
                                                            "width : 100, " +
                                                            "height : 100 " +
                                                        "});" +

                                                        "function makeCode_" + id_siswa + "(){" +
                                                            "qrcode_" + id_siswa + ".makeCode(\"" + m_siswa.Kode.ToString() + "\");" +
                                                        "}" +

                                                        "makeCode_" + id_siswa + "();" +
                                                     "</script>";
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            html = "<div style=\"margin: 0 auto; display: table; max-width: 800px;\">" +
                        html +
                   "<div>";

            return html;
        }
    }
}