using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Collections.Generic;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Controls.Res
{
    public partial class ImageViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////hanya untuk demo
                //List<UserOrtuDet> lst_user_ortu_det = DAO_UserOrtuDet.SelectByUserID(Libs.LOGGED_USER_M.UserID);
                //foreach (UserOrtuDet m_det in lst_user_ortu_det)
                //{
                //    Siswa m_siswa = DAO_Siswa.GetByID_Entity(m_det.NIS);
                //    if (m_siswa != null)
                //    {
                //        if (m_siswa.Nama != null)
                //        {
                //            string url_foto = Constantas.URL_FOTO_SISWA + m_siswa.NIS + ".jpg";
                //            Response.Redirect(url_foto);
                //        }
                //    }
                //}
                ////end hanya untuk demo

                if (Request.QueryString["ID"] != null && Request.QueryString["Jenis"] != null)
                {
                    string jenis = Request.QueryString["Jenis"];
                    string id = Request.QueryString["ID"];
                    string sql = "";
                    string fieldname = "";
                    string contenttype = "JPG";
                    SqlDbType typedb = SqlDbType.VarChar;
                    switch (jenis.ToLower().Trim())
                    {
                        case "pegawai":
                            sql = "SELECT foto FROM pegawai WHERE kode=@id";
                            fieldname = "foto";
                            typedb = SqlDbType.VarChar;
                            break;
                    }
                    if (sql.Trim() != "")
                    {
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@id", typedb).Value = Request.QueryString["ID"];
                        DataTable dt = GetData(cmd);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0][fieldname] != DBNull.Value && dt.Rows[0][fieldname].ToString().Trim() != "")
                                {
                                    Byte[] bytes = (Byte[])dt.Rows[0][fieldname];
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    Response.ContentType = contenttype;
                                    Response.AddHeader("content-disposition", "attachment;filename=tmpimage");
                                    Response.BinaryWrite(bytes);
                                    Response.Flush();
                                    Response.End();
                                }
                                else
                                {
                                    Response.Redirect("noimage.png");
                                }
                            }
                            else
                            {
                                Response.Redirect("noimage.png");
                            }
                        }
                        else
                        {
                            Response.Redirect("noimage.png");
                        }
                    }
                    else
                    {
                        Response.Redirect("noimage.png");
                    }
                }
                else
                {
                    Response.Redirect("noimage.png");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private DataTable GetData(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            String strConnString = Application_Libs.Libs.GetConnectionString_ERP();
            SqlConnection con = new SqlConnection(strConnString);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }
    }
}