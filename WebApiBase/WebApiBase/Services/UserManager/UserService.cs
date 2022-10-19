using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using WebApiBase.DatabaseAccessor;
using WebApiBase.Extensions;
using WebApiBase.Models;
using WebApiBase.Utils;

namespace WebApiBase.Services.UserManager
{
    public class UserService : BaseService, IUser
    {
        private readonly IRepository<Models.UserInfo> _userRep;
        private readonly TokenHelper _tokenHelper;
        public UserService(IRepository<Models.UserInfo> _userRep, TokenHelper _tokenHelper)
        {
            this._userRep = _userRep;
            this._tokenHelper = _tokenHelper;
        }

        public async Task<ResponseVo<object>> Login(InputUserModel userInfo)
        {
            var res =await Task.Run(() =>
            {
                if (userInfo == null || string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Pwd))
                {
                    return Fail(null, "用户名或密码不能为空！");
                }
                else
                {
                    bool isExist = _userRep.DetachedEntities.Any(i => i.UserName == userInfo.UserName && i.Pwd == userInfo.Pwd);
                    if (isExist)
                    {
                        string token = _tokenHelper.CreateJwtToken(userInfo);
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("token", token);
                        return Success(dic, "登录成功！");
                    }
                    else
                    {
                        return Fail(null, "用户名或密码错误！");
                    }
                }
            });
            return Success(res);
        }

        public async Task<ResponseVo<object>> Register(UserInfo userInfo)
        {
            var res = await Task.Run(() =>
            {
                if (userInfo == null || string.IsNullOrEmpty(userInfo.UserName)||string.IsNullOrEmpty(userInfo.Pwd))
                {
                    return Fail(null, "用户名或密码不能为空！");
                }
                 bool isExist = _userRep.DetachedEntities.Any(i => i.UserName == userInfo.UserName);
                if (isExist)
                {
                    return Fail(null, "用户名重复！");
                }
                else
                {
                    var handler= PreInsertHandler(userInfo);
                    var addRes = _userRep.InsertNow(handler);
                    if (addRes >= 1)
                    {
                        return Success(null, "注册成功！");
                    }
                    else
                    {
                        return Fail(null, "注册失败！");
                    }
                }
            });
            return res;
        }

        public async Task<ResponseVo<object>> SingleUserInfo(string id)
        {
            var res = await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Fail(null, "id不能为空！");
                }
                else
                {
                    _ = int.TryParse(id,out int intId);
                    var userInfo = _userRep.DetachedEntities.Where(i => i.ID == intId).FirstOrDefault();
                    return Success(userInfo);
                }
            });
            return res;
        }

        public async Task<ResponseVo<object>> Test()
        {
            //var list = extraDbHelper.Get();
            //list.Where(i => i.DbName == "Db1").FirstOrDefault();
            MySqlParameter[] test = { new MySqlParameter("@ID", 1) };
            var test2 = _userRep.GetDbContext().SqlQuery("select * from user_info where ID=@ID", test);
            var test3 = _userRep.SqlQuery("select * from user_info where ID=@ID", test);
            System.Transactions.TransactionScope dbTransaction = new System.Transactions.TransactionScope();
            dbTransaction.Complete();
            dbTransaction.Dispose();

            var res = await Task.Run(() =>
            {
                //var test = _userRep.DetachedEntities;
                return test;
            });
            return Success(res);
        }
    }
}
