using WebApiBase.Models;

namespace WebApiBase.Services.Test
{
    public interface ITest
    {
        public Task<ResponseVo<string>> NacosTest();
    }
}
