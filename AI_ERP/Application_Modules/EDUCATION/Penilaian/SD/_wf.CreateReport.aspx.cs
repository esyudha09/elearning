using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SD.Reports;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_CreateReport : System.Web.UI.Page
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

            public static string GetLevel()
            {
                return Libs.GetQueryString("k");
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
                CreateReport();
            }
        }

        protected void CreateReport()
        {
            if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD)
            {
                Reports_SD.NilaiRapor_KTSP(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());
            }
            else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD)
            {
                Reports_SD.UraianRapor_KTSP(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());
            } 
            else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD)
            {
                Reports_SD.NilaiRapor_KURTILAS(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());
            }
            else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD)
            {
                Reports_SD.UraianRapor_KURTILAS(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());
            }
        }
    }
}