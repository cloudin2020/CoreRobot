namespace CodeRobot.APP
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.showMsg = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbTableName = new System.Windows.Forms.TextBox();
            this.cbMP = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbWeb = new System.Windows.Forms.CheckBox();
            this.cbiPhone = new System.Windows.Forms.CheckBox();
            this.cbAndroid = new System.Windows.Forms.CheckBox();
            this.cbH5 = new System.Windows.Forms.CheckBox();
            this.cbMange = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtiOSPackage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAndroidPackage = new System.Windows.Forms.TextBox();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbList = new System.Windows.Forms.ComboBox();
            this.txtCharset = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.btnDocument = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbJava = new System.Windows.Forms.CheckBox();
            this.cbCore = new System.Windows.Forms.CheckBox();
            this.cbUniApp = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(169, 265);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(149, 50);
            this.btnStop.TabIndex = 75;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStart.Location = new System.Drawing.Point(12, 265);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(149, 50);
            this.btnStart.TabIndex = 74;
            this.btnStart.Text = "生成代码";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // showMsg
            // 
            this.showMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showMsg.Location = new System.Drawing.Point(12, 323);
            this.showMsg.Margin = new System.Windows.Forms.Padding(13, 4, 13, 12);
            this.showMsg.Name = "showMsg";
            this.showMsg.Size = new System.Drawing.Size(1021, 364);
            this.showMsg.TabIndex = 73;
            this.showMsg.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbUniApp);
            this.groupBox1.Controls.Add(this.cbCore);
            this.groupBox1.Controls.Add(this.cbJava);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tbTableName);
            this.groupBox1.Controls.Add(this.cbMP);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbWeb);
            this.groupBox1.Controls.Add(this.cbiPhone);
            this.groupBox1.Controls.Add(this.cbAndroid);
            this.groupBox1.Controls.Add(this.cbH5);
            this.groupBox1.Controls.Add(this.cbMange);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtiOSPackage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtAndroidPackage);
            this.groupBox1.Controls.Add(this.txtProjectName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(16, 138);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1019, 119);
            this.groupBox1.TabIndex = 81;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "代码保存设置";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(780, 87);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 15);
            this.label11.TabIndex = 104;
            this.label11.Text = "指定表名：";
            // 
            // tbTableName
            // 
            this.tbTableName.Location = new System.Drawing.Point(870, 80);
            this.tbTableName.Margin = new System.Windows.Forms.Padding(4);
            this.tbTableName.Name = "tbTableName";
            this.tbTableName.Size = new System.Drawing.Size(113, 25);
            this.tbTableName.TabIndex = 103;
            // 
            // cbMP
            // 
            this.cbMP.AutoSize = true;
            this.cbMP.Location = new System.Drawing.Point(241, 83);
            this.cbMP.Name = "cbMP";
            this.cbMP.Size = new System.Drawing.Size(74, 19);
            this.cbMP.TabIndex = 102;
            this.cbMP.Text = "小程序";
            this.cbMP.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 83);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 15);
            this.label9.TabIndex = 101;
            this.label9.Text = "生成代码：";
            // 
            // cbWeb
            // 
            this.cbWeb.AutoSize = true;
            this.cbWeb.Location = new System.Drawing.Point(321, 82);
            this.cbWeb.Name = "cbWeb";
            this.cbWeb.Size = new System.Drawing.Size(53, 19);
            this.cbWeb.TabIndex = 100;
            this.cbWeb.Text = "WEB";
            this.cbWeb.UseVisualStyleBackColor = true;
            // 
            // cbiPhone
            // 
            this.cbiPhone.AutoSize = true;
            this.cbiPhone.Location = new System.Drawing.Point(445, 82);
            this.cbiPhone.Name = "cbiPhone";
            this.cbiPhone.Size = new System.Drawing.Size(53, 19);
            this.cbiPhone.TabIndex = 99;
            this.cbiPhone.Text = "iOS";
            this.cbiPhone.UseVisualStyleBackColor = true;
            // 
            // cbAndroid
            // 
            this.cbAndroid.AutoSize = true;
            this.cbAndroid.Location = new System.Drawing.Point(380, 83);
            this.cbAndroid.Name = "cbAndroid";
            this.cbAndroid.Size = new System.Drawing.Size(59, 19);
            this.cbAndroid.TabIndex = 98;
            this.cbAndroid.Text = "安卓";
            this.cbAndroid.UseVisualStyleBackColor = true;
            // 
            // cbH5
            // 
            this.cbH5.AutoSize = true;
            this.cbH5.Location = new System.Drawing.Point(193, 82);
            this.cbH5.Name = "cbH5";
            this.cbH5.Size = new System.Drawing.Size(45, 19);
            this.cbH5.TabIndex = 97;
            this.cbH5.Text = "H5";
            this.cbH5.UseVisualStyleBackColor = true;
            // 
            // cbMange
            // 
            this.cbMange.AutoSize = true;
            this.cbMange.Location = new System.Drawing.Point(128, 83);
            this.cbMange.Name = "cbMange";
            this.cbMange.Size = new System.Drawing.Size(59, 19);
            this.cbMange.TabIndex = 96;
            this.cbMange.Text = "后台";
            this.cbMange.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(689, 42);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 15);
            this.label8.TabIndex = 95;
            this.label8.Text = "iOS包名：";
            // 
            // txtiOSPackage
            // 
            this.txtiOSPackage.Location = new System.Drawing.Point(784, 39);
            this.txtiOSPackage.Margin = new System.Windows.Forms.Padding(4);
            this.txtiOSPackage.Name = "txtiOSPackage";
            this.txtiOSPackage.Size = new System.Drawing.Size(199, 25);
            this.txtiOSPackage.TabIndex = 94;
            this.txtiOSPackage.Text = "com.cloudin.app";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(356, 42);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 15);
            this.label3.TabIndex = 93;
            this.label3.Text = "Android包名：";
            // 
            // txtAndroidPackage
            // 
            this.txtAndroidPackage.Location = new System.Drawing.Point(472, 39);
            this.txtAndroidPackage.Margin = new System.Windows.Forms.Padding(4);
            this.txtAndroidPackage.Name = "txtAndroidPackage";
            this.txtAndroidPackage.Size = new System.Drawing.Size(199, 25);
            this.txtAndroidPackage.TabIndex = 92;
            this.txtAndroidPackage.Text = "com.cloudin.app";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(128, 39);
            this.txtProjectName.Margin = new System.Windows.Forms.Padding(4);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(199, 25);
            this.txtProjectName.TabIndex = 91;
            this.txtProjectName.Text = "Test";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 45);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 90;
            this.label7.Text = "项目名称：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.cbList);
            this.groupBox2.Controls.Add(this.txtCharset);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtDBName);
            this.groupBox2.Controls.Add(this.txtUserID);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtServer);
            this.groupBox2.Location = new System.Drawing.Point(16, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(1019, 115);
            this.groupBox2.TabIndex = 82;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MySQL数据库连接字符串设置";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(689, 35);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 15);
            this.label10.TabIndex = 91;
            this.label10.Text = "历史项目：";
            // 
            // cbList
            // 
            this.cbList.FormattingEnabled = true;
            this.cbList.Location = new System.Drawing.Point(784, 30);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(199, 23);
            this.cbList.TabIndex = 90;
            this.cbList.SelectedIndexChanged += new System.EventHandler(this.cbList_SelectedIndexChanged);
            // 
            // txtCharset
            // 
            this.txtCharset.Location = new System.Drawing.Point(784, 70);
            this.txtCharset.Margin = new System.Windows.Forms.Padding(4);
            this.txtCharset.Name = "txtCharset";
            this.txtCharset.Size = new System.Drawing.Size(199, 25);
            this.txtCharset.TabIndex = 89;
            this.txtCharset.Text = "utf8";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(689, 76);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 88;
            this.label6.Text = "字符集：";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(451, 70);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(199, 25);
            this.txtPassword.TabIndex = 87;
            this.txtPassword.Text = "123456";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(356, 76);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 86;
            this.label4.Text = "登录密码：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(356, 35);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 85;
            this.label5.Text = "数据库名：";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(451, 30);
            this.txtDBName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(199, 25);
            this.txtDBName.TabIndex = 84;
            this.txtDBName.Text = "TestDB";
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(128, 70);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(199, 25);
            this.txtUserID.TabIndex = 83;
            this.txtUserID.Text = "root";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 82;
            this.label2.Text = "登录用户名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 81;
            this.label1.Text = "数据库地址：";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(128, 30);
            this.txtServer.Margin = new System.Windows.Forms.Padding(4);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(199, 25);
            this.txtServer.TabIndex = 80;
            this.txtServer.Text = "127.0.0.1";
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // btnDocument
            // 
            this.btnDocument.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocument.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocument.Location = new System.Drawing.Point(886, 265);
            this.btnDocument.Margin = new System.Windows.Forms.Padding(4);
            this.btnDocument.Name = "btnDocument";
            this.btnDocument.Size = new System.Drawing.Size(149, 50);
            this.btnDocument.TabIndex = 83;
            this.btnDocument.Text = "生成数据库文档";
            this.btnDocument.UseVisualStyleBackColor = true;
            this.btnDocument.Click += new System.EventHandler(this.btnDocument_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(785, 292);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 84;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(697, 292);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 85;
            this.button1.Text = "格式化";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbJava
            // 
            this.cbJava.AutoSize = true;
            this.cbJava.Checked = true;
            this.cbJava.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbJava.Location = new System.Drawing.Point(654, 83);
            this.cbJava.Name = "cbJava";
            this.cbJava.Size = new System.Drawing.Size(61, 19);
            this.cbJava.TabIndex = 105;
            this.cbJava.Text = "JAVA";
            this.cbJava.UseVisualStyleBackColor = true;
            // 
            // cbCore
            // 
            this.cbCore.AutoSize = true;
            this.cbCore.Location = new System.Drawing.Point(504, 83);
            this.cbCore.Name = "cbCore";
            this.cbCore.Size = new System.Drawing.Size(61, 19);
            this.cbCore.TabIndex = 106;
            this.cbCore.Text = "CORE";
            this.cbCore.UseVisualStyleBackColor = true;
            // 
            // cbUniApp
            // 
            this.cbUniApp.AutoSize = true;
            this.cbUniApp.Location = new System.Drawing.Point(571, 83);
            this.cbUniApp.Name = "cbUniApp";
            this.cbUniApp.Size = new System.Drawing.Size(77, 19);
            this.cbUniApp.TabIndex = 107;
            this.cbUniApp.Text = "UniApp";
            this.cbUniApp.UseVisualStyleBackColor = true;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 701);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnDocument);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.showMsg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "小码机器人";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RichTextBox showMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.TextBox txtCharset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label label7;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAndroidPackage;
        private System.Windows.Forms.CheckBox cbWeb;
        private System.Windows.Forms.CheckBox cbiPhone;
        private System.Windows.Forms.CheckBox cbAndroid;
        private System.Windows.Forms.CheckBox cbH5;
        private System.Windows.Forms.CheckBox cbMange;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtiOSPackage;
        private System.Windows.Forms.Button btnDocument;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbList;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.CheckBox cbMP;
        private System.Windows.Forms.TextBox tbTableName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbJava;
        private System.Windows.Forms.CheckBox cbCore;
        private System.Windows.Forms.CheckBox cbUniApp;
    }
}