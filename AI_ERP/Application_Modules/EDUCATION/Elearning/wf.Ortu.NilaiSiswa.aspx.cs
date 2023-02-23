using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Ortu_NilaiSiswa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-file-text-o\"></i>&nbsp;&nbsp;" +
                                       "Nilai Siswa";
            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            ShowNilai();
        }

        protected void ShowNilai()
        {
            ltrNilaiSiswa.Text = "";
            List<UserOrtuDet> lst_user_ortu_det = DAO_UserOrtuDet.SelectByUserID(Libs.LOGGED_USER_M.UserID);
            foreach (UserOrtuDet m_det in lst_user_ortu_det)
            {
                Siswa m_siswa = DAO_Siswa.GetByID_Entity(
                    Libs.GetTahunAjaranNow(),
                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                    m_det.NIS
                 );
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {
                        List<_NilaiSiswa> lst_nilai_siswa = _DAO_NilaiSiswa.GetByNISSekolah_Entity(m_siswa.NISSekolah);

                        int id_periode = 1;
                        foreach (var item in
                            lst_nilai_siswa.OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester).Select(m => new { m.TahunAjaran, m.Semester }).Distinct())
                        {
                            string list_nilai = "";
                            string id_ui = Guid.NewGuid().ToString().Replace("-", "_");

                            List<_NilaiSiswa> lst_nilai_siswa_det = lst_nilai_siswa.FindAll(
                                    m => m.TahunAjaran == item.TahunAjaran && m.Semester == item.Semester
                                );

                            bool b_ada_nilai = false;
                            foreach (var m_mapel in lst_nilai_siswa_det.Select(m => new { m.Mapel, m.NamaMapel }).Distinct())
                            {
                                b_ada_nilai = true;

                                string list_aspek = "";
                                bool ada_aspek = false;
                                foreach (var m_aspek in lst_nilai_siswa_det.FindAll(
                                                m => m.TahunAjaran == item.TahunAjaran &&
                                                     m.Semester == item.Semester &&
                                                     m.Mapel == m_mapel.Mapel
                                                ).Select(m => new { m.NamaSubAspek }).Distinct())
                                {
                                    ada_aspek = true;

                                    string list_nilai_det = "";
                                    foreach (_NilaiSiswa m_nilai_siswa in lst_nilai_siswa_det.FindAll(
                                            m=> m.TahunAjaran == item.TahunAjaran &&
                                                m.Semester == item.Semester &&
                                                m.Mapel == m_mapel.Mapel &&
                                                m.NamaSubAspek == m_aspek.NamaSubAspek
                                        ))
                                    {
                                        list_nilai_det += "<td style=\"max-width: 40px; width: 40px; padding: 0px; background-color: #F4F4F4; text-align: center; border-style: solid; border-width: 1px; border-color: #bfbfbf; color: grey;\">" +
                                                            m_nilai_siswa.UrutNilai +
                                                          "</td>";
                                    }
                                    list_nilai_det = "<tr>" +
                                                        list_nilai_det +
                                                     "</tr>";

                                    int icount_nilai = 0;
                                    foreach (_NilaiSiswa m_nilai_siswa in lst_nilai_siswa_det.FindAll(
                                            m => m.TahunAjaran == item.TahunAjaran &&
                                                m.Semester == item.Semester &&
                                                m.Mapel == m_mapel.Mapel &&
                                                m.NamaSubAspek == m_aspek.NamaSubAspek
                                        ))
                                    {
                                        list_nilai_det += "<td style=\"max-width: 40px; width: 40px; font-size: small; padding: 0px; background-color: " + (m_nilai_siswa.Nilai > 70 ? "white" : (m_nilai_siswa.Nilai == Constantas.NilaiDesimalNULL ? "white" : "#FFD3D3")) + "; text-align: center; border-style: solid; border-width: 1px; border-color: #bfbfbf; font-weight: normal;\">" +
                                                            (
                                                                m_nilai_siswa.Nilai == Constantas.NilaiDesimalNULL
                                                                ? "&nbsp;"
                                                                : Libs.GetFormatBilangan(m_nilai_siswa.Nilai, 1).ToString()
                                                            ) +
                                                          "</td>";
                                        icount_nilai++;
                                    }

                                    list_nilai_det = "<tr>" +
                                                        list_nilai_det +
                                                     "</tr>";

                                    list_nilai_det = "<table style=\"margin: 0px; float: left;\">" +
                                                        "<tr>" +
                                                            "<td colspan=\"" + icount_nilai.ToString() + "\" style=\"font-size: small; text-align: center; background-color: #F4F4F4; color: grey; border-style: solid; border-width: 1px; border-color: #bfbfbf;\">" +
                                                                (
                                                                    m_aspek.NamaSubAspek.Length > 2
                                                                    ? Libs.GetPerbaikiEjaanNama(m_aspek.NamaSubAspek)
                                                                    : m_aspek.NamaSubAspek
                                                                ) +
                                                            "</td>" +
                                                        "</tr>" +
                                                        list_nilai_det +
                                                     "</table>";

                                    list_aspek +=   list_nilai_det;
                                }
                                if (ada_aspek)
                                {
                                    list_aspek = "<table style=\"margin: 0px; width: 100%; margin-top: 15px;\">" +
                                                    "<tr>" +
                                                        "<td style=\"font-size: small; padding: 0px; background-color: white; color: grey; font-weight: bold; padding-left: 0px;\">" +
                                                            list_aspek +
                                                        "</td>" +
                                                    "</tr>" +
                                                 "</table>";
                                }

                                list_nilai += "<tr>" +
                                                "<td style=\"font-size: small; padding: 10px; background-color: white; color: grey; font-weight: bold; color: #00A1C5;\">" +
                                                    "<i class=\"fa fa-tags\"></i>" +
                                                    "&nbsp;&nbsp;" +
                                                    m_mapel.NamaMapel +
                                                    list_aspek +
                                                "</td>" +
                                              "</tr>";
                            }

                            if (b_ada_nilai)
                            {
                                list_nilai = "<table style=\"margin: 0px; width: 100%;\">" +
                                                list_nilai +
                                             "</table>";

                                ltrNilaiSiswa.Text += "<div class=\"tile tile-collapse\">" +
                                                            "<div data-target=\"#" + id_ui + "\" data-toggle=\"tile\" style=\"background-color: " + (id_periode % 2 == 0 ? "#ffffff" : "#fbfbfb") + ";\">" +
                                                                "<div class=\"tile-inner\">" +
                                                                    "<div class=\"text-overflow\" style=\"font-weight: bold;\">" +
                                                                        "<i class=\"fa fa-book\"></i>" +
                                                                        "&nbsp;" +
                                                                        "&nbsp;" +
                                                                        item.TahunAjaran +
                                                                        " Semester " +
                                                                        item.Semester.ToString() +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>" +
                                                            "<div class=\"tile-active-show collapse\" id=\"" + id_ui + "\" style=\"height: 0px;\">" +
                                                                "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                                    list_nilai +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>";
                                id_periode++;
                            }
                        }

                    }
                }       
            }
        }
    }
}