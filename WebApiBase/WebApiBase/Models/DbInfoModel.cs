namespace WebApiBase.Models
{
    public class DbInfoModel
    {
        /// <summary>
        /// 数据库自定义指定的名称
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType Type { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStrings { get; set; }
        /// <summary>
        /// 是否为外部数据库
        /// </summary>
        public bool IsExtraDb { get; set; }
    }
    public enum DataBaseType
    {
        SqlServer,
        MySql,
        Oracle,
        SQLite
    }
}
