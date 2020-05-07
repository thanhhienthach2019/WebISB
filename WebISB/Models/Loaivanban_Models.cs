using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Loaivanban_Models
    {
        ISBEntities empdb = new ISBEntities();

        public List<Get_loaiVanban_Result> Get_Loaivanban()
        {
            return empdb.Get_loaiVanban().ToList();
        }
        public List<get_loaivanban_dropdownlist_Result> Get_Loaivanban_ddl()
        {
            return empdb.get_loaivanban_dropdownlist().ToList();
        }
    }
}