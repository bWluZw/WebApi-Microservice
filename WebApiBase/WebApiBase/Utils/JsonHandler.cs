using Newtonsoft.Json;
using WebApiBase.Models;

namespace WebApiBase.Utils
{
    public static class JsonHandler
    {
        public static string Success(object data,string msg = "操作成功！")
        {
            ResponseVo<object> vo = new ResponseVo<object>();
            vo.Data = data;
            vo.Code = 200;
            vo.Message = msg;
            vo.Type = ResponseVo<object>.ResType.Success;
            return JsonConvert.SerializeObject(vo);
        }

        public static string Fail(object data = null, string msg = "操作失败!",int code = 400)
        {
            ResponseVo<object> vo = new ResponseVo<object>();
            vo.Data = data;
            vo.Code = code;
            vo.Message = msg;
            vo.Type = ResponseVo<object>.ResType.Error;
            return JsonConvert.SerializeObject(vo);
        }
    }
}
