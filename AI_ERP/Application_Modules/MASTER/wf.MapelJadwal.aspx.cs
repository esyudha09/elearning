using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_MapelJadwal : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAMAPELJADWAL";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoChangePage,
            DoShowConfirmHapus,
            DoShowPengaturan,
            DoShowPengaturanJamJadwal,
            DoShowPengaturanMapelJadwal
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
                if (t.Trim() == "") return Libs.GetTahunAjaranNow();
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                if (s.Trim() == "") return Libs.GetSemesterByTanggal(DateTime.Now).ToString();
                return s;
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

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }
            if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            {
                Libs.RedirectToBeranda(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/text-lines.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Data Jadwal Mata Pelajaran";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitInput();
                InitKeyEventClient();
                ListDropDownJam();
                ListMapelToDropDown();
            }
            if(mvMain.ActiveViewIndex == 0)
            {
                BindListView(true, Libs.GetQ());
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            //cboUnit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }

        private void InitInput()
        {
            var lst_periode = DAO_MapelJadwal.GetDistinctTahunAjaranPeriode_Entity();
            cboPeriodeJadwal.Items.Clear();
            foreach (var item in lst_periode)
            {
                cboPeriodeJadwal.Items.Add(new ListItem
                {
                    Value = item.TahunAjaran.ToString() + item.Semester.ToString(),
                    Text = item.TahunAjaran + " semester " + item.Semester
                });
            }

            if (cboPeriodeJadwal.Items.Count == 0)
            {
                cboPeriodeJadwal.Items.Add(new ListItem
                {
                    Value = Libs.GetTahunAjaranNow() + Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                    Text = Libs.GetTahunAjaranNow() + " semester " + Libs.GetSemesterByTanggal(DateTime.Now).ToString()
                });
            }

            var lst_periode_copy = DAO_MapelJadwal.GetTop20_Entity();
            cboPeriode.Items.Clear();
            cboPeriode.Items.Add("");
            foreach (var item in lst_periode_copy)
            {
                cboPeriode.Items.Add(new ListItem {
                    Value = item.Kode.ToString(),
                    Text = Libs.GetTanggalIndonesiaFromDate(item.PeriodeDariTanggal, false) +
                           HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                           Libs.GetTanggalIndonesiaFromDate(item.PeriodeSampaiTanggal, false)
                });
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            cboJenisPengaturan.SelectedValue = "";
            cboPeriode.SelectedValue = "";
            txtDariTanggal.Text = "";
            txtSampaiTanggal.Text = "";

            txtTahunAjaran.Text = GetTahunAjaran();
            txtSemester.Text = GetSemester();
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                //validasi
                if (cboJenisPengaturan.SelectedValue == "1")
                {
                    if (cboPeriode.SelectedValue.Trim() == "")
                    {
                        txtKeyAction.Value = "Periode yang dicopy belum dipilih.";
                        return;
                    }
                    else
                    {
                        MapelJadwal m0 = DAO_MapelJadwal.GetByID_Entity(cboPeriode.SelectedValue);
                        if (m0 != null)
                        {
                            if (m0.JenisPengaturan != null)
                            {
                                if (m0.PeriodeDariTanggal.DayOfWeek != Libs.GetDateFromTanggalIndonesiaStr(txtDariTanggal.Text).DayOfWeek)
                                {
                                    txtKeyAction.Value = "<br />&nbsp;&nbsp;&nbsp;Hari pada tanggal mulai periode harus sama dengan<br />&nbsp;&nbsp;&nbsp;hari pada tanggal periode yang dicopy";
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Libs.GetDateFromTanggalIndonesiaStr(txtDariTanggal.Text) > Libs.GetDateFromTanggalIndonesiaStr(txtSampaiTanggal.Text))
                    {
                        txtKeyAction.Value = "Tanggal awal harus lebih kecil dari tanggal akhir";
                        return;
                    }
                }
                //end validasi

                MapelJadwal m = new MapelJadwal();
                m.Kode = Guid.NewGuid();
                m.Rel_Kode = cboPeriode.SelectedValue;
                m.Rel_Sekolah = QS.GetUnit();
                m.TahunAjaran = txtTahunAjaran.Text;
                m.Semester = txtSemester.Text;
                m.JenisPengaturan = cboJenisPengaturan.SelectedValue;

                List<MapelJadwalDet> lst_mapel_jadwal = new List<MapelJadwalDet>();
                if (cboJenisPengaturan.SelectedValue == "0") //buat baru
                {
                    m.PeriodeDariTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtDariTanggal.Text);
                    m.PeriodeSampaiTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtSampaiTanggal.Text);

                    //generate detail
                    lst_mapel_jadwal.Clear();
                    DateTime tanggal = m.PeriodeDariTanggal;
                    for (int i = 1; i <= (Math.Abs((m.PeriodeSampaiTanggal - m.PeriodeDariTanggal).TotalDays) + 1); i++)
                    {
                        lst_mapel_jadwal.Add(new MapelJadwalDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_MapelJadwal = m.Kode.ToString(),
                            Tanggal = tanggal,
                            JamKe = 0,
                            Rel_KelasDet = "",
                            LamaMenit = 0,
                            Pukul = new DateTime(2000, 01, 01, 0, 0, 0),
                            Rel_Mapel = ""
                        });
                        tanggal = tanggal.AddDays(1);
                    }
                    //end generate detail
                }
                else if (cboJenisPengaturan.SelectedValue == "1") //copy dari sebelumnya
                {
                    MapelJadwal m0 = DAO_MapelJadwal.GetByID_Entity(cboPeriode.SelectedValue);
                    m.PeriodeDariTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtDariTanggal.Text);
                    m.PeriodeSampaiTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtDariTanggal.Text).AddDays(
                            Math.Abs((m0.PeriodeSampaiTanggal - m0.PeriodeDariTanggal).TotalDays)
                        );

                    //generate detail
                    List<MapelJadwalDet> lst_mapel_jadwal_from = DAO_MapelJadwalDet.GetAllByHeader_Entity(cboPeriode.SelectedValue);
                    double i_count = (Math.Abs((m.PeriodeSampaiTanggal - m.PeriodeDariTanggal).TotalDays) + 1);
                    lst_mapel_jadwal.Clear();
                    DateTime tanggal = m.PeriodeDariTanggal;
                    for (int i = 1; i <= i_count; i++)
                    {
                        if (i - 1 < lst_mapel_jadwal_from.Count && i - 1 >= 0)
                        {
                            Guid kode_mapel_jadwal = Guid.NewGuid();
                            lst_mapel_jadwal.Add(new MapelJadwalDet
                            {
                                Kode = kode_mapel_jadwal,
                                Rel_MapelJadwal = m.Kode.ToString(),
                                Tanggal = tanggal,
                                JamKe = lst_mapel_jadwal_from[i - 1].JamKe,
                                Rel_KelasDet = lst_mapel_jadwal_from[i - 1].Rel_KelasDet,
                                LamaMenit = lst_mapel_jadwal_from[i - 1].LamaMenit,
                                Pukul = lst_mapel_jadwal_from[i - 1].Pukul,
                                Rel_Mapel = lst_mapel_jadwal_from[i - 1].Rel_Mapel
                            });
                            tanggal = tanggal.AddDays(1);

                            List<MapelJadwalDetPukul> lst_mapel_jadwal_pukul = DAO_MapelJadwalDetPukul.GetAllByHeader_Entity(lst_mapel_jadwal_from[i - 1].Kode.ToString());
                            foreach (var item in lst_mapel_jadwal_pukul)
                            {
                                Guid kode_mapel_jadwal_det = Guid.NewGuid();
                                DAO_MapelJadwalDetPukul.Insert(new MapelJadwalDetPukul
                                {
                                    Kode = kode_mapel_jadwal_det,
                                    Rel_MapelJadwalDet = kode_mapel_jadwal.ToString(),
                                    DariJam = item.DariJam,
                                    SampaiJam = item.SampaiJam
                                });

                                List<MapelJadwalDetPukulDet> lst_mapel_jadwal_pukul_det = DAO_MapelJadwalDetPukulDet.GetAllByHeader_Entity(item.Kode.ToString());
                                foreach (var item_det in lst_mapel_jadwal_pukul_det)
                                {
                                    DAO_MapelJadwalDetPukulDet.Insert(new MapelJadwalDetPukulDet
                                    {
                                        Kode = Guid.NewGuid(),
                                        Rel_MapelJadwalDetPukul = kode_mapel_jadwal_det.ToString(),
                                        Rel_KelasDet = item_det.Rel_KelasDet,
                                        Rel_Mapel = item_det.Rel_Mapel
                                    });
                                }
                            }
                        }
                    }
                    //end generate detail
                }

                m.CopyPeriodeDariTanggal = DateTime.MinValue;
                m.CopyPeriodeSampaiTanggal = DateTime.MinValue;
                m.CreatedDate = DateTime.Now;
                m.LastUpdated = DateTime.Now;
                m.CreatedBy = Libs.LOGGED_USER_M.UserID;
                m.LastUpdatedBy = Libs.LOGGED_USER_M.UserID;
                
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_MapelJadwal.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_MapelJadwal.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }

                DAO_MapelJadwalDet.DeleteByHeader(m.Kode.ToString());
                foreach (MapelJadwalDet item in lst_mapel_jadwal)
                {
                    DAO_MapelJadwalDet.Insert(item);
                }

                InitInput();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_MapelJadwal.Delete(txtID.Value);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
                    InitInput();
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void EnableInput(bool enable)
        {
            lnkOKInput.Visible = enable;
            txtDariTanggal.Enabled = enable;
            txtSampaiTanggal.Enabled = enable;
            cboJenisPengaturan.Enabled = enable;
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            txtSemester.Text = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
            txtTahunAjaran.Text = Libs.GetTahunAjaranByTanggal(DateTime.Now);

            EnableInput(true);

            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void btnShowPengaturanJamJadwal_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                MapelJadwalDet m_det = DAO_MapelJadwalDet.GetByID_Entity(txtIDItem.Value);
                if (m_det != null)
                {
                    if (m_det.Rel_MapelJadwal != null)
                    {
                        cboDariJam_Jam.SelectedValue = "00";
                        cboDariJam_Menit.SelectedValue = "00";

                        cboSampaiJam_Jam.SelectedValue = "00";
                        cboSampaiJam_Menit.SelectedValue = "00";

                        if (txtIDItemPukul.Value.Trim() != "")
                        {
                            MapelJadwalDetPukul m_pukul = DAO_MapelJadwalDetPukul.GetByID_Entity(txtIDItemPukul.Value);
                            if (m_pukul != null)
                            {
                                if (m_pukul.Rel_MapelJadwalDet != null)
                                {
                                    cboDariJam_Jam.SelectedValue = m_pukul.DariJam.ToString("HH");
                                    cboDariJam_Menit.SelectedValue = m_pukul.DariJam.ToString("mm");

                                    cboSampaiJam_Jam.SelectedValue = m_pukul.SampaiJam.ToString("HH");
                                    cboSampaiJam_Menit.SelectedValue = m_pukul.SampaiJam.ToString("mm");
                                }
                            }
                        }

                        ltrDeskripsiPengaturanJam.Text = "<span style=\"font-size: smaller;\">" +
                                                            m_det.Tanggal.ToString("dd/MM/yyyy") +
                                                        "</span>" +
                                                        "<br />" +
                                                        "<span style=\"font-size: medium; font-weight: bold;\">" +
                                                            Libs.GetNamaHariFromTanggal(m_det.Tanggal) +
                                                        "</span>";
                        txtKeyAction.Value = JenisAction.DoShowPengaturanJamJadwal.ToString();
                    }
                }
            }
        }

        protected void lnkOKHapusPengaturanPukul_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDItemPukul.Value.Trim() == "")
                {
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                    return;
                }

                MapelJadwalDetPukul m = DAO_MapelJadwalDetPukul.GetByID_Entity(txtIDItemPukul.Value);
                if (m != null)
                {
                    if (m.Rel_MapelJadwalDet != null)
                    {
                        DAO_MapelJadwalDetPukul.Delete(txtIDItemPukul.Value);
                        RenderDetailJadwal();
                        txtKeyAction.Value = JenisAction.DoDelete.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lnkOKHapusPengaturanMapel_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDItemMapel.Value.Trim() == "")
                {
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                    return;
                }

                MapelJadwalDetPukulDet m = DAO_MapelJadwalDetPukulDet.GetByID_Entity(txtIDItemMapel.Value);
                if (m != null)
                {
                    if (m.Rel_Mapel != null)
                    {
                        DAO_MapelJadwalDetPukulDet.Delete(txtIDItemMapel.Value);
                        RenderDetailJadwal();
                        txtKeyAction.Value = JenisAction.DoDelete.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowPengaturanMapelJadwal_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDItemPukul.Value.Trim() != "" && txtIDItem.Value.Trim() != "" && txtIDRelKelasDet.Value.Trim() != "")
                {
                    MapelJadwalDet m_det = DAO_MapelJadwalDet.GetByID_Entity(txtIDItem.Value);
                    if (m_det != null)
                    {
                        if (m_det.Rel_MapelJadwal != null)
                        {
                            string s_pukul = "";
                            MapelJadwalDetPukul m_pukul = DAO_MapelJadwalDetPukul.GetByID_Entity(txtIDItemPukul.Value);
                            if (m_pukul != null)
                            {
                                if (m_pukul.Rel_MapelJadwalDet != null)
                                {
                                    string s_mapel = "";
                                    if (txtIDItemMapel.Value.Trim() != "")
                                    {
                                        MapelJadwalDetPukulDet m_jadwal_mapel = DAO_MapelJadwalDetPukulDet.GetByID_Entity(txtIDItemMapel.Value);
                                        if (m_jadwal_mapel != null)
                                        {
                                            if (m_jadwal_mapel.Rel_Mapel != null)
                                            {
                                                s_mapel = m_jadwal_mapel.Rel_Mapel;
                                            }
                                        }
                                    }
                                    cboMapelPengaturanMapel.SelectedValue = s_mapel;

                                    s_pukul = m_pukul.DariJam.ToString("HH:mm") +
                                              "&nbsp;-&nbsp;" +
                                              m_pukul.SampaiJam.ToString("HH:mm");
                                    ltrDeskripsiPengaturanMapel.Text = "<span style=\"font-size: smaller;\">" +
                                                                m_det.Tanggal.ToString("dd/MM/yyyy") +
                                                            "</span>" +
                                                            "<br />" +
                                                            "<span style=\"font-size: medium; font-weight: bold;\">" +
                                                                Libs.GetNamaHariFromTanggal(m_det.Tanggal) +
                                                            "</span> @ " +
                                                            "<span style=\"font-size: medium; font-weight: normal;\">" +
                                                                s_pukul +
                                                            "</span>" +
                                                            "<br />" +
                                                            "<span style=\"font-size: medium; font-weight: bold; color: #47079b;\">" +
                                                                DAO_KelasDet.GetByID_Entity(txtIDRelKelasDet.Value).Nama +
                                                            "</span>";
                                    txtKeyAction.Value = JenisAction.DoShowPengaturanMapelJadwal.ToString();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (txtIDItemPukul.Value.Trim() == "")
                    {
                        txtKeyAction.Value = "Jam untuk jadwal mata pelajaran belum disetting";
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                MapelJadwal m = DAO_MapelJadwal.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Rel_Kode != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus Periode : <br />" +
                                                               "<span style=\"font-weight: bold;\">\"" +
                                                                    Libs.GetTanggalIndonesiaFromDate(m.PeriodeDariTanggal, false) +
                                                                    " - " +
                                                                    Libs.GetTanggalIndonesiaFromDate(m.PeriodeSampaiTanggal, false) +
                                                              "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, "");
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                EnableInput(false);
                MapelJadwal m = DAO_MapelJadwal.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Rel_Kode != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtTahunAjaran.Text = m.TahunAjaran;
                        txtSemester.Text = m.Semester;
                        cboJenisPengaturan.SelectedValue = m.JenisPengaturan;
                        cboJenisPengaturan.Enabled = false;
                        txtDariTanggal.Text = Libs.GetTanggalIndonesiaFromDate(m.PeriodeDariTanggal, false);
                        txtSampaiTanggal.Text = Libs.GetTanggalIndonesiaFromDate(m.PeriodeSampaiTanggal, false);
                        cboPeriode.SelectedValue = m.Rel_Kode;

                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void ListDropDownJam()
        {
            cboDariJam_Jam.Items.Clear();
            cboDariJam_Menit.Items.Clear();

            cboSampaiJam_Jam.Items.Clear();
            cboSampaiJam_Menit.Items.Clear();

            for (int i = 0; i < 24; i++)
            {
                cboDariJam_Jam.Items.Add(new ListItem {
                    Value = (i < 10 ? "0" : "") + i.ToString()
                });
                cboSampaiJam_Jam.Items.Add(new ListItem
                {
                    Value = (i < 10 ? "0" : "") + i.ToString()
                });
            }

            for (int i = 0; i < 60; i++)
            {
                cboDariJam_Menit.Items.Add(new ListItem
                {
                    Value = (i < 10 ? "0" : "") + i.ToString()
                });
                cboSampaiJam_Menit.Items.Add(new ListItem
                {
                    Value = (i < 10 ? "0" : "") + i.ToString()
                });
            }
        }

        protected void ListMapelToDropDown()
        {
            cboMapelPengaturanMapel.Items.Clear();
            cboMapelPengaturanMapel.Items.Add("");
            List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(QS.GetUnit()).OrderBy(m0 => m0.Nama).ToList();
            foreach (var item_mapel in lst_mapel)
            {
                cboMapelPengaturanMapel.Items.Add(new ListItem {
                    Value = item_mapel.Kode.ToString(),
                    Text = item_mapel.Nama
                });
            }
        }

        protected string GetHTMLCellMapel(string rel_mapeljadwaldet, string rel_mapeljadwaldetpukul, string rel_mapeljadwaldetmapel, string rel_kelasdet, string s_mapel)
        {
            string s_html = "<label onclick=\"" + txtIDItem.ClientID + ".value ='" + rel_mapeljadwaldet + "'; "  + 
                                                  txtIDItemPukul.ClientID + ".value = '" + rel_mapeljadwaldetpukul + "'; " +
                                                  txtIDItemMapel.ClientID + ".value = '" + rel_mapeljadwaldetmapel + "'; " +
                                                  txtIDRelKelasDet.ClientID + ".value='" + rel_kelasdet + "'; " + 
                                                  btnShowPengaturanMapelJadwal.ClientID + ".click();\" " +
                                    "style=\"cursor: pointer; padding: 5px; padding-bottom: 1px; padding-top: 7px; border-width: 0px; border-style: dashed; border-color: #d9d9d9; border-radius: 5px;\">" +
                                //"<span style=\"color: #d9d9d9; cursor: pointer; font-size: 24pt; font-weight: normal;\">+</span>" +
                                (
                                    s_mapel.Trim() != ""
                                    ? s_mapel
                                    : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                ) +
                            "</label>";



            return s_html;
        }

        protected void RenderDetailJadwal()
        {
            ltrDetailJadwal.Text = "";
            MapelJadwal m = DAO_MapelJadwal.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.Rel_Kode != null)
                {
                    List<DAO_MapelJadwalDetPukulDet.MapelJadwalDetPukulDet_Lengkap> lst_mapel_jadwal_det_mapel =
                        DAO_MapelJadwalDetPukulDet.GetAllByJadwal_Entity(m.Kode.ToString());

                    string judul = "<div style=\"padding: 0px; color: #0e6e64;\">" +
                                       "<i class=\"fa fa-info-circle\" style=\"color: #0e6e64;\"></i>&nbsp;&nbsp;" +
                                       "<span style=\"font-weight: normal;\">Detail Jadwal Periode : </span>" +
                                       "<span style=\"font-weight: bold;\">" +
                                            Libs.GetTanggalIndonesiaFromDate(m.PeriodeDariTanggal, false) +
                                            "&nbsp;-&nbsp;" +
                                            Libs.GetTanggalIndonesiaFromDate(m.PeriodeSampaiTanggal, false) +
                                       "</span>" +
                                   "</div>";

                    string css_cell = "padding: 10px; border-style: solid; border-width: 1px; border-color: #dddddd; text-align: center; vertical-align: middle; ";
                    string css_cell_border_right_header = "box-shadow: inset -2px 0px 0 green, inset 0 -2px 0 #d5d5d5; ";
                    string css_cell_border_right = "box-shadow: inset -2px 0px 0 green, inset 0 -2px 0 white; ";
                    string css_cell_border_right_no_box_shadow = "box-shadow: inset -2px 0px 0 green, inset 0 0px 0 white; ";
                    string html_konten = "<table style=\"margin: 0px; padding: 0px; margin-top: 41px;\">";
                    List<MapelJadwalDet> lst_mapeljadwal_det = DAO_MapelJadwalDet.GetAllByHeader_Entity(m.Kode.ToString()).OrderBy(m0 => m0.Tanggal).ToList();

                    string s_kelas = "";
                    int i_count_kelas = 0;
                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(QS.GetUnit()).FindAll(m0 => m0.IsAktif == true).OrderBy(m0 => m0.UrutanLevel).ToList();
                    List<KelasDet> lst_kelas_det = DAO_KelasDet.GetBySekolah_Entity(QS.GetUnit()).FindAll(m0 => m0.IsKelasJurusan == false && m0.IsKelasSosialisasi == false && m0.IsAktif == true);
                    foreach (Kelas m_kelas in lst_kelas)
                    {
                        List<KelasDet> lst_item_kelas_det = lst_kelas_det.FindAll(m0 => m0.Rel_Kelas.ToString().ToUpper() == m_kelas.Kode.ToString().ToUpper()).OrderBy(m0 => m0.UrutanKelas).ToList();
                        foreach (KelasDet m_kelas_det in lst_item_kelas_det)
                        {
                            s_kelas += "<th style=\"" +
                                                css_cell + 
                                                " background-color: #ffffff; text-align: center; " +
                                                " position: sticky; top: 81px; box-shadow: inset -0.5px 0px 0 #d5d5d5, inset 0 -2px 0 #d5d5d5; " +
                                                " z-index: 99998; " +
                                            "\">" +
                                            "<span style=\"font-weight: bold; color: black;\">" +
                                                m_kelas_det.Nama.Trim() +
                                            "</span>" +
                                        "</th>";
                            i_count_kelas++;
                        }
                    }
                    html_konten += "<thead>" +
                                        "<tr>" +
                                            "<th colspan=\"" + (i_count_kelas + 2).ToString() + "\" style=\"" +
                                                            css_cell +
                                                            " background-color: #ffffff; background-color: #bfede8; text-align: left; " +
                                                            " position: fixed; top: 55px; left: 0px; right: 0px; z-index: 9999999; " +
                                                        "\">" +
                                                "<span style=\"font-weight: bold; color: black;\">" +
                                                    judul +
                                                "</span>" +
                                            "</th>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<th rowspan=\"2\" style=\"" +
                                                            css_cell + 
                                                            " background-color: #ffffff; text-align: center; " +
                                                            " position: sticky; top: 41px; left: 0px; box-shadow: inset -0.5px 0px 0 #d5d5d5, inset 0 -2px 0 #d5d5d5; " +
                                                            " min-width: 100px; max-width: 100px; width: 100px; " +
                                                            " z-index: 99999; " +
                                                        "\">" +
                                                "<span style=\"font-size: medium; font-weight: bold; color: black;\">" +
                                                    "Hari" +
                                                "</span>" +
                                            "</th>" +
                                            "<th rowspan=\"2\" style=\"" +
                                                            css_cell + 
                                                            " background-color: #ffffff; text-align: center; " +
                                                            " position: sticky; top: 41px; left: 99px; box-shadow: 0 2px 2px -1px rgba(0, 0, 0, 0.4); " +
                                                            " min-width: 120px; max-width: 120px; width: 120px; " +
                                                            " z-index: 99999; " +
                                                            css_cell_border_right_header +
                                                        "\">" +
                                                "<span style=\"font-size: medium; font-weight: bold; color: black;\">" +
                                                    "Pukul" +
                                                "</span>" +
                                            "</th>" +
                                            "<th colspan=\"" + i_count_kelas.ToString() + "\" style=\"" +
                                                            css_cell + 
                                                            " background-color: #ffffff; text-align: center; " +
                                                            " position: sticky; top: 41px; box-shadow: -1px 4px 2px -1px rgba(0, 0, 0, 0.4); " +
                                                            " z-index: 99998; " +
                                                        "\">" +
                                                "<span style=\"font-weight: bold; color: black;\">" +
                                                    "Kelas" +
                                                "</span>" +
                                            "</th>" +
                                       "</tr>" +
                                       "<tr>" +
                                            s_kelas +
                                       "</tr>";
                    html_konten += "</thead>";
                    html_konten += "<tbody>";
                    foreach (var item in lst_mapeljadwal_det)
                    {
                        List<MapelJadwalDetPukul> lst_mapel_jadwal_det_pukul = DAO_MapelJadwalDetPukul.GetAllByHeader_Entity(item.Kode.ToString());

                        s_kelas = "";
                        foreach (Kelas m_kelas in lst_kelas)
                        {
                            List<KelasDet> lst_item_kelas_det = lst_kelas_det.FindAll(m0 => m0.Rel_Kelas.ToString().ToUpper() == m_kelas.Kode.ToString().ToUpper()).OrderBy(m0 => m0.UrutanKelas).ToList();
                            foreach (KelasDet m_kelas_det in lst_item_kelas_det)
                            {

                                var m_mapel_jadwal_mapel = lst_mapel_jadwal_det_mapel.FindAll(
                                        m0 => m0.Rel_KelasDet.Trim().ToUpper() == m_kelas_det.Kode.ToString().ToUpper().Trim() &&
                                              m0.Rel_MapelJadwalDetPukul.Trim().ToUpper() ==
                                              (
                                                   lst_mapel_jadwal_det_pukul.Count > 0
                                                   ? lst_mapel_jadwal_det_pukul[0].Kode.ToString().ToUpper().Trim()
                                                   : ""
                                               )
                                    ).FirstOrDefault();
                                string s_caption_mapel = "";
                                string s_kode_jadwal_mapel = "";
                                if (m_mapel_jadwal_mapel != null)
                                {
                                    if (m_mapel_jadwal_mapel.Mapel != null)
                                    {
                                        s_kode_jadwal_mapel = m_mapel_jadwal_mapel.Kode.ToString();
                                        s_caption_mapel = (
                                                            m_mapel_jadwal_mapel.AliasMapel.Trim() != ""
                                                            ? m_mapel_jadwal_mapel.AliasMapel
                                                            : m_mapel_jadwal_mapel.Mapel
                                                        );
                                    }
                                }
                                string s_mapel = GetHTMLCellMapel(
                                       item.Kode.ToString(),
                                       (
                                           lst_mapel_jadwal_det_pukul.Count > 0
                                           ? lst_mapel_jadwal_det_pukul[0].Kode.ToString()
                                           : ""
                                       ),
                                       s_kode_jadwal_mapel,
                                       m_kelas_det.Kode.ToString(),
                                       s_caption_mapel
                                   );

                                s_kelas += "<td style=\"" +
                                                    css_cell +
                                                    (
                                                        Libs.GetIsHariLibur(item.Tanggal) || item.IsLibur
                                                        ? "background-color: #ffb6b6; color: black; "
                                                        : ""
                                                    ) +
                                                "\">" +
                                                (s_mapel.Trim() == "" ? "&nbsp;" : s_mapel) +
                                            "</td>";
                            }
                        }

                        string chk_id = "chk_" + item.Kode.ToString().Replace("-", "_");
                        html_konten += "<tr>" +
                                            "<td \"" + (
                                                        lst_mapel_jadwal_det_pukul.Count > 1
                                                        ? " rowspan=\"" + lst_mapel_jadwal_det_pukul.Count.ToString() + "\" "
                                                        : ""
                                                      ) +
                                               "\" " +
                                                "style=\"" + 
                                                            css_cell +
                                                            (
                                                                Libs.GetIsHariLibur(item.Tanggal) || item.IsLibur
                                                                ? "background-color: red; color: white; "
                                                                : "background-color: white; colorL black; "
                                                            ) + 
                                                            " min-width: 100px; max-width: 100px; width: 100px; " +
                                                            " position: sticky; left: 0px; z-index: 9997; box-shadow: inset -0.5px 0px 0 #d5d5d5, inset 0 -1px 0 #d5d5d5; " +
                                                      "\">" +
                                                "<span style=\"font-size: smaller;\">" +
                                                    item.Tanggal.ToString("dd/MM/yyyy") +
                                                "</span>" +
                                                "&nbsp;" +
                                                "<label onclick=\"" + txtIDItem.ClientID + ".value = '" + item.Kode.ToString() + "'; " + txtIDItemPukul.ClientID + ".value = ''; " + btnShowPengaturanJamJadwal.ClientID + ".click();\" " +
                                                       "title=\"Tambah Pengaturan\" " +
                                                       "style=\"" + (Libs.GetIsHariLibur(item.Tanggal) || item.IsLibur ? "display: none; " : "") + 
                                                                   "float: right; margin-top: -4px; margin-right: -6px; cursor: pointer; font-size: xx-small; background-color: white; color: green; " +
                                                                   "border-radius: 10px; padding: 6px; padding-bottom: 0px; padding-top: 1px; vertical-align: middle;\">" +
                                                       "<i class=\"fa fa-plus\"></i>" +
                                                "</label>" +
                                                "<br />" +
                                                "<span style=\"font-size: medium; font-weight: bold;\">" +
                                                    Libs.GetNamaHariFromTanggal(item.Tanggal) +
                                                "</span>" +
                                                (
                                                    !Libs.GetIsHariLibur(item.Tanggal) || item.IsLibur
                                                    ? "<div class=\"checkbox checkbox-adv\" style=\"margin-left: 7px;\">" +
                                                            "<label for=\"" + chk_id + "\" style=\"padding-left: 0px;\">" +
                                                                "<input  name=\"chk_pilih_siswa_rapor[]\" " +
                                                                        "value=\"" + item.Kode.ToString() + "\" " +
                                                                        "class=\"access-hide\" id=\"" + chk_id + "\" " +
                                                                        "onchange=\"" + txtIDItem.ClientID + ".value = '" + item.Kode.ToString() + "'; " + txtIDItemIsLibur.ClientID + ".value = (this.checked ? '1' : '0'); " + btnUpdateIsLibur.ClientID + ".click();\" " +
                                                                        "type=\"checkbox\" " +
                                                                        (item.IsLibur ? "checked=\"checked\" " : "") + " >" +
                                                                "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                                "<span style=\"font-weight: normal; font-size: 12px; color: black;\">" +
                                                                    "Libur" +
                                                                "</span>" +
                                                            "</label>" +
                                                      "</div>"
                                                    : "<br />&nbsp;"
                                                ) +
                                            "</td>" +
                                            "<td style=\"" +
                                                    css_cell +
                                                    (
                                                        Libs.GetIsHariLibur(item.Tanggal) || item.IsLibur
                                                        ? "background-color: #ffb6b6; color: black; " +
                                                          css_cell_border_right_no_box_shadow
                                                        : css_cell_border_right +
                                                          "background-color: white; "
                                                    ) +
                                                    " min-width: 100px; max-width: 100px; width: 100px; " +
                                                    " position: sticky; left: 99px; z-index: 9997; " +
                                                    " box-shadow: inset -2px 0px 0 green, inset 0 -1px 0 #d5d5d5; " +
                                                "\">" +
                                                (
                                                    lst_mapel_jadwal_det_pukul.Count > 0
                                                    ? "<label style=\"cursor: pointer; font-weight: bold; color: grey;\" onclick=\"" + txtIDItem.ClientID + ".value = '" + item.Kode.ToString() + "'; " +
                                                                            txtIDItemPukul.ClientID + ".value = '" + lst_mapel_jadwal_det_pukul[0].Kode.ToString() + "'; " +
                                                                            btnShowPengaturanJamJadwal.ClientID + ".click();\">" +
                                                          lst_mapel_jadwal_det_pukul[0].DariJam.ToString("HH:mm") +
                                                          "&nbsp;-&nbsp;" +
                                                          lst_mapel_jadwal_det_pukul[0].SampaiJam.ToString("HH:mm") +
                                                      "</label>"
                                                    : "&nbsp;"
                                                ) +
                                            "</td>" +
                                            s_kelas +
                                       "</tr>";

                        if (lst_mapel_jadwal_det_pukul.Count > 1)
                        {
                            for (int i = 1; i < lst_mapel_jadwal_det_pukul.Count; i++)
                            {
                                s_kelas = "";
                                foreach (Kelas m_kelas in lst_kelas)
                                {
                                    List<KelasDet> lst_item_kelas_det = lst_kelas_det.FindAll(m0 => m0.Rel_Kelas.ToString().ToUpper() == m_kelas.Kode.ToString().ToUpper()).OrderBy(m0 => m0.UrutanKelas).ToList();
                                    foreach (KelasDet m_kelas_det in lst_item_kelas_det)
                                    {
                                        var m_mapel_jadwal_mapel = lst_mapel_jadwal_det_mapel.FindAll(
                                            m0 => m0.Rel_KelasDet.Trim().ToUpper() == m_kelas_det.Kode.ToString().ToUpper().Trim() &&
                                                    m0.Rel_MapelJadwalDetPukul.Trim().ToUpper() == lst_mapel_jadwal_det_pukul[i].Kode.ToString().ToUpper().Trim()
                                        ).FirstOrDefault();
                                            string s_caption_mapel = "";
                                            string s_kode_jadwal_mapel = "";
                                            if (m_mapel_jadwal_mapel != null)
                                            {
                                                if (m_mapel_jadwal_mapel.Mapel != null)
                                                {
                                                    s_kode_jadwal_mapel = m_mapel_jadwal_mapel.Kode.ToString();
                                                    s_caption_mapel = (
                                                            m_mapel_jadwal_mapel.AliasMapel.Trim() != ""
                                                            ? m_mapel_jadwal_mapel.AliasMapel
                                                            : m_mapel_jadwal_mapel.Mapel
                                                        );
                                                }
                                            }
                                            string s_mapel = GetHTMLCellMapel(
                                                    item.Kode.ToString(),
                                                    (
                                                        lst_mapel_jadwal_det_pukul.Count > 0
                                                        ? lst_mapel_jadwal_det_pukul[i].Kode.ToString()
                                                        : ""
                                                    ),
                                                    s_kode_jadwal_mapel,
                                                    m_kelas_det.Kode.ToString(),
                                                    s_caption_mapel
                                                );

                                        s_kelas += "<td style=\"" +
                                                            css_cell +
                                                            (
                                                                Libs.GetIsHariLibur(item.Tanggal) || item.IsLibur
                                                                ? "background-color: #ffb6b6; color: black; "
                                                                : ""
                                                            ) +
                                                        "\">" +
                                                        (s_mapel.Trim() == "" ? "&nbsp;" : s_mapel) +
                                                    "</td>";
                                    }
                                }

                                html_konten += "<tr>" +
                                                    "<td style=\"" +
                                                            css_cell +
                                                            (
                                                                Libs.GetIsHariLibur(item.Tanggal) || item.IsLibur
                                                                ? "background-color: #ffb6b6; color: black; " +
                                                                  css_cell_border_right_no_box_shadow
                                                                : css_cell_border_right +
                                                                  "background-color: white; "
                                                            ) +
                                                            " min-width: 100px; max-width: 100px; width: 100px; " +
                                                            " position: sticky; left: 99px; z-index: 9997; " +
                                                            " box-shadow: inset -2px 0px 0 green, inset 0 -1px 0 #d5d5d5; " +
                                                        "\">" +
                                                        (
                                                            lst_mapel_jadwal_det_pukul.Count > 0
                                                            ? "<label onclick=\"" + txtIDItem.ClientID + ".value = '" + item.Kode.ToString() + "'; " +
                                                                                    txtIDItemPukul.ClientID + ".value = '" + lst_mapel_jadwal_det_pukul[i].Kode.ToString() + "'; " +
                                                                                    btnShowPengaturanJamJadwal.ClientID + ".click();\" " +
                                                                      "style=\"cursor: pointer; font-weight: bold; color: grey; \">" +
                                                                      lst_mapel_jadwal_det_pukul[i].DariJam.ToString("HH:mm") +
                                                                      "&nbsp;-&nbsp;" +
                                                                      lst_mapel_jadwal_det_pukul[i].SampaiJam.ToString("HH:mm") +
                                                              "</label>"
                                                            : "&nbsp;"
                                                        ) +
                                                    "</td>" +
                                                    s_kelas +
                                               "</tr>";
                            }
                        }
                    }
                    html_konten += "</tbody>";
                    html_konten += "</table>";

                    string konten = "<div id=\"div_det\" style=\"overflow-x: auto; overflow-y: auto; position: fixed; top: 55px; left: 0px; right: 0px; bottom: 0px;\">" +
                                        html_konten +
                                    "</div>";

                    ltrDetailJadwal.Text = konten;
                }
            }
        }

        protected void btnShowDetailJadwal_Click(object sender, EventArgs e)
        {
            RenderDetailJadwal();
            mvMain.ActiveViewIndex = 1;
            UpdateView();
        }

        protected void btnUpdateIsLibur_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDItem.Value.Trim() != "" && txtIDItemIsLibur.Value.Trim() != "")
                {
                    DAO_MapelJadwalDet.UpdateIsLIbur(txtIDItem.Value, (txtIDItemIsLibur.Value.Trim() == "1" ? true : false));
                    RenderDetailJadwal();
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnKembaliKeList_Click(object sender, EventArgs e)
        {
            InitInput();
            mvMain.ActiveViewIndex = 0;
            UpdateView();
        }

        public void lnkOKPengaturanMapel_Click(object sender, EventArgs e)
        {
            try
            {
                MapelJadwalDetPukulDet m = new MapelJadwalDetPukulDet();
                m.Kode = Guid.NewGuid();
                m.Rel_MapelJadwalDetPukul = txtIDItemPukul.Value;
                m.Rel_Mapel = cboMapelPengaturanMapel.SelectedValue;
                m.Rel_KelasDet = txtIDRelKelasDet.Value;
                if (txtIDItemMapel.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtIDItemMapel.Value);
                    DAO_MapelJadwalDetPukulDet.Update(m);
                    RenderDetailJadwal();
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_MapelJadwalDetPukulDet.Insert(m);
                    RenderDetailJadwal();
                    txtKeyAction.Value = JenisAction.DoAdd.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lnkOKPengaturanJam_Click(object sender, EventArgs e)
        {
            try
            {
                MapelJadwalDetPukul m = new MapelJadwalDetPukul();
                m.Kode = Guid.NewGuid();
                if (txtIDItem.Value.Trim() != "")
                {
                    m.Rel_MapelJadwalDet = txtIDItem.Value;
                    m.DariJam = new DateTime(2000, 1, 1, Libs.GetStringToInteger(cboDariJam_Jam.SelectedValue), Libs.GetStringToInteger(cboDariJam_Menit.SelectedValue), 0);
                    m.SampaiJam = new DateTime(2000, 1, 1, Libs.GetStringToInteger(cboSampaiJam_Jam.SelectedValue), Libs.GetStringToInteger(cboSampaiJam_Menit.SelectedValue), 0);
                    if (txtIDItemPukul.Value.Trim() != "")
                    {
                        m.Kode = new Guid(txtIDItemPukul.Value);
                        DAO_MapelJadwalDetPukul.Update(m);
                        RenderDetailJadwal();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                    else
                    {
                        DAO_MapelJadwalDetPukul.Insert(m);
                        RenderDetailJadwal();
                        txtKeyAction.Value = JenisAction.DoAdd.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 20)).ToString());
            pageindex--;
            this.Session[SessionViewDataName] = (pageindex < 0 ? 0 : pageindex);
        }

        protected string GetTahunAjaran()
        {
            string tahun_ajaran = Libs.GetTahunAjaranNow();
            
            if (cboPeriodeJadwal.Items.Count > 0)
            {
                if (cboPeriodeJadwal.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriodeJadwal.SelectedValue.Substring(0, 9);
                }
            }

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    tahun_ajaran = periode.Substring(0, 9);
                }
            }

            return tahun_ajaran;
        }

        protected string GetSemester()
        {
            string semester = Libs.GetSemesterByTanggal(DateTime.Now).ToString();

            if (cboPeriodeJadwal.Items.Count > 0)
            {
                if (cboPeriodeJadwal.SelectedValue.Trim() != "")
                {
                    semester = cboPeriodeJadwal.SelectedValue.Substring(cboPeriodeJadwal.SelectedValue.Length - 1, 1);
                }
            }

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    semester = periode.Substring(periode.Length - 1, 1);
                }
            }

            return semester;
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = GetTahunAjaran();
            string semester = GetSemester();

            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            sql_ds.SelectParameters.Clear();
            sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
            sql_ds.SelectParameters.Add("Semester", semester);
            sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
            sql_ds.SelectCommand = DAO_MapelJadwal.SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_SEKOLAH;

            UpdateView();
            if (isbind) lvData.DataBind();
        }

        protected void UpdateView()
        {
            if (mvMain.ActiveViewIndex == 0)
            {
                div_container_main.Attributes["class"] = "col-md-8 col-md-offset-2";
                div_card_inner.Attributes["style"] = "margin: 0px; padding: 0px; margin-right: -0.5px; padding: 0px;";
                tbl_caption.Attributes["style"] = "width: 100%;";
                div_card_main.Attributes["style"] = "margin-top: 40px; ";
            }
            else if(mvMain.ActiveViewIndex == 1)
            {
                div_container_main.Attributes["class"] = "col-md-12";
                div_card_inner.Attributes["style"] = "margin: 0px; padding: 0px; margin-right: -0.5px; padding: 10px;";
                tbl_caption.Attributes["style"] = "display: none;";
                div_card_main.Attributes["style"] = "border-style: none; ";
            }
        }

        protected void lnkOKPengaturanJadwal_Click(object sender, EventArgs e)
        {

        }

        protected void btnDoPengaturan_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowPengaturan.ToString();
        }
    }
}