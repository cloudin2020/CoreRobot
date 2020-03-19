using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRobot.Model
{
    public class Projects
    {
        private string mProjectName;
        private string mCreateDate;
        private string mFrameworkVersion;
        private string mVersion;
        private int mCode;
        private string mServer;
        private string mDBName;
        private string mDBUser;
        private string mDBPassword;
        private string mCharset;
        private string mCompany;
        private string mAuthor;
        private string mGUIDDBSQLHelper;
        private string mGUIDModel;
        private string mGUIDDAL;
        private string mGUIDBLL;
        private string mGUIDUtility;
        private string mGUIDSMS;
        private string mGUIDPush;
        private string mGUIDPay;


        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get
            {
                return mProjectName;
            }
            set
            {
                mProjectName = value;
            }
        }

        /// <summary>
        /// 项目创建日期
        /// </summary>
        public string CreateDate
        {
            get
            {
                return mCreateDate;
            }
            set
            {
                mCreateDate = value;
            }
        }

        /// <summary>
        /// Visual Studio .Net Framework 版本
        /// </summary>
        public string FrameworkVersion
        {
            get
            {
                return mFrameworkVersion;
            }
            set
            {
                mFrameworkVersion = value;
            }
        }

        /// <summary>
        /// 版本号，如：1.0
        /// </summary>
        public string Version
        {
            get
            {
                return mVersion;
            }
            set
            {
                mVersion = value;
            }
        }

        /// <summary>
        /// 版本代号：如1,2,3,4
        /// </summary>
        public int Code
        {
            get
            {
                return mCode;
            }
            set
            {
                mCode = value;
            }
        }

        /// <summary>
        /// 数据库：数据库连接地址
        /// </summary>
        public string Server
        {
            get
            {
                return mServer;
            }
            set
            {
                mServer = value;
            }
        }

        /// <summary>
        /// 数据库：数据库名称
        /// </summary>
        public string DBName
        {
            get
            {
                return mDBName;
            }
            set
            {
                mDBName = value;
            }
        }

        /// <summary>
        /// 数据库：登录用户名
        /// </summary>
        public string DBUser
        {
            get
            {
                return mDBUser;
            }
            set
            {
                mDBUser = value;
            }
        }

        /// <summary>
        /// 数据库：登录密码
        /// </summary>
        public string DBPassword
        {
            get
            {
                return mDBPassword;
            }
            set
            {
                mDBPassword = value;
            }
        }

        /// <summary>
        /// 数据库：字符集
        /// </summary>
        public string Charset
        {
            get
            {
                return mCharset;
            }
            set
            {
                mCharset = value;
            }
        }

        /// <summary>
        /// 版权：公司名称
        /// </summary>
        public string Company
        {
            get
            {
                return mCompany;
            }
            set
            {
                mCompany = value;
            }
        }

        /// <summary>
        /// 版权：作者
        /// </summary>
        public string Author
        {
            get
            {
                return mAuthor;
            }
            set
            {
                mAuthor = value;
            }
        }

        /// <summary>
        /// 数据接口项目GUID
        /// </summary>
        public string GUIDDBSQLHelper
        {
            get
            {
                return mGUIDDBSQLHelper;
            }
            set
            {
                mGUIDDBSQLHelper = value;
            }
        }

        /// <summary>
        /// 数据模型GUID
        /// </summary>
        public string GUIDModel
        {
            get
            {
                return mGUIDModel;
            }
            set
            {
                mGUIDModel = value;
            }
        }

        /// <summary>
        /// 数据处理项目GUID
        /// </summary>
        public string GUIDDAL
        {
            get
            {
                return mGUIDDAL;
            }
            set
            {
                mGUIDDAL = value;
            }
        }

        /// <summary>
        /// 业务逻辑处理项目GUID
        /// </summary>
        public string GUIDBLL
        {
            get
            {
                return mGUIDBLL;
            }
            set
            {
                mGUIDBLL = value;
            }
        }

        /// <summary>
        /// 工具类封装项目GUID
        /// </summary>
        public string GUIDUtility
        {
            get
            {
                return mGUIDUtility;
            }
            set
            {
                mGUIDUtility = value;
            }
        }

        /// <summary>
        /// 短信项目GUID
        /// </summary>
        public string GUIDSMS
        {
            get
            {
                return mGUIDSMS;
            }
            set
            {
                mGUIDSMS = value;
            }
        }

        /// <summary>
        /// 推送项目GUID
        /// </summary>
        public string GUIDPush
        {
            get
            {
                return mGUIDPush;
            }
            set
            {
                mGUIDPush = value;
            }
        }

        /// <summary>
        /// 支付项目GUID
        /// </summary>
        public string GUIDPay
        {
            get
            {
                return mGUIDPay;
            }
            set
            {
                mGUIDPay = value;
            }
        }
    }
}
