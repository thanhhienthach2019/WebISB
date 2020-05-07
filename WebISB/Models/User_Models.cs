using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class User_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<Get_User_Result> GetUser()
        {
            return empdb.Get_User().ToList();
        }
        public List<Get_PhanQuyen_Result> GetPhanquyen()
        {
            return empdb.Get_PhanQuyen().ToList();
        }
    }
}