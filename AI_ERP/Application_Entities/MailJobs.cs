using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class MailJobs
    {
        public Guid Kode { get; set; }
        public string Rel_Aplikasi { get; set; }
        public string Keterangan { get; set; }
        public string Sender { get; set; }
        public string SMTP { get; set; }
        public string Port { get; set; }
        public string PWD { get; set; }
        public string SenderName { get; set; }
        public string Tujuan { get; set; }
        public string ToPerson { get; set; }
        public string Subjek { get; set; }
        public string Message { get; set; }
        public DateTime Tanggal { get; set; }
        public string Status { get; set; }
        public bool IsDone { get; set; }
        public string UserIDSender { get; set; }
        public DateTime LinkExpiredDate { get; set; }
    }
}