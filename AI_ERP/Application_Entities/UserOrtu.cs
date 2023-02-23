using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class UserOrtu
    {
        public Guid Kode { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public bool IsNonAktif { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}