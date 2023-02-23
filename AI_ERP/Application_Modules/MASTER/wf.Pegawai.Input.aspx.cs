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
    public partial class wf_Pegawai_Input : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEINPUTPEGAWAI";
        public const string C_ID = "{{id}}";

        public static List<PegawaiPendidikan> lst_pendidikan = new List<PegawaiPendidikan>();
        public static List<PegawaiPendidikanNonFormal> lst_pendidikan_non_formal = new List<PegawaiPendidikanNonFormal>();
        public static List<PegawaiPengalamanKerjaDalam> lst_pengalaman_kerja_dalam = new List<PegawaiPengalamanKerjaDalam>();
        public static List<PegawaiPengalamanKerjaLuar> lst_pengalaman_kerja_luar = new List<PegawaiPengalamanKerjaLuar>();
        public static List<PegawaiPengalamanSharing> lst_pengalaman_sharing = new List<PegawaiPengalamanSharing>();
        public static List<PegawaiPengalamanKepanitiaan> lst_pengalaman_kepanitiaan = new List<PegawaiPengalamanKepanitiaan>();
        public static List<PegawaiRiwayatKesehatan> lst_pengalaman_riwayat_kesehatan = new List<PegawaiRiwayatKesehatan>();
        public static List<PegawaiRiwayatKesehatanMCU> lst_pengalaman_riwayat_mcu = new List<PegawaiRiwayatKesehatanMCU>();

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
            DoShowDataPendidikanFormal,
            DoShowDataPendidikanNonFormal,
            DoShowDataPengalamanKerjaDalam,
            DoShowDataPengalamanKerjaLuar,
            DoShowDataPengalamanSharing,
            DoShowDataPengalamanKepanitiaan,
            DoShowDataRiwayatKesehatan,
            DoShowDataRiwayatMCU,
            DoShowDataRiwayatKesehatanAndDoDeleteDirectory,
            DoShowDataRiwayatMCUAndDoDeleteDirectory,
            DoShowDataPendidikanNonFormalAndDoDeleteDirectory,
            DoShowDataFilePendukung,
            DoShowDataListFilePendukung,
            DoShowDataListFilePendukungWithNotifDelete,
            DoShowConfirmHapus,
            DoShowConfirmHapusPendidikan,
            DoShowConfirmHapusPendidikanNonFormal,
            DoShowConfirmHapusPengalamanKerjaDalam,
            DoShowConfirmHapusPengalamanKerjaLuar,
            DoShowConfirmHapusPengalamanSharing,
            DoShowConfirmHapusPengalamanKepanitiaan,
            DoShowConfirmHapusRiwayatKesehatan,
            DoShowConfirmHapusRiwayatMCU,
            DoShowInputPendidikan,
            DoShowInputPendidikanNonFormal,
            DoShowInputPengalamanKerjaDalam,
            DoShowInputPengalamanKerjaLuar,
            DoShowInputPengalamanSharing,
            DoShowInputPengalamanKepanitiaan,
            DoShowInputRiwayatKesehatan,
            DoShowInputRiwayatMCU,
            DoShowInputUploadFilePendukung,
            ItemKelasDetKosong
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowHeaderSubTitle = false;
            this.Master.HeaderTittle = "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/student-3.svg") + "\" " +
                                            "style=\"margin: 0 auto; height: 40px; width: 40px; margin-top: -10px; margin-right: 5px; float: left;\" />" +
                                       "&nbsp;&nbsp;" +
                                       "Pegawai";
            this.Master.ShowHeaderTools = false;

            if (!IsPostBack)
            {
                this.Master.txtCariData.Text = Libs.GetQ();
                this.Master.Attributes["onclose"] = "DoDeleteDirectoryNewPendidikanNonFormalCancel();";
                txtNIKaryawan.Attributes["disabled"] = "disabled";
                txtNIKaryawan.Attributes["style"] = "background-color: white;";
                InitFields();
                ShowData();
            }
            RenderListviewDetail();
        }

        private static class QS
        {
            public static string GetUnit()
            {
                return Libs.GetQueryString("unit");
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

            public static string GetURLVariable()
            {
                string s_url_var = "";
                s_url_var += (QS.GetUnit().Trim() != "" ? "unit=" + QS.GetUnit().Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();
                s_url_var += (s_url_var.Trim() != "" && Libs.GetQueryString("q").Trim() != "" ? "&" : "") + (Libs.GetQueryString("q").Trim() != "" ? "q=" : "") + Libs.GetQueryString("q").Trim();

                return (QS.GetUnit().Trim() != "" || QS.GetToken().Trim() != "" || Libs.GetQueryString("q").Trim() != "" ? "?" : "") +
                        s_url_var;
            }
        }

        protected void ShowData()
        {
            div_list_data_pegawai.Visible = true;
            string p = Libs.GetQueryString("p");
            if (Libs.GetQueryString("act") == RandomLibs.RND.USER)
            {
                p = Libs.Decryptdata(Libs.GetQueryString("p"));
                div_list_data_pegawai.Visible = false;
            }
            Pegawai m = DAO_Pegawai.GetByID_Entity(p);
            if (m != null)
            {
                if (m.Nama != null)
                {
                    ltrFotoPegawai.Text = "<img " +
                                            "src=\"" +
                                                    ResolveUrl("~/Application_Controls/Res/ImageViewer.aspx?ID=" + m.Kode.ToString() + "&Jenis=Pegawai" + "&Time=" + DateTime.Now.ToString()) +
                                                 "\" " +
                                            "style=\"margin: 0 auto; height: 100px; width: 100px; border-radius: 100%; margin-left: 7px; margin-top: 5px; margin-bottom: 5px;\" />";

                    lblTanggalMasuk.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalMasuk, false);
                    txtNIKaryawan.Text = m.Kode;
                    txtNama.Text = m.Nama;
                    txtTempatLahir.Text = m.TempatLahir;
                    txtTanggalLahir.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahir, false);
                    cboJenisKelamin.SelectedValue = m.JenisKelamin;
                    cboAgama.SelectedValue = m.Agama;
                    cboTempatTinggal.SelectedValue = m.TempatTinggal;
                    txtAlamat.Text = m.AlamatRumah;
                    txtKota.Text = m.Kota;
                    txtKodePOS.Text = m.KodePOS;
                    txtTelpon.Text = m.Telpon;
                    txtEmail.Text = m.Email;
                    txtEmailPribadi.Text = m.EmailPribadi;
                    txtNoKTP.Text = m.NoKTP;
                    txtNoKK.Text = m.NoKK;
                    cboStatusPerkawinan.SelectedValue = m.StatusKeluarga;

                    txtNamaAyah.Text = m.NamaAyah;
                    txtTempatLahirAyah.Text = m.TempatLahirAyah;
                    if (m.TanggalLahirAyah == DateTime.MinValue)
                    {
                        txtTanggalLahirAyah.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAyah.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAyah, false);
                    }
                    txtNIKAyah.Text = m.NIKAyah;
                    txtNoBPJSKesehatanAyah.Text = m.NoBPJSKesAyah;

                    txtNamaIbu.Text = m.NamaIbu;
                    txtTempatLahirIbu.Text = m.TempatLahirIbu;
                    if (m.TanggalLahirIbu == DateTime.MinValue)
                    {
                        txtTanggalLahirIbu.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirIbu.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirIbu, false);
                    }
                    txtNIKIbu.Text = m.NIKIbu;
                    txtNoBPJSKesehatanIbu.Text = m.NoBPJSKesIbu;

                    txtNamaAyahMertua.Text = m.NamaAyahMertua;
                    txtTempatLahirAyahMertua.Text = m.TempatLahirAyahMertua;
                    if (m.TanggalLahirAyahMertua == DateTime.MinValue)
                    {
                        txtTanggalLahirAyahMertua.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAyahMertua.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAyahMertua, false);
                    }
                    txtNIKAyahMertua.Text = m.NIKAyahMertua;
                    txtNoBPJSKesehatanAyahMertua.Text = m.NoBPJSKesAyahMertua;

                    txtNamaIbuMertua.Text = m.NamaIbuMertua;
                    txtTempatLahirIbuMertua.Text = m.TempatLahirIbuMertua;
                    if (m.TanggalLahirIbuMertua == DateTime.MinValue)
                    {
                        txtTanggalLahirIbuMertua.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirIbuMertua.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirIbuMertua, false);
                    }                    
                    txtNIKIbuMertua.Text = m.NIKIbuMertua;
                    txtNoBPJSKesehatanIbuMertua.Text = m.NoBPJSKesIbuMertua;

                    txtNamaSuamiIstri.Text = m.NamaSuamiIstri;
                    txtTempatLahirSuamiIstri.Text = m.TempatLahirSuamiIstri;
                    if (m.TanggalLahirSuamiIstri == DateTime.MinValue)
                    {
                        txtTanggalLahirSuamiIstri.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirSuamiIstri.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirSuamiIstri, false);
                    }                    
                    txtNIKSuamiIstri.Text = m.NIKSuamiIstri;
                    txtNoBPJSKesehatanSuamiIstri.Text = m.NoBPJSKesSuamiIstri;

                    txtNamaAnakKe1.Text = m.NamaAnakKe1;
                    txtTempatLahirAnakKe1.Text = m.TempatLahirAnakKe1;
                    if (m.TanggalLahirAnakKe1 == DateTime.MinValue)
                    {
                        txtTanggalLahirAnakKe1.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAnakKe1.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAnakKe1, false);
                    }
                    txtNIKAnakKe1.Text = m.NIKAnakKe1;
                    txtNoBPJSKesehatanAnakKe1.Text = m.NoBPJSKesAnakKe1;

                    txtNamaAnakKe2.Text = m.NamaAnakKe2;
                    txtTempatLahirAnakKe2.Text = m.TempatLahirAnakKe2;
                    if (m.TanggalLahirAnakKe2 == DateTime.MinValue)
                    {
                        txtTanggalLahirAnakKe2.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAnakKe2.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAnakKe2, false);
                    }
                    txtNIKAnakKe2.Text = m.NIKAnakKe2;
                    txtNoBPJSKesehatanAnakKe2.Text = m.NoBPJSKesAnakKe2;


                    txtNamaAnakKe3.Text = m.NamaAnakKe3;
                    txtTempatLahirAnakKe3.Text = m.TempatLahirAnakKe3;
                    if (m.TanggalLahirAnakKe3 == DateTime.MinValue)
                    {
                        txtTanggalLahirAnakKe3.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAnakKe3.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAnakKe3, false);
                    }
                    txtNIKAnakKe3.Text = m.NIKAnakKe3;
                    txtNoBPJSKesehatanAnakKe3.Text = m.NoBPJSKesAnakKe3;

                    txtNamaAnakKe4.Text = m.NamaAnakKe4;
                    txtTempatLahirAnakKe4.Text = m.TempatLahirAnakKe4;
                    if (m.TanggalLahirAnakKe4 == DateTime.MinValue)
                    {
                        txtTanggalLahirAnakKe4.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAnakKe4.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAnakKe4, false);
                    }
                    txtNIKAnakKe4.Text = m.NIKAnakKe4;
                    txtNoBPJSKesehatanAnakKe4.Text = m.NoBPJSKesAnakKe4;

                    txtNamaAnakKe5.Text = m.NamaAnakKe5;
                    txtTempatLahirAnakKe5.Text = m.TempatLahirAnakKe5;
                    if (m.TanggalLahirAnakKe5 == DateTime.MinValue)
                    {
                        txtTanggalLahirAnakKe5.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAnakKe5.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAnakKe5, false);
                    }
                    txtNIKAnakKe5.Text = m.NIKAnakKe5;
                    txtNoBPJSKesehatanAnakKe5.Text = m.NoBPJSKesAnakKe5;

                    txtNamaAnakKe6.Text = m.NamaAnakKe6;
                    txtTempatLahirAnakKe6.Text = m.TempatLahirAnakKe6;
                    if (m.TanggalLahirAnakKe6 == DateTime.MinValue)
                    {
                        txtTanggalLahirAnakKe6.Text = "";
                    }
                    else
                    {
                        txtTanggalLahirAnakKe6.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalLahirAnakKe6, false);
                    }
                    txtNIKAnakKe6.Text = m.NIKAnakKe6;
                    txtNoBPJSKesehatanAnakKe6.Text = m.NoBPJSKesAnakKe6;

                    ShowDataDetail();
                    ShowUploadedFilesPendukung();

                    RenderListviewDetail();
                }
            }
        }

        protected void ShowDataDetail()
        {
            //pendidikan formal
            lst_pendidikan.Clear();
            lst_pendidikan = DAO_PegawaiPendidikan.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end pendidikan formal

            //pendidikan non formal
            lst_pendidikan_non_formal.Clear();
            lst_pendidikan_non_formal = DAO_PegawaiPendidikanNonFormal.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end pendidikan non formal

            //pengalaman kerja dalam perusahaan
            lst_pengalaman_kerja_dalam.Clear();
            lst_pengalaman_kerja_dalam = DAO_PegawaiPengalamanKerjaDalam.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end pengalaman kerja dalam perusahaan

            //pengalaman kerja luar perusahaan
            lst_pengalaman_kerja_luar.Clear();
            lst_pengalaman_kerja_luar = DAO_PegawaiPengalamanKerjaLuar.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end pengalaman kerja luar perusahaan

            //pengalaman sharing
            lst_pengalaman_sharing.Clear();
            lst_pengalaman_sharing = DAO_PegawaiPengalamanSharing.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end pengalaman sharing

            //pengalaman kepanitiaan
            lst_pengalaman_kepanitiaan.Clear();
            lst_pengalaman_kepanitiaan = DAO_PegawaiPengalamanKepanitiaan.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end pengalaman kepanitiaan

            //riwayat kesehatan
            lst_pengalaman_riwayat_kesehatan.Clear();
            lst_pengalaman_riwayat_kesehatan = DAO_PegawaiRiwayatKesehatan.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end riwayat kesehatan

            //riwayat kesehatan mcu
            lst_pengalaman_riwayat_mcu.Clear();
            lst_pengalaman_riwayat_mcu = DAO_PegawaiRiwayatKesehatanMCU.GetAllByHeader_Entity(txtNIKaryawan.Text);
            //end riwayat kesehatan mcu
        }

        protected void InitFields()
        {
            ltrFotoPegawai.Text = "";
            lblTanggalMasuk.Text = "";
            txtNIKaryawan.Text = "";
            txtNama.Text = "";
            txtTempatLahir.Text = "";
            txtTanggalLahir.Text = "";
            cboJenisKelamin.SelectedValue = "";
            cboAgama.SelectedValue = "";
            cboTempatTinggal.SelectedValue = "";
            txtAlamat.Text = "";
            txtKota.Text = "";
            txtKodePOS.Text = "";
            txtTelpon.Text = "";
            txtEmail.Text = "";
            txtEmailPribadi.Text = "";
            txtNoKTP.Text = "";
            txtNoKK.Text = "";
            cboStatusPerkawinan.SelectedValue = "";

            txtNamaAyah.Text = "";
            txtTempatLahirAyah.Text = "";
            txtTanggalLahirAyah.Text = "";
            txtNIKAyah.Text = "";
            txtNoBPJSKesehatanAyah.Text = "";

            txtNamaIbu.Text = "";
            txtTempatLahirIbu.Text = "";
            txtTanggalLahirIbu.Text = "";
            txtNIKIbu.Text = "";
            txtNoBPJSKesehatanIbu.Text = "";

            txtNamaAyahMertua.Text = "";
            txtTempatLahirAyahMertua.Text = "";
            txtTanggalLahirAyahMertua.Text = "";
            txtNIKAyahMertua.Text = "";
            txtNoBPJSKesehatanAyahMertua.Text = "";

            txtNamaIbuMertua.Text = "";
            txtTempatLahirIbuMertua.Text = "";
            txtTanggalLahirIbuMertua.Text = "";
            txtNIKIbuMertua.Text = "";
            txtNoBPJSKesehatanIbuMertua.Text = "";

            txtNamaSuamiIstri.Text = "";
            txtTempatLahirSuamiIstri.Text = "";
            txtTanggalLahirSuamiIstri.Text = "";
            txtNIKSuamiIstri.Text = "";
            txtNoBPJSKesehatanSuamiIstri.Text = "";

            txtNamaAnakKe1.Text = "";
            txtTempatLahirAnakKe1.Text = "";
            txtTanggalLahirAnakKe1.Text = "";
            txtNIKAnakKe1.Text = "";
            txtNoBPJSKesehatanAnakKe1.Text = "";

            txtNamaAnakKe2.Text = "";
            txtTempatLahirAnakKe2.Text = "";
            txtTanggalLahirAnakKe2.Text = "";
            txtNIKAnakKe2.Text = "";
            txtNoBPJSKesehatanAnakKe2.Text = "";

            txtNamaAnakKe3.Text = "";
            txtTempatLahirAnakKe3.Text = "";
            txtTanggalLahirAnakKe3.Text = "";
            txtNIKAnakKe3.Text = "";
            txtNoBPJSKesehatanAnakKe3.Text = "";

            txtNamaAnakKe4.Text = "";
            txtTempatLahirAnakKe4.Text = "";
            txtTanggalLahirAnakKe4.Text = "";
            txtNIKAnakKe4.Text = "";
            txtNoBPJSKesehatanAnakKe4.Text = "";

            txtNamaAnakKe5.Text = "";
            txtTempatLahirAnakKe5.Text = "";
            txtTanggalLahirAnakKe5.Text = "";
            txtNIKAnakKe5.Text = "";
            txtNoBPJSKesehatanAnakKe5.Text = "";

            txtNamaAnakKe6.Text = "";
            txtTempatLahirAnakKe6.Text = "";
            txtTanggalLahirAnakKe6.Text = "";
            txtNIKAnakKe6.Text = "";
            txtNoBPJSKesehatanAnakKe6.Text = "";

            RenderListviewDetail();
        }

        protected void RenderListviewDetail()
        {
            if (lst_pendidikan.FindAll(m => m.JenisPendidikan.Trim() == "").Count == 0 && lst_pendidikan.Count > 0)
            {
                lst_pendidikan.Add(new PegawaiPendidikan
                {
                    Kode = Guid.NewGuid(),
                    JenisPendidikan = "",
                    DariTahun = "99999",
                    Urutan = 9999
                });
            }
            lvDataPendidikan.DataSource = Libs.ToDataTable(lst_pendidikan.OrderBy(m => m.Urutan).ThenBy(m => m.DariTahun).ToList());
            lvDataPendidikan.DataBind();

            if (lst_pendidikan_non_formal.FindAll(m => m.JenisPendidikan.Trim() == "").Count == 0 && lst_pendidikan_non_formal.Count > 0)
            {
                lst_pendidikan_non_formal.Add(new PegawaiPendidikanNonFormal
                {
                    Kode = Guid.NewGuid(),
                    JenisPendidikan = "",
                    DariTahun = "99999",
                    Urutan = 9999
                });
            }
            lvDataPendidikanNonFormal.DataSource = Libs.ToDataTable(lst_pendidikan_non_formal.OrderBy(m => m.Urutan).ToList());
            lvDataPendidikanNonFormal.DataBind();

            if (lst_pengalaman_kerja_dalam.FindAll(m => m.Rel_Divisi.Trim() == "").Count == 0 && lst_pengalaman_kerja_dalam.Count > 0)
            {
                lst_pengalaman_kerja_dalam.Add(new PegawaiPengalamanKerjaDalam
                {
                    Kode = Guid.NewGuid(),
                    Rel_Divisi = "",
                    Dari = "99999",
                    Urutan = 9999
                });
            }
            lvDataPengalamanDalam.DataSource = Libs.ToDataTable(lst_pengalaman_kerja_dalam.OrderBy(m => m.Urutan).ToList());
            lvDataPengalamanDalam.DataBind();

            if (lst_pengalaman_kerja_luar.FindAll(m => m.NamaPerusahaan.Trim() == "").Count == 0 && lst_pengalaman_kerja_luar.Count > 0)
            {
                lst_pengalaman_kerja_luar.Add(new PegawaiPengalamanKerjaLuar
                {
                    Kode = Guid.NewGuid(),
                    NamaPerusahaan = "",
                    Dari = "99999",
                    Urutan = 9999
                });
            }
            lvDataPengalamanLuar.DataSource = Libs.ToDataTable(lst_pengalaman_kerja_luar.OrderBy(m => m.Urutan).ToList());
            lvDataPengalamanLuar.DataBind();

            if (lst_pengalaman_sharing.FindAll(m => m.Tahun.Trim() == "").Count == 0 && lst_pengalaman_sharing.Count > 0)
            {
                lst_pengalaman_sharing.Add(new PegawaiPengalamanSharing
                {
                    Kode = Guid.NewGuid(),
                    Tahun = "",
                    Urutan = 9999
                });
            }
            lvPegawaiPengalamanSharing.DataSource = Libs.ToDataTable(lst_pengalaman_sharing.OrderBy(m => m.Urutan).ToList());
            lvPegawaiPengalamanSharing.DataBind();

            if (lst_pengalaman_kepanitiaan.FindAll(m => m.Tahun.Trim() == "").Count == 0 && lst_pengalaman_kepanitiaan.Count > 0)
            {
                lst_pengalaman_kepanitiaan.Add(new PegawaiPengalamanKepanitiaan
                {
                    Kode = Guid.NewGuid(),
                    Tahun = "",
                    Urutan = 9999
                });
            }
            lvPegawaiPengalamanKepanitian.DataSource = Libs.ToDataTable(lst_pengalaman_kepanitiaan.OrderBy(m => m.Urutan).ToList());
            lvPegawaiPengalamanKepanitian.DataBind();

            if (lst_pengalaman_riwayat_kesehatan.FindAll(m => m.DariTanggal == DateTime.MaxValue).Count == 0 && lst_pengalaman_riwayat_kesehatan.Count > 0)
            {
                lst_pengalaman_riwayat_kesehatan.Add(new PegawaiRiwayatKesehatan
                {
                    Kode = Guid.NewGuid(),
                    DariTanggal = DateTime.MaxValue,
                    Urutan = 9999
                });
            }
            lvPegawaiRiwayatKesehatan.DataSource = Libs.ToDataTable(lst_pengalaman_riwayat_kesehatan.OrderBy(m => m.DariTanggal).ToList());
            lvPegawaiRiwayatKesehatan.DataBind();

            if (lst_pengalaman_riwayat_mcu.FindAll(m => m.Tanggal == DateTime.MaxValue).Count == 0 && lst_pengalaman_riwayat_mcu.Count > 0)
            {
                lst_pengalaman_riwayat_mcu.Add(new PegawaiRiwayatKesehatanMCU
                {
                    Kode = Guid.NewGuid(),
                    Tanggal = DateTime.MaxValue,
                    Urutan = 9999
                });
            }
            lvPegawaiRiwayatKesehatanMCU.DataSource = Libs.ToDataTable(lst_pengalaman_riwayat_mcu.OrderBy(m => m.Tanggal).ToList());
            lvPegawaiRiwayatKesehatanMCU.DataBind();
        }
        
        protected void btnBackToMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.ROUTE +
                            QS.GetURLVariable()
                        )
                );
        }

        protected void SaveData()
        {
            try
            {
                Pegawai m = DAO_Pegawai.GetByID_Entity(txtNIKaryawan.Text);
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        m.Nama = txtNama.Text;
                        m.TempatLahir = txtTempatLahir.Text;
                        m.TanggalLahir = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahir.Text);
                        m.JenisKelamin = cboJenisKelamin.SelectedValue;
                        m.Agama = cboAgama.SelectedValue;
                        m.TempatTinggal = cboTempatTinggal.SelectedValue;
                        m.AlamatRumah = txtAlamat.Text;
                        m.Kota = txtKota.Text;
                        m.KodePOS = txtKodePOS.Text;
                        m.Telpon = txtTelpon.Text;
                        m.Email = txtEmail.Text;
                        m.EmailPribadi = txtEmailPribadi.Text;
                        m.NoKTP = txtNoKTP.Text;
                        m.NoKK = txtNoKK.Text;
                        m.StatusKeluarga = cboStatusPerkawinan.SelectedValue;

                        m.NamaAyah = txtNamaAyah.Text;
                        m.TempatLahirAyah = txtTempatLahirAyah.Text;
                        if (txtTanggalLahirAyah.Text == "")
                        {
                            m.TanggalLahirAyah = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAyah = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAyah.Text);
                        }
                        m.NIKAyah = txtNIKAyah.Text;
                        m.NoBPJSKesAyah = txtNoBPJSKesehatanAyah.Text;

                        m.NamaIbu = txtNamaIbu.Text;
                        m.TempatLahirIbu = txtTempatLahirIbu.Text;
                        if (txtTanggalLahirIbu.Text == "")
                        {
                            m.TanggalLahirIbu = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirIbu = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirIbu.Text);
                        }
                        m.NIKIbu = txtNIKIbu.Text;
                        m.NoBPJSKesIbu = txtNoBPJSKesehatanIbu.Text;

                        m.NamaAyahMertua = txtNamaAyahMertua.Text;
                        m.TempatLahirAyahMertua = txtTempatLahirAyahMertua.Text;
                        if (txtTanggalLahirAyahMertua.Text == "")
                        {
                            m.TanggalLahirAyahMertua = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAyahMertua = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAyahMertua.Text);
                        }
                        m.NIKAyahMertua = txtNIKAyahMertua.Text;
                        m.NoBPJSKesAyahMertua = txtNoBPJSKesehatanAyahMertua.Text;

                        m.NamaIbuMertua = txtNamaIbuMertua.Text;
                        m.TempatLahirIbuMertua = txtTempatLahirIbuMertua.Text;
                        if (txtTanggalLahirIbuMertua.Text == "")
                        {
                            m.TanggalLahirIbuMertua = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirIbuMertua = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirIbuMertua.Text);
                        }
                        m.NIKIbuMertua = txtNIKIbuMertua.Text;
                        m.NoBPJSKesIbuMertua = txtNoBPJSKesehatanIbuMertua.Text;

                        m.NamaSuamiIstri = txtNamaSuamiIstri.Text;
                        m.TempatLahirSuamiIstri = txtTempatLahirSuamiIstri.Text;
                        if (txtTanggalLahirSuamiIstri.Text == "")
                        {
                            m.TanggalLahirSuamiIstri = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirSuamiIstri = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirSuamiIstri.Text);
                        }
                        m.NIKSuamiIstri = txtNIKSuamiIstri.Text;
                        m.NoBPJSKesSuamiIstri = txtNoBPJSKesehatanSuamiIstri.Text;

                        m.NamaAnakKe1 = txtNamaAnakKe1.Text;
                        m.TempatLahirAnakKe1 = txtTempatLahirAnakKe1.Text;
                        if (txtTanggalLahirAnakKe1.Text == "")
                        {
                            m.TanggalLahirAnakKe1 = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAnakKe1 = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAnakKe1.Text);
                        }
                        m.NIKAnakKe1 = txtNIKAnakKe1.Text;
                        m.NoBPJSKesAnakKe1 = txtNoBPJSKesehatanAnakKe1.Text;

                        m.NamaAnakKe2 = txtNamaAnakKe2.Text;
                        m.TempatLahirAnakKe2 = txtTempatLahirAnakKe2.Text;
                        if (txtTanggalLahirAnakKe2.Text == "")
                        {
                            m.TanggalLahirAnakKe2 = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAnakKe2 = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAnakKe2.Text);
                        }
                        m.NIKAnakKe2 = txtNIKAnakKe2.Text;
                        m.NoBPJSKesAnakKe2 = txtNoBPJSKesehatanAnakKe2.Text;

                        m.NamaAnakKe3 = txtNamaAnakKe3.Text;
                        m.TempatLahirAnakKe3 = txtTempatLahirAnakKe3.Text;
                        if (txtTanggalLahirAnakKe3.Text == "")
                        {
                            m.TanggalLahirAnakKe3 = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAnakKe3 = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAnakKe3.Text);
                        }
                        m.NIKAnakKe3 = txtNIKAnakKe3.Text;
                        m.NoBPJSKesAnakKe3 = txtNoBPJSKesehatanAnakKe3.Text;

                        m.NamaAnakKe4 = txtNamaAnakKe4.Text;
                        m.TempatLahirAnakKe4 = txtTempatLahirAnakKe4.Text;
                        if (txtTanggalLahirAnakKe4.Text == "")
                        {
                            m.TanggalLahirAnakKe4 = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAnakKe4 = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAnakKe4.Text);
                        }
                        m.NIKAnakKe4 = txtNIKAnakKe4.Text;
                        m.NoBPJSKesAnakKe4 = txtNoBPJSKesehatanAnakKe4.Text;

                        m.NamaAnakKe5 = txtNamaAnakKe5.Text;
                        m.TempatLahirAnakKe5 = txtTempatLahirAnakKe5.Text;
                        if (txtTanggalLahirAnakKe5.Text == "")
                        {
                            m.TanggalLahirAnakKe5 = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAnakKe5 = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAnakKe5.Text);
                        }
                        m.NIKAnakKe5 = txtNIKAnakKe5.Text;
                        m.NoBPJSKesAnakKe5 = txtNoBPJSKesehatanAnakKe5.Text;

                        m.NamaAnakKe6 = txtNamaAnakKe6.Text;
                        m.TempatLahirAnakKe6 = txtTempatLahirAnakKe6.Text;
                        if (txtTanggalLahirAnakKe6.Text == "")
                        {
                            m.TanggalLahirAnakKe6 = DateTime.MinValue;
                        }
                        else
                        {
                            m.TanggalLahirAnakKe6 = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLahirAnakKe6.Text);
                        }
                        m.NIKAnakKe6 = txtNIKAnakKe6.Text;
                        m.NoBPJSKesAnakKe6 = txtNoBPJSKesehatanAnakKe6.Text;

                        DAO_Pegawai.Update(m, Libs.LOGGED_USER_M.UserID);

                        SaveDataPendidikanFormal();
                        SaveDataPendidikanNonFormal();
                        SaveDataPengalamaKerjaDalam();
                        SaveDataPengalamaKerjaLuar();
                        SaveDataPengalamaSharing();
                        SaveDataPengalamaKepanitiaan();
                        SaveDataRiwayatKesehatan();
                        SaveDataRiwayatMCU();

                        txtIDPendidikanNonFormal.Value = "";
                        txtIDPendidikanNonFormalNew.Value = "";

                        txtIDRiwayatKesehatan.Value = "";
                        txtIDRiwayatKesehatanNew.Value = "";

                        txtIDRiwayatMCU.Value = "";
                        txtIDRiwayatMCUNew.Value = "";

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
            SaveData();
        }

        protected void SaveDataPendidikanFormal()
        {
            DAO_PegawaiPendidikan.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiPendidikan item in lst_pendidikan)
            {
                if (item.JenisPendidikan.Trim() != "")
                {
                    JurusanPendidikan m_jurusan = DAO_JurusanPendidikan.GetByID_Entity(item.Rel_Jurusan);
                    Universitas m_universitas = DAO_Universitas.GetByID_Entity(item.Rel_Lembaga);

                    bool ada_jurusan = false;
                    if (m_jurusan != null)
                    {
                        if (m_jurusan.Nama != null)
                        {
                            ada_jurusan = true;
                        }
                    }
                    if (!ada_jurusan)
                    {
                        DAO_JurusanPendidikan.Insert(new JurusanPendidikan
                        {
                            Kode = new Guid(item.Rel_Jurusan),
                            Nama = item.Jurusan,
                            Keterangan = ""
                        });
                    }

                    bool ada_universitas = false;
                    if (m_universitas != null)
                    {
                        if (m_universitas.Nama != null)
                        {
                            ada_universitas = true;
                        }
                    }
                    if (!ada_universitas)
                    {
                        DAO_Universitas.Insert(new Universitas
                        {
                            Kode = new Guid(item.Rel_Lembaga),
                            Nama = item.Lembaga,
                            Keterangan = ""
                        });
                    }

                    DAO_PegawaiPendidikan.Insert(item);
                }
            }
        }

        protected void SaveDataPendidikanNonFormal()
        {
            DAO_PegawaiPendidikanNonFormal.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiPendidikanNonFormal item in lst_pendidikan_non_formal)
            {
                if (item.JenisPendidikan.Trim() != "")
                {
                    DAO_PegawaiPendidikanNonFormal.Insert(item);
                }
            }
        }


        protected void SaveDataPengalamaKerjaDalam()
        {
            DAO_PegawaiPengalamanKerjaDalam.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiPengalamanKerjaDalam item in lst_pengalaman_kerja_dalam)
            {
                if (item.Rel_Divisi.Trim() != "")
                {
                    Jabatan m_jabatan = DAO_Jabatan.GetByID_Entity(item.Rel_Jabatan);
                    
                    bool ada_jabatan = false;
                    if (m_jabatan != null)
                    {
                        if (m_jabatan.Nama != null)
                        {
                            ada_jabatan = true;
                        }
                    }
                    if (!ada_jabatan)
                    {
                        DAO_Jabatan.Insert(new Jabatan
                        {
                            Kode = new Guid(item.Rel_Jabatan).ToString(),
                            Nama = item.Jabatan,
                            Keterangan = ""
                        });
                    }
                    
                    DAO_PegawaiPengalamanKerjaDalam.Insert(item);
                }
            }
        }

        protected void SaveDataPengalamaKerjaLuar()
        {
            DAO_PegawaiPengalamanKerjaLuar.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiPengalamanKerjaLuar item in lst_pengalaman_kerja_luar)
            {
                if (item.NamaPerusahaan.Trim() != "")
                {
                    Jabatan m_jabatan = DAO_Jabatan.GetByID_Entity(item.Rel_Jabatan);

                    bool ada_jabatan = false;
                    if (m_jabatan != null)
                    {
                        if (m_jabatan.Nama != null)
                        {
                            ada_jabatan = true;
                        }
                    }
                    if (!ada_jabatan)
                    {
                        DAO_Jabatan.Insert(new Jabatan
                        {
                            Kode = new Guid(item.Rel_Jabatan).ToString(),
                            Nama = item.Jabatan,
                            Keterangan = ""
                        });
                    }

                    DAO_PegawaiPengalamanKerjaLuar.Insert(item);
                }
            }
        }

        protected void SaveDataPengalamaSharing()
        {
            DAO_PegawaiPengalamanSharing.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiPengalamanSharing item in lst_pengalaman_sharing)
            {
                if (item.Tahun.Trim() != "")
                {
                    DAO_PegawaiPengalamanSharing.Insert(item);
                }
            }
        }

        protected void SaveDataPengalamaKepanitiaan()
        {
            DAO_PegawaiPengalamanKepanitiaan.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiPengalamanKepanitiaan item in lst_pengalaman_kepanitiaan)
            {
                if (item.Tahun.Trim() != "")
                {
                    DAO_PegawaiPengalamanKepanitiaan.Insert(item);
                }
            }
        }

        protected void SaveDataRiwayatKesehatan()
        {
            DAO_PegawaiRiwayatKesehatan.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiRiwayatKesehatan item in lst_pengalaman_riwayat_kesehatan)
            {
                if (item.DariTanggal != DateTime.MaxValue)
                {
                    DAO_PegawaiRiwayatKesehatan.Insert(item);
                }
            }
        }

        protected void SaveDataRiwayatMCU()
        {
            DAO_PegawaiRiwayatKesehatanMCU.DeleteByHeader(txtNIKaryawan.Text);
            foreach (PegawaiRiwayatKesehatanMCU item in lst_pengalaman_riwayat_mcu)
            {
                if (item.Tanggal != DateTime.MaxValue)
                {
                    DAO_PegawaiRiwayatKesehatanMCU.Insert(item);
                }
            }
        }

        protected void AddDataPendidikanFormal()
        {
            string kode = Guid.NewGuid().ToString();
            string kode_lembaga = (txtUniversitas.Value.Trim() != "" ? txtUniversitas.Value : Guid.NewGuid().ToString());
            string kode_jurusan = (txtJurusanPendidikan.Value.Trim() != "" ? txtJurusanPendidikan.Value : Guid.NewGuid().ToString());
            int urutan = cboJenisPendidikan.SelectedIndex;

            lst_pendidikan.Add(new PegawaiPendidikan {
                Kode = new Guid(kode),
                JenisPendidikan = cboJenisPendidikan.SelectedValue,
                Lembaga = txtUniversitas.Text,
                Rel_Lembaga = kode_lembaga,
                Jurusan = txtJurusanPendidikan.Text,
                Rel_Jurusan = kode_jurusan,
                DariTahun = txtPendidikanDariTahun.Text,
                SampaiTahun = txtPendidikanSampaiTahun.Text,
                NilaiAkhir = txtNilaiAkhir.Text,
                Keterangan = txtKeterangan.Text,
                Rel_Pegawai = txtNIKaryawan.Text,
                Urutan = urutan
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPendidikanFormal.ToString();
        }

        protected void AddDataPendidikanNonFormal()
        {
            string kode = txtIDPendidikanNonFormalNew.Value;
            
            UpdateUrutanPendidikanNonFormal();
            lst_pendidikan_non_formal.Add(new PegawaiPendidikanNonFormal
            {
                Kode = new Guid(kode),
                JenisPendidikan = txtJenisPendidikanNonFormal.Text,
                Lembaga = txtLembagaPendidikanNonFormal.Text,
                DariTahun = txtDariPendidikanNonFormal.Text,
                SampaiTahun = txtSampaiPendidikanNonFormal.Text,                
                NilaiAkhir = txtNilaiAkhirPendidikanNonFormal.Text,
                Divisi = txtDivisiPendidikanNonFormal.Text,
                Unit = txtUnitPendidikanNonFormal.Text,
                Keterangan = txtKeteranganPendidikanNonFormal.Text,
                Rel_Pegawai = txtNIKaryawan.Text
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPendidikanFormal.ToString();
        }

        protected void AddDataPengalamanKerjaDalam()
        {
            string kode = Guid.NewGuid().ToString();
            string kode_jabatan = (txtJabatanPengalamanKerjaDalam.Value.Trim() != "" ? txtJabatanPengalamanKerjaDalam.Value : Guid.NewGuid().ToString());

            UpdateUrutanPengalamanKerjaDalam();
            lst_pengalaman_kerja_dalam.Add(new PegawaiPengalamanKerjaDalam
            {
                Kode = new Guid(kode),
                Rel_Pegawai = txtNIKaryawan.Text,
                Rel_Divisi = cboDivisiPengalamanKerjaDalam.SelectedValue,
                Divisi = cboDivisiPengalamanKerjaDalam.SelectedItem.Text,
                Rel_Unit = cboUnitPengalamanKerjaDalam.SelectedValue,
                Unit = cboUnitPengalamanKerjaDalam.SelectedItem.Text,
                Dari = txtPengalamanKerjaDalamDari.Text,
                Sampai = txtPengalamanKerjaDalamSampai.Text,
                Rel_Jabatan = kode_jabatan,
                Jabatan = txtJabatanPengalamanKerjaDalam.Text
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaDalam.ToString();
        }

        protected void AddDataPengalamanKerjaLuar()
        {
            string kode = Guid.NewGuid().ToString();
            string kode_jabatan = (txtJabatanPengalamanKerjaLuar.Value.Trim() != "" ? txtJabatanPengalamanKerjaLuar.Value : Guid.NewGuid().ToString());
            
            UpdateUrutanPengalamanKerjaLuar();
            lst_pengalaman_kerja_luar.Add(new PegawaiPengalamanKerjaLuar
            {
                Kode = new Guid(kode),
                NamaPerusahaan = txtNamaPerusahaanPengalamanLuar.Text,
                Rel_Pegawai = txtNIKaryawan.Text,
                Dari = txtPengalamanKerjaLuarDari.Text,
                Sampai = txtPengalamanKerjaLuarSampai.Text,
                Rel_Jabatan = kode_jabatan,
                Jabatan = txtJabatanPengalamanKerjaLuar.Text
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaDalam.ToString();
        }

        protected void AddDataPengalamanSharing()
        {
            string kode = Guid.NewGuid().ToString();
            UpdateUrutanPengalamanSharing();

            lst_pengalaman_sharing.Add(new PegawaiPengalamanSharing
            {
                Kode = new Guid(kode),
                Tahun = txtTahunPengalamanSharing.Text,
                Rel_Pegawai = txtNIKaryawan.Text,
                Topik = txtTopikPengalamanSharing.Text,
                Penyelenggara = txtPenyelenggaraPengalamanSharing.Text,
                Kota = txtKotaPengalamanSharing.Text
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaDalam.ToString();
        }

        protected void AddDataPengalamanKepanitiaan()
        {
            string kode = Guid.NewGuid().ToString();            
            UpdateUrutanPengalamanKepanitiaan();

            lst_pengalaman_kepanitiaan.Add(new PegawaiPengalamanKepanitiaan
            {
                Kode = new Guid(kode),
                Tahun = txtTahunPengalamanKepanitiaan.Text,
                Rel_Pegawai = txtNIKaryawan.Text,
                Kegiatan = txtKegiatanPengalamanKepanitiaan.Text,
                Jabatan = txtJabatanPengalamanKepanitiaan.Text,
                NoSuratTugas = txtNoSuratTugasPengalamanKepanitiaan.Text,
                Keterangan = txtKeteranganPengalamanKepanitiaan.Text
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKepanitiaan.ToString();
        }

        protected void AddDataRiwayatKesehatan()
        {
            string kode = txtIDRiwayatKesehatanNew.Value;
            UpdateUrutanRiwayatKesehatan();

            lst_pengalaman_riwayat_kesehatan.Add(new PegawaiRiwayatKesehatan
            {
                Kode = new Guid(kode),
                DariTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtRiwayatKesehatanDariTanggal.Text),
                SampaiTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtRiwayatKesehatanSampaiTanggal.Text),
                Rel_Pegawai = txtNIKaryawan.Text,
                NamaPenyakit = txtRiwayatKesehatanNamaPenyakit.Text,
                Dokter = txtRiwayatKesehatanNamaDokter.Text,
                IsIzin = chkIsIzin.Checked,
                RSKlinik = txtRiwayatKesehatanKlinikRumahSakit.Text,
                Keterangan = txtRiwayatKesehatanKeterangan.Text
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataRiwayatKesehatan.ToString();
        }

        protected void AddDataRiwayatMCU()
        {
            string kode = txtIDRiwayatMCUNew.Value;
            UpdateUrutanRiwayatMCU();

            lst_pengalaman_riwayat_mcu.Add(new PegawaiRiwayatKesehatanMCU
            {
                Kode = new Guid(kode),
                Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtRiwayatMCUTanggal.Text),
                Rel_Pegawai = txtNIKaryawan.Text,
                Kesimpulan = txtRiwayatMCUKesimpulan.Text,
                Saran = txtRiwayatMCUSaran.Text,
                Keterangan = txtRiwayatMCUKeterangan.Text
            });

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataRiwayatMCU.ToString();
        }

        protected void UpdateUrutanPendidikanNonFormal()
        {
            int urutan = 1;
            foreach (var item in lst_pendidikan_non_formal)
            {
                if (lst_pendidikan_non_formal[urutan - 1].JenisPendidikan.Trim() != "")
                {
                    lst_pendidikan_non_formal[urutan - 1].Urutan = urutan;
                }
                urutan++;
            }
        }

        protected void UpdateUrutanPengalamanKerjaDalam()
        {
            int urutan = 1;
            foreach (var item in lst_pengalaman_kerja_dalam)
            {
                if (lst_pengalaman_kerja_dalam[urutan - 1].Rel_Divisi.Trim() != "")
                {
                    lst_pengalaman_kerja_dalam[urutan - 1].Urutan = urutan;
                }
                urutan++;
            }
        }

        protected void UpdateUrutanPengalamanKerjaLuar()
        {
            int urutan = 1;
            foreach (var item in lst_pengalaman_kerja_luar)
            {
                if (lst_pengalaman_kerja_luar[urutan - 1].NamaPerusahaan.Trim() != "")
                {
                    lst_pengalaman_kerja_luar[urutan - 1].Urutan = urutan;
                }
                urutan++;
            }
        }

        protected void UpdateUrutanPengalamanSharing()
        {
            int urutan = 1;
            foreach (var item in lst_pengalaman_sharing)
            {
                if (lst_pengalaman_sharing[urutan - 1].Tahun.Trim() != "")
                {
                    lst_pengalaman_sharing[urutan - 1].Urutan = urutan;
                }
                urutan++;
            }
        }

        protected void UpdateUrutanPengalamanKepanitiaan()
        {
            int urutan = 1;
            foreach (var item in lst_pengalaman_kepanitiaan)
            {
                if (lst_pengalaman_kepanitiaan[urutan - 1].Tahun.Trim() != "")
                {
                    lst_pengalaman_kepanitiaan[urutan - 1].Urutan = urutan;
                }
                urutan++;
            }
        }

        protected void UpdateUrutanRiwayatKesehatan()
        {
            int urutan = 1;
            foreach (var item in lst_pengalaman_riwayat_kesehatan)
            {
                if (lst_pengalaman_riwayat_kesehatan[urutan - 1].DariTanggal != DateTime.MaxValue)
                {
                    lst_pengalaman_riwayat_kesehatan[urutan - 1].Urutan = urutan;
                    urutan++;
                }
            }
        }

        protected void UpdateUrutanRiwayatMCU()
        {
            int urutan = 1;
            foreach (var item in lst_pengalaman_riwayat_mcu)
            {
                if (lst_pengalaman_riwayat_mcu[urutan - 1].Tanggal != DateTime.MaxValue)
                {
                    lst_pengalaman_riwayat_mcu[urutan - 1].Urutan = urutan;
                }
                urutan++;
            }
        }

        protected void UpdateDataPendidikanFormal()
        {
            string kode = txtIDPendidikanFormal.Value;
            string kode_lembaga = (txtUniversitas.Value.Trim() != "" ? txtUniversitas.Value : Guid.NewGuid().ToString());
            string kode_jurusan = (txtJurusanPendidikan.Value.Trim() != "" ? txtJurusanPendidikan.Value : Guid.NewGuid().ToString());
            int urutan = cboJenisPendidikan.SelectedIndex;

            if (lst_pendidikan.Count > 0)
            {
                for (int i = 0; i < lst_pendidikan.Count; i++)
                {
                    if (lst_pendidikan[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pendidikan[i].JenisPendidikan = cboJenisPendidikan.SelectedValue;
                        lst_pendidikan[i].Lembaga = txtUniversitas.Text;
                        lst_pendidikan[i].Rel_Lembaga = kode_lembaga;
                        lst_pendidikan[i].Jurusan = txtJurusanPendidikan.Text;
                        lst_pendidikan[i].Rel_Jurusan = kode_jurusan;
                        lst_pendidikan[i].DariTahun = txtPendidikanDariTahun.Text;
                        lst_pendidikan[i].SampaiTahun = txtPendidikanSampaiTahun.Text;
                        lst_pendidikan[i].NilaiAkhir = txtNilaiAkhir.Text;
                        lst_pendidikan[i].Keterangan = txtKeterangan.Text;
                        lst_pendidikan[i].Rel_Pegawai = txtNIKaryawan.Text;
                        lst_pendidikan[i].Urutan = urutan;

                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPendidikanFormal.ToString();
        }

        protected void UpdateDataPendidikanNonFormal()
        {
            string kode = txtIDPendidikanNonFormal.Value;
            
            if (lst_pendidikan_non_formal.Count > 0)
            {
                for (int i = 0; i < lst_pendidikan_non_formal.Count; i++)
                {
                    if (lst_pendidikan_non_formal[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pendidikan_non_formal[i].JenisPendidikan = txtJenisPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].Lembaga = txtLembagaPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].Divisi = txtDivisiPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].Unit = txtUnitPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].DariTahun = txtDariPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].SampaiTahun = txtSampaiPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].NilaiAkhir = txtNilaiAkhirPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].Keterangan = txtKeteranganPendidikanNonFormal.Text;
                        lst_pendidikan_non_formal[i].Rel_Pegawai = txtNIKaryawan.Text;
                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPendidikanNonFormal.ToString();
        }

        protected void UpdateDataPengalamanKerjaDalam()
        {
            string kode = txtIDPengalamanKerjaDalam.Value;
            string kode_jabatan = (txtJabatanPengalamanKerjaDalam.Value.Trim() != "" ? txtJabatanPengalamanKerjaDalam.Value : Guid.NewGuid().ToString());

            if (lst_pengalaman_kerja_dalam.Count > 0)
            {
                for (int i = 0; i < lst_pengalaman_kerja_dalam.Count; i++)
                {
                    if (lst_pengalaman_kerja_dalam[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pengalaman_kerja_dalam[i].Rel_Divisi = cboDivisiPengalamanKerjaDalam.SelectedValue;
                        lst_pengalaman_kerja_dalam[i].Rel_Unit = cboUnitPengalamanKerjaDalam.SelectedValue;
                        lst_pengalaman_kerja_dalam[i].Dari = txtPengalamanKerjaDalamDari.Text;
                        lst_pengalaman_kerja_dalam[i].Sampai = txtPengalamanKerjaDalamSampai.Text;
                        lst_pengalaman_kerja_dalam[i].Rel_Jabatan = kode_jabatan;
                        lst_pengalaman_kerja_dalam[i].Jabatan = txtJabatanPengalamanKerjaDalam.Text;
                        lst_pengalaman_kerja_dalam[i].Rel_Pegawai = txtNIKaryawan.Text;
                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaDalam.ToString();
        }

        protected void UpdateDataPengalamanKerjaLuar()
        {
            string kode = txtIDPengalamanKerjaLuar.Value;
            string kode_jabatan = (txtJabatanPengalamanKerjaLuar.Value.Trim() != "" ? txtJabatanPengalamanKerjaLuar.Value : Guid.NewGuid().ToString());

            if (lst_pengalaman_kerja_luar.Count > 0)
            {
                for (int i = 0; i < lst_pengalaman_kerja_luar.Count; i++)
                {
                    if (lst_pengalaman_kerja_luar[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pengalaman_kerja_luar[i].NamaPerusahaan = txtNamaPerusahaanPengalamanLuar.Text;
                        lst_pengalaman_kerja_luar[i].Dari = txtPengalamanKerjaLuarDari.Text;
                        lst_pengalaman_kerja_luar[i].Sampai = txtPengalamanKerjaLuarSampai.Text;
                        lst_pengalaman_kerja_luar[i].Jabatan = txtJabatanPengalamanKerjaLuar.Text;
                        lst_pengalaman_kerja_luar[i].Rel_Jabatan = kode_jabatan;
                        lst_pengalaman_kerja_luar[i].Rel_Pegawai = txtNIKaryawan.Text;
                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaLuar.ToString();
        }

        protected void UpdateDataPengalamanSharing()
        {
            string kode = txtIDPengalamanSharing.Value;
            
            if (lst_pengalaman_sharing.Count > 0)
            {
                for (int i = 0; i < lst_pengalaman_sharing.Count; i++)
                {
                    if (lst_pengalaman_sharing[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pengalaman_sharing[i].Tahun = txtTahunPengalamanSharing.Text;
                        lst_pengalaman_sharing[i].Topik = txtTopikPengalamanSharing.Text;
                        lst_pengalaman_sharing[i].Penyelenggara = txtPenyelenggaraPengalamanSharing.Text;
                        lst_pengalaman_sharing[i].Kota = txtKotaPengalamanSharing.Text;
                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaLuar.ToString();
        }

        protected void UpdateDataRiwayatKesehatan()
        {
            string kode = txtIDRiwayatKesehatan.Value;

            if (lst_pengalaman_riwayat_kesehatan.Count > 0)
            {
                for (int i = 0; i < lst_pengalaman_riwayat_kesehatan.Count; i++)
                {
                    if (lst_pengalaman_riwayat_kesehatan[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pengalaman_riwayat_kesehatan[i].Kode = new Guid(kode);
                        lst_pengalaman_riwayat_kesehatan[i].DariTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtRiwayatKesehatanDariTanggal.Text);
                        lst_pengalaman_riwayat_kesehatan[i].SampaiTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtRiwayatKesehatanSampaiTanggal.Text);
                        lst_pengalaman_riwayat_kesehatan[i].Rel_Pegawai = txtNIKaryawan.Text;
                        lst_pengalaman_riwayat_kesehatan[i].NamaPenyakit = txtRiwayatKesehatanNamaPenyakit.Text;
                        lst_pengalaman_riwayat_kesehatan[i].Dokter = txtRiwayatKesehatanNamaDokter.Text;
                        lst_pengalaman_riwayat_kesehatan[i].IsIzin = chkIsIzin.Checked;
                        lst_pengalaman_riwayat_kesehatan[i].RSKlinik = txtRiwayatKesehatanKlinikRumahSakit.Text;
                        lst_pengalaman_riwayat_kesehatan[i].Keterangan = txtRiwayatKesehatanKeterangan.Text;
                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataRiwayatKesehatan.ToString();
        }

        protected void UpdateDataRiwayatMCU()
        {
            string kode = txtIDRiwayatMCU.Value;

            if (lst_pengalaman_riwayat_mcu.Count > 0)
            {
                for (int i = 0; i < lst_pengalaman_riwayat_mcu.Count; i++)
                {
                    if (lst_pengalaman_riwayat_mcu[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pengalaman_riwayat_mcu[i].Kode = new Guid(kode);
                        lst_pengalaman_riwayat_mcu[i].Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtRiwayatMCUTanggal.Text);
                        lst_pengalaman_riwayat_mcu[i].Rel_Pegawai = txtNIKaryawan.Text;
                        lst_pengalaman_riwayat_mcu[i].Kesimpulan = txtRiwayatMCUKesimpulan.Text;
                        lst_pengalaman_riwayat_mcu[i].Saran = txtRiwayatMCUSaran.Text;
                        lst_pengalaman_riwayat_mcu[i].Keterangan = txtRiwayatMCUKeterangan.Text;
                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataRiwayatMCU.ToString();
        }

        protected void UpdateDataPengalamanKepanitiaan()
        {
            string kode = txtIDPengalamanKepanitiaan.Value;

            if (lst_pengalaman_kepanitiaan.Count > 0)
            {
                for (int i = 0; i < lst_pengalaman_kepanitiaan.Count; i++)
                {
                    if (lst_pengalaman_kepanitiaan[i].Kode.ToString().Trim().ToUpper() == kode.Trim().ToUpper())
                    {
                        lst_pengalaman_kepanitiaan[i].Tahun = txtTahunPengalamanKepanitiaan.Text;
                        lst_pengalaman_kepanitiaan[i].Kegiatan = txtKegiatanPengalamanKepanitiaan.Text;
                        lst_pengalaman_kepanitiaan[i].Jabatan = txtJabatanPengalamanKepanitiaan.Text;
                        lst_pengalaman_kepanitiaan[i].NoSuratTugas = txtNoSuratTugasPengalamanKepanitiaan.Text;
                        lst_pengalaman_kepanitiaan[i].Keterangan = txtKeteranganPengalamanKepanitiaan.Text;
                        break;
                    }
                }
            }

            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaLuar.ToString();
        }

        protected void InitFieldsPendidikanFormal()
        {
            cboJenisPendidikan.SelectedValue = "";
            txtUniversitas.Value = "";
            txtUniversitas.Text = "";
            txtJurusanPendidikan.Value = "";
            txtJurusanPendidikan.Text = "";
            txtPendidikanDariTahun.Text = "";
            txtPendidikanSampaiTahun.Text = "";
            txtNilaiAkhir.Text = "";
            txtKeterangan.Text = "";
        }

        protected void InitFieldsPendidikanNonFormal()
        {
            txtJenisPendidikanNonFormal.Text = "";
            txtLembagaPendidikanNonFormal.Text = "";
            txtDariPendidikanNonFormal.Text = "";
            txtSampaiPendidikanNonFormal.Text = "";
            txtNilaiAkhirPendidikanNonFormal.Text = "";
            txtDivisiPendidikanNonFormal.Text = "";
            txtUnitPendidikanNonFormal.Text = "";
            txtKeteranganPendidikanNonFormal.Text = "";
        }

        protected void InitFieldsPengalamanKerjaDalam()
        {
            cboDivisiPengalamanKerjaDalam.SelectedValue = "";
            cboUnitPengalamanKerjaDalam.SelectedValue = "";
            txtPengalamanKerjaDalamDari.Text = "";
            txtPengalamanKerjaDalamSampai.Text = "";
            txtJabatanPengalamanKerjaDalam.Value = "";

            cboDivisiPengalamanKerjaDalam.Items.Clear();
            cboDivisiPengalamanKerjaDalam.Items.Add(new ListItem { Value = "", Text = "" });
            foreach (var item in DAO_Divisi.GetAll_Entity().OrderBy(m => m.Nama).ToList())
            {
                cboDivisiPengalamanKerjaDalam.Items.Add(new ListItem { Value = item.Kode.ToString(), Text = item.Nama });
            }

            cboUnitPengalamanKerjaDalam.Items.Clear();
            cboUnitPengalamanKerjaDalam.Items.Add(new ListItem { Value = "", Text = "" });
            foreach (var item in DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList())
            {
                cboUnitPengalamanKerjaDalam.Items.Add(new ListItem { Value = item.Kode.ToString(), Text = item.Nama });
            }
            foreach (var item in DAO_Unit.GetAll_Entity().OrderBy(m => m.Nama).ToList())
            {
                cboUnitPengalamanKerjaDalam.Items.Add(new ListItem { Value = item.Kode.ToString(), Text = item.Nama });
            }
        }

        protected void InitFieldsPengalamanKerjaLuar()
        {
            txtNamaPerusahaanPengalamanLuar.Text = "";
            txtPengalamanKerjaLuarDari.Text = "";
            txtPengalamanKerjaLuarSampai.Text = "";
            txtJabatanPengalamanKerjaLuar.Value = "";
        }

        protected void InitFieldsPengalamanSharing()
        {
            txtTahunPengalamanSharing.Text = "";
            txtTopikPengalamanSharing.Text = "";
            txtPenyelenggaraPengalamanSharing.Text = "";
            txtKotaPengalamanSharing.Text = "";
        }

        protected void InitFieldsPengalamanRiwayatKesehatan()
        {
            txtRiwayatKesehatanDariTanggal.Text = "";
            txtRiwayatKesehatanSampaiTanggal.Text = "";
            chkIsIzin.Checked = false;
            txtRiwayatKesehatanNamaPenyakit.Text = "";
            txtRiwayatKesehatanKlinikRumahSakit.Text = "";
            txtRiwayatKesehatanNamaDokter.Text = "";
            txtRiwayatKesehatanKeterangan.Text = "";
        }

        protected void InitFieldsPengalamanRiwayatMCU()
        {
            txtRiwayatMCUTanggal.Text = "";
            txtRiwayatMCUKesimpulan.Text = "";
            txtRiwayatMCUSaran.Text = "";
            txtRiwayatMCUKeterangan.Text = "";
        }

        protected void InitFieldsPengalamanKepanitiaan()
        {
            txtTahunPengalamanKepanitiaan.Text = "";
            txtKegiatanPengalamanKepanitiaan.Text = "";
            txtJabatanPengalamanKepanitiaan.Text = "";
            txtNoSuratTugasPengalamanKepanitiaan.Text = "";
            txtKeteranganPengalamanKepanitiaan.Text = "";
        }

        protected void lnkTambahDataPendidikanFormal_Click(object sender, EventArgs e)
        {
            txtIDPendidikanFormal.Value = "";
            InitFieldsPendidikanFormal();
            txtKeyAction.Value = JenisAction.DoShowInputPendidikan.ToString();
        }

        protected void lnkOKInputPendidikan_Click(object sender, EventArgs e)
        {
            if (txtIDPendidikanFormal.Value.Trim() == "")
            {
                AddDataPendidikanFormal();
            }
            else
            {
                UpdateDataPendidikanFormal();
            }
            SaveData();
        }

        protected void btnShowConfirmDeletePendidikanFormal_Click(object sender, EventArgs e)
        {
            PegawaiPendidikan m_pendidikan = lst_pendidikan.FindAll(m => m.Kode.ToString().ToUpper() == txtIDPendidikanFormal.Value.Trim().ToUpper()).FirstOrDefault();
            if (m_pendidikan != null)
            {
                if (m_pendidikan.Lembaga != null)
                {
                    ltrMsgConfirmHapusPendidikan.Text = 
                        "Anda yakin akan menghapus data pendidikan formal : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            m_pendidikan.Lembaga + 
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusPendidikan.ToString();
                }
            }            
        }

        protected void btnShowDetailPendidikanFormal_Click(object sender, EventArgs e)
        {
            InitFieldsPendidikanFormal();
            PegawaiPendidikan m_pendidikan = lst_pendidikan.FindAll(m => m.Kode == new Guid(txtIDPendidikanFormal.Value)).FirstOrDefault();
            if (m_pendidikan != null)
            {
                if (m_pendidikan.Lembaga != null)
                {
                    cboJenisPendidikan.SelectedValue = m_pendidikan.JenisPendidikan;
                    txtUniversitas.Value = m_pendidikan.Rel_Lembaga;
                    txtUniversitas.Text = m_pendidikan.Lembaga;
                    txtJurusanPendidikan.Value = m_pendidikan.Rel_Jurusan;
                    txtJurusanPendidikan.Text = m_pendidikan.Jurusan;
                    txtPendidikanDariTahun.Text = m_pendidikan.DariTahun;
                    txtPendidikanSampaiTahun.Text = m_pendidikan.SampaiTahun;
                    txtNilaiAkhir.Text = m_pendidikan.NilaiAkhir;
                    txtKeterangan.Text = m_pendidikan.Keterangan;
                }
            }
            txtKeyAction.Value = JenisAction.DoShowInputPendidikan.ToString();
        }

        protected void lnkOKHapusPendidikan_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pendidikan)
            {
                if (item.Kode.ToString().ToUpper() == txtIDPendidikanFormal.Value.Trim().ToUpper())
                {
                    lst_pendidikan.Remove(item);
                    break;
                }
            }
            RenderListviewDetail();
            txtIDPendidikanFormal.Value = "";
            SaveData();
            txtKeyAction.Value = JenisAction.DoShowDataPendidikanFormal.ToString();
        }

        protected void ShowUploadedFilesPendidikanNonFormal()
        {
            divUploadedFilesPendidikanNonFormal.Visible = false;
            string id_pendidikan_non_formal = 
                (
                    txtIDPendidikanNonFormal.Value.Trim() == "" && txtIDPendidikanNonFormalNew.Value.Trim() != ""
                    ? txtIDPendidikanNonFormalNew.Value.Trim()
                    : txtIDPendidikanNonFormal.Value.Trim()
                );
            ltrUploadedFilesPendidikanNonFormal.Text = Libs.GetHTMLListUploadedFiles(
                    this.Page,
                    Libs.GetFolderPendidikanNonFormal(txtNIKaryawan.Text, id_pendidikan_non_formal),
                    Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL,
                    txtNIKaryawan.Text,
                    (id_pendidikan_non_formal),
                    true
                );
            if (ltrUploadedFilesPendidikanNonFormal.Text.Trim() != "")
            {
                divUploadedFilesPendidikanNonFormal.Visible = true;
            }
        }

        protected void ShowUploadedFilesRiwayatKesehatan()
        {
            divUploadedFilesRiwayatKesehatan.Visible = false;
            string id_riwayat_kesehatan =
                (
                    txtIDRiwayatKesehatan.Value.Trim() == "" && txtIDRiwayatKesehatanNew.Value.Trim() != ""
                    ? txtIDRiwayatKesehatanNew.Value.Trim()
                    : txtIDRiwayatKesehatan.Value.Trim()
                );
            ltrUploadedFilesRiwayatKesehatan.Text = Libs.GetHTMLListUploadedFiles(
                    this.Page,
                    Libs.GetFolderRiwayatKesehatan(txtNIKaryawan.Text, id_riwayat_kesehatan),
                    Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN,
                    txtNIKaryawan.Text,
                    (id_riwayat_kesehatan),
                    true
                );
            if (ltrUploadedFilesRiwayatKesehatan.Text.Trim() != "")
            {
                divUploadedFilesRiwayatKesehatan.Visible = true;
            }
        }

        protected void ShowUploadedFilesRiwayatMCU()
        {
            divUploadedFilesRiwayatMCU.Visible = false;
            string id_riwayat_mcu =
                (
                    txtIDRiwayatMCU.Value.Trim() == "" && txtIDRiwayatMCUNew.Value.Trim() != ""
                    ? txtIDRiwayatMCUNew.Value.Trim()
                    : txtIDRiwayatMCU.Value.Trim()
                );
            ltrUploadedFilesRiwayatMCU.Text = Libs.GetHTMLListUploadedFiles(
                    this.Page,
                    Libs.GetFolderRiwayatMCU(txtNIKaryawan.Text, id_riwayat_mcu),
                    Libs.JENIS_UPLOAD.RIWAYAT_MCU,
                    txtNIKaryawan.Text,
                    (id_riwayat_mcu),
                    true
                );
            if (ltrUploadedFilesRiwayatMCU.Text.Trim() != "")
            {
                divUploadedFilesRiwayatMCU.Visible = true;
            }
        }

        protected void ShowUploadedFilesPendukung()
        {
            ltrListFileUploadPendukung.Text = Libs.GetHTMLListUploadedFiles(
                    this.Page,
                    Libs.GetFolderFilePendukung(txtNIKaryawan.Text, "Files"),
                    Libs.JENIS_UPLOAD.FILE_PENDUKUNG,
                    txtNIKaryawan.Text,
                    "Files",
                    true
                );
            if (ltrListFileUploadPendukung.Text.Trim() == "")
            {
                ltrListFileUploadPendukung.Text = "<div onclick=\"ShowProgress(true); " + lnkTambahUploadFilePendukung.ClientID + ".click();\" style=\"cursor: pointer; padding: 15px; width: 100%;\">" +
                                                    "<div style=\"cursor: pointer; margin: 0 auto; display: table; padding: 15px;\">" +
                                                        "<img src=\"" + 
                                                                  ResolveUrl("~/Application_CLibs/images/svg/folder-2.svg") +
                                                                 "\" style=\"margin: 0 auto; display: table; height: 64px; width: 64px;\" />" +
                                                    "</div>" +
                                                    "<div style=\"cursor: pointer; margin: 0 auto; display: table; color: grey;\">" +
                                                        "..:: Data Kosong ::.." +
                                                    "</div>" +
                                                  "</div>";
            }
            else
            {
                ltrListFileUploadPendukung.Text += "<br />";
            }
        }

        protected void btnShowDetailPendidikanNonFormal_Click(object sender, EventArgs e)
        {
            InitFieldsPendidikanNonFormal();
            txtIDPendidikanNonFormalNew.Value = "";
            PegawaiPendidikanNonFormal m_pendidikan = lst_pendidikan_non_formal.FindAll(m => m.Kode == new Guid(txtIDPendidikanNonFormal.Value)).FirstOrDefault();
            if (m_pendidikan != null)
            {
                if (m_pendidikan.Lembaga != null)
                {
                    txtJenisPendidikanNonFormal.Text = m_pendidikan.JenisPendidikan;
                    txtLembagaPendidikanNonFormal.Text = m_pendidikan.Lembaga;
                    txtDariPendidikanNonFormal.Text = m_pendidikan.DariTahun;
                    txtSampaiPendidikanNonFormal.Text = m_pendidikan.SampaiTahun;
                    txtNilaiAkhirPendidikanNonFormal.Text = m_pendidikan.NilaiAkhir;
                    txtDivisiPendidikanNonFormal.Text = m_pendidikan.Divisi;
                    txtUnitPendidikanNonFormal.Text = m_pendidikan.Unit;
                    txtKeteranganPendidikanNonFormal.Text = m_pendidikan.Keterangan;
                }
            }
            ShowUploadedFilesPendidikanNonFormal();
            txtKeyAction.Value = JenisAction.DoShowInputPendidikanNonFormal.ToString();
        }

        protected void btnShowConfirmDeletePendidikanNonFormal_Click(object sender, EventArgs e)
        {
            PegawaiPendidikanNonFormal m_pendidikan = lst_pendidikan_non_formal.FindAll(m => m.Kode.ToString().ToUpper() == txtIDPendidikanNonFormal.Value.Trim().ToUpper()).FirstOrDefault();
            if (m_pendidikan != null)
            {
                if (m_pendidikan.Lembaga != null)
                {
                    ltrMsgConfirmHapusPendidikanNonFormal.Text =
                        "Anda yakin akan menghapus data pendidikan non formal : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            m_pendidikan.Lembaga +
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusPendidikanNonFormal.ToString();
                }
            }
        }

        protected void lnkOKInputPendidikanNonFormal_Click(object sender, EventArgs e)
        {
            if (txtIDPendidikanNonFormal.Value.Trim() == "")
            {
                AddDataPendidikanNonFormal();
            }
            else
            {
                UpdateDataPendidikanNonFormal();
            }
            SaveData();
        }

        protected void lnkTambahDataPendidikanFormalNonFormal_Click(object sender, EventArgs e)
        {
            txtIDPendidikanNonFormal.Value = "";
            txtIDPendidikanNonFormalNew.Value = Guid.NewGuid().ToString();
            txtURLDeletePendidikanNonFormal.Value = GetURLDeletePendidikanNonFormal(txtNIKaryawan.Text, txtIDPendidikanNonFormalNew.Value);
            InitFieldsPendidikanNonFormal();
            ShowUploadedFilesPendidikanNonFormal();
            txtKeyAction.Value = JenisAction.DoShowInputPendidikanNonFormal.ToString();
        }

        protected string GetURLDeletePendidikanNonFormal(string id, string id2 = "")
        {
            return ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE) +
                              "?jenis=" + Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL +
                              "&id=" + id.ToString() +
                              (id2.Trim() != "" ? "&id2=" + id2.ToString() : "");
        }

        protected string GetURLDeleteRiwayatKesehatan(string id, string id2 = "")
        {
            return ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE) +
                              "?jenis=" + Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN +
                              "&id=" + id.ToString() +
                              (id2.Trim() != "" ? "&id2=" + id2.ToString() : "");
        }

        protected string GetURLDeleteRiwayatMCU(string id, string id2 = "")
        {
            return ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE) +
                              "?jenis=" + Libs.JENIS_UPLOAD.RIWAYAT_MCU +
                              "&id=" + id.ToString() +
                              (id2.Trim() != "" ? "&id2=" + id2.ToString() : "");
        }

        protected void lnkOKHapusPendidikanNonFormal_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pendidikan_non_formal)
            {
                if (item.Kode.ToString().ToUpper() == txtIDPendidikanNonFormal.Value.Trim().ToUpper())
                {
                    lst_pendidikan_non_formal.Remove(item);
                    break;
                }
            }
            UpdateDataPendidikanNonFormal();
            RenderListviewDetail();
            txtURLDeletePendidikanNonFormal.Value = GetURLDeletePendidikanNonFormal(txtNIKaryawan.Text, txtIDPendidikanNonFormal.Value);
            txtIDPendidikanNonFormal.Value = "";
            SaveData();
            txtKeyAction.Value = JenisAction.DoShowDataPendidikanNonFormalAndDoDeleteDirectory.ToString();
        }

        protected void lnkTambahDataPengalamanKerjaDalam_Click(object sender, EventArgs e)
        {
            txtIDPengalamanKerjaDalam.Value = "";
            InitFieldsPengalamanKerjaDalam();
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanKerjaDalam.ToString();
        }

        protected void lnkOKInputPegalamanKerjaDalam_Click(object sender, EventArgs e)
        {
            if (txtIDPengalamanKerjaDalam.Value.Trim() == "")
            {
                AddDataPengalamanKerjaDalam();
            }
            else
            {
                UpdateDataPengalamanKerjaDalam();
            }
            SaveData();
        }

        protected void btnShowDetailPengalamanKerjaDalam_Click(object sender, EventArgs e)
        {
            InitFieldsPengalamanKerjaDalam();
            PegawaiPengalamanKerjaDalam m = lst_pengalaman_kerja_dalam.FindAll(m0 => m0.Kode == new Guid(txtIDPengalamanKerjaDalam.Value)).FirstOrDefault();
            if (m != null)
            {
                if (m.Rel_Divisi != null)
                {
                    cboDivisiPengalamanKerjaDalam.SelectedValue = m.Rel_Divisi;
                    cboUnitPengalamanKerjaDalam.SelectedValue = m.Rel_Unit;
                    txtPengalamanKerjaDalamDari.Text = m.Dari;
                    txtPengalamanKerjaDalamSampai.Text = m.Sampai;
                    txtJabatanPengalamanKerjaDalam.Value = m.Rel_Jabatan;
                }
            }
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanKerjaDalam.ToString();
        }

        protected void btnShowConfirmDeletePengalamanKerjaDalam_Click(object sender, EventArgs e)
        {
            PegawaiPengalamanKerjaDalam m = lst_pengalaman_kerja_dalam.FindAll(m0 => m0.Kode.ToString().ToUpper() == txtIDPengalamanKerjaDalam.Value.Trim().ToUpper()).FirstOrDefault();
            if (m != null)
            {
                if (m.Rel_Divisi != null)
                {
                    ltrMsgConfirmHapusPengalamanKerjaDalam.Text =
                        "Anda yakin akan menghapus data pengalaman kerja dalam perusahaan : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            m.Divisi + ", " + m.Unit +
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusPengalamanKerjaDalam.ToString();
                }
            }
        }

        protected void lnkOKHapusPengalamanKerjaDalam_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pengalaman_kerja_dalam)
            {
                if (item.Kode.ToString().ToUpper() == txtIDPengalamanKerjaDalam.Value.Trim().ToUpper())
                {
                    lst_pengalaman_kerja_dalam.Remove(item);
                    break;
                }
            }
            txtIDPengalamanKerjaDalam.Value = "";
            SaveData();
            ShowDataDetail();
            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaDalam.ToString();
        }

        protected void lnkTambahDataPengalamanKerjaLuar_Click(object sender, EventArgs e)
        {
            txtIDPengalamanKerjaLuar.Value = "";
            InitFieldsPengalamanKerjaLuar();
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanKerjaLuar.ToString();
        }

        protected void lnkOKInputPegalamanKerjaLuar_Click(object sender, EventArgs e)
        {
            if (txtIDPengalamanKerjaLuar.Value.Trim() == "")
            {
                AddDataPengalamanKerjaLuar();
            }
            else
            {
                UpdateDataPengalamanKerjaLuar();
            }
            SaveData();
        }

        protected void lnkOKHapusPengalamanKerjaLuar_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pengalaman_kerja_luar)
            {
                if (item.Kode.ToString().ToUpper() == txtIDPengalamanKerjaLuar.Value.Trim().ToUpper())
                {
                    lst_pengalaman_kerja_luar.Remove(item);
                    break;
                }
            }
            txtIDPengalamanKerjaLuar.Value = "";
            SaveData();
            ShowDataDetail();
            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKerjaLuar.ToString();
        }

        protected void btnShowDetailPengalamanKerjaLuar_Click(object sender, EventArgs e)
        {
            InitFieldsPengalamanKerjaLuar();
            PegawaiPengalamanKerjaLuar m = lst_pengalaman_kerja_luar.FindAll(m0 => m0.Kode == new Guid(txtIDPengalamanKerjaLuar.Value)).FirstOrDefault();
            if (m != null)
            {
                if (m.NamaPerusahaan != null)
                {
                    txtNamaPerusahaanPengalamanLuar.Text = m.NamaPerusahaan;
                    txtPengalamanKerjaLuarDari.Text = m.Dari;
                    txtPengalamanKerjaLuarSampai.Text = m.Sampai;
                    txtJabatanPengalamanKerjaLuar.Value = m.Rel_Jabatan;
                }
            }
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanKerjaLuar.ToString();
        }

        protected void btnShowConfirmDeletePengalamanKerjaLuar_Click(object sender, EventArgs e)
        {
            PegawaiPengalamanKerjaLuar m = lst_pengalaman_kerja_luar.FindAll(m0 => m0.Kode.ToString().ToUpper() == txtIDPengalamanKerjaLuar.Value.Trim().ToUpper()).FirstOrDefault();
            if (m != null)
            {
                if (m.NamaPerusahaan != null)
                {
                    ltrMsgConfirmHapusPengalamanKerjaLuar.Text =
                        "Anda yakin akan menghapus data pengalaman kerja luar perusahaan : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            m.NamaPerusahaan + ", " + m.Jabatan +
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusPengalamanKerjaLuar.ToString();
                }
            }
        }

        protected void lnkTambahDataPengalamanSharing_Click(object sender, EventArgs e)
        {
            txtIDPengalamanSharing.Value = "";
            InitFieldsPengalamanSharing();
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanSharing.ToString();
        }

        protected void btnShowConfirmDeletePengalamanSharing_Click(object sender, EventArgs e)
        {
            PegawaiPengalamanSharing m = lst_pengalaman_sharing.FindAll(m0 => m0.Kode.ToString().ToUpper() == txtIDPengalamanSharing.Value.Trim().ToUpper()).FirstOrDefault();
            if (m != null)
            {
                if (m.Tahun != null)
                {
                    ltrMsgConfirmHapusPengalamanSharing.Text =
                        "Anda yakin akan menghapus data pengalaman sharing : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            m.Tahun + ", " + m.Topik +
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusPengalamanSharing.ToString();
                }
            }
        }

        protected void btnShowDetailPengalamanSharing_Click(object sender, EventArgs e)
        {
            InitFieldsPengalamanSharing();
            PegawaiPengalamanSharing m = lst_pengalaman_sharing.FindAll(m0 => m0.Kode == new Guid(txtIDPengalamanSharing.Value)).FirstOrDefault();
            if (m != null)
            {
                if (m.Tahun != null)
                {
                    txtTahunPengalamanSharing.Text = m.Tahun;
                    txtTopikPengalamanSharing.Text = m.Topik;
                    txtPenyelenggaraPengalamanSharing.Text = m.Penyelenggara;
                    txtKotaPengalamanSharing.Text = m.Kota;
                }
            }
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanSharing.ToString();
        }

        protected void btnShowConfirmDeletePengalamanKepanitiaan_Click(object sender, EventArgs e)
        {
            PegawaiPengalamanKepanitiaan m = lst_pengalaman_kepanitiaan.FindAll(m0 => m0.Kode.ToString().ToUpper() == txtIDPengalamanKepanitiaan.Value.Trim().ToUpper()).FirstOrDefault();
            if (m != null)
            {
                if (m.Tahun != null)
                {
                    ltrMsgConfirmHapusPengalamanKepanitiaan.Text =
                        "Anda yakin akan menghapus <br />Data pengalaman kepanitiaan : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            m.Tahun + ", " + m.Kegiatan +
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusPengalamanKepanitiaan.ToString();
                }
            }
        }

        protected void btnShowDetailPengalamanKepanitiaan_Click(object sender, EventArgs e)
        {
            InitFieldsPengalamanKepanitiaan();
            PegawaiPengalamanKepanitiaan m = lst_pengalaman_kepanitiaan.FindAll(m0 => m0.Kode == new Guid(txtIDPengalamanKepanitiaan.Value)).FirstOrDefault();
            if (m != null)
            {
                if (m.Tahun != null)
                {
                    txtTahunPengalamanKepanitiaan.Text = m.Tahun;
                    txtKegiatanPengalamanKepanitiaan.Text = m.Kegiatan;
                    txtJabatanPengalamanKepanitiaan.Text = m.Jabatan;
                    txtNoSuratTugasPengalamanKepanitiaan.Text = m.NoSuratTugas;
                    txtKeteranganPengalamanKepanitiaan.Text = m.Keterangan;
                }
            }
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanKepanitiaan.ToString();
        }

        protected void lnkOKInputPegalamanSharing_Click(object sender, EventArgs e)
        {
            if (txtIDPengalamanSharing.Value.Trim() == "")
            {
                AddDataPengalamanSharing();
            }
            else
            {
                UpdateDataPengalamanSharing();
            }
            SaveData();
        }

        protected void lnkOKHapusPengalamanSharing_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pengalaman_sharing)
            {
                if (item.Kode.ToString().ToUpper() == txtIDPengalamanSharing.Value.Trim().ToUpper())
                {
                    lst_pengalaman_sharing.Remove(item);
                    break;
                }
            }
            txtIDPengalamanSharing.Value = "";
            SaveData();
            ShowDataDetail();
            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanSharing.ToString();
        }

        protected void lnkTambahDataPengalamanKepanitiaan_Click(object sender, EventArgs e)
        {
            txtIDPengalamanKepanitiaan.Value = "";
            InitFieldsPengalamanKepanitiaan();
            txtKeyAction.Value = JenisAction.DoShowInputPengalamanKepanitiaan.ToString();
        }

        protected void lnkOKHapusPengalamanKepanitiaan_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pengalaman_kepanitiaan)
            {
                if (item.Kode.ToString().ToUpper() == txtIDPengalamanKepanitiaan.Value.Trim().ToUpper())
                {
                    lst_pengalaman_kepanitiaan.Remove(item);
                    break;
                }
            }
            txtIDPengalamanKepanitiaan.Value = "";
            SaveData();
            ShowDataDetail();
            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataPengalamanKepanitiaan.ToString();
        }

        protected void lnkOKInputPegalamanKepanitiaan_Click(object sender, EventArgs e)
        {
            if (txtIDPengalamanKepanitiaan.Value.Trim() == "")
            {
                AddDataPengalamanKepanitiaan();
            }
            else
            {
                UpdateDataPengalamanKepanitiaan();
            }
            SaveData();
        }

        protected void lnkOKInputRiwayatKesehatan_Click(object sender, EventArgs e)
        {
            if (txtIDRiwayatKesehatan.Value.Trim() == "")
            {
                AddDataRiwayatKesehatan();
            }
            else
            {
                UpdateDataRiwayatKesehatan();
            }
            SaveData();
        }

        protected void lnkOKHapusRiwayatKesehatan_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pengalaman_riwayat_kesehatan)
            {
                if (item.Kode.ToString().ToUpper() == txtIDRiwayatKesehatan.Value.Trim().ToUpper())
                {
                    lst_pengalaman_riwayat_kesehatan.Remove(item);
                    break;
                }
            }
            txtURLDeleteRiwayatKesehatan.Value = GetURLDeleteRiwayatKesehatan(txtNIKaryawan.Text, txtIDRiwayatKesehatan.Value);
            txtIDRiwayatKesehatan.Value = "";
            SaveData();
            ShowDataDetail();
            RenderListviewDetail();            
            txtKeyAction.Value = JenisAction.DoShowDataRiwayatKesehatan.ToString();
        }

        protected void lnkOKHapusRiwayatMCU_Click(object sender, EventArgs e)
        {
            foreach (var item in lst_pengalaman_riwayat_mcu)
            {
                if (item.Kode.ToString().ToUpper() == txtIDRiwayatMCU.Value.Trim().ToUpper())
                {
                    lst_pengalaman_riwayat_mcu.Remove(item);
                    break;
                }
            }
            txtURLDeleteRiwayatMCU.Value = GetURLDeleteRiwayatMCU(txtNIKaryawan.Text, txtIDRiwayatMCU.Value);
            txtIDRiwayatMCU.Value = "";
            SaveData();
            ShowDataDetail();
            RenderListviewDetail();
            txtKeyAction.Value = JenisAction.DoShowDataRiwayatMCU.ToString();
        }

        protected void lnkOKInputRiwayatMCU_Click(object sender, EventArgs e)
        {
            if (txtIDRiwayatMCU.Value.Trim() == "")
            {
                AddDataRiwayatMCU();
            }
            else
            {
                UpdateDataRiwayatMCU();
            }
            SaveData();
        }

        protected void lnkTambahDataRiwayatKesehatan_Click(object sender, EventArgs e)
        {
            txtIDRiwayatKesehatan.Value = "";
            txtIDRiwayatKesehatanNew.Value = Guid.NewGuid().ToString();
            InitFieldsPengalamanRiwayatKesehatan();
            ShowUploadedFilesRiwayatKesehatan();
            txtKeyAction.Value = JenisAction.DoShowInputRiwayatKesehatan.ToString();
        }

        protected void lnkTambahDataRiwayatMCU_Click(object sender, EventArgs e)
        {
            txtIDRiwayatMCU.Value = "";
            txtIDRiwayatMCUNew.Value = Guid.NewGuid().ToString();
            InitFieldsPengalamanRiwayatMCU();
            ShowUploadedFilesRiwayatMCU();
            txtKeyAction.Value = JenisAction.DoShowInputRiwayatMCU.ToString();
        }

        protected void btnShowDetailRiwayatKesehatan_Click(object sender, EventArgs e)
        {
            InitFieldsPengalamanRiwayatKesehatan();
            txtIDRiwayatKesehatanNew.Value = "";
            PegawaiRiwayatKesehatan m = lst_pengalaman_riwayat_kesehatan.FindAll(m0 => m0.Kode == new Guid(txtIDRiwayatKesehatan.Value)).FirstOrDefault();
            if (m != null)
            {
                if (m.DariTanggal != DateTime.MaxValue)
                {
                    txtRiwayatKesehatanDariTanggal.Text = Libs.GetTanggalIndonesiaFromDate(m.DariTanggal, false);
                    txtRiwayatKesehatanSampaiTanggal.Text = Libs.GetTanggalIndonesiaFromDate(m.SampaiTanggal, false);
                    txtNIKaryawan.Text = m.Rel_Pegawai;
                    txtRiwayatKesehatanNamaPenyakit.Text = m.NamaPenyakit;
                    txtRiwayatKesehatanNamaDokter.Text = m.Dokter;
                    chkIsIzin.Checked = m.IsIzin;
                    txtRiwayatKesehatanKlinikRumahSakit.Text = m.RSKlinik;
                    txtRiwayatKesehatanKeterangan.Text = m.Keterangan;
                }
            }
            ShowUploadedFilesRiwayatKesehatan();
            txtKeyAction.Value = JenisAction.DoShowInputRiwayatKesehatan.ToString();
        }

        protected void btnShowConfirmDeleteRiwayatKesehatan_Click(object sender, EventArgs e)
        {
            PegawaiRiwayatKesehatan m = lst_pengalaman_riwayat_kesehatan.FindAll(m0 => m0.Kode.ToString().ToUpper() == txtIDRiwayatKesehatan.Value.Trim().ToUpper()).FirstOrDefault();
            if (m != null)
            {
                if (m.DariTanggal != DateTime.MaxValue)
                {
                    ltrMsgConfirmHapusRiwayatKesehatan.Text =
                        "Anda yakin akan menghapus <br />Data riwayat kesehatan : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            Libs.GetTanggalIndonesiaFromDate(m.DariTanggal, false) + ", " + m.NamaPenyakit +
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusRiwayatKesehatan.ToString();
                }
            }
        }

        protected void btnShowDetailRiwayatMCU_Click(object sender, EventArgs e)
        {
            InitFieldsPengalamanRiwayatMCU();
            txtIDRiwayatMCUNew.Value = "";
            PegawaiRiwayatKesehatanMCU m = lst_pengalaman_riwayat_mcu.FindAll(m0 => m0.Kode == new Guid(txtIDRiwayatMCU.Value)).FirstOrDefault();
            if (m != null)
            {
                if (m.Tanggal != DateTime.MaxValue)
                {
                    txtRiwayatMCUTanggal.Text = Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false);
                    txtNIKaryawan.Text = m.Rel_Pegawai;
                    txtRiwayatMCUKesimpulan.Text = m.Kesimpulan;
                    txtRiwayatMCUSaran.Text = m.Saran;
                    txtRiwayatMCUKeterangan.Text = m.Keterangan;
                }
            }
            ShowUploadedFilesRiwayatMCU();
            txtKeyAction.Value = JenisAction.DoShowInputRiwayatMCU.ToString();
        }

        protected void btnShowConfirmDeleteRiwayatMCU_Click(object sender, EventArgs e)
        {
            PegawaiRiwayatKesehatanMCU m = lst_pengalaman_riwayat_mcu.FindAll(m0 => m0.Kode.ToString().ToUpper() == txtIDRiwayatMCU.Value.Trim().ToUpper()).FirstOrDefault();
            if (m != null)
            {
                if (m.Tanggal != DateTime.MaxValue)
                {
                    ltrMsgConfirmHapusRiwayatMCU.Text =
                        "Anda yakin akan menghapus <br />Data riwayat kesehatan : " +
                        "<br />" +
                        "<span style=\"font-weight: bold;\">" +
                            Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false) + ", " + m.Kesimpulan +
                        "</span>?";
                    txtKeyAction.Value = JenisAction.DoShowConfirmHapusRiwayatMCU.ToString();
                }
            }
        }

        protected void lnkTambahUploadFilePendukung_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowInputUploadFilePendukung.ToString();
        }

        protected void lnkOKUploadFilePendukung_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowDataFilePendukung.ToString();
        }

        protected void btnShowUploadFilePendukung_Click(object sender, EventArgs e)
        {
            ShowUploadedFilesPendukung();
            txtKeyAction.Value = JenisAction.DoShowDataListFilePendukung.ToString();
        }

        protected void btnShowUploadFilePendukungWithNotifDelete_Click(object sender, EventArgs e)
        {
            ShowUploadedFilesPendukung();
            txtKeyAction.Value = JenisAction.DoShowDataListFilePendukungWithNotifDelete.ToString();
        }
    }
}