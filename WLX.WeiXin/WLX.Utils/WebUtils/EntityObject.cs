using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace WLX.Utils.WebUtils
{
    public class EntityObject
    {

        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }



        public static string PrintEntity(object entity)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] pis = entity.GetType().GetProperties();
            sb.Append(entity.GetType().Name + ":{");
            foreach (PropertyInfo propertyInfo in pis)
            {
                sb.Append(propertyInfo.Name + ":" + propertyInfo.GetValue(entity, null) + ",");
            }
            sb.Append("}");
            return sb.ToString();
        }




        /// <summary>
        /// 对象填充，自动填充同名同类型的属性(Memo字段不填充)
        /// </summary>
        /// <param name="originalEntity"></param>
        /// <param name="targetEntity"></param>
        public static void FillEntity(object originalEntity, object targetEntity)
        {
            if (originalEntity == null)
            {
                targetEntity = new object();
                return;
            }
            var originalProperties = originalEntity.GetType().GetProperties();
            var targetProperties = targetEntity.GetType().GetProperties();
            foreach (PropertyInfo property in originalProperties)
            {
                if (property.Name == "Memo") continue;
                var targetPropertie = targetProperties.FirstOrDefault(t => t.Name.Equals(property.Name));
                if (targetPropertie != null && targetPropertie.PropertyType == property.PropertyType)
                {
                    object value = property.GetValue(originalEntity, null);
                    if (value != null)
                    {
                        targetPropertie.SetValue(targetEntity, value, null);
                    }
                }
            }
        }

        public static List<string> Enum2List<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(v => v.ToString()).ToList();
        }

        public static string DateTime2String(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        ///   <param   name=data要序列化的对象</param
        ///   <returns返回存放序列化后的数据缓冲区</returns
        public static byte[] Serialize(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, data);
            return rems.GetBuffer();
        }

        ///   <summary
        ///   反序列化
        ///   </summary
        ///   <param   name=data数据缓冲区</param
        ///   <returns对象</returns
        public static object Deserialize(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream(data);
            return formatter.Deserialize(rems);
        }


        /// 将一个DataTable转换成单个实体类
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>
        /// <returns>返回一个实体类</returns>
        public static T DT2SimpleEntity<T>(DataTable dt)
        {
            List<T> entity = DT2EntityList<T>(dt);

            if (entity == null || entity.Count == 0)
                return default(T);

            return entity[0];
        }


        public static T DT2SimpleEntity<T>(DataSet ds)
        {
            List<T> entity = DT2EntityList<T>(ds);

            if (entity == null || entity.Count == 0)
                return default(T);

            return entity[0];
        }

        public static List<T> DT2EntityList<T>(DataSet ds)
        {
            if (ds == null)
            {
                return new List<T>();
            }

            if (ds.Tables.Count == 0)
            {
                return new List<T>();
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                return new List<T>();
            }

            DataTable dt = ds.Tables[0];

            return DT2EntityList<T>(dt);
        }


        public static List<T> DT2EntityList<T>(DataTable dt, string tableName)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new List<T>();
            }

            List<T> entityList = new List<T>();
            T entity = default(T);
            foreach (DataRow dr in dt.Rows)
            {
                entity = Activator.CreateInstance<T>();
                Type type = entity.GetType();
                PropertyInfo[] pis = type.GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    string name = pi.Name;
                    if (dt.Columns.Contains("DefCN") || dt.Columns.Contains("ID"))
                    {
                        //if ((name.Equals(tableName + "ID") || name.Equals(tableName + "DefCN") || name.Equals(tableName + "DefEN"))) name = name.Replace(tableName, "");
                    }
                    if (dt.Columns.Contains(name))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        object value = dr[name];
                        if (dr[name] != DBNull.Value)
                        {
                            try
                            {
                                if (Nullable.GetUnderlyingType(pi.PropertyType) != null)
                                {
                                    pi.SetValue(entity, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(pi.PropertyType).ToString())), null);
                                }
                                else
                                {
                                    pi.SetValue(entity, Convert.ChangeType(value, Type.GetType(pi.PropertyType.ToString())), null);
                                }

                            }
                            catch
                            {
                            }
                        }
                    }
                }
                entityList.Add(entity);
            }
            //            entityList.Reverse();
            return entityList;
        }
        public static List<T> DT2EntityList<T>(DataTable dt)
        {
            return DT2EntityList<T>(dt, typeof(T).Name);
        }

        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string Dt2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":[");
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataRow2Json(DataRow dr)
        {
            StringBuilder jsonBuilder = new StringBuilder();


            jsonBuilder.Append("{");
            foreach (DataColumn column in dr.Table.Columns)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(column.ColumnName);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(dr[column.ColumnName].ToString());
                jsonBuilder.Append("\",");
            }

            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");


            return jsonBuilder.ToString().TrimEnd(',');
        }

        public static string DataRow2Json(DataRow dr, string ColumnName)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            jsonBuilder.Append("{");
            foreach (DataColumn column in dr.Table.Columns)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(column.ColumnName);
                jsonBuilder.Append("\":\"");
                if (!column.ColumnName.Equals(ColumnName))
                {
                    jsonBuilder.Append(dr[column.ColumnName].ToString());
                }
                jsonBuilder.Append("\",");
            }

            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");


            return jsonBuilder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 实体转换json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string EntityToJson(Object obj)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            Object result = null;
            Type type = obj.GetType();
            PropertyInfo[] pis = type.GetProperties();

            jsonBuilder.Append("{");
            for (int i = 0; i < pis.Length; i++)
            {
                PropertyInfo pi = pis[i];

                result = pi.GetValue(obj, null);
                if (!pis[i].Name.Equals("ExtensionData"))
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(pis[i].Name);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(result);
                    jsonBuilder.Append("\",");
                }

            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");



            return jsonBuilder.ToString().TrimEnd(',');
        }

        public static string EntityToJson(Object obj, string Name)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            Object result = null;
            Type type = obj.GetType();
            PropertyInfo[] pis = type.GetProperties();

            jsonBuilder.Append("{");
            for (int i = 0; i < pis.Length; i++)
            {
                PropertyInfo pi = pis[i];

                result = pi.GetValue(obj, null);
                if (!pis[i].Name.Equals("ExtensionData") && !pis[i].Name.Equals(Name))
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(pis[i].Name);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(result);
                    jsonBuilder.Append("\",");
                }

            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");



            return jsonBuilder.ToString().TrimEnd(',');
        }

    }
}
