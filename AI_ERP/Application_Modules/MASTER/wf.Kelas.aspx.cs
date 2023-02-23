using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using AI_ERP.Application_Libs;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_Kelas : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAKELAS";

        private static List<KelasDet> lst_item_kelas = new List<KelasDet>();
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
            ShowDataList,
            ShowDataListWithUpdate,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            DoShowEditKelas,
            ItemKelasDetKosong
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this);
            }

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                SetTitle("Level Kelas");
                lst_item_kelas.Clear();
                ClearItemKelas();
            }
            if (!(IsPostBack || this.Session[SessionViewDataName] == null))
            {
                dpData.SetPageProperties((int)this.Session[SessionViewDataName], dpData.MaximumRows, true);
            }

            InitKeyEventClient();
            BindListView(!IsPostBack, Libs.GetQ().Trim());
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        private void SetTitle(string teks)
        {
            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/folder-1.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       teks;
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKCari.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Kelas.SP_SELECT_ALL_FOR_SEARCH;
                sql_ds.SelectParameters.Add("nama", keyword);
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Kelas.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListView()
        {
            lvData.DataSource = null;
            lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_sekolah = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_sekolah");
            System.Web.UI.WebControls.Literal imgh_nama = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_nama");
            System.Web.UI.WebControls.Literal imgh_urutanlevel = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_urutanlevel");
            System.Web.UI.WebControls.Literal imgh_keterangan = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_keterangan");
            System.Web.UI.WebControls.Literal imgh_aktif = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_aktif");

            string html_image = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-up\" style=\"color: white;\"></i>";
            }
            else
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-down\" style=\"color: white;\"></i>";
            }

            imgh_sekolah.Text = html_image;
            imgh_nama.Text = html_image;
            imgh_urutanlevel.Text = html_image;
            imgh_keterangan.Text = html_image;
            imgh_aktif.Text = html_image;

            imgh_sekolah.Visible = false;
            imgh_nama.Visible = false;
            imgh_urutanlevel.Visible = false;
            imgh_keterangan.Visible = false;
            imgh_aktif.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Sekolah":
                    imgh_sekolah.Visible = true;
                    break;
                case "Nama":
                    imgh_nama.Visible = true;
                    break;
                case "UrutanLevel":
                    imgh_urutanlevel.Visible = true;
                    break;                
                case "Keterangan":
                    imgh_keterangan.Visible = true;
                    break;
                case "IsAktif":
                    imgh_aktif.Visible = true;
                    break;
            }

            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 50)).ToString());
            pageindex--;
            this.Session[SessionViewDataName] = (pageindex < 0 ? 0 : pageindex);
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDItemKelas.Value.Trim() == "")
                {
                    lst_item_kelas.Add(new KelasDet
                    {
                        Kode = Guid.NewGuid(),
                        Nama = txtNamaItemKelas.Text,
                        UrutanKelas = Convert.ToInt16(cboNoUrutKelas.SelectedValue),
                        Keterangan = txtKeteranganKelas.Text,
                        IsKelasJurusan = (rdoKelasRombel.Checked ? false : rdoKelasJurusan.Checked),
                        IsKelasSosialisasi = (rdoKelasRombel.Checked ? false : rdoKelasSosialisasi.Checked),
                        IsAktif = chkAktif.Checked
                    });
                }
                else
                {
                    int id = lst_item_kelas.FindIndex(m => m.Kode == new Guid(txtIDItemKelas.Value));
                    if (id >= 0)
                    {
                        lst_item_kelas[id] = new KelasDet
                        {
                            Kode = new Guid(txtIDItemKelas.Value),
                            Nama = txtNamaItemKelas.Text,
                            UrutanKelas = Convert.ToInt16(cboNoUrutKelas.SelectedValue),
                            Keterangan = txtKeteranganKelas.Text,
                            IsKelasJurusan = (rdoKelasRombel.Checked ? false : rdoKelasJurusan.Checked),
                            IsKelasSosialisasi = (rdoKelasRombel.Checked ? false : rdoKelasSosialisasi.Checked),
                            IsAktif = chkAktif.Checked
                        };
                    }
                }

                RenderItemKelas();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void RenderItemKelas()
        {
            if (lst_item_kelas.Count == 0)
            {
                ClearItemKelas();
            }
            else
            {
                lst_item_kelas = lst_item_kelas.OrderBy(m => m.UrutanKelas).ThenBy(m => m.Nama).ToList();
                ltrItemKelas.Text = "";
                int id = 1;
                foreach (KelasDet item in lst_item_kelas)
                {
                    string jenis_kelas = "Kelas Perwalian";
                    string warna_kelas = "green";
                    if (item.IsKelasJurusan)
                    {
                        jenis_kelas = "Kelas Jurusan";
                        warna_kelas = "darkorange";
                    }
                    else if (item.IsKelasSosialisasi)
                    {
                        jenis_kelas = "Kelas Sosialisasi";
                        warna_kelas = "red";
                    }

                    ltrItemKelas.Text += "<tr>" +
                                            "<td style=\"text-align: left; padding: 15px; background-color: white;\">" +
                                                "<div class=\"checkbox checkbox-adv pull-left\">" +
                                                    "<label for=\"" + item.Kode.ToString().Replace("-", "_") + "\" style=\"font-weight: bold;\">" +
                                                        "<input class=\"access-hide\" id=\"" + item.Kode.ToString().Replace("-", "_") + "\" name=\"CHKITEMKELAS[]\" type=\"checkbox\">" +
                                                        "&nbsp;&nbsp;&nbsp;" +
                                                        item.Nama +
                                                        "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                    "</label>" +
                                                "</div>" +
                                                "<label onclick=\"" + txtIDItemKelas.ClientID + ".value = '" + item.Kode.ToString() + "'; " + btnDoEditItemKelas.ClientID + ".click();\" class=\"pull-left\" style=\"float: right; cursor: pointer; color: #1DA1F2;\" title=\" Edit \">" +
                                                    "&nbsp;&nbsp;&nbsp;" +
                                                    "<i class=\"fa fa-pencil\"></i>" +
                                                    "&nbsp;&nbsp;&nbsp;" +
                                                "</label>" +
                                            "</td>" +
                                            "<td style=\"text-align: center; padding: 15px; background-color: white; color: grey;\">" +
                                                "# <label>" + item.UrutanKelas.ToString() + "</label>" +
                                            "</td>" +
                                            "<td style=\"text-align: center; padding: 15px; background-color: white; color: " + warna_kelas + "; font-style: italic;\">" +
                                                jenis_kelas +
                                            "</td>" +
                                            "<td style=\"text-align: center; padding: 15px; background-color: white; color: grey;\">" +
                                                (
                                                    item.IsAktif
                                                    ? "<i class='fa fa-check'></i>"
                                                    : ""
                                                ) +
                                            "</td>" +
                                            "<td style=\"text-align: left; padding: 15px; background-color: white; \">" +
                                                item.Keterangan +
                                            "</td>" +
                                         "</tr>" +
                                         (
                                            id < lst_item_kelas.Count
                                            ? "<tr>" +
                                                "<td colspan=\"5\" style=\"margin: 0px; padding: 0px;\" ><hr style=\"margin: 0px; padding: 0px; border-color: #e9e7e7;\" /></td>" +
                                             "</tr>"
                                            : ""
                                         );
                    id++;
                }
            }
        }

        protected void ClearItemKelas()
        {
            ltrItemKelas.Text = "<tr>" +
                                    "<td colspan=\"5\" style=\"text-align: center; background-color: white; padding-left: 0px; padding-right: 0px; font-weight: normal; padding-top: 15px;\">" +
                                        "..:: Data Kosong ::.." +
                                        "<br /><br />" +
                                        //"<hr style=\"margin: 0px;\" />" +
                                    "</td>" +
                                "</tr>";
        }

        protected void InitFields()
        {
            ClearItemKelas();

            cboUnitSekolah.Items.Clear();
            cboUnitSekolah.Items.Add("");
            foreach (Sekolah item in DAO_Sekolah.GetAll_Entity())
            {
                cboUnitSekolah.Items.Add(
                    new ListItem
                    {
                        Value = item.Kode.ToString(),
                        Text = item.Nama
                    }
                );
            }

            txtNamaLevel.Text = "";
            txtKeterangan.Text = "";
            chkIsAktifKelas.Checked = true;
            rdoKelasJurusan.Checked = false;
            rdoKelasSosialisasi.Checked = false;

            lst_item_kelas.Clear();

            cboUrutanLevel.Items.Clear();
            cboUrutanLevel.Items.Add("");
            cboUrutanLevel.Items.Add("0");
            for (int i = 1; i <= 50; i++)
            {
                cboUrutanLevel.Items.Add(i.ToString());
            }
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            txtID.Value = "";
            InitFields();
            ShowInputKelas();
        }

        protected void ShowInputKelas()
        {
            SetTitle("Pengaturan Kelas dalam Level");
            mvMain.ActiveViewIndex = 1;
            div_button_settings.Visible = false;
        }

        protected void ShowListKelas()
        {
            SetTitle("Data Level Kelas");
            mvMain.ActiveViewIndex = 0;
            div_button_settings.Visible = true;
        }

        protected void btnCancelSave_Click(object sender, EventArgs e)
        {
            ShowListKelas();
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (lst_item_kelas.Count == 0)
            {
                txtKeyAction.Value = JenisAction.ItemKelasDetKosong.ToString();
                return;
            }

            //simpan pengaturan kelas
            if (txtID.Value.Trim() == "")
            {
                DAO_Kelas.Insert(new Kelas
                {
                    Rel_Sekolah = new Guid(cboUnitSekolah.SelectedValue),
                    Nama = txtNamaLevel.Text,
                    UrutanLevel = Convert.ToInt16(cboUrutanLevel.Text),
                    Keterangan = txtKeterangan.Text,
                    IsAktif = chkIsAktifKelas.Checked
                }, lst_item_kelas);
            }
            else
            {
                DAO_Kelas.Update(new Kelas
                {
                    Kode = new Guid(txtID.Value),
                    Rel_Sekolah = new Guid(cboUnitSekolah.SelectedValue),
                    Nama = txtNamaLevel.Text,
                    UrutanLevel = Convert.ToInt16(cboUrutanLevel.Text),
                    Keterangan = txtKeterangan.Text,
                    IsAktif = chkIsAktifKelas.Checked
                }, lst_item_kelas);
            }

            ShowListKelas();
            BindListView(true, Libs.GetQ().Trim());
            txtKeyAction.Value = JenisAction.ShowDataListWithUpdate.ToString();
        }

        protected void InitFieldsItemKelas()
        {
            txtNamaItemKelas.Text = "";

            cboNoUrutKelas.Items.Clear();
            cboNoUrutKelas.Items.Add("");
            cboNoUrutKelas.Items.Add("0");
            for (int i = 1; i <= 50; i++)
            {
                cboNoUrutKelas.Items.Add(new ListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            txtKeteranganKelas.Text = "";
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            InitFieldsItemKelas();
            txtIDItemKelas.Value = "";
            txtKeyAction.Value = JenisAction.AddItemKelas.ToString();
        }

        protected void lnkHapusItemKelas_Click(object sender, EventArgs e)
        {
            //cek validasi hapus
            //end cek validasi

            string[] arr_kode_kelas = txtKeyItemsKelas.Value.Replace("_", "-").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in arr_kode_kelas)
            {
                lst_item_kelas.Remove(
                        lst_item_kelas.Find(m => m.Kode == new Guid(item))
                    );
            }
            RenderItemKelas();
        }

        protected void btnDoEditItemKelas_Click(object sender, EventArgs e)
        {
            InitFieldsItemKelas();
            KelasDet kelasdet = lst_item_kelas.Find(m => m.Kode == new Guid(txtIDItemKelas.Value));
            if (kelasdet != null)
            {
                txtNamaItemKelas.Text = kelasdet.Nama;
                cboNoUrutKelas.SelectedValue = kelasdet.UrutanKelas.ToString();
                txtKeteranganKelas.Text = kelasdet.Keterangan;
                rdoKelasRombel.Checked = (!kelasdet.IsKelasJurusan && !kelasdet.IsKelasSosialisasi ? true : false);
                rdoKelasJurusan.Checked = kelasdet.IsKelasJurusan;
                rdoKelasSosialisasi.Checked = kelasdet.IsKelasSosialisasi;
                chkAktif.Checked = kelasdet.IsAktif;
                txtKeyAction.Value = JenisAction.EditItemKelas.ToString();
            }
        }

        protected void lnkOKHapusKelas_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    //cek validasi hapus
                    //end cek validasi

                    DAO_Kelas.Delete(txtID.Value);
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                }
                else
                {
                    txtKeyAction.Value = Messages.MSG_HAPUS_TIDAK_VALID;
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnDoShowConfirmHapusKelas_Click(object sender, EventArgs e)
        {
            ltrMsgConfirmHapusKelas.Text = "Anda yakin akan menghapus data Level Kelas : " + DAO_Kelas.GetByID_Entity(txtID.Value).Nama;
            txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
        }

        protected void btnDoEditKelas_Click(object sender, EventArgs e)
        {
            Kelas kelas = DAO_Kelas.GetByID_Entity(txtID.Value);
            if (kelas != null)
            {
                InitFields();

                cboUnitSekolah.SelectedValue = kelas.Rel_Sekolah.ToString();
                txtNamaLevel.Text = kelas.Nama;
                cboUrutanLevel.SelectedValue = kelas.UrutanLevel.ToString();
                txtKeterangan.Text = kelas.Keterangan;
                chkIsAktifKelas.Checked = kelas.IsAktif;

                lst_item_kelas = DAO_KelasDet.GetByKelas_Entity(kelas.Kode.ToString());
                RenderItemKelas();
                ShowInputKelas();
                txtKeyAction.Value = JenisAction.DoShowEditKelas.ToString();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL);
        }

        protected void lnkOKCari_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL + (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : ""));
        }
    }
}