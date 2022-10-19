using AutoMapper;
using System.Linq.Expressions;
namespace WebApiBase
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// AutoMapper的默认配置,不进行任何字段替换操作
        /// </summary>
        /// <typeparam name="T1">源数据类型</typeparam>
        /// <typeparam name="T2">目标类型</typeparam>
        /// <param name="ob"></param>
        /// <param name="source">数据源(待转换的数据)</param>
        /// <returns></returns>
        public static T2 Mapper<T1, T2>(this T1 ob) where T1 : new() where T2 : new()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T1, T2>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<T1, T2>(ob);
        }


        //private static dynamic Mapper(DataView dataView,InputDataViewModel inputDataViewModel)
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<DataView, InputDataViewModel>().ForMember(i=>i.VNName,j=>j.MapFrom(src=>src.VNName));
        //    });

        //    IMapper mapper = config.CreateMapper();

        //    return mapper.Map<DataView, InputDataViewModel>(dataView);
        //}


    }
}
