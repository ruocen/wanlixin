using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using Webdiyer.WebControls.Mvc;
using WLX.Data;
using WLX.Utils;

namespace WLX.Data
{
    public class CommDAL
    {
        EntityContext ctx = new EntityContext();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public PagedList<T> GetList<T,TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, int pageIndex, int pageSize = 20, bool ascSort = true) where T : class,new()
        {
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 20;
            if (ascSort)
                return entities.Where(where).OrderBy(order).ToPagedList(pageIndex, pageSize);
            else
                return entities.Where(where).OrderByDescending(order).ToPagedList(pageIndex, pageSize);
        }
        public PagedList<T> GetList<T,TKey>(Expression<Func<T, TKey>> order, int pageIndex, int pageSize = 20, bool ascSort = true) where T : class,new()
        {
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 20;
            if (ascSort)
                return entities.OrderBy(order).ToPagedList(pageIndex, pageSize);
            else
                return entities.OrderByDescending(order).ToPagedList(pageIndex, pageSize);
        }

        public T SaveOrUpdate<T>(T o) where T : class,new()
        {
            if (o == null) return o;
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
            string id = Reflection.GetProperty<string>(o, "ID");
            if (string.IsNullOrWhiteSpace(id))
            {
                Reflection.SetProperty(o, "ID", Guid.NewGuid().ToString().Replace("-", ""));
                entities.Add(o);
            }
            else
            {
                //T oo = FindById(id);
                T oo = entities.Find(id);
                if (oo != o) Reflection.CopyTo<T>(o, oo);
            }

            this.ctx.SaveChanges();

            return o;
        }

        public void SaveOrUpdate<T>(IEnumerable<T> ls) where T : class,new()
        {
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
            foreach (var o in ls)
            {
               
                string id = Reflection.GetProperty<string>(o, "ID");
                if (string.IsNullOrWhiteSpace(id))
                {
                    Reflection.SetProperty(o, "ID", Guid.NewGuid().ToString().Replace("-", ""));
                    entities.Add(o);
                }
                else
                {
                    //T oo = FindById(id);
                    T oo = entities.Find(id);
                    if (oo != o) Reflection.CopyTo<T>(o, oo);
                }
            }
            this.ctx.SaveChanges();
        }
        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="id">主键ID</param>
        public bool Delete<T>(string id) where T : class,new()
        {
            return Delete<T>(FindById<T>(id));
        }
        public bool Delete<T>(T o) where T : class,new()
        {
            if (o == null) return true;
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
            entities.Remove(o);
            if (ctx.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete<T>(string[] ids) where T : class,new()
        {
            var param = Expression.Parameter(typeof(T), "m");
            var prop = Expression.Property(param, "ID");
            var cost = Expression.Constant(ids);
            var ms = typeof(Enumerable).GetMethods().SingleOrDefault(p =>
                p.Name == "Contains" && p.IsGenericMethod
                && p.GetParameters().Count() == 2);
            var call = Expression.Call(ms.MakeGenericMethod(new Type[] { typeof(string) }), cost, prop);
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
            var li = entities.Where((Expression<Func<T, bool>>)Expression.Lambda(call, param)).ToList();
            return Delete(li);
        }
        public bool Delete<T>(ICollection<T> os) where T : class,new()
        {
            if (os == null || os.Count == 0) return true;
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
            os.ToList().ForEach(p => entities.Remove(p));
            if (ctx.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExist<T>(string id) where T : class,new()
        {
            return FindById<T>(id) != null;
        }
        /// <summary>
        /// 通过ID获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindById<T>(string id) where T : class,new()
        {
            try
            {
                var param = Expression.Parameter(typeof(T), "m");
                var prop = Expression.Property(param, "ID");
                var cost = Expression.Constant(id);
                DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
                return entities.SingleOrDefault((Expression<Func<T, bool>>)Expression.Lambda(Expression.Equal(prop, cost), param));
                // return Entities.SingleOrDefault(m => IdProp.GetValue(m, null).ToString() == id);
            }
            catch (Exception ex)
            {
                Tools.MessBox(ex.ToString());
                return null;
            }
        }


     /// <summary>
     /// 通过某个栏位获得一条数据库记录
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="field"></param>
     /// <param name="val"></param>
     /// <returns></returns>
        public T FindByField<T>(string field,string val) where T : class,new()
        {
            try
            {
                var param = Expression.Parameter(typeof(T), "m");
                var prop = Expression.Property(param, field);
                var cost = Expression.Constant(val);
                DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);
                return entities.SingleOrDefault((Expression<Func<T, bool>>)Expression.Lambda(Expression.Equal(prop, cost), param));
                // return Entities.SingleOrDefault(m => IdProp.GetValue(m, null).ToString() == id);
            }
            catch (Exception ex)
            {
                Tools.MessBox(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<T> GetList<T>(Expression<Func<T, bool>> where) where T : class,new()
        {
            DbSet<T> entities = Reflection.GetProperty<DbSet<T>>(ctx, typeof(T).Name);

            return entities.Where(where).ToList<T>();

        }

        //
        // 摘要: 
        //     对数据库执行给定的 DDL/DML 命令。与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。您可以在
        //     SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为 DbParameter。context.Database.ExecuteSqlCommand("UPDATE
        //     dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor); 或者，您还可以构造一个
        //     DbParameter 并将它提供给 SqlQuery。这允许您在 SQL 查询字符串中使用命名参数。context.Database.ExecuteSqlCommand("UPDATE
        //     dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author",
        //     userSuppliedAuthor));
        //
        // 参数: 
        //   sql:
        //     命令字符串。
        //
        //   parameters:
        //     要应用于命令字符串的参数。
        //
        // 返回结果: 
        //     执行命令后由数据库返回的结果。
        public int ExecuteSqlCommand(string sql)
        {
            return ctx.Database.ExecuteSqlCommand(sql);
        }
        //
        // 摘要: 
        //     创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。该类型不必是实体类型。即使返回对象的类型是实体类型，上下文也决不会跟踪此查询的结果。使用
        //     System.Data.Entity.DbSet<TEntity>.SqlQuery(System.String,System.Object[])
        //     方法可返回上下文跟踪的实体。与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。您可以在 SQL
        //     查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为 DbParameter。context.Database.SqlQuery&amp;lt;Post&amp;gt;("SELECT
        //     * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); 或者，您还可以构造一个 DbParameter
        //     并将它提供给 SqlQuery。这允许您在 SQL 查询字符串中使用命名参数。context.Database.SqlQuery&amp;lt;Post&amp;gt;("SELECT
        //     * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        //
        // 参数: 
        //   sql:
        //     SQL 查询字符串。
        //
        //   parameters:
        //     要应用于 SQL 查询字符串的参数。如果使用输出参数，则它们的值在完全读取结果之前不可用。这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见
        //     http://go.microsoft.com/fwlink/?LinkID=398589。
        //
        // 类型参数: 
        //   TElement:
        //     查询所返回对象的类型。
        //
        // 返回结果: 
        //     一个 System.Data.Entity.Infrastructure.DbRawSqlQuery<TElement> 对象，此对象在枚举时将执行查询。
        public List<string> SqlQuery(string sql, object[] param)
        {
           List<string> list = ctx.Database.SqlQuery<string>(sql,param).ToList();
           return list;
        }

        public int CountSqlQuery(string sql)
        {
            try
            {
                int cRet = ctx.Database.SqlQuery<int>(sql).First();
                return int.Parse(cRet.ToString());
            }
            catch (Exception ex) {
                return 0;
            }
            
        }


        public List<T> SqlQuery<T>(string sql)
        {
            List<T> list = ctx.Database.SqlQuery<T>(sql).ToList();
            return list;
        }

    }



}
