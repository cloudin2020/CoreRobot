# CodeRobot.Utility

###数据处理类

demo:
```
/// <summary>
/// 新增一条数据
/// </summary>
/// <param name="userModel">实体类</param>
/// <returns></returns>
public static bool InsertUser(UserModel userModel)
{
	UserDAL userDAL = new UserDAL();

	return userDAL.InsertUser(userModel);
}

```

####引用程序集说明：

MySql.Data //连接MySQL数据库
System.Windows.Forms //调用WinForm封装类库

####引用第三方组件：（工具-》NuGet包管理-》程序包管理控制台）


####引用项目说明：

CodeRobot.Utility //常用工具类封装
CodeRobot.DBSqlHelper //数据连接封装

