using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Quantri_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<Get_PhanQuyen_Result> GetPhanQuyen()
        {
            return empdb.Get_PhanQuyen().ToList();
        }
    }
}