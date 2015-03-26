using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class GY
    {
        public string GetGyMx(string did)
        {
            #region sql语句
            string sql = @"select  types,YPPCenter.extension,YPPCenter.extension1 as univalent,unit,YPPCenter.extension2 as price,ProductAmount  from YPPCenter      left join product   on yppcenter.projectid=product.productid
 where YPPCenter.TypeId=@did";
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
               
                //types	extension	univalent	unit	price	ProductAmount

                var zxobj = new { tp = ExChange(row["types"].ToSafeString()), extension = row["extension"].ToSafeString(), univalent = row["univalent"].ToSafeString(), unit = row["unit"].ToSafeString(), price = row["price"].ToSafeString(), ProductAmount = row["ProductAmount"].ToSafeString(), };
 
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

    }
}
