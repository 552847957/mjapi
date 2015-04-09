using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class Dp
    {
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string To16Md5(this string str)
        {
           return Commen.DEncrypt.MD5(str,true);
        }
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            if (str.Replace(" ","").Length==0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 安全的tostring方法，屏蔽为null的情况
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSafeString(this object obj)
        {
            if (obj==null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        
        }

        public static List<T> GetList<T>(T o)
        {
             var lis=new  List<T>();

             lis.Add(o);
             return lis;
        }
    }
}
