using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WLX.DAL.Models;
using WLX.Data;
using WLX.Data.DataBase;
using WLX.Data.Models;
using WLX.Utils;
using WLX.Utils.WebUtils;

namespace WLX.DAL
{
    public class CheckMobileService
    {
        SqlHelper helper = new SqlHelper();
        CommDAL dal = new CommDAL();
        #region 变量声明
        // 声明静态变量 
        private const string PARA_CheckMobileInfo_CheckID = "@ID";
        private const string PARA_CheckMobileInfo_UserID = "@UserID";
        private const string PARA_CheckMobileInfo_Mobile = "@Mobile";
        private const string PARA_CheckMobileInfo_UpdateTime = "@UpdateTime";
        private const string PARA_CheckMobileInfo_CheckCode = "@CheckCode";
        private const string PARA_CheckMobileInfo_GetCodeTimes = "@GetCodeTimes";

        private const string CheckMobileInfo_SQL_SELECT = "SELECT ID, UserID, Mobile, UpdateTime, CheckCode, GetCodeTimes  FROM UserCheckMobile";
        private const string CheckMobileInfo_SQL_DELETE = "DELETE FROM UserCheckMobile WHERE ID = @ID";
        private const string CheckMobileInfo_SQL_UPDATE = "UPDATE UserCheckMobile SET  UserID= @UserID, Mobile= @Mobile, UpdateTime= @UpdateTime, CheckCode= @CheckCode, GetCodeTimes= @GetCodeTimes  WHERE ID = @ID";
        private const string CheckMobileInfo_SQL_INSERT = "INSERT INTO UserCheckMobile(UserID, Mobile, UpdateTime, CheckCode, GetCodeTimes ) values ( @UserID, @Mobile, @UpdateTime, @CheckCode, @GetCodeTimes ) ";
        #endregion

        #region 向数据库中插入一条新记录。
        /// <summary>
        /// 向数据库中插入一条新记录。
        /// </summary>
        /// <param name="info">实体类</param>
        /// <returns>新插入记录的编号</returns>
        public UserCheckMobile InsertCheckMobileInfo(UserCheckMobile info)
        {
            using (EntityContext db=new EntityContext())
            {
                info.ID = CommonUtil.GetNewID();
                info.UpdateTime = DateTime.Now;
                db.UserCheckMobiles.Add(info);
                db.SaveChanges();
                return info;
            }       
        }
        #endregion

        #region 向数据库中插入一条新记录。带事务
        /// <summary>
        /// 向数据库中插入一条新记录。带事务
        /// </summary>
        /// <param name="sp">事务对象</param>
        /// <param name="info">实体类</param>
        /// <returns>新插入记录的编号</returns>
        public  int InsertCheckMobileInfo(SqlTransaction sp, UserCheckMobile info)
        {
            // 声明参数数组并赋值
            SqlParameter[] _param =
            {
                SqlHelper.MakeParam(PARA_CheckMobileInfo_CheckID, SqlDbType.NVarChar,0,info.ID),
                SqlHelper.MakeParam(PARA_CheckMobileInfo_UserID, SqlDbType.NVarChar,0,info.UserID),
                SqlHelper.MakeParam(PARA_CheckMobileInfo_Mobile, SqlDbType.NVarChar,0,info.Mobile),
                SqlHelper.MakeParam(PARA_CheckMobileInfo_UpdateTime, SqlDbType.DateTime,0,info.UpdateTime),
                SqlHelper.MakeParam(PARA_CheckMobileInfo_CheckCode, SqlDbType.NVarChar,0,info.CheckCode),
                SqlHelper.MakeParam(PARA_CheckMobileInfo_GetCodeTimes, SqlDbType.Int,0,info.GetCodeTimes)
            };


            // 定义返回的ID
            int outID = 0;

            SqlHelper.ExecuteNonQuery(sp, CommandType.Text, CheckMobileInfo_SQL_INSERT, _param);

            //  返回
            return outID;

        }
        #endregion

        #region 向数据表UserCheckMobile更新一条记录

        /// <summary>
        /// 向数据表TaoXieUser_CheckMobile更新一条记录。
        /// </summary>
        /// <param name="info">实体类</param>
        /// <returns>影响的行数</returns>
        public  int UpdateCheckMobileInfo(UserCheckMobile info)
        {
            SqlParameter[] prams = new SqlParameter[6];
            prams[0] = new SqlParameter("@ID", info.ID);
            prams[1] = new SqlParameter("@UserID", info.UserID);
            prams[2] = new SqlParameter("@Mobile", info.Mobile);
            prams[3] = new SqlParameter("@UpdateTime", info.UpdateTime);
            prams[4] = new SqlParameter("@CheckCode", info.CheckCode);
            prams[5] = new SqlParameter("@GetCodeTimes", info.GetCodeTimes);

            return helper.ExecuteNonQuery(CheckMobileInfo_SQL_UPDATE, prams);
        }
        #endregion

        #region  向数据表UserCheckMobile更新一条记录。带事务
        /// <summary>
        /// 向数据表TaoXieUser_CheckMobile更新一条记录。带事务
        /// </summary>
        /// <param name="sp">事务对象</param>
        /// <param name="info">实体类</param>
        /// <returns>影响的行数</returns>
        public int UpdateCheckMobileInfo(SqlTransaction sp, CheckMobileInfo info)
        {
            // 声明参数数组并赋值
            SqlParameter[] _param = new SqlParameter[3];
            _param[0] = new SqlParameter("@CheckID", info.CheckID);
            _param[1] = new SqlParameter("@UserID", info.UserID);
            _param[2] = new SqlParameter("@Mobile", info.Mobile);
            _param[3] = new SqlParameter("@UpdateTime", info.UpdateTime);
            _param[4] = new SqlParameter("@CheckCode", info.CheckCode);
            _param[5] = new SqlParameter("@GetCodeTimes", info.GetCodeTimes);

            return SqlHelper.ExecuteNonQuery(sp, CommandType.Text, CheckMobileInfo_SQL_UPDATE, _param);
        }
        #endregion

        #region 删除数据表UserCheckMobile中的记录
        /// <summary>
        /// 删除数据表UserCheckMobile中的一条记录
        /// </summary>
        /// <param name="CheckID">checkID</param>
        /// <returns>影响的行数</returns>
        public bool DeleteCheckMobileInfo(string CheckID)
        {
            return dal.Delete<UserCheckMobile>(CheckID);
        }
        public bool DeleteCheckMobileInfo(string[] ids)
        {
            return dal.Delete<UserCheckMobile>(ids);
        }
        #endregion

        #region 删除数据表TaoXieUser_CheckMobile中的记录, 带事务
        /// <summary>
        /// 删除数据表TaoXieUser_CheckMobile中的一条记录, 带事务
        /// </summary>
        /// <param name="sp">事务对象</param>
        /// <param name="CheckID">checkID</param>
        /// <returns>影响的行数</returns>
        public int DeleteCheckMobileInfo(SqlTransaction sp, int CheckID)
        {
            SqlParameter[] _param ={
            SqlHelper.MakeParam(PARA_CheckMobileInfo_CheckID, SqlDbType.Int,0,CheckID),

            };

            return SqlHelper.ExecuteNonQuery(sp, CommandType.Text, CheckMobileInfo_SQL_DELETE, _param);
        }
        public int DeleteCheckMobileInfo(SqlTransaction sp, string ids)
        {
            return SqlHelper.ExecuteNonQuery(sp, CommandType.Text, "DELETE FROM UserCheckMobile WHERE CheckID IN (" + ids + ")");
        }
        #endregion

        #region 得到  usercheckmobile 数据实体
        /// <summary>
        /// 得到  taoxieuser_checkmobile 数据实体
        /// </summary>
        /// <param name="row">row</param>
        /// <returns>实体类</returns>
        public UserCheckMobile GetCheckMobileInfo(DataRow row)
        {
            UserCheckMobile Obj = new UserCheckMobile();
            if (row != null)
            {
                Obj.ID = ((row["ID"]) == DBNull.Value) ? "" : row["ID"].ToString();
                Obj.UserID = ((row["UserID"]) == DBNull.Value) ? "" : row["UserID"].ToString();
                Obj.Mobile = row["Mobile"].ToString();
                Obj.UpdateTime = ((row["UpdateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900-1-1") : Convert.ToDateTime(row["UpdateTime"]);
                Obj.CheckCode = row["CheckCode"].ToString();
                Obj.GetCodeTimes = ((row["GetCodeTimes"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["GetCodeTimes"]);
            }
            else
            {
                return null;
            }
            return Obj;
        }

        /// <summary>
        /// 得到  taoxieuser_checkmobile 数据实体
        /// </summary>
        /// <param name="dr">dr</param>
        /// <returns>实体类</returns>
        public UserCheckMobile GetCheckMobileInfo(IDataReader dr)
        {
            UserCheckMobile Obj = new UserCheckMobile();

            Obj.ID = ((dr["ID"]) == DBNull.Value) ? "" : dr["ID"].ToString();
            Obj.UserID = ((dr["UserID"]) == DBNull.Value) ? "" : dr["UserID"].ToString();
            Obj.Mobile = dr["Mobile"].ToString();
            Obj.UpdateTime = ((dr["UpdateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900-1-1") : Convert.ToDateTime(dr["UpdateTime"]);
            Obj.CheckCode = dr["CheckCode"].ToString();
            Obj.GetCodeTimes = ((dr["GetCodeTimes"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["GetCodeTimes"]);

            return Obj;
        }
        #endregion

        #region 根据ID, 返回一个UserCheckMobile对象
        /// <summary>
        /// 根据ID, 返回一个UserCheckMobile对象
        /// </summary>
        /// <param name="checkID">checkID</param>
        /// <returns>实体类</returns>
        public UserCheckMobile GetCheckMobileInfo(string checkID)
        {
            return dal.FindById<UserCheckMobile>(checkID);
        }


        public UserCheckMobile GetCheckMobileInfo(string userID, string mobile)
        {
            return dal.FindByField<UserCheckMobile>(userID,mobile);
        }

        /// <summary>
        /// 取得最近一次的用户手机验证信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        //public UserCheckMobile GetLastCheckMobileInfo(string userID)
        //{
        //    using (EntityContext db=new EntityContext())
        //    {
        //        return db.UserCheckMobiles.Where(i => i.UserID == userID).OrderByDescending(s => s.UpdateTime).FirstOrDefault();
        //    }
        //}
        public static UserCheckMobile GetLastCheckMobileInfo(string mobile)
        {
            using (EntityContext db = new EntityContext())
            {
                return db.UserCheckMobiles.Where(i => i.Mobile == mobile).OrderByDescending(s => s.UpdateTime).FirstOrDefault();
            }
        }
        #endregion

        #region 得到数据表taoxieuser_checkmobile所有记录
        /// <summary>
        /// 得到数据表taoxieuser_checkmobile所有记录
        /// </summary>
        /// <returns>数据集</returns>
        public List<UserCheckMobile> GetAllCheckMobileInfo()
        {
            using (EntityContext db = new EntityContext())
            {
                return db.UserCheckMobiles.ToList();
            }
        }
        #endregion

        #region 分页得到数据表taoxieuser_checkmobile的记录
        /// <summary>
        /// 分页得到数据表taoxieuser_checkmobile记录
        /// </summary>
        /// <returns>数据集</returns>
        //public List<CheckMobileInfo> GetCheckMobileInfoList(int pagesize, int pageindex, string condition, string orderfield, string sort)
        //{
        //    List<CheckMobileInfo> Obj = new List<CheckMobileInfo>();

        //    SqlDataReader dr = GetList("TaoXieUser_CheckMobile", pagesize, pageindex, condition, orderfield, sort);

        //    while (dr.Read())
        //    {
        //        Obj.Add(GetCheckMobileInfo(dr));
        //    }

        //    dr.Close();
        //    dr.Dispose();

        //    return Obj;
        //}

        /// <summary>
        /// 分页得到数据表taoxieuser_checkmobile记录（使用临时表）
        /// </summary>
        /// <returns>数据集</returns>
        //public List<CheckMobileInfo> GetCheckMobileInfoList2(int pagesize, int pageindex, string condition, string orderfield, string sort)
        //{
        //    List<CheckMobileInfo> Obj = new List<CheckMobileInfo>();

        //    SqlDataReader dr = GetListByTempTable("TaoXieUser_CheckMobile", pagesize, pageindex, condition, "CheckID", orderfield, sort);

        //    while (dr.Read())
        //    {
        //        Obj.Add(GetCheckMobileInfo(dr));
        //    }

        //    dr.Close();
        //    dr.Dispose();

        //    return Obj;
        //}
        #endregion

        #region 检测是否存在根据主键
        /// <summary>
        /// 检测是否存在根据主键
        /// </summary>
        /// <param name="checkID">checkID</param>
        /// <returns>是/否</returns>
        public bool IsExistCheckMobileInfo(string checkID)
        {
            return dal.IsExist<UserCheckMobile>(checkID);
        }
        #endregion

        #region 取得数据表CheckMobileInfo的记录总数
        /// <summary>
        /// 取得数据表CheckMobileInfo的记录总数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>记录总数</returns>
        public int GetCheckMobileInfoCount(string condition)
        {
            using (EntityContext db = new EntityContext())
            {
                var list= db.UserCheckMobiles.ToList();
                if (list != null)
                    return list.Count();
                else
                    return 0;
            }
        }
        #endregion
    }
}
