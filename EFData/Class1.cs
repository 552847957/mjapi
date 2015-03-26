using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFData
{
    public class Class1
    {

        public string Getsss()
        {


            using (MJEntities entity = new MJEntities())
            {



                var lis = entity.Tentent.Where(n=>n.Extension=="1").Select(n=>n.Extension) ;


                var liss = from u in entity.Tentent                       
                           where u.CreateTime == "2012"  
                           select u;


              


                return lis.Count().ToString();
            }



           
        }
    }
}
