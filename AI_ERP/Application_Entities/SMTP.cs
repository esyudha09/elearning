using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class SMTP
    {
        public Guid Kode { get; set; }
        public Guid Rel_Aplikasi { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int MaxKirim { get; set; }
    }
}