# CodeRobot.APP

С�������
����Ϊ���ʡ20%�Ĵ�����ʱ��

####��ʵ�ֹ���:
```
1. �Զ��������ݿ�������
2. �Զ��������ݿ��У������Ӧ��ʵ����
3. �Զ��������ݿ��У������Ӧ�����ݴ�����
4. �Զ��������ݿ��У������Ӧ��ҵ������
5. �Զ����� Restful API ҵ���߼��ӿ�
6. �Զ����� HTML5 ���װ���������
7. �Զ����� Android ����ҵ���߼�����


```

***
���ݿ����ע������
1. ���б����ͼƬ��������Ϊ cover_img
2. ���б�ı����������Ϊ"����_title"���� news_title,video_title

***

####��һ�汾ʵ�ֹ���:
```
1. �Զ����� Object C ����ҵ���߼�����

```

####������־��
```
2017-03-28(Adin)

1. �޸��ش�BUG���޸������д���null����ʱ����ȡ���ֶ������쳣
2. 

2017-7-20(Adin)
1. ����ģ�飺���ڵ�����Ч����ѡ������
2. ����ɾ��ģ�������⣬ҳ�漰�ӿڶ���Ҫ����


```

### **ע������**

1. ����������ɱ��ֶ����ظ��������ݿ��д�������һ�������ݿ⣬��ʱ����Ϊ�˲��Դ�����û��ɾ��

http://www.cnblogs.com/zachary93/p/6106829.html


### **���������**
*
˵������һ���޸���ֻ�����string�ֶ�������Ч������int���͵ı��ֶ��׳������쳣
��¼ʱ�䣺2017-07-13 14:11:41,218 
�߳�ID:[5] 
��־����  ERROR 
�����ࣺMZH.DAL.UsersDAL property: [(null)] - 
���������������Զ���������ѯ����һ�����ݣ�SQL��䣺SELECT user_id,country_code,user_phone,user_avatar,user_password,nick_name,user_gender,user_type,user_status,user_lat,user_lng,last_loginedat,user_slogan,user_from,introduction,user_job,company_name,user_email,user_qq,mobile_phone,user_address,province_id,city_id,district_id,town_id,school_id,grade_id,class_id,team_id,id_card,student_no,user_point,created_at  FROM mzh_users  WHERE user_phone='18518769198' 
 
System.InvalidCastException: �����ܴ� DBNull ת��Ϊ�������͡�
   �� System.DBNull.System.IConvertible.ToInt32(IFormatProvider provider)
   �� System.Convert.ToInt32(Object value)
   �� MZH.DAL.UsersDAL.SelectOneByWhere(String strWhere) λ�� D:\�ٶ���ͬ����\Work\Adin\MinZhiHui\MZH.DAL\UsersDAL.cs:�к� 319
*

�Ż���

�޸�roleid=2&rank=2&code=12400
����û����༭ÿ�еĳ���

ͼƬ�ɵ��
����type_name ��������Ҫ�Զ���ȡ����

��ֵ��ѯ �ĳ� ��ֵ�б�

2019-12-08
1. ����SQL
2. ����ժҪ������+ע��

2020-02-03
ɾ���˼��ܽ����߼������ǳ�һЩĪ�������⣬�ر��ڸ������ԣ���װϵͳʱ����



2020-03-17
1��-����created_at��Ҫ��ʼ��һ��
2��-�����������Ҫ��ӡLOG
3��-bigint��Ӧ�ĸ�����Ҫ����һ��
4��-created_at��һ��Ĭ��ֵ
5��select new ����Ҫ���11
