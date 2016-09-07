using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace WLX.Utils.WebUtils
{
    public static class ListUtils
    {
        public static string ListJoin<T>(string separator, IList<T> list, Func<T, string> GetValue)
        {
            string result = string.Empty;
            var valueList = list.Select(GetValue).ToList();
            if (valueList.Count > 0)
                result = string.Join(separator, valueList);
            return result;
        }

        public static string StringJoin<T>(this IList<T> list, Func<T, string> GetValue)
        {
            return ListJoin<T>(",", list, GetValue);
        }
        public static string StringJoin<T>(this IList<T> list, Func<T, string> GetValue, string separator)
        {
            return ListJoin<T>(separator, list, GetValue);
        }

        /// <summary>
        /// 作者：王赟
        /// 时间：2014-11-12
        /// 描述：list集合转换string类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>返回："'xx','xx','xx'"</returns>
        public static string ListToSqlIn<T>(this List<T> list)
        {
            string st = string.Empty;
            var len = list.Count;
            for (int i = 0; i < len; i++)
            {
                st += string.Format("'{0}'", list[i]) + ",";
            }

            return st.Length > 0 ? st.Remove(st.Length - 1, 1) : st;
        }

        /// <summary>
        /// 分钱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="totalMoney">要分的钱数</param>
        /// <param name="list">分钱依据的对象</param>
        /// <param name="GetDivider">计算分钱比例的字段</param>
        /// <param name="GetKey">用作key的字段</param>
        /// <returns></returns>
        public static Dictionary<string, decimal> MoneyDivide<T>(decimal totalMoney, List<T> list, Func<T, decimal> GetDivider, Func<T, string> GetKey)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            decimal totalDevide = list.Sum(GetDivider);

            foreach (T item in list)
            {
                decimal value = GetDivider(item) / totalDevide * totalMoney;
                value = Math.Round(value, 2);
                result.Add(GetKey(item), value);
            }
            decimal totalResult = result.Values.Sum();
            result[GetKey(list[0])] += totalMoney - totalResult;
            return result;
        }

        public static List<T> Distinct<T, V>(this List<T> list, Func<T, V> Comparer)
        {
            return list.Distinct(new DistinctComparer<T, V>(Comparer)).ToList();
        }

        public static List<T> Sort<T, V>(this List<T> list, Func<T, V> Comparer)
        {
            return list.Distinct(new DistinctComparer<T, V>(Comparer)).ToList();
        }

        /// <summary>
        /// 作者：王赟
        /// 时间：2015-03-30
        /// 描述：转化一个DataTable    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="list"></param>    
        /// <returns></returns>    
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {

            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }


        /// <summary>
        /// 作者：王赟
        /// 时间：2015-03-30
        /// 描述：DataTable 转换为List 集合    
        /// </summary>    
        /// <typeparam name="TResult">类型</typeparam>    
        /// <param name="dt">DataTable</param>    
        /// <returns></returns>    
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            return EntityObject.DT2EntityList<T>(dt);
            ////创建一个属性的列表    
            //List<PropertyInfo> prlist = new List<PropertyInfo>();
            ////获取TResult的类型实例  反射的入口    

            //Type t = typeof(T);

            ////获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表     
            //Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            ////创建返回的集合    

            //List<T> oblist = new List<T>();

            //foreach (DataRow row in dt.Rows)
            //{
            //    //创建TResult的实例    
            //    T ob = new T();
            //    //找到对应的数据  并赋值    
            //    prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
            //    //放入到返回的集合中.    
            //    oblist.Add(ob);
            //}
            //return oblist;
        }




        /// <summary>
        /// 作者：王赟
        /// 时间：2015-03-30
        /// 描述：将集合类转换成DataTable    
        /// </summary>    
        /// <param name="list">集合</param>    
        /// <returns></returns>    
        public static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }



        /// <summary>
        /// 作者：王赟
        /// 时间：2015-03-30
        /// 描述：将泛型集合类转换成DataTable
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>    
        /// <returns>数据集(表)</returns>    
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable<T>(list, null);

        }



        /// <summary>
        /// 作者：王赟
        /// 时间：2015-03-30
        /// 描述：将泛型集合类转换成DataTable    
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>    
        /// <param name="list">集合</param>    
        /// <param name="propertyName">需要返回的列的列名</param>    
        /// <returns>数据集(表)</returns>    
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

       

        public enum OrderBy
        {
            Desc,
            Asc
        }
    }

    /// <summary>
    /// DataTable与实体类互相转换
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public  class ModelHandler<T> where T : new()
    {
        #region DataTable转换成实体类

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public static T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                    propertyInfo.SetValue(model, dr[i], null);
            }
            return model;
        }

        #endregion

        
    }
}
