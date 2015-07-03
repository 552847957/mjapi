using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsApi
{


    /// <summary>
    ///请在此处填写这个构造函数的说明
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_did">设计师或工长名称</param>
    /// <param name="_dgrade">工长有服务地区设计师是所在城市</param>
    /// <param name="_dconment">个人简介或设计理念</param>
    /// <param name="_dpname">设计能力或装修经验</param>
    /// <param name="_dpid">施工能力或设计经验</param>
    /// <param name="_casename">工作效率</param>
    /// <param name="_createtime">服务态度</param>
    /// <param name="_extension">所属公司</param>
    /// <param name="_extension2">邮箱</param>
    /// <param name="_extension3">头像</param>
    /// <param name="_extension4">从业年限</param>
    /// <param name="_extension5">施工团队</param>
    /// <param name="_mphone">手机</param>
    /// <param name="_appointmentnum">预约人数</param>
    /// <param name="_tp">记录类型(装修公司,设计师,工长)</param>
    /// <param name="_extension6">密码</param>
    /// <param name="_extension7">来源</param>
    /// <param name="_address">地址（存id 1,2,3）</param>
    /// <param name="_xxaddress">详细地址</param>
    /// <param name="_gyzj">关于自己</param>
    /// <param name="_idnumber">身份证号</param>
    /// <param name="_idnumberz">身份证正面</param>
    /// <param name="_idnumberf">身份证反面</param>
    /// <param name="_examinenr">审核内容</param>
    /// <param name="_examine">审核状态</param>
    /// <param name="_ctime">时间</param>
    public class DesignerGrade
    {
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string dID;
        public string DID
        {
            get { return dID; }
            set { dID = value; }
        }

        private string dgrade;
        public string Dgrade
        {
            get { return dgrade; }
            set { dgrade = value; }
        }

        private string dconment;
        public string Dconment
        {
            get { return dconment; }
            set { dconment = value; }
        }

        private string dpname;
        public string Dpname
        {
            get { return dpname; }
            set { dpname = value; }
        }

        private string dPID;
        public string DPID
        {
            get { return dPID; }
            set { dPID = value; }
        }

        private string caseName;
        public string CaseName
        {
            get { return caseName; }
            set { caseName = value; }
        }

        private string createtime;
        public string Createtime
        {
            get { return createtime; }
            set { createtime = value; }
        }

        private string extension;
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        private string extension2;
        public string Extension2
        {
            get { return extension2; }
            set { extension2 = value; }
        }

        private string extension3;
        public string Extension3
        {
            get { return extension3; }
            set { extension3 = value; }
        }

        private string extension4;
        public string Extension4
        {
            get { return extension4; }
            set { extension4 = value; }
        }

        private string extension5;
        public string Extension5
        {
            get { return extension5; }
            set { extension5 = value; }
        }

        private string mPhone;
        public string MPhone
        {
            get { return mPhone; }
            set { mPhone = value; }
        }

        private int appointmentNum;
        public int AppointmentNum
        {
            get { return appointmentNum; }
            set { appointmentNum = value; }
        }

        private string tp;
        public string Tp
        {
            get { return tp; }
            set { tp = value; }
        }

        private string extension6;
        public string Extension6
        {
            get { return extension6; }
            set { extension6 = value; }
        }

        private string extension7;
        public string Extension7
        {
            get { return extension7; }
            set { extension7 = value; }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private string xxAddress;
        public string XxAddress
        {
            get { return xxAddress; }
            set { xxAddress = value; }
        }

        private string gyZj;
        public string GyZj
        {
            get { return gyZj; }
            set { gyZj = value; }
        }

        private string idNumber;
        public string IdNumber
        {
            get { return idNumber; }
            set { idNumber = value; }
        }

        private string idNumberZ;
        public string IdNumberZ
        {
            get { return idNumberZ; }
            set { idNumberZ = value; }
        }

        private string idNumberF;
        public string IdNumberF
        {
            get { return idNumberF; }
            set { idNumberF = value; }
        }

        private string examineNr;
        public string ExamineNr
        {
            get { return examineNr; }
            set { examineNr = value; }
        }

        private string examine;
        public string Examine
        {
            get { return examine; }
            set { examine = value; }
        }

        private string mjRztz;
        public string MjRztz
        {
            get { return mjRztz; }
            set { mjRztz = value; }
        }

        private string cTime;
        public string CTime
        {
            get { return cTime; }
            set { cTime = value; }
        }
    }
}
