using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs.Elearning;

using AI_ERP.Application_Entities.Elearning.KB;
using AI_ERP.Application_DAOs.Elearning.KB;

using AI_ERP.Application_Entities.Elearning.TK;
using AI_ERP.Application_DAOs.Elearning.TK;

using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;

using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL
{
    public partial class wf_FileRaporView : System.Web.UI.Page
    {
        private static class QS
        {
            public static string GetUnit()
            {
                if (Libs.GetQueryString("u").Trim() != "")
                {
                    return Libs.GetQueryString("u");
                }
                else
                {
                    return Libs.GetQueryString("unit");
                }
            }

            public static string GetToken()
            {
                return Libs.GetQueryString("token");
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetAct()
            {
                return Libs.GetQueryString("act");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetTipeRapor()
            {
                return Libs.GetQueryString("tr");
            }

            public static string GetSemester()
            {
                string sm = Libs.GetQueryString("sm");
                return sm;
            }

            public static string GetSiswa()
            {
                string sw = Libs.GetQueryString("sw");
                return sw;
            }

            public static string GetURLVariable()
            {
                string s_url_var = "";
                s_url_var += (QS.GetUnit().Trim() != "" ? "unit=" + QS.GetUnit().Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();

                return (
                            QS.GetUnit().Trim() != "" || QS.GetToken().Trim() != ""
                        ? "?" : "") +
                        s_url_var;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowListFileRaporDet();
        }

        protected void ShowListFileRaporDet()
        {
            string html = "";
            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                    QS.GetTahunAjaran(),
                    QS.GetSemester(),
                    QS.GetSiswa()
                );

            if (m_siswa != null)
            {
                if (m_siswa.Nama != null)
                {
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {
                            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(
                                    m_siswa.Rel_KelasDet
                                );
                            if (m_kelas_det != null)
                            {
                                if (m_kelas_det.Nama != null)
                                {
                                    //get settingan rapor per unit
                                    string s_tanggal_buka_rapor = "";
                                    bool is_buka_rapor = true;
                                    switch ((Libs.UnitSekolah)m_sekolah.UrutanJenjang)
                                    {
                                        case Libs.UnitSekolah.SALAH:
                                            break;
                                        case Libs.UnitSekolah.KB:
                                            Application_Entities.Elearning.KB.Rapor_Arsip m_arsip_kb =
                                                Application_DAOs.Elearning.KB.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                                        m0 => m0.TahunAjaran == QS.GetTahunAjaran() && m0.Semester == QS.GetSemester() &&
                                                              m0.JenisRapor == (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim() ? "Semester" : "LTS")
                                                    ).FirstOrDefault();
                                            if (m_arsip_kb != null)
                                            {
                                                if (m_arsip_kb.TahunAjaran != null)
                                                {
                                                    is_buka_rapor = (m_arsip_kb.TanggalBukaLinkRapor > DateTime.Now ? false : true);
                                                    s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_arsip_kb.TanggalBukaLinkRapor, true, "Jam");
                                                }
                                            }
                                            break;
                                        case Libs.UnitSekolah.TK:
                                            Application_Entities.Elearning.TK.Rapor_Arsip m_arsip_tk =
                                                Application_DAOs.Elearning.TK.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                                        m0 => m0.TahunAjaran == QS.GetTahunAjaran() && m0.Semester == QS.GetSemester() &&
                                                              m0.JenisRapor == (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim() ? "Semester" : "LTS")
                                                    ).FirstOrDefault();
                                            if (m_arsip_tk != null)
                                            {
                                                if (m_arsip_tk.TahunAjaran != null)
                                                {
                                                    is_buka_rapor = (m_arsip_tk.TanggalBukaLinkRapor > DateTime.Now ? false : true);
                                                    s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_arsip_tk.TanggalBukaLinkRapor, true, "Jam");
                                                }
                                            }
                                            break;
                                        case Libs.UnitSekolah.SD:
                                            Application_Entities.Elearning.SD.Rapor_Pengaturan m_pengaturan_sd =
                                                Application_DAOs.Elearning.SD.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(
                                                        m0 => m0.TahunAjaran == QS.GetTahunAjaran() && m0.Semester == QS.GetSemester()
                                                    ).FirstOrDefault();
                                            if (m_pengaturan_sd != null)
                                            {
                                                if (m_pengaturan_sd.TahunAjaran != null)
                                                {
                                                    if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim())
                                                    {
                                                        is_buka_rapor = (m_pengaturan_sd.TanggalBukaLinkRapor > DateTime.Now ? false : true);
                                                        s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_pengaturan_sd.TanggalBukaLinkRapor, true, "Jam");
                                                    }
                                                    else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS.ToUpper().Trim())
                                                    {
                                                        is_buka_rapor = (m_pengaturan_sd.TanggalBukaLinkLTS > DateTime.Now ? false : true);
                                                        s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_pengaturan_sd.TanggalBukaLinkLTS, true, "Jam");
                                                    }
                                                }
                                            }
                                            break;
                                        case Libs.UnitSekolah.SMP:
                                            Application_Entities.Elearning.SMP.Rapor_Pengaturan m_pengaturan_smp =
                                                Application_DAOs.Elearning.SMP.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(
                                                        m0 => m0.TahunAjaran == QS.GetTahunAjaran() && m0.Semester == QS.GetSemester()
                                                    ).FirstOrDefault();
                                            if (m_pengaturan_smp != null)
                                            {
                                                if (m_pengaturan_smp.TahunAjaran != null)
                                                {
                                                    if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim())
                                                    {
                                                        is_buka_rapor = (m_pengaturan_smp.TanggalBukaLinkRapor > DateTime.Now ? false : true);
                                                        s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_pengaturan_smp.TanggalBukaLinkRapor, true, "Jam");
                                                    }
                                                    else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS.ToUpper().Trim())
                                                    {
                                                        is_buka_rapor = (m_pengaturan_smp.TanggalBukaLinkLTS > DateTime.Now ? false : true);
                                                        s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_pengaturan_smp.TanggalBukaLinkLTS, true, "Jam");
                                                    }
                                                }
                                            }
                                            break;
                                        case Libs.UnitSekolah.SMA:
                                            Application_Entities.Elearning.SMA.Rapor_Pengaturan m_pengaturan_sma =
                                                Application_DAOs.Elearning.SMA.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(
                                                        m0 => m0.TahunAjaran == QS.GetTahunAjaran() && m0.Semester == QS.GetSemester()
                                                    ).FirstOrDefault();
                                            if (m_pengaturan_sma != null)
                                            {
                                                if (m_pengaturan_sma.TahunAjaran != null)
                                                {
                                                    if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim())
                                                    {
                                                        is_buka_rapor = (m_pengaturan_sma.TanggalBukaLinkRapor > DateTime.Now ? false : true);
                                                        s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_pengaturan_sma.TanggalBukaLinkRapor, true, "Jam");
                                                    }
                                                    else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS.ToUpper().Trim())
                                                    {
                                                        is_buka_rapor = (m_pengaturan_sma.TanggalBukaLinkLTS > DateTime.Now ? false : true);
                                                        s_tanggal_buka_rapor = Libs.GetTanggalIndonesiaFromDate(m_pengaturan_sma.TanggalBukaLinkLTS, true, "Jam");
                                                    }
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    ltrTanggalDownloadRapor.Text = s_tanggal_buka_rapor;
                                    //end get settingan rapor per unit

                                    ltrNamaSiswa.Text = m_siswa.Nama.ToUpper().Trim();
                                    ltrKelasSiswa.Text = m_kelas_det.Nama.Trim();
                                    ltrTahunAjaran.Text = QS.GetTahunAjaran();
                                    ltrSemester.Text = QS.GetSemester();

                                    //siswa custom
                                    if (
                                        m_siswa.Kode.ToString().ToUpper().Trim() == "6A054869-3A9B-4B33-B5C7-38B0ED8D950E" ||
                                        m_siswa.Kode.ToString().ToUpper().Trim() == "59A5A74A-68ED-4EA1-96F4-09704AB3C542"
                                    )
                                    {
                                        is_buka_rapor = true;
                                    }
                                    //end siswa custom

                                    if (QS.GetAct() == Constantas.TOKEN_ADMIN) is_buka_rapor = true;
                                    if (!is_buka_rapor)
                                    {
                                        mvMain.ActiveViewIndex = 1;
                                        mvFileRapor.ActiveViewIndex = 0;

                                        //update view
                                        if (Libs.GetQueryString("act").Trim() != Constantas.TOKEN_ADMIN)
                                        {
                                            DAO_RaporViewEmail.Insert(
                                                    new RaporViewEmail
                                                    {
                                                        Kode = Guid.NewGuid(),
                                                        Rel_Email = Libs.GetQueryString("idm").Trim(),
                                                        Rel_Siswa = Libs.GetQueryString("sw").Trim(),
                                                        Tanggal = DateTime.Now,
                                                        URL = "Blocked Date"
                                                    }
                                                );
                                        }
                                        //end update view
                                        return;
                                    }

                                    List<string> lst_file = new List<string>();
                                    if (QS.GetTipeRapor().Trim().ToUpper() == TipeRapor.LTS)
                                    {
                                        lst_file = Libs.GetListUploadedFiles(this.Page, Libs.GetLokasiFolderFileLTS(
                                            m_siswa.Kode.ToString(), QS.GetTahunAjaran(), QS.GetSemester(), m_siswa.Rel_KelasDet, (Libs.UnitSekolah)m_sekolah.UrutanJenjang
                                        ));
                                    }
                                    else if (QS.GetTipeRapor().Trim().ToUpper() == TipeRapor.SEMESTER)
                                    {
                                        lst_file = Libs.GetListUploadedFiles(this.Page, Libs.GetLokasiFolderFileRapor(
                                            m_siswa.Kode.ToString(), QS.GetTahunAjaran(), QS.GetSemester(), m_siswa.Rel_KelasDet, (Libs.UnitSekolah)m_sekolah.UrutanJenjang
                                        ));
                                    }

                                    if (lst_file.Count == 0)
                                    {
                                        html = "<div style=\"padding: 25px; text-align: center; font-weight: bold; color: darkorange; padding-top: 0px; padding-bottom: 0px;\"><i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;File Kosong</div>";
                                    }
                                    else
                                    {
                                        html = Libs.GetHTMLListUploadedFilesRapor(this.Page,
                                                m_siswa.Kode.ToString(), QS.GetTahunAjaran(), QS.GetSemester(), m_siswa.Rel_KelasDet.ToString(), (Libs.UnitSekolah)m_sekolah.UrutanJenjang
                                           , m_siswa.Kode.ToString(), QS.GetTipeRapor(), false, false);
                                    }

                                    ltrFileRapor.Text = html;

                                    mvMain.ActiveViewIndex = 1;
                                    mvFileRapor.ActiveViewIndex = 1;

                                    //update view
                                    if (Libs.GetQueryString("act").Trim() != Constantas.TOKEN_ADMIN)
                                    {
                                        DAO_RaporViewEmail.Insert(
                                                new RaporViewEmail
                                                {
                                                    Kode = Guid.NewGuid(),
                                                    Rel_Email = Libs.GetQueryString("idm").Trim(),
                                                    Rel_Siswa = Libs.GetQueryString("sw").Trim(),
                                                    Tanggal = DateTime.Now,
                                                    URL = ""
                                                }
                                            );
                                    }
                                    //end update view
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            //update view
            if (Libs.GetQueryString("act").Trim() != Constantas.TOKEN_ADMIN)
            {
                DAO_RaporViewEmail.Insert(
                        new RaporViewEmail
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Email = Libs.GetQueryString("idm").Trim(),
                            Rel_Siswa = Libs.GetQueryString("sw").Trim(),
                            Tanggal = DateTime.Now,
                            URL = "No Data"
                        }
                    );
            }
            //end update view
            mvMain.ActiveViewIndex = 0;
        }
    }
}