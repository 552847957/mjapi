using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsApi
{
    public class WinningRecord
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

        private string prizename;
        public string Prizename
        {
            get { return prizename; }
            set { prizename = value; }
        }

        private DateTime drawtime;
        public DateTime Drawtime
        {
            get { return drawtime; }
            set { drawtime = value; }
        }

        private int issend;
        public int Issend
        {
            get { return issend; }
            set { issend = value; }
        }

        private DateTime sendtime;
        public DateTime Sendtime
        {
            get { return sendtime; }
            set { sendtime = value; }
        }
    }
}
