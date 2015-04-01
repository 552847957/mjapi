using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MJAPI._666
{
    public partial class _666 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string info = "客户端IP：" + Page.Request.UserHostAddress;

            new BLL.Hdbll().Add2(info,"点击链接");
        }
    }
}