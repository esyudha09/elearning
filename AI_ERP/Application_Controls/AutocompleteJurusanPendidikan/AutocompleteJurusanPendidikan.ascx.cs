using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_Controls.AutocompleteJurusanPendidikan
{
    public partial class AutocompleteJurusanPendidikan : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string Value { get { return hflKode.Value; } set { hflKode.Value = value; CekDataByValue(hflKode.Value); } }
        public string Text { get { return txtNama.Text; } set { txtNama.Text = value; } }

        private void CekDataByValue(string s_val)
        {
            JurusanPendidikan m = DAO_JurusanPendidikan.GetByID_Entity(s_val);
            if (m != null) txtNama.Text = m.Nama;
        }

        public string NamaClientID { get { return txtNama.ClientID; } }
        public string KodeClientID { get { return hflKode.ClientID; } }

        public TextBox NamaControl { get { return txtNama; } }
        public HiddenField KodeControl { get { return hflKode; } }
    }
}