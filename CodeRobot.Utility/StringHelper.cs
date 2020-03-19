using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRobot.Utility
{
    /// <summary>
    /// 字符处理
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 根据MySQL数据类型转换成C#中的数据类型
        /// </summary>
        /// <param name="strDBType">数据类型</param>
        /// <returns></returns>
        public static string GetCSharpDBType(string strDBType)
        {
            switch (strDBType)
            {
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "int":
                case "integer":
                    return "int";
                case "bigint":
                    return "long";
                case "double":
                    return "double";
                case "float":
                    return "double";//float有问题
                case "decimal":
                    return "double";
                case "numeric":
                case "real":
                    return "decimal";
                case "bit":
                    return "bool";
                case "date":
                case "time":
                case "year":
                case "datetime":
                case "timestamp":
                    return "DateTime";
                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblog":
                case "binary":
                case "varbinary":
                    return "byte[]";
                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    return "string";
                case "point":
                case "linestring":
                case "polygon":
                case "geometry":
                case "multipoint":
                case "multilinestring":
                case "multipolygon":
                case "geometrycollection":
                case "enum":
                case "set":
                default:
                    return strDBType;
            }
        }

    }
}
