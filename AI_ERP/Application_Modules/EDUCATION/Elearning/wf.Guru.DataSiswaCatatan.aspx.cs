using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Guru_DataSiswaCatatan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowListCatatanSiswa();
        }

        protected void ShowListCatatanSiswa()
        {
            string rel_siswa = Libs.GetQueryString("s");
            List<CatatanSiswa> lst_catatan = DAO_CatatanSiswa.GetBySiswa_Entity(rel_siswa);
            
            string html = "";
            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                Libs.GetTahunAjaranNow(),
                Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                rel_siswa);
            if (m_siswa != null)
            {
                if (m_siswa.Nama != null)
                {
                    html = "<img " +
                                "src=\"" +
                                    ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg")) +
                                    "\" " +
                                "style=\"margin: 0 auto; display: table; height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;\" /> ";
                    html += "<div style=\"margin: 0 auto; display: table; font-weight: bold;\">" +
                                m_siswa.Nama.ToUpper().Trim() +
                            "</div>" +
                            "<hr style=\"margin: 3px; margin-left: 0px; margin-right: 0px; margin-bottom: 10px; margin-top: 10px;\" />";
                    html += "<table style=\"margin: 0px; width: 100%;\">";

                    foreach (var m in lst_catatan)
                    {
                        string s_kategori = "<span style=\"float: right;\">" +
                                                (
                                                    m.Rel_Kategori.ToString().Trim().ToUpper() == AI_ERP.Application_Libs.KategoriCatatanSiswa.Pelanggaran.Kode.Trim().ToUpper()
                                                    ? "<i title=\" Pelanggaran \" class=\"fa fa-exclamation-triangle\" style=\"color: orange;\"></i>"
                                                    : (
                                                        m.Rel_Kategori.ToString().Trim().ToUpper() == AI_ERP.Application_Libs.KategoriCatatanSiswa.Prestasi.Kode.Trim().ToUpper()
                                                        ? "<i title=\" Prestasi \" class=\"fa fa-check-circle\" style=\"color: green;\"></i>"
                                                        : "<i class=\"fa fa-info-circle\" style=\"color: #bfbfbf;\"></i>"
                                                        )
                                                ) +
                                            "</span>";

                        UserLogin m_user = DAO_UserLogin.GetByNIP(m.Rel_Guru);
                        if (m_user != null)
                        {
                            if (m_user.UserID != null)
                            {
                                html += "<tr>" +
                                            "<td style=\"padding: 5px; background-color: white;\">" +
                                                "<span style='font-weight: bold; color: grey'>" +
                                                    m_user.UserID +
                                                "</span>" +
                                                "&nbsp;" +
                                                "<span style=\"font-weight: normal;\">" +
                                                    "@" +
                                                    Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false) +
                                                "<span>" +
                                                "&nbsp;" +
                                                s_kategori +
                                                "<div style=\"margin-top: 5px; padding-left: 10px; padding-right: 10px; background-color: #F1F9F7; font-weight: normal; width: 100%; border-style: solid; border-color: #F1F9F7; border-width: 1px; border-radius: 5px;\">" +
                                                    m.Catatan +
                                                "<div>" +
                                            "</td>" +
                                        "</tr>";
                            }
                        }
                    }

                    html += "</table>";

                }
            }

            ltrCatatanSiswa.Text = html;
        }
    }
}