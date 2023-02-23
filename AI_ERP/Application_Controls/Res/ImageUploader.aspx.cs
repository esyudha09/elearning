using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Controls.Res
{
    public partial class ImageUploader : System.Web.UI.Page
    {
        private string sp_name = "";
        private string sp_deletename = "";
        private string param_kodename = "";
        private string param_imagename = "";
        private string btnrefreshid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string js_validate = "";
            string js_upload = "";
            string js_delete = "";
            if (Request.QueryString["inputkeyid"] != null && Request.QueryString["inputkeyname"] != null)
            {
                js_validate = "if(parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "') != null) " +
                              "{ " +
                              "     if(parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "').value == \"\") " +
                              "     { " +
                              "         alert(\"Sebelum Upload Image,\\nInput '" + Request.QueryString["inputkeyname"] + "' harus diisi terlebih dahulu.\"); " +
                              "         parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "').focus(); " +
                              "         this.value = \"\"; " +
                              "         return false; " +
                              "     } " +
                              "} ";
            }
            if (Request.QueryString["cidsetting"] != null)
            {
                string js_teksinfo = "";
                if (Request.QueryString["textinfoid"] != null)
                {
                    js_teksinfo = "parent.document.getElementById('" + Request.QueryString["textinfoid"] + "').innerHTML = 'Mengupload Image...'; ";
                }
                js_upload = js_teksinfo +
                            "document.getElementById('" + txtkeyvalue.ClientID + "').value = parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "').value; " +
                            "parent.document.getElementById('" + Request.QueryString["cidsetting"] + "').style.display = 'none'; " +
                            "document.getElementById('" + btnuploadimage.ClientID + "').click(); ";

                if (Request.QueryString["textinfoid"] != null)
                {
                    js_teksinfo = "parent.document.getElementById('" + Request.QueryString["textinfoid"] + "').innerHTML = 'Menghapus Image...'; ";
                }
                js_delete = js_teksinfo +
                            "document.getElementById('" + txtkeyvalue.ClientID + "').value = parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "').value; " +
                            "parent.document.getElementById('" + Request.QueryString["cidsetting"] + "').style.display = 'none'; ";
            }
            FileUpload1.Attributes.Add("onchange", "javascript: " + js_validate + " if(confirm('Anda yakin akan meng-upload image?')) { " + js_upload + " } else { this.value = ''; return false; }");
            btnhapusimage.Attributes.Add("onclick", "javascript: " + js_delete);

            if (Request.QueryString["satu"] != null)
            {
                param_kodename = "@" + Request.QueryString["satu"];
            }
            if (Request.QueryString["dua"] != null)
            {
                param_imagename = "@" + Request.QueryString["dua"];
            }
            if (Request.QueryString["tiga"] != null)
            {
                sp_name = Request.QueryString["tiga"];
            }
            if (Request.QueryString["empat"] != null)
            {
                sp_deletename = Request.QueryString["empat"];
            }
            if (Request.QueryString["imgid"] != null)
            {
                btnrefreshid = Request.QueryString["imgid"];
            }
        }

        protected void UploadImage()
        {
            try
            {
                if (FileUpload1.PostedFile.FileName.Trim() != "")
                {
                    Stream fs = FileUpload1.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    string filePath = FileUpload1.PostedFile.FileName;
                    string filename = Path.GetFileName(filePath);
                    string ext = Path.GetExtension(filename);
                    string contenttype = String.Empty;
                    switch (ext)
                    {
                        case ".doc":
                            contenttype = "application/vnd.ms-word";
                            break;
                        case ".docx":
                            contenttype = "application/vnd.ms-word";
                            break;
                        case ".xls":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".jpg":
                            contenttype = "image/jpg";
                            break;
                        case ".png":
                            contenttype = "image/png";
                            break;
                        case ".gif":
                            contenttype = "image/gif";
                            break;
                        case ".pdf":
                            contenttype = "application/pdf";
                            break;
                    }

                    if (ext.ToString().ToUpper() == ".JPG")
                    {
                        SqlConnection conn = new SqlConnection(Application_Libs.Libs.GetConnectionString());
                        conn.Open();

                        SqlCommand cmd = conn.CreateCommand();

                        DbParameter param_kode = cmd.CreateParameter();
                        param_kode.ParameterName = param_kodename;
                        param_kode.DbType = DbType.AnsiString;
                        param_kode.Value = txtkeyvalue.Value;

                        DbParameter param_image = cmd.CreateParameter();
                        param_image.ParameterName = param_imagename;
                        param_image.DbType = DbType.Binary;
                        param_image.Value = bytes;

                        cmd.Parameters.Add(param_kode);
                        cmd.Parameters.Add(param_image);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = sp_name;
                        cmd.ExecuteNonQuery();

                        conn.Close();

                        Page.ClientScript.RegisterStartupScript(this.GetType()
                                                                , @"CloseProgressbar"
                                                                , @"document.getElementById('btnbatal').click();" +
                                                                (btnrefreshid.Trim() != "" ? "parent.document.getElementById('" + btnrefreshid + "').click();" : "")
                                                                , true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType()
                                                                , @"ErrMsg"
                                                                , @"alert('File image yang di-upload harus berformat: JPG'); document.getElementById('btnbatal').click();" +
                                                                (btnrefreshid.Trim() != "" ? "parent.document.getElementById('" + btnrefreshid + "').click();" : "")
                                                                , true);
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType()
                                                                , @"ErrMsg"
                                                                , @"alert('" + ex.Message + "');"
                                                                , true);
            }
        }

        protected void DeleteImage()
        {
            try
            {
                SqlConnection conn = new SqlConnection(Application_Libs.Libs.GetConnectionString());
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                DbParameter param_kode = cmd.CreateParameter();
                param_kode.ParameterName = param_kodename;
                param_kode.DbType = DbType.AnsiString;
                param_kode.Value = txtkeyvalue.Value;

                cmd.Parameters.Add(param_kode);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp_deletename;
                cmd.ExecuteNonQuery();

                conn.Close();

                Page.ClientScript.RegisterStartupScript(this.GetType()
                                                        , @"CloseProgressbar"
                                                        , @"document.getElementById('btnbatal').click();" +
                                                        (btnrefreshid.Trim() != "" ? "parent.document.getElementById('" + btnrefreshid + "').click();" : "")
                                                        , true);
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType()
                                                                , @"ErrMsg"
                                                                , @"alert('" + ex.Message + "');"
                                                                , true);
            }
        }

        protected void btnhapusimage_Click(object sender, EventArgs e)
        {
            DeleteImage();
        }

        protected void btnuploadimage_Click(object sender, EventArgs e)
        {
            UploadImage();
        }
    }
}