using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class LienHe_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<lienhe> Get_LienHe()
        {
            return empdb.FN_Get_LienHe().ToList();
        }

    }
}