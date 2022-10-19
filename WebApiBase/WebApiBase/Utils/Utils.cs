using WebApiBase.Models;

namespace WebApiBase.Utils
{
    public class Utils
    {
        private static IConfiguration Configuration { get; set; }
        public Utils(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string GetAppSetting(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }
            return "";
        }

        /// <summary>
        /// 处理数据库类型
        /// </summary>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static (DataBaseType dbType, string providerName) DbTypeHandler(string strType)
        {
            DataBaseType dbType = DataBaseType.MySql;
            string providerName = "";
            if (strType.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                dbType = DataBaseType.Oracle;
                providerName = "Oracle";
            }
            else if (strType.IndexOf("SqlServer", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                dbType = DataBaseType.SqlServer;
                providerName = "SqlServer";
            }
            else if (strType.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                dbType = DataBaseType.MySql;
                providerName = "MySql";
            }
            return (dbType, providerName);
        }
    }
}
