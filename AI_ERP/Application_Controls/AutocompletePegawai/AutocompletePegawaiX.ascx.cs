using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_Controls.AutocompletePegawai
{
    public partial class AutocompletePegawaiX : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetJSAutocomplete(bool use_scrip_tab = true)
        {
            return  (use_scrip_tab ? "<script type=\"text/javascript\">" : "") +
                    "function " + txtNama.ClientID + "_SHOW_AUTOCOMPLETE(){" +
                        "$('[id*=" + txtNama.ClientID + "]').typeahead({" +
                            "hint: true," +
                            "highlight: true," +
                            "minLength: 2," +
                            "source: function(request, response) {" +
                                "$.ajax({" +
                                    "url: '" + ResolveUrl("~/APIs/Master/Pegawai.svc/ShowAutocomplete?kata_kunci=") + "' + request," +
                                    "dataType: \"json\"," +
                                    "type: \"GET\"," +
                                    "contentType: \"application/json; charset=utf-8\"," +
                                    "success: function(data) {" +
                                        "items = [];" +
                                        "map = { };" +
                                        "$.each(data.d, function(i, item) {" +
                                            "var id = item.split('~')[1];" +
                                            "var name = item.split('~')[0];" +
                                            "map[name] = { id: id, name: name };" +
                                            "items.push(name);" +
                                        "});" +
                                        "response(items);" +
                                        "$(\".dropdown-menu\").css(\"height\", \"auto\");" +
                                    "}," +
                                    "error: function(response) {" +
                                        "alert(response.responseText);" +
                                    "}," +
                                    "failure: function(response) {" +
                                        "alert(response.responseText);" +
                                    "}" +
                                "});" +
                            "}," +
                            "updater: function(item) {" +
                            "$('[id*=" + hflKode.ClientID + "]').val(map[item].id);" +
                                "return item;" +
                            "}" +
                        "});" +
                    "}" +
                    (use_scrip_tab ? "</script>" : "");
        }

        public string GetJSAutocompleteWithEvent(bool use_scrip_tab = true)
        {
            return  (use_scrip_tab ? "<script type=\"text/javascript\">" : "") + 
                    "function " + txtNama.ClientID + "_SHOW_AUTOCOMPLETE(btn_do){" +
                        "$('[id*=" + txtNama.ClientID + "]').typeahead({" +
                            "hint: true," +
                            "highlight: true," +
                            "minLength: 2," +
                            "source: function(request, response) {" +
                                "$.ajax({" +
                                    "url: '" + ResolveUrl("~/APIs/Master/Pegawai.svc/ShowAutocomplete?kata_kunci=") + "' + request," +
                                    "dataType: \"json\"," +
                                    "type: \"GET\"," +
                                    "contentType: \"application/json; charset=utf-8\"," +
                                    "success: function(data) {" +
                                        "items = [];" +
                                        "map = { };" +
                                        "$.each(data.d, function(i, item) {" +
                                            "var id = item.split('~')[1];" +
                                            "var name = item.split('~')[0];" +
                                            "map[name] = { id: id, name: name };" +
                                            "items.push(name);" +
                                        "});" +
                                        "response(items);" +
                                        "$(\".dropdown-menu\").css(\"height\", \"auto\");" +
                                    "}," +
                                    "error: function(response) {" +
                                        "alert(response.responseText);" +
                                    "}," +
                                    "failure: function(response) {" +
                                        "alert(response.responseText);" +
                                    "}" +
                                "});" +
                            "}," +
                            "updater: function(item) {" +
                                "$('[id*=" + hflKode.ClientID + "]').val(map[item].id);" +
                                "document.getElementById(btn_do).click();" +
                                "return item;" +
                            "}" +
                        "});" +
                    "}" +
                    (use_scrip_tab ? "</script>" : "");
        }

        public string Value { get { return hflKode.Value; } set { hflKode.Value = value; CekDataByValue(hflKode.Value); } }
        public string Text { get { return txtNama.Text; } set { txtNama.Text = value; } }

        private void CekDataByValue(string s_val)
        {
            Pegawai m = DAO_Pegawai.GetByID_Entity(s_val);
            if (m != null) txtNama.Text = m.Nama;
        }

        public string NamaClientID { get { return txtNama.ClientID; } }
        public string KodeClientID { get { return hflKode.ClientID; } }

        public TextBox NamaControl { get { return txtNama; } }
        public HiddenField KodeControl { get { return hflKode; } }
    }
}