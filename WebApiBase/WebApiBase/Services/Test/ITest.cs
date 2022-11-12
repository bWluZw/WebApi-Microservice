using WebApiBase.Models;

namespace WebApiBase.Services.Test
{
    public interface ITest
    {
        public Task<ResponseVo<object>> NacosTest();
        public Task<ResponseVo<object>> CallNacosService();
    }
}
