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
    public partial class wf_BankSoal_View : System.Web.UI.Page
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








        //protected void InitFields()
        //{
        //    txtID.Value = "";
        //    txtSoal.Text = "";
        //    txtJwbEssay.Text = "";
        //    txtJwbGanda1.Text = "";
        //    txtJwbGanda2.Text = "";
        //    txtJwbGanda3.Text = "";
        //    txtJwbGanda4.Text = "";
        //    txtJwbGanda5.Text = "";
        //    //chkEssay.Checked = false;
        //    //chkGanda.Checked = false;
        //}





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

                // m.Jenis = cboJenis.SelectedValue;

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



                m.JwbEssay = txtJwbEssay.Text;
                m.JwbGanda1 = txtJwbGanda1.Text;
                m.JwbGanda2 = txtJwbGanda2.Text;
                m.JwbGanda3 = txtJwbGanda3.Text;
                m.JwbGanda4 = txtJwbGanda4.Text;
                m.JwbGanda5 = txtJwbGanda5.Text;

                DAO_CBT_BankSoal.Insert(m, Libs.LOGGED_USER_M.UserID);

                Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE + QS.GetURLVariable()));

                //InitFields();
                // txtKeyAction.Value = JenisAction.AddWithMessage.ToString();

            }
            catch (Exception ex)
            {

            }
        }

        protected void getData(string idsoal)
        {

            CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(idsoal.Trim());
            if (m != null)
            {
                if (m.Soal != null)
                {

                    txtSoal.Text = m.Soal.ToString();
                    //txtJenis.Text = m.Jenis.ToString();
                    txtJwbEssay.Text = m.JwbEssay.ToString();
                    txtJwbGanda1.Text = m.JwbGanda1.ToString();
                    txtJwbGanda2.Text = m.JwbGanda2.ToString();
                    txtJwbGanda3.Text = m.JwbGanda3.ToString();
                    txtJwbGanda4.Text = m.JwbGanda4.ToString();
                    txtJwbGanda5.Text = m.JwbGanda5.ToString();





                    // cboJenis.SelectedValue = m.Jenis.ToString();
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







        protected void btnBackToMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE +
                            QS.GetURLVariable()
                        )
                );
        }
    }
}