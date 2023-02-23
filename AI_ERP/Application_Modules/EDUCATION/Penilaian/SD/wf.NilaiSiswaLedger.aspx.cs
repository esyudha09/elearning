﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_NilaiSiswaLedger : System.Web.UI.Page
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
            Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == QS.GetTahunAjaran() &&
                            m0.Semester == QS.GetSemester() &&
                            m0.JenisRapor == "Semester"

                ).FirstOrDefault();
            if (m_rapor_arsip == null)
            {
                Response.Write("Data tidak dapat ditampilkan karena pengaturan \"Proses Rapor\" belum dilakukan.");
                return;
            }

            string s_html = "";
            if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()) == Libs.JenisKurikulum.SD.KTSP)
            {
                if (Libs.GetQueryString("r") == "1")
                {
                    s_html = DAO_Rapor_Semester.GetHTMLLedger_KTSP_RataRata(
                        this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                    );

                    s_html = "DAFTAR NILAI RAPOR RATA-RATA SEMESTER 1 & 2 TAHUN " + QS.GetTahunAjaran() +
                             "<br />" +
                             "SD ISLAM AL-IZHAR PONDOK LABU" +
                             "<br /><br />" +
                             "KELAS : " + DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Nama.Trim().ToUpper() +
                             s_html;
                }
                else if (Libs.GetQueryString("skp") == "1")
                {
                    s_html = DAO_Rapor_Semester.GetHTMLLedgerSikap_KTSP(
                        this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                    );

                    s_html = "DAFTAR NILAI SIKAP SEMESTER " + QS.GetSemester() + " TAHUN " + QS.GetTahunAjaran() +
                             "<br />" +
                             "SD ISLAM AL-IZHAR PONDOK LABU" +
                             "<br /><br />" +
                             "KELAS : " + DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Nama.Trim().ToUpper() +
                             s_html;
                }
                else
                {
                    s_html = DAO_Rapor_Semester.GetHTMLLedger_KTSP(
                        this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                    );

                    s_html = "DAFTAR NILAI RAPOR SEMESTER " + QS.GetSemester() + " TAHUN " + QS.GetTahunAjaran() +
                             "<br />" +
                             "SD ISLAM AL-IZHAR PONDOK LABU" +
                             "<br /><br />" +
                             "KELAS : " + DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Nama.Trim().ToUpper() +
                             s_html;
                }                
            }
            else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()) == Libs.JenisKurikulum.SD.KURTILAS)
            {
                if (Libs.GetQueryString("r") == "1")
                {
                    if (Libs.GetStringToDecimal(QS.GetTahunAjaran().Substring(0, 4)) >= 2020)
                    {
                        s_html = DAO_Rapor_Semester.GetHTMLLedger_KURTILAS_RataRata_2020(
                            this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                        );
                    }
                    else
                    {
                        s_html = DAO_Rapor_Semester.GetHTMLLedger_KURTILAS_RataRata(
                            this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                        );
                    }
                    
                    s_html = "DAFTAR NILAI RAPOR RATA-RATA SEMESTER 1 & 2 TAHUN " + QS.GetTahunAjaran() + 
                             "<br />" +
                             "SD ISLAM AL-IZHAR PONDOK LABU" +
                             "<br /><br />" +
                             "KELAS : " + DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Nama.Trim().ToUpper() +
                             s_html;
                }
                else if (Libs.GetQueryString("skp") == "1")
                {
                    if (Libs.GetStringToInteger(QS.GetTahunAjaran().Substring(0, 4)) >= 2020)
                    {
                        s_html = DAO_Rapor_Semester.GetHTMLLedgerSikap_KURTILAS_2020(
                            this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                        );
                    }
                    else
                    {
                        s_html = DAO_Rapor_Semester.GetHTMLLedgerSikap_KURTILAS(
                            this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                        );
                    }

                    s_html = "DAFTAR NILAI SIKAP SEMESTER " + QS.GetSemester() + " TAHUN " + QS.GetTahunAjaran() +
                             "<br />" +
                             "SD ISLAM AL-IZHAR PONDOK LABU" +
                             "<br /><br />" +
                             "KELAS : " + DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Nama.Trim().ToUpper() +
                             s_html;
                }
                else
                {
                    if (Libs.GetStringToInteger(QS.GetTahunAjaran().Substring(0, 4)) >= 2020)
                    {
                        s_html = DAO_Rapor_Semester.GetHTMLLedger_KURTILAS_2020(
                        this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                    );
                    }
                    else
                    {
                        s_html = DAO_Rapor_Semester.GetHTMLLedger_KURTILAS(
                        this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                    );
                    }
                    
                    s_html = "DAFTAR NILAI RAPOR SEMESTER " + QS.GetSemester() + " TAHUN " + QS.GetTahunAjaran() +
                             "<br />" +
                             "SD ISLAM AL-IZHAR PONDOK LABU" +
                             "<br /><br />" +
                             "KELAS : " + DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Nama.Trim().ToUpper() +
                             s_html;
                }
            }

            ltrHTML.Text = s_html;
        }
    }
}