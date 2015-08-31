using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsApi
{
    public class Bargain
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string openid;
        public string Openid
        {
            get { return openid; }
            set { openid = value; }
        }

        private string nickname;
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        private string headimg;
        public string Headimg
        {
            get { return headimg; }
            set { headimg = value; }
        }

        private string foruserid;
        public string Foruserid
        {
            get { return foruserid; }
            set { foruserid = value; }
        }

        private int value1;
        public int Value
        {
            get { return value1; }
            set { value1 = value; }
        }

        private string bargainname;
        public string Bargainname
        {
            get { return bargainname; }
            set { bargainname = value; }
        }

        private DateTime createtime;
        public DateTime Createtime
        {
            get { return createtime; }
            set { createtime = value; }
        }
    }
}
