using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_Siswa_Input : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEINPUTSISWA";
        public const string C_ID = "{{id}}";

        public enum JenisAction
        {
            Add,
            Edit,
            AddItemKelas,
            EditItemKelas,
            Update,
            Delete,
            DeleteItemKelas,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            ItemKelasDetKosong
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowHeaderSubTitle = false;
            this.Master.HeaderTittle = "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/student.svg") + "\" " +
                                            "style=\"margin: 0 auto; height: 40px; width: 40px; margin-top: -10px; margin-right: 5px; float: left;\" />" +
                                       "&nbsp;&nbsp;" +
                                       "Biodata Siswa";
            this.Master.ShowHeaderTools = false;

            if (!IsPostBack)
            {
                this.Master.txtCariData.Text = Libs.GetQ();
                InitFields();
                ShowData();
            }
        }

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

            public static string GetSiswa()
            {
                return Libs.GetQueryString("sw");
            }

            public static string GetSemseter()
            {
                return Libs.GetQueryString("s");
            }

            public static string GetURLVariable()
            {
                string s_url_var = "";
                s_url_var += (QS.GetUnit().Trim() != "" ? "unit=" + QS.GetUnit().Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();                
                s_url_var += (s_url_var.Trim() != "" && QS.GetTahunAjaranPure().Trim() != "" ? "&" : "") + (QS.GetTahunAjaranPure().Trim() != "" ? "t=" : "") + QS.GetTahunAjaranPure().Trim(); 
                s_url_var += (s_url_var.Trim() != "" && QS.GetKelas().Trim() != "" ? "&" : "") + (QS.GetKelas().Trim() != "" ? "k=" : "") + QS.GetKelas().Trim();
                s_url_var += (s_url_var.Trim() != "" && Libs.GetQ().Trim() != "" ? "&" : "") + (Libs.GetQ() != "" ? "q=" : "") + Libs.GetQ().Trim();

                return (
                            QS.GetUnit().Trim() != "" || QS.GetToken().Trim() != "" ||
                            QS.GetTahunAjaranPure().Trim() != "" || QS.GetKelas().Trim() != "" ||
                            Libs.GetQ().Trim() != ""
                        ? "?" : "") +
                        s_url_var;
            }
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {

        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {

        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }
        
        protected void ShowData()
        {
            Siswa m = DAO_Siswa.GetByKode_Entity(
                Libs.GetTahunAjaranNow(),
                Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                QS.GetSiswa());
            if (m != null)
            {
                if (m.Nama != null)
                {
                    ltrFotoSiswa.Text = "<img " +
                                            "src=\"" +
                                                    ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m.NIS + ".jpg")) +
                                                 "\" " +
                                            "style=\"margin: 0 auto; height: 100px; width: 100px; border-radius: 100%; margin-left: 7px; margin-top: 5px; margin-bottom: 5px;\" />";

                    txtNISSekolah.Text = m.NISSekolah;
                    txtNama.Text = m.Nama;
                    txtPanggilan.Text = m.Panggilan;
                    cboDiterimaDiKelas.SelectedValue = m.Rel_Kelas.ToString();
                    cboJenisKelamin.SelectedValue = m.JenisKelamin;
                    txtEmail.Text = m.Email;
                    txtTempatLahir.Text = m.TempatLahir;
                    txtTanggalLahir.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahir, false);
                    txtNISLama.Text = m.NISLama;
                    
                    if (cboAsalSekolah.SelectedValue.Trim() != "")
                    {
                        if ((Libs.JenisAsalSekolah)Convert.ToInt16(cboAsalSekolah.SelectedValue) == Libs.JenisAsalSekolah.SiswaNonAlizhar)
                        {
                            txtNISLama.Enabled = false;
                            txtNISLama.Attributes["style"] = "background-color: #FCF8E3;";
                        }
                        else
                        {
                            txtNISLama.Attributes["style"] = "background-color: white;";
                        }
                    }
                    else
                    {
                        txtNISLama.Attributes["style"] = "background-color: white;";
                    }

                    if (m != null)
                    {
                        if (m.NIS != null)
                        {
                            cboAgama.SelectedValue = m.Agama;
                            cboStatusAnak.SelectedValue = m.StatusAnak;
                            txtAnakKe.Text = m.AnakKe;
                            txtJumlahSaudara.Text = m.DariBersaudara;
                            txtJumlahKakak.Text = m.JumlahKakak;
                            txtJumlahAdik.Text = m.JumlahAdik;
                            txtNISN.Text = m.NISN;
                            txtNIK.Text = m.NIK;
                            txtWargaNegara.Text = m.WargaNegara;
                            txtBahasaSehariHari.Text = m.BahasaSehariHari;
                            cboHobi.SelectedValue = m.Hobi;
                            txtHobiLainnya.Text = m.HobiLainnya;

                            cboUnitSekolah.SelectedValue = m.Rel_Sekolah.ToString();
                            ShowKelas(m.Rel_Sekolah.ToString(), m.Rel_KelasDet.ToString());
                            Libs.SelectDropdownListByValue(cboKelasRombel, m.Rel_KelasDet);
                            Libs.SelectDropdownListByValue(cboKelasJurusan, m.Rel_KelasDetJurusan);
                            Libs.SelectDropdownListByValue(cboKelasSosialisasi, m.Rel_KelasDetSosialisasi);
                            txtTahunPelajaran.Text = m.TahunAjaran;

                            txtAlamat.Text = m.Alamat;
                            txtRT.Text = m.RT;
                            txtRW.Text = m.RW;
                            txtKelurahan.Text = m.Kelurahan;
                            txtKecamatan.Text = m.Kecamatan;
                            txtKabupatenKota.Text = m.Kabupaten;
                            txtProvinsi.Text = m.Provinsi;
                            txtKodePOS.Text = m.KodePOS;
                            cboStatusTempatTinggal.SelectedValue = m.StatusTempatTinggal;
                            cboJarakKeSekolah.SelectedValue = m.JarakKeSekolah;
                            cboKeSekolahDengan.SelectedValue = m.KeSekolahDengan;
                            txtWaktuTempuh.Text = m.WaktuTempuh;

                            txtSMA.Text = m.AsalSMA;
                            txtSMP.Text = m.AsalSMP;
                            txtSD.Text = m.AsalSD;
                            txtTK.Text = m.AsalTK;

                            txtKesenian.Text = m.BakatKesenian;
                            txtOlahraga.Text = m.BakatOlahRaga;
                            txtKemasyarakatan.Text = m.BakatKemasyarakatan;
                            txtBakatLain.Text = m.BakatLainLain;

                            cboStatusHubOrtu.SelectedValue = m.StatusHubunganDenganOrtu;
                            cboStatusPernikahanOrtu.SelectedValue = m.StatusPernikahanOrtu;

                            txtNamaAyah.Text = m.NamaAyah;
                            txtTempatLahirAyah.Text = m.TempatLahirAyah;
                            txtTanggalLahirAyah.Text = (m.TanggalLahirAyah == DateTime.MinValue ? "" :
                                Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAyah, false));
                            cboAgamaAyah.SelectedValue = m.AgamaAyah;
                            txtSukuBangsaAyah.Text = m.SukuBangsaAyah;
                            txtWargaNegaraAyah.Text = m.WargaNegaraAyah;
                            cboPendidikanAyah.SelectedValue = m.PendidikanAyah;
                            txtPendidikanAyahLainnya.Text = m.PendidikanAyahLainnya;
                            txtJurusanPendidikanAyah.Text = m.JurusanPendidikanAyah;
                            txtAlamatAyah.Text = m.AlamatRumahAyah;
                            txtNIKAyah.Text = m.NIKAyah;
                            txtNoTelponAyah.Text = m.NoTelponAyah;
                            txtEmailAyah.Text = m.EmailAyah;
                            txtPekerjaanAyah.Text = m.PekerjaanAyah;
                            txtNamaInstansiAyah.Text = m.NamaInstansiAyah;
                            txtNoTelponKantorAyah.Text = m.NoTelponKantorAyah;
                            txtAlamatKantorAyah.Text = m.AlamatKantorAyah;

                            txtNamaIbu.Text = m.NamaIbu;
                            txtTempatLahirIbu.Text = m.TempatLahirIbu;
                            txtTanggalLahirIbu.Text = (m.TanggalLahirIbu == DateTime.MinValue ? "" :
                                Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirIbu, false));
                            cboAgamaIbu.SelectedValue = m.AgamaIbu;
                            txtSukuBangsaIbu.Text = m.SukuBangsaIbu;
                            txtWargaNegaraIbu.Text = m.WargaNegaraIbu;
                            cboPendidikanIbu.SelectedValue = m.PendidikanIbu;
                            txtPendidikanIbuLainnya.Text = m.PendidikanIbuLainnya;
                            txtJurusanPendidikanIbu.Text = m.JurusanPendidikanIbu;
                            txtAlamatIbu.Text = m.AlamatRumahIbu;
                            txtNIKIbu.Text = m.NIKIbu;
                            txtNoTelponIbu.Text = m.NoTelponIbu;
                            txtEmailIbu.Text = m.EmailIbu;
                            txtPekerjaanIbu.Text = m.PekerjaanIbu;
                            txtNamaInstansiIbu.Text = m.NamaInstansiIbu;
                            txtNoTelponKantorIbu.Text = m.NoTelponKantorIbu;
                            txtAlamatKantorIbu.Text = m.AlamatKantorIbu;

                            txtNamaKontakDarurat.Text = m.NamaKontakDarurat;
                            txtHubunganKontakDarurat.Text = m.HubunganKontakDarurat;
                            txtNoTelponHPKontakDarurat.Text = m.NoTelponKontakDarurat;
                            txtAlamatKontakDarurat.Text = m.AlamatKontakDarurat;

                            chkAyahKandung.Checked = m.IsTinggalDgAyahKandung;
                            chkIbuKandung.Checked = m.IsTinggalDgIbuKandung;
                            chkAyahTiri.Checked = m.IsTinggalDgAyahTiri;
                            chkIbuTiri.Checked = m.IsTinggalDgIbuTiri;
                            chkKakek.Checked = m.IsTinggalDgKakek;
                            chkNenek.Checked = m.IsTinggalDgNenek;
                            chkKakak.Checked = m.IsTinggalDgKakak;
                            chkAdik.Checked = m.IsTinggalDgAdik;
                            txtTinggalDenganLainnya.Text = m.TinggalDenganLainnya;

                            txtNamaIbu.Text = m.NamaIbu;
                            txtNoTelponIbu.Text = m.NoTelponAyah;
                            txtNamaAyah.Text = m.NamaAyah;
                            txtNoTelponAyah.Text = m.NoTelponAyah;

                            //list data saudara
                            List<SiswaSaudara> lst_saudara = DAO_Siswa.GetSaudaraByCalonSiswa(m.Kode.ToString());
                            foreach (SiswaSaudara saudara_siswa in lst_saudara)
                            {
                                switch (saudara_siswa.Urut)
                                {
                                    case 0:
                                        cboHubungan_1.SelectedValue = saudara_siswa.Hubungan;
                                        txtNamaLengkapSaudara_1.Text = saudara_siswa.Nama;
                                        cboJenisKelamin_1.SelectedValue = saudara_siswa.JenisKelamin;
                                        txtUmurSaudara_1.Text = saudara_siswa.Umur;
                                        txtNamaSekolahSaudara_1.Text = saudara_siswa.Sekolah;
                                        txtKeteranganSaudara_1.Text = saudara_siswa.Keterangan;
                                        break;
                                    case 1:
                                        cboHubungan_2.SelectedValue = saudara_siswa.Hubungan;
                                        txtNamaLengkapSaudara_2.Text = saudara_siswa.Nama;
                                        cboJenisKelamin_2.SelectedValue = saudara_siswa.JenisKelamin;
                                        txtUmurSaudara_2.Text = saudara_siswa.Umur;
                                        txtNamaSekolahSaudara_2.Text = saudara_siswa.Sekolah;
                                        txtKeteranganSaudara_2.Text = saudara_siswa.Keterangan;
                                        break;
                                    case 2:
                                        cboHubungan_3.SelectedValue = saudara_siswa.Hubungan;
                                        txtNamaLengkapSaudara_3.Text = saudara_siswa.Nama;
                                        cboJenisKelamin_3.SelectedValue = saudara_siswa.JenisKelamin;
                                        txtUmurSaudara_3.Text = saudara_siswa.Umur;
                                        txtNamaSekolahSaudara_3.Text = saudara_siswa.Sekolah;
                                        txtKeteranganSaudara_3.Text = saudara_siswa.Keterangan;
                                        break;
                                    case 3:
                                        cboHubungan_4.SelectedValue = saudara_siswa.Hubungan;
                                        txtNamaLengkapSaudara_4.Text = saudara_siswa.Nama;
                                        cboJenisKelamin_4.SelectedValue = saudara_siswa.JenisKelamin;
                                        txtUmurSaudara_4.Text = saudara_siswa.Umur;
                                        txtNamaSekolahSaudara_4.Text = saudara_siswa.Sekolah;
                                        txtKeteranganSaudara_4.Text = saudara_siswa.Keterangan;
                                        break;
                                    case 4:
                                        cboHubungan_5.SelectedValue = saudara_siswa.Hubungan;
                                        txtNamaLengkapSaudara_5.Text = saudara_siswa.Nama;
                                        cboJenisKelamin_5.SelectedValue = saudara_siswa.JenisKelamin;
                                        txtUmurSaudara_5.Text = saudara_siswa.Umur;
                                        txtNamaSekolahSaudara_5.Text = saudara_siswa.Sekolah;
                                        txtKeteranganSaudara_5.Text = saudara_siswa.Keterangan;
                                        break;
                                    case 5:
                                        cboHubungan_6.SelectedValue = saudara_siswa.Hubungan;
                                        txtNamaLengkapSaudara_6.Text = saudara_siswa.Nama;
                                        cboJenisKelamin_6.SelectedValue = saudara_siswa.JenisKelamin;
                                        txtUmurSaudara_6.Text = saudara_siswa.Umur;
                                        txtNamaSekolahSaudara_6.Text = saudara_siswa.Sekolah;
                                        txtKeteranganSaudara_6.Text = saudara_siswa.Keterangan;
                                        break;
                                }
                            }

                            div_survey.Visible = false;
                            //if (!(Libs.GetJenisSiswa(Libs.LOGGED_USER_SISWA_M) == Libs.JenisSiswa.Bergulir))
                            //{
                            //    chkFacebook.Checked = (m.SM_Facebook ? true : false);
                            //    chkInstagram.Checked = (m.SM_Instagram ? true : false);
                            //    chkTwitter.Checked = (m.SM_Twitter ? true : false);
                            //    chkWebsite.Checked = (m.SM_Website ? true : false);
                            //    chkYoutube.Checked = (m.SM_Youtube ? true : false);
                            //    chkFlyer.Checked = (m.MC_Flyer ? true : false);
                            //    chkSpanduk.Checked = (m.MC_Spanduk ? true : false);
                            //    chkBrosur.Checked = (m.MC_Brosur ? true : false);
                            //    chkBaliho.Checked = (m.MC_Baliho ? true : false);
                            //    chkAtlas.Checked = (m.KA_Atlas ? true : false);
                            //    chkCare.Checked = (m.KA_Care ? true : false);
                            //    chkSchoolVisit.Checked = (m.KA_SchoolVisit ? true : false);
                            //    chkAirGames.Checked = (m.KA_AirGames ? true : false);
                            //    chkAlizharCup.Checked = (m.KA_AlizharCup ? true : false);
                            //    chkProgramParenting.Checked = (m.KA_ProgramParenting ? true : false);
                            //    txtInfoAlizharLainnya.Text = m.MengetahuiDariLainnya;
                            //    div_survey.Visible = true;
                            //}
                        }
                    }
                }
            }
        }

        protected void ShowKelas(string rel_sekolah, string kode_kelas)
        {
            string kelas_rombel = cboKelasRombel.SelectedValue;
            string kelas_jurusan = cboKelasJurusan.SelectedValue;
            string kelas_sosialisasi = cboKelasSosialisasi.SelectedValue;

            div_kelaspeminatan.Visible = false;
            div_kelassosialisasi.Visible = false;
            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(rel_sekolah);
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                    {
                        ltrJudulKelasRombel.Text = "Kelas Perwalian";
                        cboKelasRombel.Items.Clear();
                        cboKelasRombel.Items.Add("");
                        foreach (var kelas in DAO_Kelas.GetAll_Entity().FindAll(m => m.Rel_Sekolah.ToString().ToUpper() == rel_sekolah.ToUpper() && m.IsAktif == true).OrderBy(m => m.UrutanLevel).ToList())
                        {
                            List<KelasDet> lst_kelas_det = DAO_KelasDet.GetAll_Entity().FindAll(m => m.Rel_Kelas == kelas.Kode && m.IsAktif == true && (m.IsKelasSosialisasi == false && m.IsKelasJurusan == false)).OrderBy(m => m.UrutanKelas).ToList();
                            foreach (var kelas_det in lst_kelas_det)
                            {
                                if (kelas_det.Nama.Trim() != "")
                                {
                                    cboKelasRombel.Items.Add(new ListItem
                                    {
                                        Value = kelas_det.Kode.ToString(),
                                        Text = kelas_det.Nama
                                    });
                                }
                            }
                        }

                        cboKelasJurusan.Items.Clear();
                        cboKelasJurusan.Items.Add("");
                        foreach (var kelas in DAO_Kelas.GetAll_Entity().FindAll(m => m.Rel_Sekolah.ToString().ToUpper() == rel_sekolah.ToUpper() && m.IsAktif == true).OrderBy(m => m.UrutanLevel).ToList())
                        {
                            foreach (var kelas_det_perwalian in DAO_KelasDet.GetAll_Entity().FindAll(m => m.Rel_Kelas == kelas.Kode && m.IsAktif == true && m.IsKelasJurusan == true).OrderBy(m => m.UrutanKelas).ToList())
                            {
                                if (kelas_det_perwalian.Nama.Trim() != "")
                                {
                                    cboKelasJurusan.Items.Add(new ListItem
                                    {
                                        Value = kelas_det_perwalian.Kode.ToString(),
                                        Text = kelas_det_perwalian.Nama
                                    });
                                }
                            }
                        }

                        cboKelasSosialisasi.Items.Clear();
                        cboKelasSosialisasi.Items.Add("");
                        foreach (var kelas in DAO_Kelas.GetAll_Entity().FindAll(m => m.Rel_Sekolah.ToString().ToUpper() == rel_sekolah.ToUpper() && m.IsAktif == true).OrderBy(m => m.UrutanLevel).ToList())
                        {
                            foreach (var kelas_det_perwalian in DAO_KelasDet.GetAll_Entity().FindAll(m => m.Rel_Kelas == kelas.Kode && m.IsAktif == true && m.IsKelasSosialisasi == true).OrderBy(m => m.UrutanKelas).ToList())
                            {
                                if (kelas_det_perwalian.Nama.Trim() != "")
                                {
                                    cboKelasSosialisasi.Items.Add(new ListItem
                                    {
                                        Value = kelas_det_perwalian.Kode.ToString(),
                                        Text = kelas_det_perwalian.Nama
                                    });
                                }
                            }
                        }

                        div_kelaspeminatan.Visible = true;
                        div_kelassosialisasi.Visible = true;
                    }
                    else
                    {
                        ltrJudulKelasRombel.Text = "Kelas / Rombel";
                        cboKelasRombel.Items.Clear();
                        cboKelasRombel.Items.Add("");
                        foreach (var kelas in DAO_Kelas.GetAll_Entity().FindAll(m => m.Rel_Sekolah.ToString().ToUpper() == rel_sekolah.ToUpper() && m.IsAktif == true).OrderBy(m => m.UrutanLevel).ToList())
                        {
                            foreach (var kelas_det in DAO_KelasDet.GetAll_Entity().FindAll(m => m.Rel_Kelas == kelas.Kode && m.IsAktif == true && m.IsKelasJurusan == false).OrderBy(m => m.UrutanKelas).ToList())
                            {
                                if (kelas_det.Nama.Trim() != "")
                                {
                                    cboKelasRombel.Items.Add(new ListItem
                                    {
                                        Value = kelas_det.Kode.ToString(),
                                        Text = kelas_det.Nama
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void InitFields()
        {
            ltrFotoSiswa.Text = "";

            txtNISSekolah.Text = "";
            txtNama.Text = "";
            txtPanggilan.Text = "";

            cboUnitSekolah.Items.Clear();
            cboDiterimaDiKelas.Items.Clear();
            cboUnitSekolah.Items.Add("");
            cboDiterimaDiKelas.Items.Add("");
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (var sekolah in lst_sekolah)
            {
                cboUnitSekolah.Items.Add(new ListItem {
                    Value = sekolah.Kode.ToString(),
                    Text = sekolah.Nama.ToString(),
                    Selected = (QS.GetUnit().ToUpper() == sekolah.Kode.ToString().ToUpper() ? true : false)
                });

                foreach (var kelas in DAO_Kelas.GetAll_Entity().FindAll(m => m.Rel_Sekolah == sekolah.Kode && m.IsAktif == true).OrderBy(m => m.UrutanLevel).ToList())
                {
                    cboDiterimaDiKelas.Items.Add(new ListItem
                        {
                            Value = kelas.Kode.ToString(),
                            Text = sekolah.Nama + " - " + kelas.Nama
                        });
                }
            }

            ShowKelas(cboUnitSekolah.SelectedValue, "");
            txtTahunPelajaran.Text = "";

            cboJenisKelamin.SelectedValue = "";
            txtEmail.Text = "";
            txtTempatLahir.Text = "";
            txtTanggalLahir.Text = "";
            txtNISLama.Text = "";

            cboAgama.SelectedValue = "";
            cboStatusAnak.SelectedValue = "";
            txtAnakKe.Text = "";
            txtJumlahSaudara.Text = "";
            txtJumlahKakak.Text = "";
            txtJumlahAdik.Text = "";
            txtNISN.Text = "";
            txtNIK.Text = "";
            txtWargaNegara.Text = "";
            txtBahasaSehariHari.Text = "";
            cboHobi.SelectedValue = "";
            txtHobiLainnya.Text = "";

            txtAlamat.Text = "";
            txtRT.Text = "";
            txtRW.Text = "";
            txtKelurahan.Text = "";
            txtKecamatan.Text = "";
            txtKabupatenKota.Text = "";
            txtProvinsi.Text = "";
            txtKodePOS.Text = "";
            cboStatusTempatTinggal.SelectedValue = "";
            cboJarakKeSekolah.SelectedValue = "";
            cboKeSekolahDengan.SelectedValue = "";
            txtWaktuTempuh.Text = "";

            txtSMA.Text = "";
            txtSMP.Text = "";
            txtSD.Text = "";
            txtTK.Text = "";

            txtKesenian.Text = "";
            txtOlahraga.Text = "";
            txtKemasyarakatan.Text = "";
            txtBakatLain.Text = "";

            cboStatusHubOrtu.SelectedValue = "";
            cboStatusPernikahanOrtu.SelectedValue = "";

            txtNamaAyah.Text = "";
            txtTempatLahirAyah.Text = "";
            txtTanggalLahirAyah.Text = "";
            cboAgamaAyah.SelectedValue = "";
            txtSukuBangsaAyah.Text = "";
            txtWargaNegaraAyah.Text = "";
            cboPendidikanAyah.SelectedValue = "";
            txtPendidikanAyahLainnya.Text = "";
            txtJurusanPendidikanAyah.Text = "";
            txtAlamatAyah.Text = "";
            txtNIKAyah.Text = "";
            txtNoTelponAyah.Text = "";
            txtEmailAyah.Text = "";
            txtPekerjaanAyah.Text = "";
            txtNamaInstansiAyah.Text = "";
            txtNoTelponKantorAyah.Text = "";
            txtAlamatKantorAyah.Text = "";

            txtNamaIbu.Text = "";
            txtTempatLahirIbu.Text = "";
            txtTanggalLahirIbu.Text = "";
            cboAgamaIbu.SelectedValue = "";
            txtSukuBangsaIbu.Text = "";
            txtWargaNegaraIbu.Text = "";
            cboPendidikanIbu.SelectedValue = "";
            txtPendidikanIbuLainnya.Text = "";
            txtJurusanPendidikanIbu.Text = "";
            txtAlamatIbu.Text = "";
            txtNIKIbu.Text = "";
            txtNoTelponIbu.Text = "";
            txtEmailIbu.Text = "";
            txtPekerjaanIbu.Text = "";
            txtNamaInstansiIbu.Text = "";
            txtNoTelponKantorIbu.Text = "";
            txtAlamatKantorIbu.Text = "";

            txtNamaKontakDarurat.Text = "";
            txtHubunganKontakDarurat.Text = "";
            txtNoTelponHPKontakDarurat.Text = "";
            txtAlamatKontakDarurat.Text = "";

            chkAyahKandung.Checked = false;
            chkIbuKandung.Checked = false;
            chkAyahTiri.Checked = false;
            chkIbuTiri.Checked = false;
            chkKakek.Checked = false;
            chkNenek.Checked = false;
            chkKakak.Checked = false;
            chkAdik.Checked = false;
            txtTinggalDenganLainnya.Text = "";

            txtNamaIbu.Text = "";
            txtNoTelponIbu.Text = "";
            txtNamaAyah.Text = "";
            txtNoTelponAyah.Text = "";

            Libs.ListAsalSekolahToDropDown(cboAsalSekolah, false, true);
        }

        protected void btnBackToMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE +
                            QS.GetURLVariable()
                        )
                );
        }

        protected void DoSave()
        {
            try
            {
                //siswa
                Siswa m = DAO_Siswa.GetByKode_Entity(
                    Libs.GetTahunAjaranNow(),
                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                    QS.GetSiswa());
                if (m != null)
                {
                    if (m.NoSeleksi != null)
                    {
                        m.Nama = txtNama.Text;
                        m.Panggilan = txtPanggilan.Text;
                        m.NISSekolah = txtNISSekolah.Text;
                        m.Agama = cboAgama.SelectedValue;
                        m.StatusAnak = cboStatusAnak.SelectedValue;
                        m.AnakKe = txtAnakKe.Text;
                        m.DariBersaudara = txtJumlahSaudara.Text;
                        m.JumlahKakak = txtJumlahKakak.Text;
                        m.JumlahAdik = txtJumlahAdik.Text;
                        m.NISN = txtNISN.Text;
                        m.NIK = txtNIK.Text;
                        m.WargaNegara = txtWargaNegara.Text;
                        m.BahasaSehariHari = txtBahasaSehariHari.Text;
                        m.Hobi = cboHobi.SelectedValue;
                        m.HobiLainnya = txtHobiLainnya.Text;
                        m.Email = txtEmail.Text;

                        m.TahunAjaran = txtTahunPelajaran.Text;
                        m.Rel_Sekolah = cboUnitSekolah.SelectedValue;
                        m.Rel_KelasDet = cboKelasRombel.SelectedValue;
                        m.Rel_KelasDetJurusan = cboKelasJurusan.SelectedValue;
                        m.Rel_KelasDetSosialisasi = cboKelasSosialisasi.SelectedValue;

                        m.Alamat = txtAlamat.Text;
                        m.RT = txtRT.Text;
                        m.RW = txtRW.Text;
                        m.Kelurahan = txtKelurahan.Text;
                        m.Kecamatan = txtKecamatan.Text;
                        m.Kabupaten = txtKabupatenKota.Text;
                        m.Provinsi = txtProvinsi.Text;
                        m.KodePOS = txtKodePOS.Text;
                        m.StatusTempatTinggal = cboStatusTempatTinggal.SelectedValue;

                        m.JarakKeSekolah = cboJarakKeSekolah.SelectedValue;
                        m.KeSekolahDengan = cboKeSekolahDengan.SelectedValue;
                        m.WaktuTempuh = txtWaktuTempuh.Text;

                        m.AsalSMA = txtSMA.Text;
                        m.AsalSMP = txtSMP.Text;
                        m.AsalSD = txtSD.Text;
                        m.AsalTK = txtTK.Text;
                        m.AsalKB = "";

                        m.BakatKesenian = txtKesenian.Text;
                        m.BakatOlahRaga = txtOlahraga.Text;
                        m.BakatKemasyarakatan = txtKemasyarakatan.Text;
                        m.BakatLainLain = txtBakatLain.Text;

                        m.StatusHubunganDenganOrtu = cboStatusHubOrtu.SelectedValue;
                        m.StatusPernikahanOrtu = cboStatusPernikahanOrtu.SelectedValue;
                        m.SiswaTinggalDengan = "";

                        m.NamaAyah = txtNamaAyah.Text;
                        m.TempatLahirAyah = txtTempatLahirAyah.Text;
                        m.TanggalLahirAyah = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAyah.Text);
                        m.AgamaAyah = cboAgamaAyah.SelectedValue;
                        m.SukuBangsaAyah = txtSukuBangsaAyah.Text;
                        m.WargaNegaraAyah = txtWargaNegaraAyah.Text;
                        m.PendidikanAyah = cboPendidikanAyah.SelectedValue;
                        m.PendidikanAyahLainnya = txtPendidikanAyahLainnya.Text;
                        m.JurusanPendidikanAyah = txtJurusanPendidikanAyah.Text;
                        m.AlamatRumahAyah = txtAlamatAyah.Text;
                        m.NIKAyah = txtNIKAyah.Text;
                        m.NoTelponAyah = txtNoTelponAyah.Text;
                        m.EmailAyah = txtEmailAyah.Text;
                        m.PekerjaanAyah = txtPekerjaanAyah.Text;
                        m.NamaInstansiAyah = txtNamaInstansiAyah.Text;
                        m.NoTelponKantorAyah = txtNoTelponKantorAyah.Text;
                        m.AlamatKantorAyah = txtAlamatKantorAyah.Text;

                        m.NamaIbu = txtNamaIbu.Text;
                        m.TempatLahirIbu = txtTempatLahirIbu.Text;
                        m.TanggalLahirIbu = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirIbu.Text);
                        m.AgamaIbu = cboAgamaIbu.SelectedValue;
                        m.SukuBangsaIbu = txtSukuBangsaIbu.Text;
                        m.WargaNegaraIbu = txtWargaNegaraIbu.Text;
                        m.PendidikanIbu = cboPendidikanIbu.SelectedValue;
                        m.PendidikanIbuLainnya = txtPendidikanIbuLainnya.Text;
                        m.JurusanPendidikanIbu = txtJurusanPendidikanIbu.Text;
                        m.AlamatRumahIbu = txtAlamatIbu.Text;
                        m.NIKIbu = txtNIKIbu.Text;
                        m.NoTelponIbu = txtNoTelponIbu.Text;
                        m.EmailIbu = txtEmailIbu.Text;
                        m.PekerjaanIbu = txtPekerjaanIbu.Text;
                        m.NamaInstansiIbu = txtNamaInstansiIbu.Text;
                        m.NoTelponKantorIbu = txtNoTelponKantorIbu.Text;
                        m.AlamatKantorIbu = txtAlamatKantorIbu.Text;
                        m.NamaKontakDarurat = txtNamaKontakDarurat.Text;
                        m.HubunganKontakDarurat = txtHubunganKontakDarurat.Text;
                        m.NoTelponKontakDarurat = txtNoTelponHPKontakDarurat.Text;
                        m.AlamatKontakDarurat = txtAlamatKontakDarurat.Text;

                        m.IsTinggalDgAyahKandung = chkAyahKandung.Checked;
                        m.IsTinggalDgIbuKandung = chkIbuKandung.Checked;
                        m.IsTinggalDgAyahTiri = chkAyahTiri.Checked;
                        m.IsTinggalDgIbuTiri = chkIbuTiri.Checked;
                        m.IsTinggalDgKakek = chkKakek.Checked;
                        m.IsTinggalDgNenek = chkNenek.Checked;
                        m.IsTinggalDgKakak = chkKakak.Checked;
                        m.IsTinggalDgAdik = chkAdik.Checked;
                        m.TinggalDenganLainnya = txtTinggalDenganLainnya.Text;

                        //data saudara
                        List<SiswaSaudara> lst_saudara = new List<SiswaSaudara>();
                        lst_saudara.Clear();
                        lst_saudara.Add(new SiswaSaudara
                        {
                            Rel_Siswa = m.Kode,
                            Urut = 1,
                            Hubungan = cboHubungan_1.SelectedValue,
                            Nama = txtNamaLengkapSaudara_1.Text,
                            JenisKelamin = cboJenisKelamin_1.SelectedValue,
                            Umur = txtUmurSaudara_1.Text,
                            Sekolah = txtNamaSekolahSaudara_1.Text,
                            IsSaudaraKandung = true,
                            Keterangan = txtKeteranganSaudara_1.Text,
                            KeteranganLain = ""
                        });
                        lst_saudara.Add(new SiswaSaudara
                        {
                            Rel_Siswa = m.Kode,
                            Urut = 2,
                            Hubungan = cboHubungan_2.SelectedValue,
                            Nama = txtNamaLengkapSaudara_2.Text,
                            JenisKelamin = cboJenisKelamin_2.SelectedValue,
                            Umur = txtUmurSaudara_2.Text,
                            Sekolah = txtNamaSekolahSaudara_2.Text,
                            IsSaudaraKandung = true,
                            Keterangan = txtKeteranganSaudara_2.Text,
                            KeteranganLain = ""
                        });
                        lst_saudara.Add(new SiswaSaudara
                        {
                            Rel_Siswa = m.Kode,
                            Urut = 3,
                            Hubungan = cboHubungan_3.SelectedValue,
                            Nama = txtNamaLengkapSaudara_3.Text,
                            JenisKelamin = cboJenisKelamin_3.SelectedValue,
                            Umur = txtUmurSaudara_3.Text,
                            Sekolah = txtNamaSekolahSaudara_3.Text,
                            IsSaudaraKandung = true,
                            Keterangan = txtKeteranganSaudara_3.Text,
                            KeteranganLain = ""
                        });
                        lst_saudara.Add(new SiswaSaudara
                        {
                            Rel_Siswa = m.Kode,
                            Urut = 4,
                            Hubungan = cboHubungan_4.SelectedValue,
                            Nama = txtNamaLengkapSaudara_4.Text,
                            JenisKelamin = cboJenisKelamin_4.SelectedValue,
                            Umur = txtUmurSaudara_4.Text,
                            Sekolah = txtNamaSekolahSaudara_4.Text,
                            IsSaudaraKandung = true,
                            Keterangan = txtKeteranganSaudara_4.Text,
                            KeteranganLain = ""
                        });
                        lst_saudara.Add(new SiswaSaudara
                        {
                            Rel_Siswa = m.Kode,
                            Urut = 5,
                            Hubungan = cboHubungan_5.SelectedValue,
                            Nama = txtNamaLengkapSaudara_5.Text,
                            JenisKelamin = cboJenisKelamin_5.SelectedValue,
                            Umur = txtUmurSaudara_5.Text,
                            Sekolah = txtNamaSekolahSaudara_5.Text,
                            IsSaudaraKandung = true,
                            Keterangan = txtKeteranganSaudara_5.Text,
                            KeteranganLain = ""
                        });
                        lst_saudara.Add(new SiswaSaudara
                        {
                            Rel_Siswa = m.Kode,
                            Urut = 6,
                            Hubungan = cboHubungan_6.SelectedValue,
                            Nama = txtNamaLengkapSaudara_6.Text,
                            JenisKelamin = cboJenisKelamin_6.SelectedValue,
                            Umur = txtUmurSaudara_6.Text,
                            Sekolah = txtNamaSekolahSaudara_6.Text,
                            IsSaudaraKandung = true,
                            Keterangan = txtKeteranganSaudara_6.Text,
                            KeteranganLain = ""
                        });

                        //survey
                        //m.SM_Facebook = chkFacebook.Checked;
                        //m.SM_Instagram = chkInstagram.Checked;
                        //m.SM_Twitter = chkTwitter.Checked;
                        //m.SM_Website = chkWebsite.Checked;
                        //m.SM_Youtube = chkYoutube.Checked;
                        //m.MC_Flyer = chkFlyer.Checked;
                        //m.MC_Spanduk = chkSpanduk.Checked;
                        //m.MC_Brosur = chkBrosur.Checked;
                        //m.MC_Baliho = chkBaliho.Checked;
                        //m.KA_Atlas = chkAtlas.Checked;
                        //m.KA_Care = chkCare.Checked;
                        //m.KA_SchoolVisit = chkSchoolVisit.Checked;
                        //m.KA_AirGames = chkAirGames.Checked;
                        //m.KA_AlizharCup = chkAlizharCup.Checked;
                        //m.KA_ProgramParenting = chkProgramParenting.Checked;
                        //m.MengetahuiDariLainnya = txtInfoAlizharLainnya.Text;
                        //end survey

                        DAO_Siswa.Update(m, lst_saudara);
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnSaveData_Click(object sender, EventArgs e)
        {
            DoSave();
        }
        
        protected void cboUnitSekolah_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowKelas(cboUnitSekolah.SelectedValue, "");
        }
    }
}