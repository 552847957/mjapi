using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJAPI
{
    public class Class1:IHttpModule
    {
         
        public void Dispose()
        {
          
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {

            context.BeginRequest += context_BeginRequest;
           
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}