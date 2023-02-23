using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Controls.AutocompleteSiswa
{
    public partial class AutocompleteSiswaNISSekolahByUnit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string Value { get { return hflKode.Value; } set { hflKode.Value = value; CekDataByValue(hflKode.Value); } }
        public string Text { get { return txtNama.Text; } set { txtNama.Text = value; } }

        private void CekDataByValue(string s_val)
        {
            Siswa m = DAO_Siswa.GetByKode_Entity("_", "_", s_val);
            if (m != null)
            {
                txtNama.Text = (
                                Libs.GetPerbaikiEjaanNama(m.Nama) != ""
                                ? m.NISSekolah +
                                  HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                  Libs.GetPerbaikiEjaanNama(m.Nama)
                                : ""
                               );
            }
        }

        public string NamaClientID { get { return txtNama.ClientID; } }
        public string KodeClientID { get { return hflKode.ClientID; } }

        public TextBox NamaControl { get { return txtNama; } }
        public HiddenField KodeControl { get { return hflKode; } }
        public string KodeUnit
        {
            get { return txtKodeUnit.Value; }
            set { txtKodeUnit.Value = value; }
        }

        public string TahunAjaran
        {
            get { return txtTahunAjaran.Value.Replace("-", "/"); }
            set { txtTahunAjaran.Value = value.Replace("/", "-"); }
        }

        public string Semester
        {
            get { return txtSemester.Value; }
            set { txtSemester.Value = value; }
        }
    }
}