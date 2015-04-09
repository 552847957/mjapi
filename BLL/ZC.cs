using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using System.Data.SqlClient;
namespace BLL
{
    public class ZC
    {

        /// <summary>
        /// 得到主材明细
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public string GetZcMx(string did)
        {
            #region sql语句
            string sql = @"select * from (
select sum(cast(num as float))  as Num ,sum(cast(Price as float)) as price,ShowroomId,Bname,Pmodel,unit,
netPrice,view_sp.extension,view_sp.gg ,Marketprice,pname,cName,ProductId,projectTypeId,pc.SmallPic,
tp from view_sp left join Pic as pc on pc.TypeId=ProductId and pc.PicId=(select MAX(PicId) 
from pic where Pic.TypeId=view_sp.ProductId) where showRoomid=@did and  
view_sp.Extension1='房间' group by ShowroomId,Bname,Pmodel,unit
,netPrice,view_sp.extension,view_sp.gg,Marketprice,pname,cName,ProductId,projectTypeId,SmallPic,tp) as b";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@did",did)
            };

            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);

            #endregion
            //     Num	price	ShowroomId	Bname	Pmodel	unit	netPrice	extension	Marketprice	pname	cName	ProductId	projectTypeId	SmallPic	tp


            #region 按组序列化
            Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                //http://www.mj100.com/admin/UploadFile/100X65/201404/MCLT_C_2014040117553105537432.jpg   201405\\DSWJ_C_2014050614221202358523.jpg
                var zxobj = new { productid = row["productid"].ToSafeString(), num = row["num"].ToSafeString(), price = row["price"].ToSafeString(), unit = row["unit"].ToSafeString(), tp = ExChange(row["tp"].ToSafeString()), netprice = row["netprice"].ToSafeString(), pname = row["pname"].ToSafeString(), bnmae = row["bname"].ToSafeString(), pmodel = row["pmodel"].ToSafeString(), gg = row["gg"].ToSafeString(), smallpic = "http://www.mj100.com/admin/UploadFile/100X65/" + row["smallpic"].ToSafeString().Replace("\\", "/") };

                if (dic.ContainsKey(zxobj.tp))
                {
                    List<object> listemp = dic[zxobj.tp];
                    listemp.Add(zxobj);
                    dic[zxobj.tp] = listemp;
                }
                else
                {
                    List<object> listemp = new List<object>();
                    listemp.Add(zxobj);
                    dic.Add(zxobj.tp, listemp);
                }
            }
            #endregion


            return JsonConvert.SerializeObject(dic);

        }

        public Dictionary<string, List<object>> GetZcMxExt(string did)
        {
            #region sql语句
            string sql = @"select * from (
select sum(cast(num as float))  as Num ,sum(cast(Price as float)) as price,ShowroomId,Bname,Pmodel,unit,
netPrice,view_sp.extension,view_sp.gg ,Marketprice,pname,cName,ProductId,projectTypeId,pc.SmallPic,
tp from view_sp left join Pic as pc on pc.TypeId=ProductId and pc.PicId=(select MAX(PicId) 
from pic where Pic.TypeId=view_sp.ProductId) where showRoomid=@did and  
view_sp.Extension1='房间' group by ShowroomId,Bname,Pmodel,unit
,netPrice,view_sp.extension,view_sp.gg,Marketprice,pname,cName,ProductId,projectTypeId,SmallPic,tp) as b";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@did",did)
            };

            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);

            #endregion
            //     Num	price	ShowroomId	Bname	Pmodel	unit	netPrice	extension	Marketprice	pname	cName	ProductId	projectTypeId	SmallPic	tp


            #region 按组序列化
            Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                //http://www.mj100.com/admin/UploadFile/100X65/201404/MCLT_C_2014040117553105537432.jpg   201405\\DSWJ_C_2014050614221202358523.jpg
                var zxobj = new { productid = row["productid"].ToSafeString(), num = row["num"].ToSafeString(), price = row["price"].ToSafeString(), unit = row["unit"].ToSafeString(), tp = ExChange(row["tp"].ToSafeString()), netprice = row["netprice"].ToSafeString(), pname = row["pname"].ToSafeString(), bnmae = row["bname"].ToSafeString(), pmodel = row["pmodel"].ToSafeString(), gg = row["gg"].ToSafeString(), smallpic = "http://www.mj100.com/admin/UploadFile/100X65/" + row["smallpic"].ToSafeString().Replace("\\", "/") };

                if (dic.ContainsKey(zxobj.tp))
                {
                    List<object> listemp = dic[zxobj.tp];
                    listemp.Add(zxobj);
                    dic[zxobj.tp] = listemp;
                }
                else
                {
                    List<object> listemp = new List<object>();
                    listemp.Add(zxobj);
                    dic.Add(zxobj.tp, listemp);
                }
            }
            #endregion


            return dic;

        }

        public string ExChange(string input)
        {
            string returnstr = input;

            switch (input)
            {
                case "qm":
                    returnstr = "墙面";
                    break;
                case "dm":
                    returnstr = "顶面";
                    break;
                case "ld":
                    returnstr = "地面";
                    break;
                case "jj":
                    returnstr = "洁具";
                    break;
                case "ju":
                    returnstr = "家具";
                    break;
                case "cj":
                    returnstr = "厨具";
                    break;
                default:
                    break;
            }

            return returnstr;
        }


        /// <summary>
        /// 推荐主材
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public string RecommendZc(string productid)
        {

            string sql = @" select top 6 a.BID , PID,Unit,Netprice,Pname,Pmodel,  gg, 

 smallpic ,Brand.Bname from
(select top 6 BID , PID,Unit,Netprice,Pname,Pmodel,extension1 as gg, 

extension as smallpic from Products where MOID in(
select MOID from Products where PID=@productid
) and pid<>@productid) as a left join Brand on a.BID=Brand.BID";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@productid",productid)
            };
            List<object> lis = new List<object>();

            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row=dt.Rows[i];

                var zxobj = new { productid = row["pid"].ToSafeString(), unit = row["unit"].ToSafeString(), netprice = row["netprice"].ToSafeString(), pname = row["pname"].ToSafeString(), bnmae = row["bname"].ToSafeString(), pmodel = row["pmodel"].ToSafeString(), gg = row["gg"].ToSafeString(), smallpic = "http://www.mj100.com/admin/UploadFile/100X65/" + row["smallpic"].ToSafeString().Replace("\\", "/") };
                lis.Add(zxobj);
            }


            return JsonConvert.SerializeObject(lis);
        }
    }
}
