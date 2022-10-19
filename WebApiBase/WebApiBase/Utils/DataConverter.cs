using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace WebApiBase.Utils
{
    public static class DataConverter
    {

        /// <summary>
        /// DataTable转换为List
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="dt">datatable表</param>
        /// <returns>返回list集合</returns>
        public static List<T> TableToList<T>(DataTable dt) where T : new()
        {
            //定义集合
            List<T> list = new List<T>();
            //获得此模型的类型
            Type type = typeof(T);
            //定义一个临时变量
            string tempName = string.Empty;
            //遍历Datatable中所有的数据行
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                //获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性
                foreach (PropertyInfo pi in propertys)
                {
                    //将属性名称赋值给临时变量
                    tempName = pi.Name;
                    //检查DataTable是否包含此列（列名==对象的属性名）
                    if (dt.Columns.Contains(tempName))
                    {
                        //判断此属性是否有Setter
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出
                        //取值
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性
                        if (value != DBNull.Value)
                        {
                            //加一重if判断，如果属性值是int32类型的，就进行一次强制转换
                            if (pi.GetMethod.ReturnParameter.ParameterType.Name == "Int32")
                            {
                                value = Convert.ToInt32(value);
                            }
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 将 DbDataReader 转 DataTable
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this DbDataReader dataReader)
        {
            var dataTable = new DataTable();
            // 创建列
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                var dataClumn = new DataColumn
                {
                    DataType = dataReader.GetFieldType(i),
                    ColumnName = dataReader.GetName(i)
                };

                dataTable.Columns.Add(dataClumn);
            }
            // 循环读取
            while (dataReader.Read())
            {
                // 创建行
                var dataRow = dataTable.NewRow();
                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    dataRow[i] = dataReader[i];
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        /// <summary>
        /// 将 DbDataReader 转 DataSet
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static DataSet ToDataSet(this DbDataReader dataReader)
        {
            var dataSet = new DataSet();

            do
            {
                // 获取元数据
                var schemaTable = dataReader.GetSchemaTable();
                var dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (var i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        var dataRow = schemaTable.Rows[i];

                        var columnName = (string)dataRow["ColumnName"];
                        var column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // 循环读取
                    while (dataReader.Read())
                    {
                        var dataRow = dataTable.NewRow();

                        for (var i = 0; i < dataReader.FieldCount; i++)
                        {
                            dataRow[i] = dataReader.GetValue(i);
                        }

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    var column = new DataColumn("RecordsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);

                    var dataRow = dataTable.NewRow();
                    dataRow[0] = dataReader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }

            // 读取下一个结果
            while (dataReader.NextResult());

            return dataSet;
        }



    }
}
