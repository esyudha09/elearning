using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities.Elearning.KB;
using AI_ERP.Application_DAOs.Elearning.KB;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Controls.Elearning.KB.AutocompleteSubKategoriPencapaian
{
    public partial class AutocompleteSubKategoriPencapaian : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string Value { get { return hflKode.Value; } set { hflKode.Value = value; CekDataByValue(hflKode.Value); } }
        public string Text { get { return txtNama.Text; } set { txtNama.Text = value; } }

        private void CekDataByValue(string s_val)
        {
            if (s_val.Trim() == "")
            {
                hflKode.Value = "";
                txtNama.Text = "";
                return;
            }
            Rapor_SubKategoriPencapaian m = DAO_Rapor_SubKategoriPencapaian.GetByID_Entity(s_val);
            if (m != null) txtNama.Text = Libs.GetHTMLSimpleText(m.Nama);
        }

        public string NamaClientID { get { return txtNama.ClientID; } }
        public string KodeClientID { get { return hflKode.ClientID; } }

        public TextBox NamaControl { get { return txtNama; } }
        public HiddenField KodeControl { get { return hflKode; } }
    }
}