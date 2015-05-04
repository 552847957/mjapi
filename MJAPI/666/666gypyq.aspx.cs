using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MJAPI._666
{
    public partial class _666gypyq : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string info = "客户端IP：" + Page.Request.UserHostAddress;

                new BLL.Hdbll().Add2(info, "贵阳-朋友圈--点击链接");

            }
            catch (Exception ee)
            {
                Response.Write(ee.Message);

            }
        }
    }
}