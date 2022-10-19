using Newtonsoft.Json;
using System.Reflection;
using WebApiBase.Models;

namespace WebApiBase.Services
{
    public class BaseService
    {
        public static ResponseVo<object> Success(object data, string msg = "操作成功！")
        {
            ResponseVo<object> vo = new ResponseVo<object>();
            if (data == null)
            {
                data = "null";
            }
            vo.Data = data;
            vo.Code = 200;
            vo.Message = msg;
            vo.Type = ResponseVo<object>.ResType.Success;
            return vo;
        }

        public static ResponseVo<object> Fail(object data = null, string msg = "操作失败!", int code = 400)
        {
            ResponseVo<object> vo = new ResponseVo<object>();
            if (data == null)
            {
                data = "null";
            }
            vo.Data = data;
            vo.Code = code;
            vo.Message = msg;
            vo.Type = ResponseVo<object>.ResType.Error;
            return vo;
        }

        /// <summary>
        /// insert前处理操作
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity PreInsertHandler<TEntity>(TEntity entity)
        {
            Type type = entity.GetType();


            PropertyInfo propDr = type.GetProperty("CreationTime");
            if (propDr != null)
            {
                propDr.SetValue(entity, DateTime.Now);
            }


            return entity;
        }

        /// <summary>
        /// update前处理操作
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity PreUpdateHandler<TEntity>(TEntity entity)
        {
            Type type = entity.GetType();


            PropertyInfo propDr = type.GetProperty("ModifyTime");
            if (propDr != null)
            {
                propDr.SetValue(entity, DateTime.Now);
            }

            return entity;
        }
    }
}
