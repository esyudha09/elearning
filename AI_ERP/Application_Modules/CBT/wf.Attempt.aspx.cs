using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using System.Windows.Input;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using Newtonsoft.Json.Linq;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_Attempt : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAMAPEL";

        public static List<CBT_DesignSoal> lstDesignSoalJwb = null;
        public static List<CBT_Jwb> lstJwb = null;

        public static int st_lstIdx = 0;
        public static int st_mxIdx = 0;
        public static string st_jenis = "";



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
            Clear
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

        //public class CountDownTimer
        //{
        //    public TimeSpan TimeLeft;
        //    System.Threading.Thread thread;
        //    public CountDownTimer(TimeSpan original)
        //    {
        //        this.TimeLeft = original;
        //    }
        //    public void Start()
        //    {
        //        // Start a background thread to count down time
        //        thread = new System.Threading.Thread(() =>
        //        {
        //            while (true)
        //            {
        //                System.Threading.Thread.Sleep(1000);
        //                TimeLeft = TimeLeft.Subtract(TimeSpan.Parse("00:00:01"));                                                                                         
        //            }

        //        });
        //        thread.Start();

        //    }
        //}


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
                btnPrev.Enabled = false;

                st_lstIdx = 0;
                lstDesignSoalJwb = null;
                getListDs();
                getListJwb();
                getListLInk();
                getData(lstDesignSoalJwb[st_lstIdx].Rel_BankSoal.ToString());
                CBT_RumahSoal m = DAO_CBT_RumahSoal_Input.GetByID_Entity(Libs.GetQueryString("rs"));
                int tcd = 0;
                txtNamaKP.Text = m.Nama.ToString();
                txtKelas.Text = m.NamaKelas.ToString();
                txtMapel.Text = m.NamaMapel.ToString();
                txtTahunAjaran.Text = m.TahunAjaran.ToString();
                txtSemester.Text = m.Semester.ToString();
                if (m != null)
                {
                    if (m.LimitTime > 0)
                    {
                        if (m.LimitSatuan == "Menit")
                        {
                            tcd = m.LimitTime;
                        }
                    }


                }

              

                //if (Session["CountdownTimer"] == null)
                //{
                //    Session["CountdownTimer"] = new CountDownTimer(TimeSpan.FromMinutes(10));
                //    (Session["CountdownTimer"] as CountDownTimer).Start();
                //}
            }


          
            //DataTable dt = new DataTable();
            //dt.Columns.Add("EndDate", typeof(DateTime));
            //dt.Rows.Add("2023-03-07 00:49:00");
            DateTime startDate = DateTime.Now;
            DateTime endDate = Convert.ToDateTime("2023-03-07 01:00:00");
            lblTime.Text = CalculateTimeDifference(startDate, endDate);

            var IdSoal = Libs.GetQueryString("id");
            if (!string.IsNullOrEmpty(IdSoal))
            {
                getData(IdSoal);
            }
        }

        public string CalculateTimeDifference(DateTime startDate, DateTime endDate)
        {
            int days = 0; int hours = 0; int mins = 0; int secs = 0;
            string final = string.Empty;
            if (endDate > startDate)
            {
                days = (endDate - startDate).Days;
                hours = (endDate - startDate).Hours;
                mins = (endDate - startDate).Minutes;
                secs = (endDate - startDate).Seconds;
                final = string.Format("{0} days {1} hours {2} mins {3} secs", days, hours, mins, secs);
            }
            else
            {
                timer.Enabled = false;
            }
            return final;
            
        }
        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
        //    if (Session["CountdownTimer"] != null)
        //    {
        //        Label1.Text = (Session["CountdownTimer"] as CountDownTimer).TimeLeft.ToString();
        //        if ((Session["CountdownTimer"] as CountDownTimer).TimeLeft.TotalMilliseconds <= 0)
        //        {
        //            Session["CountdownTimer"] = null;
        //            Timer1.Enabled = false;
        //            //Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.CBT.START_ATTEMPT.ROUTE + "?rs=" + Libs.GetQueryString("rs")));

        //        }
        //    }
        //}


        //protected void CountStop(object sender, EventArgs e)
        //{
        //    Session["CountdownTimer"] = null;
        //    Timer1.Enabled = false;
        //}

        //protected void CountStart(object sender, EventArgs e)
        //{
        //    if (Session["CountdownTimer"] == null)
        //    {
        //        Session["CountdownTimer"] = new CountDownTimer(TimeSpan.Parse(Label1.Text));
        //        (Session["CountdownTimer"] as CountDownTimer).Start();
        //    }
        //}


        protected void getListDs()
        {


            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;
            conn.Open();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = DAO_CBT_DesignSoal.SP_SELECT_BY_RS;
            comm.Parameters.Clear();
            //comm.Parameters.AddWithValue("@Rel_Siswa", Libs.LOGGED_USER_M.NoInduk);
            comm.Parameters.AddWithValue("@Rel_RumahSoal", Libs.GetQueryString("rs"));
            DataTable dtResult2 = new DataTable();
            sqlDA = new SqlDataAdapter(comm);
            sqlDA.Fill(dtResult2);

            lstDesignSoalJwb = new List<CBT_DesignSoal>();

            foreach (DataRow row in dtResult2.Rows)
            {
                CBT_DesignSoal j = new CBT_DesignSoal();
                j = DAO_CBT_DesignSoal.GetEntityFromDataRow(row);
                lstDesignSoalJwb.Add(j);
            }
            st_mxIdx = lstDesignSoalJwb.Count - 1;
        }

        protected void getListJwb()
        {


            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;
            conn.Open();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = DAO_CBT_DesignSoal.SP_SELECT_JWB_BY_RS_SISWA;
            comm.Parameters.Clear();
            comm.Parameters.AddWithValue("@Rel_Siswa", Libs.LOGGED_USER_M.NoInduk);
            comm.Parameters.AddWithValue("@Rel_RumahSoal", Libs.GetQueryString("rs"));
            DataTable dtResult2 = new DataTable();
            sqlDA = new SqlDataAdapter(comm);
            sqlDA.Fill(dtResult2);
            lstJwb = new List<CBT_Jwb>();

            foreach (DataRow row in dtResult2.Rows)
            {
                CBT_Jwb j = new CBT_Jwb();
                j = DAO_CBT_DesignSoal.GetEntityJwbFromDataRow(row);
                lstJwb.Add(j);
            }
        }

        protected void checkButton()
        {
            if (st_lstIdx > 0)
            {
                btnPrev.Visible = true;
                btnPrev.Enabled = true;
            }
            else
            {
                btnPrev.Visible = false;
            }

            if (st_lstIdx < st_mxIdx)
            {
                btnNext.Visible = true;
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Visible = false;
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {


            st_lstIdx = st_lstIdx + 1;

            getData(lstDesignSoalJwb[st_lstIdx].Rel_BankSoal.ToString());

        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            st_lstIdx = st_lstIdx - 1;

            getData(lstDesignSoalJwb[st_lstIdx].Rel_BankSoal.ToString());

        }

        protected void btnLink_Click(object sender, EventArgs e)
        {
            st_lstIdx = Convert.ToInt32(hdIdx.Value) - 1;


            getData(lstDesignSoalJwb[st_lstIdx].Rel_BankSoal.ToString());

        }

        protected void getData(string idsoal)
        {
            st_jenis = "";



            CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(idsoal.Trim());

            if (m != null)
            {
                if (m.Soal != null)
                {


                    txtSoal.Text = m.Soal.ToString();



                    if (m.Jenis == "essay")
                    {
                        EssayDiv.Attributes.Add("style", "display:block");
                        GandaDiv.Attributes.Add("style", "display:none");
                    }
                    else if (m.Jenis == "ganda")
                    {
                        EssayDiv.Attributes.Add("style", "display:none");
                        GandaDiv.Attributes.Add("style", "display:block");
                    }

                    st_jenis = m.Jenis;


                    hdKodejwbGanda1.Value = m.ListJwbGanda[0].Kode.ToString();
                    hdKodejwbGanda2.Value = m.ListJwbGanda[1].Kode.ToString();
                    hdKodejwbGanda3.Value = m.ListJwbGanda[2].Kode.ToString();
                    hdKodejwbGanda4.Value = m.ListJwbGanda[3].Kode.ToString();
                    hdKodejwbGanda5.Value = m.ListJwbGanda[4].Kode.ToString();

                    txtJwbGanda1.Text = m.ListJwbGanda[0].Jawaban.ToString();
                    txtJwbGanda2.Text = m.ListJwbGanda[1].Jawaban.ToString();
                    txtJwbGanda3.Text = m.ListJwbGanda[2].Jawaban.ToString();
                    txtJwbGanda4.Text = m.ListJwbGanda[3].Jawaban.ToString();
                    txtJwbGanda5.Text = m.ListJwbGanda[4].Jawaban.ToString();

                    ChkJwbGanda1.Checked = false;
                    ChkJwbGanda2.Checked = false;
                    ChkJwbGanda3.Checked = false;
                    ChkJwbGanda4.Checked = false;
                    ChkJwbGanda5.Checked = false;

                    var ds = lstDesignSoalJwb[st_lstIdx].Kode.ToString();
                    var jwb = lstJwb.Where(x => x.Rel_DesignSoal.ToUpper() == ds.ToUpper()).FirstOrDefault();
                    if (jwb != null)
                    {
                        txtJwbEssayVal.Value = jwb.JwbEssay;
                        txtJwbEssay.Text = jwb.JwbEssay;

                        //.Rel_JwbGanda;
                        if (hdKodejwbGanda1.Value == jwb.Rel_JwbGanda.ToString())
                        {
                            ChkJwbGanda1.Checked = true;
                        }
                        else if (hdKodejwbGanda2.Value == jwb.Rel_JwbGanda.ToString())
                        {
                            ChkJwbGanda2.Checked = true;
                        }
                        else if (hdKodejwbGanda3.Value == jwb.Rel_JwbGanda.ToString())
                        {
                            ChkJwbGanda3.Checked = true;
                        }
                        else if (hdKodejwbGanda4.Value == jwb.Rel_JwbGanda.ToString())
                        {
                            ChkJwbGanda4.Checked = true;
                        }
                        else if (hdKodejwbGanda5.Value == jwb.Rel_JwbGanda.ToString())
                        {
                            ChkJwbGanda5.Checked = true;
                        }

                        
                    }




                }

            }
            checkButton();
            getListLInk();
            if (m.Jenis == "essay")
            {
                txtKeyAction.Value = JenisAction.DoShowData.ToString();
            }
            
        }


        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                string jwbGanda = "";
                string jwbEssay = "";
                if (st_jenis == "ganda")
                {

                    bool[] arrJwbGandaChk = { ChkJwbGanda1.Checked, ChkJwbGanda2.Checked, ChkJwbGanda3.Checked, ChkJwbGanda4.Checked, ChkJwbGanda5.Checked };
                    string[] arrKodeJwbGanda = { hdKodejwbGanda1.Value, hdKodejwbGanda2.Value, hdKodejwbGanda3.Value, hdKodejwbGanda4.Value, hdKodejwbGanda5.Value };

                    for (int i = 0; i < 5; i++)
                    {
                        if (arrJwbGandaChk[i])
                            jwbGanda = arrKodeJwbGanda[i].ToString();
                    }
                }
                else
                {
                    jwbEssay = txtJwbEssayVal.Value != "" ? txtJwbEssayVal.Value : txtJwbEssay.Text;
                }



                if (jwbGanda != "" || jwbEssay != "")
                {
                    CBT_Jwb m = new CBT_Jwb()
                    {
                        Rel_RumahSoal = Libs.GetQueryString("rs"),
                        Rel_DesignSoal = lstDesignSoalJwb[st_lstIdx].Kode.ToString(),
                        Rel_BankSoal = lstDesignSoalJwb[st_lstIdx].Rel_BankSoal.ToString(),
                        Rel_Siswa = Libs.LOGGED_USER_M.NoInduk.ToString(),
                        Rel_JwbGanda = jwbGanda.ToString(),
                        JwbEssay = jwbEssay.ToString()
                    };

                    DAO_CBT_DesignSoal.DeleteJwb(m.Rel_DesignSoal);
                    DAO_CBT_DesignSoal.InsertJwb(m);
                    //txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                    //if(st_lstIdx < st_mxIdx)
                    //btnNext_Click(null, null);
                    getListJwb();
                    getListLInk();
                }


            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }




        protected void getListLInk()
        {
            string link = "";
            int i = 1;
            bool exist = false;
            foreach (CBT_DesignSoal row in lstDesignSoalJwb)
            {

                if (lstJwb != null)
                    exist = lstJwb.Any(m => m.Rel_DesignSoal.ToUpper() == row.Kode.ToString().ToUpper());
                if (exist)
                {
                    link += "<div class=\"col-md-2\">    <button class=\"btn btn-green\"  onclick=\"ContentPlaceHolder1_hdIdx.value=\' " + i + "  \';ContentPlaceHolder1_btnLinkClick.click();\" style=\"padding:15px\"> " + i + " </button> </div>";

                }
                else
                {
                    link += "<div class=\"col-md-2\">    <button class=\"btn btn-grey\"  onclick=\"ContentPlaceHolder1_hdIdx.value=\' " + i + "  \';ContentPlaceHolder1_btnLinkClick.click();\" style=\"padding:15px\"> " + i + " </button> </div>";

                }
                i++;
            }

            txtLink.Text = link;
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