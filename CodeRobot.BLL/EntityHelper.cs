using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CodeRobot.APP
{
    public class EntityHelper
    {
        public static string strConnectionStr = "Server=rm-bp1273l4xvr9864ur.mysql.rds.aliyuncs.com;Database=paxy;Uid=paxy365;Pwd=Paxy53344521;CharSet=utf8";

        public static List<Entity> GetEntities(List<string> databases)
        {
            var list = new List<Entity>();
            var conn = new MySqlConnection(strConnectionStr);
            try
            {
                conn.Open();
                var dbs = string.Join("','", databases.ToArray());
                var cmd = string.Format(@"SELECT `information_schema`.`COLUMNS`.`TABLE_SCHEMA`
                                                      ,`information_schema`.`COLUMNS`.`TABLE_NAME`
                                                      ,`information_schema`.`COLUMNS`.`COLUMN_NAME`
                                                      ,`information_schema`.`COLUMNS`.`DATA_TYPE`
                                                      ,`information_schema`.`COLUMNS`.`COLUMN_COMMENT`
                                                  FROM `information_schema`.`COLUMNS`
                                                  WHERE `information_schema`.`COLUMNS`.`TABLE_SCHEMA` IN ('{}') ", dbs);
                using (var reader = MySqlHelper.ExecuteReader(conn, cmd))
                {
                    while (reader.Read())
                    {
                        var db = reader["TABLE_SCHEMA"].ToString();
                        var table = reader["TABLE_NAME"].ToString();
                        var column = reader["COLUMN_NAME"].ToString();
                        var type = reader["DATA_TYPE"].ToString();
                        var comment = reader["COLUMN_COMMENT"].ToString();
                        var entity = list.FirstOrDefault(x => x.EntityName == table);
                        if (entity == null)
                        {
                            entity = new Entity(table);
                            entity.Fields.Add(new Field
                            {
                                Name = column,
                                Type = GetCLRType(type),
                                Comment = comment
                            });

                            list.Add(entity);
                        }
                        else
                        {
                            entity.Fields.Add(new Field
                            {
                                Name = column,
                                Type = GetCLRType(type),
                                Comment = comment
                            });
                        }
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            return list;
        }

        public static string GetCLRType(string dbType)
        {
            switch (dbType)
            {
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "int":
                case "integer":
                    return "int";
                case "double":
                    return "double";
                case "float":
                    return "float";
                case "decimal":
                    return "decimal";
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
                    return dbType;
            }
        }


        public class Entity
        {
            public Entity()
            {
                this.Fields = new List<Field>();
            }

            public Entity(string name)
                : this()
            {
                this.EntityName = name;
            }

            public string EntityName { get; set; }
            public List<Field> Fields { get; set; }
        }

        public class Field
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Comment { get; set; }
        }
    }
}
