using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public enum JenisUser
    {
        Siswa,
        OrangTua,
        Karyawan
    }

    public class UserLogin
    {
        public string UserID { get; set; }
        public string NoInduk { get; set; }
        public string Password { get; set; }
        public JenisUser JenisUser { get; set; }
        public int ProgressPercent { get; set; }
    }
}