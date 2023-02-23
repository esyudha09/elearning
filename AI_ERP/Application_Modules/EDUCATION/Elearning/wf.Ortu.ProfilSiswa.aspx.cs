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
    public partial class wf_ProfilSiswa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-address-card\"></i>&nbsp;&nbsp;" +
                                       "Profil Siswa";
            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            ShowDataProfil();
        }

        public static string GetFileFoto(string nis)
        {
            string hasil = "";
            try
            {
                svcFileFoto.FileFotoSoapClient svc = new svcFileFoto.FileFotoSoapClient();
                hasil = svc.LokasiFileFotoSiswa(nis);
                return hasil;
            }
            catch (Exception)
            {
                return hasil;
            }
        }

        protected void ShowDataProfil()
        {
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
                        string url_foto = Constantas.URL_FOTO_SISWA + m_siswa.NIS + ".jpg";
                        if (!Libs.IsURLExists(url_foto))
                        {
                            imgFoto.ImageUrl = ResolveUrl("~/Application_Templates/material-master/images/users/avatar-001.jpg");
                        }
                        else
                        {
                            imgFoto.ImageUrl = url_foto;
                        }

                        Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_siswa.Rel_Sekolah);
                        string unit_sekolah = "";
                        if (m_sekolah != null)
                        {
                            if (m_sekolah.Nama != null)
                            {
                                unit_sekolah = m_sekolah.Nama;
                            }
                        }

                        KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                        string kelas = "";
                        if (m_kelas_det != null)
                        {
                            if (m_kelas_det.Nama != null)
                            {
                                kelas = m_kelas_det.Nama;
                            }
                        }

                        lblTTL.Text = Libs.GetPerbaikiEjaanNama(m_siswa.TempatLahir) +
                                      (m_siswa.TempatLahir.Trim() != "" && m_siswa.TanggalLahir != DateTime.MinValue ? ", " : "") +
                                      (m_siswa.TanggalLahir != DateTime.MinValue ? Libs.GetTanggalIndonesiaFromDate(m_siswa.TanggalLahir, false) : "");

                        lblNIS.Text = m_siswa.NIS + " / " + m_siswa.NISSekolah;
                        lblNamaSiswa.Text = Libs.GetPerbaikiEjaanNama(m_siswa.Nama);
                        lblPanggilan.Text = Libs.GetPerbaikiEjaanNama(m_siswa.Panggilan);
                        lblUnitSekolah.Text = unit_sekolah;
                        lblKelas.Text = kelas;
                        if (!m_siswa.IsNonAktif)
                        {
                            ltrStatusSiswa.Text = "<label style=\"padding: 3px; color: white; background-color: #40B3D2; border-radius: 5px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: x-small;\">" +
                                                    "Aktif" +
                                                  "</label>";
                        }
                        else
                        {
                            ltrStatusSiswa.Text = "<label style=\"padding: 3px; color: white; background-color: #B7B7B7; border-radius: 5px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: x-small;\">" +
                                                    "Non Aktif" +
                                                  "</label>";
                        }
                        lblAlamat.Text = Libs.GetPerbaikiEjaanNama(m_siswa.Alamat);
                        lblEmail.Text = m_siswa.Email;
                        lblNamaAyah.Text = Libs.GetPerbaikiEjaanNama(m_siswa.NamaAyah);
                        lblNamaIbu.Text = Libs.GetPerbaikiEjaanNama(m_siswa.NamaIbu);
                        lblTahunAjaran.Text = m_siswa.TahunAjaran;
                    }
                }
            }
        }
    }
}