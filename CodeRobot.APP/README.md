# CodeRobot.APP

小码机器人
——为你节省20%的代码编程时间

####已实现功能:
```
1. 自动生成数据库连接类
2. 自动生成数据库中，各表对应的实体类
3. 自动生成数据库中，各表对应的数据处理类
4. 自动生成数据库中，各表对应的业务处理类
5. 自动生成 Restful API 业务逻辑接口
6. 自动生成 HTML5 简易版完整代码
7. 自动生成 Android 基础业务逻辑代码


```

***
数据库设计注意事项
1. 所有表封面图片必须命名为 cover_img
2. 所有表的标题必须命名为"表名_title"，如 news_title,video_title

***

####下一版本实现功能:
```
1. 自动生成 Object C 基础业务逻辑代码

```

####升级日志：
```
2017-03-28(Adin)

1. 修复重大BUG：修复当表中存在null数据时，读取表字段数据异常
2. 

2017-7-20(Adin)
1. 搜索模块：日期当日无效、单选有问题
2. 批量删除模块有问题，页面及接口都需要调整


```

### **注意问题**

1. 如果发现生成表字段有重复，则数据库中存在两个一样的数据库，当时可能为了测试创建，没有删除

http://www.cnblogs.com/zachary93/p/6106829.html


### **待解决问题**
*
说明：上一次修复的只是针对string字段类型有效，但是int类型的表字段抛出如下异常
记录时间：2017-07-13 14:11:41,218 
线程ID:[5] 
日志级别：  ERROR 
出错类：MZH.DAL.UsersDAL property: [(null)] - 
错误描述：根据自定义条件查询表中一条数据，SQL语句：SELECT user_id,country_code,user_phone,user_avatar,user_password,nick_name,user_gender,user_type,user_status,user_lat,user_lng,last_loginedat,user_slogan,user_from,introduction,user_job,company_name,user_email,user_qq,mobile_phone,user_address,province_id,city_id,district_id,town_id,school_id,grade_id,class_id,team_id,id_card,student_no,user_point,created_at  FROM mzh_users  WHERE user_phone='18518769198' 
 
System.InvalidCastException: 对象不能从 DBNull 转换为其他类型。
   在 System.DBNull.System.IConvertible.ToInt32(IFormatProvider provider)
   在 System.Convert.ToInt32(Object value)
   在 MZH.DAL.UsersDAL.SelectOneByWhere(String strWhere) 位置 D:\百度云同步盘\Work\Adin\MinZhiHui\MZH.DAL\UsersDAL.cs:行号 319
*

优化：

修复roleid=2&rank=2&code=12400
添加用户、编辑每行的长度

图片可点击
除了type_name 其他表需要自动获取名称

充值查询 改成 充值列表

2019-12-08
1. 导出SQL
2. 内容摘要：表名+注释

2020-02-03
删除了加密解密逻辑，老是出一些莫名的问题，特别在更换电脑，重装系统时出现



2020-03-17
1、-新增created_at需要初始化一下
2、-新增或更新需要打印LOG
3、-bigint对应的更改需要调整一下
4、-created_at给一个默认值
5、select new 不需要左侧11
