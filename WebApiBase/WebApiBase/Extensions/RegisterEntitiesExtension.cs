using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApiBase.DatabaseAccessor;
using WebApiBase.Models;

namespace WebApiBase.Extensions
{
    public static class RegisterEntitiesExtension
    {
        /// <summary>
        /// 迁移时注册程序集中所有实体
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RegisterEntities(this ModelBuilder modelBuilder, Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            Type[] types = assembly.GetTypes();

            List<Type> list = new List<Type>();

            foreach (Type item in types)
            {
                if (item.IsInterface)
                { 
                    continue;//判断是否是接口
                }
                Type[] ins = item.GetInterfaces();
                foreach (Type ty in ins)
                {
                    if (ty == typeof(IEntity))
                    {
                        list.Add(item);
                    }
                }
            }

            foreach (var item in list)
            {
                modelBuilder.Entity(item);
            }
            
        }
    }
}
