using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class GioiThieu_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<gioithieu> Get_GioiThieu()
        {
            return empdb.FN_Get_GioiThieu().ToList();
        }
    }
}