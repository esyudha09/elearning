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
    public partial class wf_UserManagement : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAUSERMANAGEMENT";

        private static List<UserManagementDet> lst_item_user = new List<UserManagementDet>();
        public enum JenisAction
        {
            Add,
            Edit,
            AddItemUser,
            EditItemUser,
            Update,
            Delete,
            DeleteItemUser,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            ItemUserDetKosong
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
                SetTitle("Level User");
                lst_item_user.Clear();
                ClearItemUser();
            }
            if (!(IsPostBack || this.Session[SessionViewDataName] == null))
            {
                dpData.SetPageProperties((int)this.Session[SessionViewDataName], dpData.MaximumRows, true);
            }

            InitKeyEventClient();
            BindListView(!IsPostBack, Libs.GetQ().Trim());
            if (!IsPostBack)
            {
                this.Master.txtCariData.Text = Libs.GetQ();
                ltrJSHead.Text = txtPegawai.GetJSAutocomplete();
            }
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
            txtPegawai.NamaControl.Attributes.Add("onkeydown", sKeyEnter + "return false; }");
            txtPegawai.NamaControl.Attributes.Add("onfocus", txtPegawai.NamaClientID + "_SHOW_AUTOCOMPLETE();");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_UserManagement.SP_SELECT_ALL_FOR_SEARCH;
                sql_ds.SelectParameters.Add("nama", keyword);
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_UserManagement.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListView()
        {
            lvData.DataSource = null;
            lvData.DataBind();
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtIDItemUser.Value.Trim() == "")
                //{
                //    lst_item_user.Add(new UserDet
                //    {
                //        Kode = Guid.NewGuid(),
                //        Nama = txtNamaItemUser.Text,
                //        UrutanUser = Convert.ToInt16(cboNoUrutUser.SelectedValue),
                //        Keterangan = txtKeteranganUser.Text,
                //        IsUserJurusan = chkUserJurusan.Checked,
                //        IsAktif = chkAktif.Checked
                //    });
                //}
                //else
                //{
                //    int id = lst_item_user.FindIndex(m => m.Kode == new Guid(txtIDItemUser.Value));
                //    if (id >= 0)
                //    {
                //        lst_item_user[id] = new UserDet
                //        {
                //            Kode = new Guid(txtIDItemUser.Value),
                //            Nama = txtNamaItemUser.Text,
                //            UrutanUser = Convert.ToInt16(cboNoUrutUser.SelectedValue),
                //            Keterangan = txtKeteranganUser.Text,
                //            IsUserJurusan = chkUserJurusan.Checked,
                //            IsAktif = chkAktif.Checked
                //        };
                //    }
                //}

                RenderItemUser();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void RenderItemUser()
        {
            if (lst_item_user.Count == 0)
            {
                ClearItemUser();
            }
            else
            {
                //lst_item_user = lst_item_user.OrderBy(m => m.UrutanUser).ThenBy(m => m.Nama).ToList();
                //ltrItemUser.Text = "";
                //foreach (UserDet item in lst_item_user)
                //{
                //    ltrItemUser.Text += "<tr>" +
                //                            "<td style=\"text-align: left; padding: 15px; background-color: white;\">" +
                //                                "<div class=\"checkbox checkbox-adv pull-left\">" +
                //                                    "<label for=\"" + item.Kode.ToString().Replace("-", "_") + "\" style=\"font-weight: bold;\">" +
                //                                        "<input class=\"access-hide\" id=\"" + item.Kode.ToString().Replace("-", "_") + "\" name=\"CHKITEMUser[]\" type=\"checkbox\">" +
                //                                        "&nbsp;&nbsp;&nbsp;" +
                //                                        item.Nama +
                //                                        "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                //                                    "</label>" +
                //                                "</div>" +
                //                                "<label onclick=\"" + txtIDItemUser.ClientID + ".value = '" + item.Kode.ToString() + "'; " + btnDoEditItemUser.ClientID + ".click();\" class=\"pull-left\" style=\"cursor: pointer;\" title=\" Edit \">" +
                //                                    "&nbsp;&nbsp;&nbsp;" +
                //                                    "<i class=\"fa fa-pencil\"></i>" +
                //                                    "&nbsp;&nbsp;&nbsp;" +
                //                                "</label>" +
                //                            "</td>" +
                //                            "<td style=\"text-align: center; padding: 15px; background-color: white; color: grey;\">" +
                //                                "# <label>" + item.UrutanUser.ToString() + "</label>" +
                //                            "</td>" +
                //                            "<td style=\"text-align: center; padding: 15px; background-color: white; color: grey;\">" +
                //                                (
                //                                    item.IsUserJurusan
                //                                    ? "<i class='fa fa-check'></i>"
                //                                    : ""
                //                                ) +
                //                            "</td>" +
                //                            "<td style=\"text-align: center; padding: 15px; background-color: white; color: grey;\">" +
                //                                (
                //                                    item.IsAktif
                //                                    ? "<i class='fa fa-check'></i>"
                //                                    : ""
                //                                ) +
                //                            "</td>" +
                //                            "<td style=\"text-align: left; padding: 15px; background-color: white; \">" +
                //                                item.Keterangan +
                //                            "</td>" +
                //                         "</tr>" +
                //                         "<tr>" +
                //                            "<td colspan=\"5\" style=\"margin: 0px; padding: 0px;\" ><hr style=\"margin: 0px; padding: 0px; border-color: #e9e7e7;\" /></td>" +
                //                         "</tr>";
                //}
            }
        }

        protected void ClearItemUser()
        {
            ltrMenuAkses.Text = "<div>" +
                                    "<label style=\"margin: 0 auto; display: table;\">" +
                                        "..:: Data Kosong ::.." +
                                    "</label>" +
                                "</div>";
        }

        protected void InitFields()
        {
            ClearItemUser();
            txtPegawai.Value = "";
            UserManagementSettings.JenisUser.ListUserToDropdown(cboJenisUser);
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            txtID.Value = "";
            InitFields();
            ShowInputUser();
        }

        protected void ShowInputUser()
        {
            SetTitle("Pengaturan User dalam Level");
            mvMain.ActiveViewIndex = 1;
            div_button_settings.Visible = false;
        }

        protected void ShowListUser()
        {
            SetTitle("Data Level User");
            mvMain.ActiveViewIndex = 0;
            div_button_settings.Visible = true;
        }

        protected void btnCancelSave_Click(object sender, EventArgs e)
        {
            ShowListUser();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (lst_item_user.Count == 0)
            {
                txtKeyAction.Value = JenisAction.ItemUserDetKosong.ToString();
                return;
            }

            //simpan pengaturan User
            //if (txtID.Value.Trim() == "")
            //{
            //    DAO_User.Insert(new User
            //    {
            //        Rel_Sekolah = new Guid(cboUnitSekolah.SelectedValue),
            //        Nama = txtNamaLevel.Text,
            //        UrutanLevel = Convert.ToInt16(cboUrutanLevel.Text),
            //        Keterangan = txtKeterangan.Text,
            //        IsAktif = chkIsAktifUser.Checked
            //    }, lst_item_user);
            //}
            //else
            //{
            //    DAO_User.Update(new User
            //    {
            //        Kode = new Guid(txtID.Value),
            //        Rel_Sekolah = new Guid(cboUnitSekolah.SelectedValue),
            //        Nama = txtNamaLevel.Text,
            //        UrutanLevel = Convert.ToInt16(cboUrutanLevel.Text),
            //        Keterangan = txtKeterangan.Text,
            //        IsAktif = chkIsAktifUser.Checked
            //    }, lst_item_user);
            //}

            ShowListUser();
            BindListView(!IsPostBack, Libs.GetQ().Trim());
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void InitFieldsItemUser()
        {
            

        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            InitFieldsItemUser();
            txtIDItemUser.Value = "";
            txtKeyAction.Value = JenisAction.AddItemUser.ToString();
        }

        protected void lnkHapusItemUser_Click(object sender, EventArgs e)
        {
            //cek validasi hapus
            //end cek validasi

            RenderItemUser();
        }

        protected void btnDoEditItemUser_Click(object sender, EventArgs e)
        {
            InitFieldsItemUser();
            //UserDet Userdet = lst_item_user.Find(m => m.Kode == new Guid(txtIDItemUser.Value));
            //if (Userdet != null)
            //{
            //    txtNamaItemUser.Text = Userdet.Nama;
            //    cboNoUrutUser.SelectedValue = Userdet.UrutanUser.ToString();
            //    txtKeteranganUser.Text = Userdet.Keterangan;
            //    chkUserJurusan.Checked = Userdet.IsUserJurusan;
            //    chkAktif.Checked = Userdet.IsAktif;
            //    txtKeyAction.Value = JenisAction.EditItemUser.ToString();
            //}
        }

        protected void lnkOKHapusUser_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtID.Value.Trim() != "")
                //{
                //    //cek validasi hapus
                //    //end cek validasi

                //    DAO_User.Delete(txtID.Value);
                //    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                //    BindListView(!IsPostBack, Libs.GetQ().Trim());
                //}
                //else
                //{
                //    txtKeyAction.Value = Messages.MSG_HAPUS_TIDAK_VALID;
                //}
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnDoShowConfirmHapusUser_Click(object sender, EventArgs e)
        {
            //ltrMsgConfirmHapusUser.Text = "Anda yakin akan menghapus data Level User : " + DAO_User.GetByID_Entity(txtID.Value).Nama;
            txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
        }

        protected void btnDoEditUser_Click(object sender, EventArgs e)
        {
            //User User = DAO_User.GetByID_Entity(txtID.Value);
            //if (User != null)
            //{
            //    InitFields();

            //    cboUnitSekolah.SelectedValue = User.Rel_Sekolah.ToString();
            //    txtNamaLevel.Text = User.Nama;
            //    cboUrutanLevel.SelectedValue = User.UrutanLevel.ToString();
            //    txtKeterangan.Text = User.Keterangan;
            //    chkIsAktifUser.Checked = User.IsAktif;

            //    lst_item_user = DAO_UserDet.GetByUser_Entity(User.Kode.ToString());
            //    RenderItemUser();
            //    ShowInputUser();
            //}
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