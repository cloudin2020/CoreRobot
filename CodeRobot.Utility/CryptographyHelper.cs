using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CodeRobot.Utility
{
    /// <summary>
    /// 版权所有：版权所有(C) 2018，Cloudin
    /// 内容摘要：常用工具封装
    /// 完成日期：2018年01月14日
    /// 版    本：V1.0 
    /// 作    者：Adin Lee
    /// </summary>
    public static class CryptographyHelper
    {
        /// <summary>
        /// 加密内容
        /// </summary>
        /// <param name="strContent">需要加密的内容</param>
        /// <param name="strKey">密钥</param>
        /// <returns></returns>
        public static string Encryption(string strContent, string strKey)
        {
            CspParameters param = new CspParameters();
            param.Flags = CspProviderFlags.UseMachineKeyStore;
            param.KeyContainerName = strKey;//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(strContent);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
        }

        /// <summary>
        /// 解密内容
        /// </summary>
        /// <param name="strContent">需要解密的加密内容</param>
        /// <param name="strKey">密钥</param>
        /// <returns></returns>
        public static string Decrypt(string strContent, string strKey)
        {
            CspParameters param = new CspParameters();
            param.Flags = CspProviderFlags.UseMachineKeyStore;
            param.KeyContainerName = strKey;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, param))
            {
                byte[] encryptdata = Convert.FromBase64String(strContent);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return Encoding.Default.GetString(decryptdata);
            }
        }
    }
}
