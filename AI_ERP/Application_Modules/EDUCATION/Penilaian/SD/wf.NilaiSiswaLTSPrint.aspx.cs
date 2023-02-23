using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_NilaiSiswaLTSPrint : System.Web.UI.Page
    {
        private static class QS
        {
            public static string GetUnit()
            {
                KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelasdet != null)
                {
                    if (m_kelasdet.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelasdet.Rel_Kelas.ToString());
                        return m_kelas.Rel_Sekolah.ToString();
                    }
                }

                return "";
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetTahunAjaranPure()
            {
                string t = Libs.GetQueryString("t");
                return t;
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                return s;
            }

            public static string GetMapel()
            {
                string m = Libs.GetQueryString("m");
                return m;
            }

            public static string GetMapelSikap()
            {
                string ms = Libs.GetQueryString("ms");
                return ms;
            }

            public static string GetGuru()
            {
                string guru = Libs.GetQueryString("g");
                if (guru.Trim() == "") return Libs.LOGGED_USER_M.NoInduk;
                return guru;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowData();
            }
        }

        protected void ShowData()
        {
            string s_html = "";
            string s = Libs.GetQueryString("sis");
            string j = Libs.GetQueryString("j");
            string[] arr_siswa = s.Split(new string[] { ";" }, StringSplitOptions.None);
            string s_kurikulum = DAO_Rapor_StrukturNilai.GetKurikulumByKelas(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());

            int id = 1;

            if (j == "deskripsi_nc")
            {
                if (Libs.GetStringToDecimal(QS.GetTahunAjaran().Replace("-", "").Replace("/", "") + QS.GetSemester()) <= 201920201)
                {
                    s_html += DAO_Rapor_LTS.GetHTMLBahasaKDNoCheck(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), (id < arr_siswa.Length && arr_siswa.Length > 1 ? true : false), true);
                }
                else {
                    if (s_kurikulum == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        s_html += DAO_Rapor_LTS.GetHTMLBahasaKDNoCheck_KURTILAS(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), (id < arr_siswa.Length && arr_siswa.Length > 1 ? true : false), true);
                    }
                    else if (s_kurikulum == Libs.JenisKurikulum.SD.KTSP)
                    {
                        s_html += DAO_Rapor_LTS.GetHTMLBahasaKDNoCheck(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), (id < arr_siswa.Length && arr_siswa.Length > 1 ? true : false), true);
                    }
                }
            }
            else
            {
                foreach (var siswa in arr_siswa)
                {
                    if (siswa.Trim() != "")
                    {
                        if (j == "bkd")
                        {
                            s_html += DAO_Rapor_LTS.GetHTMLBahasaKD(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), (id < arr_siswa.Length && arr_siswa.Length > 1 ? true : false), siswa, true);
                        }
                        else
                        {
                            if (s_kurikulum == Libs.JenisKurikulum.SD.KURTILAS)
                            {
                                s_html += DAO_Rapor_LTS.GetHTMLReport_KURTILAS(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), (id < arr_siswa.Length && arr_siswa.Length > 1 ? true : false), siswa, true);
                            }
                            else if (s_kurikulum == Libs.JenisKurikulum.SD.KTSP)
                            {
                                s_html += DAO_Rapor_LTS.GetHTMLReport_KTSP(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), (id < arr_siswa.Length && arr_siswa.Length > 1 ? true : false), siswa, true);
                            }
                        }

                        id++;
                    }
                }
            }
            
            ltrHTML.Text = s_html;
        }
    }
}