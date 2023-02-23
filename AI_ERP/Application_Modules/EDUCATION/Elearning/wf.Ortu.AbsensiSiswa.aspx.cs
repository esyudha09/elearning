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
    public partial class wf_AbsensiSiswa : System.Web.UI.Page
    {
        private static class QS
        {
            public static string GetUnit()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
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

            public static string GetMapel()
            {
                return Libs.GetQueryString("m");
            }

            public static string GetLevel()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        return m_kelas_det.Rel_Kelas.ToString();
                    }
                }

                return "";
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                if (s.Trim() == "") return Libs.GetSemesterByTanggal(DateTime.Now).ToString();
                return Libs.GetQueryString("s").Trim();
            }

            public static string GetJenisLaporan()
            {
                return Libs.GetQueryString("jl");
            }

            public static string GetJenisPresensi()
            {
                return Libs.GetQueryString("jp");
            }

            public static string GetSiswa()
            {
                return Libs.GetQueryString("sw");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-calendar-check-o\"></i>&nbsp;&nbsp;" +
                                       "Presensi Siswa";
            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            //ShowListAbsensi();

            if (!IsPostBack)
            {
                txtKD.Value = QS.GetKelas();
                InitInput();
            }
        }
        
        protected void InitInput()
        {
            KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
            if (m_kelasdet != null)
            {
                if (m_kelasdet.Nama != null)
                {
                    ltrLabelKelas.Text = m_kelasdet.Nama;
                }
            }

            txtDariTanggal.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            txtSampaiTanggal.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);

            cboTahunAjaran.Items.Clear();
            List<AI_ERP.Application_Libs.PeriodeTahunAjaranAbsen> lst =
                AI_ERP.Application_DAOs.Elearning.DAO_SiswaAbsen.GetPeriode_Entity();
            foreach (var item in lst)
            {
                cboTahunAjaran.Items.Add(
                    new ListItem
                    {
                        Value = item.TahunAjaran + item.Semester,
                        Text = item.TahunAjaran + ", Semester " + item.Semester
                    });
            }

            ListSiswaToDropDown();
        }

        protected void ListSiswaToDropDown()
        {
            cboSiswa.Items.Clear();
            cboSiswa.Items.Add(new ListItem
            {
                 Value = "0",
                 Text = "Semua"
            });

            List<Siswa> lst_siswa = DAO_Siswa.GetByRombel_Entity(QS.GetUnit(), QS.GetKelas(), cboTahunAjaran.SelectedValue.Substring(0, cboTahunAjaran.SelectedValue.Length - 1), cboTahunAjaran.SelectedValue.Substring(cboTahunAjaran.SelectedValue.Length - 1, 1));
            foreach (var item in lst_siswa.FindAll(m0 => m0.IsNonAktif == false).OrderBy(m0 => m0.Nama).ToList())
            {
                cboSiswa.Items.Add(new ListItem {
                    Value = item.Kode.ToString(),
                    Text = item.Nama.ToUpper().ToString().Trim()
                });
            }
        }
        
        protected void ShowListAbsensi()
        {
            ltrAbsensi.Text = "";

            List<UserOrtuDet> lst_user_ortu_det = DAO_UserOrtuDet.SelectByUserID(Libs.LOGGED_USER_M.UserID);
            foreach (UserOrtuDet m_det in lst_user_ortu_det)
            {
                Siswa siswa = DAO_Siswa.GetByID_Entity(
                    Libs.GetTahunAjaranNow(),
                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                    m_det.NIS
                );
                if (siswa != null)
                {
                    if (siswa.Nama != null)
                    {
                        List<_AbsenAjar> lst_absen_ajar = _DAO_AbsenAjar.GetByNISSekolah_Entity(siswa.NISSekolah);
                        int id_periode = 1;
                        foreach (var item in
                            lst_absen_ajar.Select(m => new { m.Tanggal.Year, m.Tanggal.Month }).Distinct().OrderByDescending(m => ((m.Year * 100) + m.Month)))
                        {
                            string id_ui = Guid.NewGuid().ToString().Replace("-", "_");
                            
                            DateTime tgl_awal = new DateTime(item.Year, item.Month, 1);
                            DateTime tgl_akhir = new DateTime(item.Year, item.Month, 1);
                            DateTime tgl_akhir_blnsebelumnya = tgl_awal.AddDays(-1);

                            tgl_akhir = tgl_akhir.AddMonths(1);
                            tgl_akhir = tgl_akhir.AddDays(-1);

                            string list_absen = "<table style=\"margin: 0px; width: 100%;\">";
                            for (int i = 1; i <= tgl_akhir.Day; i++)
                            {
                                DateTime tanggal = new DateTime(item.Year, item.Month, i);
                                List<_AbsenAjar> lst_absen = lst_absen_ajar.FindAll(
                                        m => m.Tanggal == tanggal
                                    );
                                string jadwal = "";
                                bool ada_jadwal = false;
                                foreach (var _absen in lst_absen)
                                {
                                    if (_absen.NamaMapel.Trim() != "")
                                    {
                                        ada_jadwal = true;
                                        jadwal = "<tr>" +
                                                    "<td style=\"background-color: white; padding: 5px; width: 25%;\">" +
                                                        _absen.NamaMapel +
                                                    "</td>" +
                                                    "<td style=\"background-color: white; padding: 5px; width: 25%;\">" +
                                                        _absen.JamMasuk +
                                                    "</td>" +
                                                    "<td style=\"background-color: white; padding: 5px; width: 25%;\">" +
                                                        _absen.JamKeluar +
                                                    "</td>" +
                                                    "<td style=\"background-color: white; padding: 5px; width: 24%;\">" +
                                                        (
                                                            _absen.IsAlfa ? "Alfa"
                                                            : (
                                                                _absen.IsIzin ? "Izin"
                                                                : (
                                                                    _absen.IsSakit ? "Sakit"
                                                                    : (
                                                                        "Masuk"
                                                                      )
                                                                  )
                                                              )
                                                        ) +
                                                    "</td>" +
                                                 "</tr>";
                                    }
                                }
                                if (ada_jadwal)
                                {
                                    jadwal = "<table style=\"width: 100%; margin: 0px;\">" +
                                                jadwal +
                                             "</table>";
                                    list_absen += "<tr>" +
                                                    "<td style=\"padding: 10px; background-color: white; font-weight: bold; width: 200px;\">" +
                                                        tanggal.ToString("dd/MM/yyyy") +
                                                    "</td>" +
                                                    "<td style=\"padding: 10px; background-color: white; font-weight: bold;\">" +
                                                        jadwal +
                                                    "</td>" +
                                                  "</tr>";
                                    list_absen += "<tr>" +
                                                    "<td colspan=\"2\" style=\"padding: 10px; background-color: white; font-weight: bold; padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px;\" />" +
                                                    "</td>" +
                                                  "</tr>";
                                }
                            }
                            list_absen += "</table>";

                            ltrAbsensi.Text += "<div class=\"tile tile-collapse\">" +
                                                    "<div data-target=\"#" + id_ui + "\" data-toggle=\"tile\" style=\"background-color: " + (id_periode % 2 == 0 ? "#ffffff" : "#fbfbfb") + ";\">" +
                                                        "<div class=\"tile-inner\">" +
                                                            "<div class=\"text-overflow\" style=\"font-weight: bold;\">" +
                                                                "<i class=\"fa fa-calendar\"></i>" +
                                                                "&nbsp;" +
                                                                "&nbsp;" +
                                                                Libs.Array_Bulan[item.Month - 1] + " " + item.Year.ToString() +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class=\"tile-active-show collapse\" id=\"" + id_ui + "\" style=\"height: 0px;\">" +
                                                        "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                            list_absen +
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