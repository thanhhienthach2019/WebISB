using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Donvi_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<FN_get_donvi_Result> GetDonVi()
        {
            return empdb.FN_get_donvi().ToList();
        }
    }
}