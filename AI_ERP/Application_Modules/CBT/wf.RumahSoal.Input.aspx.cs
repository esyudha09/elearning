using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using static AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES;
using Microsoft.SqlServer.Server;
using static AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.CBT;
using System.Windows.Controls;
using System.Windows.Ink;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_RumahSoal_Input : System.Web.UI.Page
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
                s_url_var += (s_url_var.Trim() != "" && Libs.GetQueryString("u").Trim() != "" ? "&" : "") + (Libs.GetQueryString("u").Trim() != "" ? "u=" : "") + Libs.GetQueryString("u").Trim();
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


            

            if (!IsPostBack)
            {
                SetDropdown();
                //InitInput();
                InitKeyEventClient();
                getData();

            }

        }

        private void SetDropdown()
        {

            for (int i = 0; i <= 24; i++)
            {
                if (i < 10)
                {
                    cboStartJam.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                    cboEndJam.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    cboStartJam.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    cboEndJam.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }


            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {
                    cboStartMenit.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                    cboEndMenit.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    cboStartMenit.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    cboEndMenit.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }

            cboLimitSatuan.Items.Add(new ListItem("", ""));
            cboLimitSatuan.Items.Add(new ListItem("Menit", "Menit"));
            cboLimitSatuan.Items.Add(new ListItem("Jam", "Jam"));
            cboLimitSatuan.Items.Add(new ListItem("Hari", "Hari"));
            cboLimitSatuan.Items.Add(new ListItem("Minggu", "Minggu"));
            
            
         
           
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
            txtNama.Text = "";
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
                CBT_RumahSoal m = new CBT_RumahSoal();
                m.Rel_Mapel = Libs.GetQueryString("m");
                m.Rel_Kelas = Libs.GetQueryString("k");
                m.Rel_Rapor_StrukturNilai_KP = Libs.GetQueryString("kp");
                m.Rel_Guru = Libs.LOGGED_USER_M.NoInduk;
                m.Kurikulum = Libs.GetQueryString("kur");
                m.TahunAjaran = Libs.GetQueryString("ta");
                m.Semester = Libs.GetQueryString("sm");
                m.Deskripsi = txtDeskripsi.Text;



                if (txtID.Value.Trim() != "")
                {
                    m.Nama = txtNama.Text;
                    
                    var a = txtStartDate.Text;
                    m.StartDatetime = !string.IsNullOrEmpty(txtStartDate.Text) ? Convert.ToDateTime(Libs.GetDateFromTanggalIndonesiaStr(txtStartDate.Text).ToString("yyyy-MM-dd") + " " + cboStartJam.Text + ":" + cboStartMenit.Text + ":00") : Convert.ToDateTime("1900-01-01 00:00:00");
                    m.EndDatetime = !string.IsNullOrEmpty(txtEndDate.Text) ? Convert.ToDateTime(Libs.GetDateFromTanggalIndonesiaStr(txtEndDate.Text).ToString("yyyy-MM-dd") + " " + cboEndJam.Text + ":" + cboEndMenit.Text + ":00") : Convert.ToDateTime("1900-01-01 00:00:00");
                    m.LimitTime = !string.IsNullOrEmpty(txtTimeLimit.Text) ? Convert.ToInt32(txtTimeLimit.Text) : 0;
                    m.LimitSatuan = cboLimitSatuan.Text;
                    m.Kode = new Guid(txtID.Value);
                    DAO_CBT_RumahSoal_Input.Update(m, Libs.LOGGED_USER_M.UserID);

                   
                    //DAO_CBT_BankSoal.Update(m, Libs.LOGGED_USER_M.UserID);
                    //getData(txtID.Value);

                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {

                    m.Nama = txtNama.Text;
                    //m.Deskripsi = txtDeskripsi.Text;
                    
                    m.StartDatetime = !string.IsNullOrEmpty(txtStartDate.Text) ? Convert.ToDateTime(Libs.GetDateFromTanggalIndonesiaStr(txtStartDate.Text).ToString("yyyy-MM-dd") + " " + cboStartJam.Text + ":" + cboStartMenit.Text + ":00") : Convert.ToDateTime("1900-01-01 00:00:00");
                    m.EndDatetime = !string.IsNullOrEmpty(txtEndDate.Text) ? Convert.ToDateTime(Libs.GetDateFromTanggalIndonesiaStr(txtEndDate.Text).ToString("yyyy-MM-dd") + " " + cboEndJam.Text + ":" + cboEndMenit.Text + ":00") : Convert.ToDateTime("1900-01-01 00:00:00");
                    m.LimitTime = !string.IsNullOrEmpty(txtTimeLimit.Text)? Convert.ToInt32(txtTimeLimit.Text): 0 ;
                    m.LimitSatuan = cboLimitSatuan.Text;

                    DAO_CBT_RumahSoal_Input.Insert(m, Libs.LOGGED_USER_M.UserID);

                    getData();
                    //Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE + QS.GetURLVariable()));

                    //InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }
                getData();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void getData()
        {
            
            CBT_RumahSoal m = DAO_CBT_RumahSoal_Input.GetByKP_Entity(Libs.GetQueryString("kp"));
            if (m != null)
            {
                if (m.Kode != Guid.Empty)
                {
                    btnSoal.Attributes.Add("style", "display:block");
                    txtID.Value = m.Kode.ToString();
                    txtNama.Text = m.Nama.ToString();
                    txtDeskripsi.Text = m.Deskripsi.ToString();
                    //string startdate = m.StartDatetime.ToString());
                    //DateTime enddate = Convert.ToDateTime(m.EndDatetime.ToString());
                  
                    if (m.StartDatetime > Convert.ToDateTime("2000-01-01"))
                    {
                        txtStartDate.Text = Libs.GetTanggalIndonesiaFromDate(m.StartDatetime,false);
                        cboStartJam.SelectedValue = m.StartDatetime.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 2);
                        cboStartMenit.SelectedValue = m.StartDatetime.ToString("yyyy-MM-dd HH:mm:ss").Substring (14, 2);
                    }

                    if (m.EndDatetime > Convert.ToDateTime("2000-01-01"))
                    {
                        txtEndDate.Text = Libs.GetTanggalIndonesiaFromDate(m.EndDatetime, false);
                        cboEndJam.SelectedValue = m.EndDatetime.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 2);
                        cboEndMenit.SelectedValue = m.EndDatetime.ToString("yyyy-MM-dd HH:mm:ss").Substring(14, 2);
                    }
                    txtTimeLimit.Text = m.LimitTime.ToString();
                    cboLimitSatuan.SelectedValue = m.LimitSatuan.ToString();  


                }
                else
                {
                   // txtDeskripsi.Text = "<p><img src=\"../../Application_Templates/sd header.png\" width=\"100%\"></p>";
                                       
                    txtNama.Text = Libs.GetQueryString("nama");

                }

                txtKeyAction.Value = JenisAction.DoShowData.ToString();

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

        protected void btnSoal_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                   ResolveUrl(
                           Routing.URL.APPLIACTION_MODULES.CBT.DESIGN_SOAL.ROUTE + "?rs="+txtID.Value + "&m=" + m + "&kp=" + kp + "&kur=" + kur + "&u=" + unit

                       )
               );
        }

    }
}