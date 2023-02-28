using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_BankSoal_Input : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAMAPEL";

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
            DoShowConfirmHapus
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

            public static string GetURLVariable()
            {
                string s_url_var = "";
                s_url_var += (Libs.GetQueryString("m").Trim() != "" ? "m=" + Libs.GetQueryString("m").Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();

                return (
                            Libs.GetQueryString("m").Trim() != "" || QS.GetToken().Trim() != ""
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
            //if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            //{
            //    Libs.RedirectToLogin(this.Page);
            //}
            //if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            //{
            //    Libs.RedirectToBeranda(this.Page);
            //}

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/document.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Form Input Soal";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            if (!string.IsNullOrEmpty(Libs.GetQueryString("rs")))
            {
                fromBankSoal.Attributes.Add("style", "display:none");
                fromDesignSoal.Attributes.Add("style", "display:block");
            }
            else
            {
                fromBankSoal.Attributes.Add("style", "display:block");
                fromDesignSoal.Attributes.Add("style", "display:none");
            }


            if (!IsPostBack)
            {
                //InitInput();
                InitKeyEventClient();
                var IdSoal = Libs.GetQueryString("id");
                if (!string.IsNullOrEmpty(IdSoal))
                {
                    getData(IdSoal);
                }
            }




        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            //this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            //cboUnit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
            //txtSoal.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtSoal.ClientID + "').focus(); return false; }");
            //txtJwbEssay.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtSoal.ClientID + "').focus(); return false; }");
            //cboJenis.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            //txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        //private void InitInput()
        //{
        //    cboUnit.Items.Clear();
        //    cboUnit.Items.Add("");
        //    List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
        //    div_input_filter_unit.Visible = true;
        //    if (IsByAdminUnit())
        //    {
        //        cboUnit.Items.Clear();
        //        lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
        //        div_input_filter_unit.Visible = false;
        //    }
        //    foreach (Sekolah m in lst_sekolah)
        //    {
        //        cboUnit.Items.Add(new ListItem
        //        {
        //            Value = m.Kode.ToString().ToUpper(),
        //            Text = m.Nama
        //        });
        //    }

        //    Libs.JENIS_MAPEL.ListToDropdown(cboJenis, true, QS.GetUnit());
        //}





        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }


        protected void InitFields()
        {
            txtID.Value = "";
            txtSoal.Text = "";
            txtJwbEssay.Text = "";
            txtJwbGanda1.Text = "";
            txtJwbGanda2.Text = "";
            txtJwbGanda3.Text = "";
            txtJwbGanda4.Text = "";
            txtJwbGanda5.Text = "";
            //chkEssay.Checked = false;
            //chkGanda.Checked = false;
        }



        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_CBT_BankSoal.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);

                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                CBT_BankSoal m = new CBT_BankSoal();
                m.Rel_Mapel = Libs.GetQueryString("m");
                m.Rel_Guru = Libs.LOGGED_USER_M.NoInduk;

                //if (chkEssay.Checked)
                //{
                //    m.Jenis = "essay";
                //}
                //else if (chkGanda.Checked)
                //{
                //    m.Jenis = "ganda";
                //}
                m.Soal = txtSoalVal.Value;
                m.Jenis = cboJenis.SelectedValue;

                if (ChkJwbGanda1.Checked)
                {
                    m.JwbGanda = "a";
                }
                else if (ChkJwbGanda2.Checked)
                {
                    m.JwbGanda = "b";
                }
                else if (ChkJwbGanda3.Checked)
                {
                    m.JwbGanda = "c";
                }
                else if (ChkJwbGanda4.Checked)
                {
                    m.JwbGanda = "d";
                }
                else if (ChkJwbGanda5.Checked)
                {
                    m.JwbGanda = "e";
                }
                else
                {
                    m.JwbGanda = "";
                }

                if (txtID.Value.Trim() != "")
                {

                    m.JwbEssay = txtJwbEssayVal.Value != "" ? txtJwbEssayVal.Value : txtJwbEssay.Text;
                    m.JwbGanda1 = txtJwbGanda1Val.Value != "" ? txtJwbGanda1Val.Value : txtJwbGanda1.Text;
                    m.JwbGanda2 = txtJwbGanda2Val.Value != "" ? txtJwbGanda2Val.Value : txtJwbGanda2.Text;
                    m.JwbGanda3 = txtJwbGanda3Val.Value != "" ? txtJwbGanda3Val.Value : txtJwbGanda3.Text;
                    m.JwbGanda4 = txtJwbGanda4Val.Value != "" ? txtJwbGanda4Val.Value : txtJwbGanda4.Text;
                    m.JwbGanda5 = txtJwbGanda5Val.Value != "" ? txtJwbGanda5Val.Value : txtJwbGanda5.Text;

                    m.Kode = new Guid(txtID.Value);
                    DAO_CBT_BankSoal.Update(m, Libs.LOGGED_USER_M.UserID);
                    getData(txtID.Value);

                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {

                    m.JwbEssay = txtJwbEssay.Text;
                    m.JwbGanda1 = txtJwbGanda1.Text;
                    m.JwbGanda2 = txtJwbGanda2.Text;
                    m.JwbGanda3 = txtJwbGanda3.Text;
                    m.JwbGanda4 = txtJwbGanda4.Text;
                    m.JwbGanda5 = txtJwbGanda5.Text;
                    m.Kode = Guid.NewGuid();
                    DAO_CBT_BankSoal.Insert(m, Libs.LOGGED_USER_M.UserID);

                    if (!string.IsNullOrEmpty(Libs.GetQueryString("rs")))
                    {

                        CBT_DesignSoal d = new CBT_DesignSoal();
                        d.Rel_RumahSoal = Libs.GetQueryString("rs");
                        d.Rel_BankSoal = m.Kode.ToString();
                        DAO_CBT_DesignSoal.Insert(d, Libs.LOGGED_USER_M.UserID);

                       // btnBackToDesignSoal_Click(null, null);
                    }
                    //else
                    //{
                    //    btnBackToSoal_Click(null, null);
                    //}

                    //InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void getData(string idsoal)
        {

            CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(idsoal.Trim());
            if (m != null)
            {
                if (m.Soal != null)
                {
                    txtID.Value = m.Kode.ToString();

                    txtSoal.Text = m.Soal.ToString();
                    txtJwbEssay.Text = m.JwbEssay.ToString();
                    txtJwbGanda1.Text = m.JwbGanda1.ToString();
                    txtJwbGanda2.Text = m.JwbGanda2.ToString();
                    txtJwbGanda3.Text = m.JwbGanda3.ToString();
                    txtJwbGanda4.Text = m.JwbGanda4.ToString();
                    txtJwbGanda5.Text = m.JwbGanda5.ToString();

                    txtSoalVal.Value = m.Soal.ToString();
                    txtJwbEssayVal.Value = m.JwbEssay.ToString();
                    txtJwbGanda1Val.Value = m.JwbGanda1.ToString();
                    txtJwbGanda2Val.Value = m.JwbGanda2.ToString();
                    txtJwbGanda3Val.Value = m.JwbGanda3.ToString();
                    txtJwbGanda4Val.Value = m.JwbGanda4.ToString();
                    txtJwbGanda5Val.Value = m.JwbGanda5.ToString();

                    cboJenis.SelectedValue = m.Jenis.ToString();
                    if (m.Jenis == "essay")
                    {
                        EssayDiv.Attributes.Add("style", "display:block");
                        GandaDiv.Attributes.Add("style", "display:none");
                        //chkEssay.Checked = true;
                    }
                    else if (m.Jenis == "ganda")
                    {
                        EssayDiv.Attributes.Add("style", "display:none");
                        GandaDiv.Attributes.Add("style", "display:block");
                        //chkGanda.Checked = true;
                    }

                    if (m.JwbGanda.ToString() == "a")
                    {
                        ChkJwbGanda1.Checked = true;
                    }
                    else if (m.JwbGanda.ToString() == "b")
                    {
                        ChkJwbGanda2.Checked = true;
                    }
                    else if (m.JwbGanda.ToString() == "c")
                    {
                        ChkJwbGanda3.Checked = true;
                    }
                    else if (m.JwbGanda.ToString() == "d")
                    {
                        ChkJwbGanda4.Checked = true;
                    }
                    else if (m.JwbGanda.ToString() == "e")
                    {
                        ChkJwbGanda5.Checked = true;
                    }
                }

                //txtKeyAction.Value = JenisAction.DoShowData.ToString();

            }
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Soal != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.Soal) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnBackToSoal_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE +
                            QS.GetURLVariable()
                        )
                );
        }

        protected void btnBackToMapel_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.MAPEL.ROUTE // + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&u=" + unit
                        )
                );
        }

        protected void btnBackToKelas_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_SMA.ROUTE + "?&m=" + m + "&u=" + unit
                        )
                );
        }

        protected void btnBackToFormRumahSoal_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_INPUT.ROUTE + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&u=" + unit
                        )
                );
        }

        protected void btnBackToDesignSoal_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            var rs = Libs.GetQueryString("rs");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.DESIGN_SOAL.ROUTE + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&u=" + unit + "&rs=" + rs
                        )
                );
        }
    }
}