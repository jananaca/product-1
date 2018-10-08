using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using 投票系统.Entity;

namespace 投票系统.SYS
{
    public class app
    {
        public static string username { get; set; }
        public static  string password { get; set; }
        public static bool CheckLogin()
        {
            if (!string.IsNullOrWhiteSpace(app.username))
            {
                string username = app.username;
                string password = app.password;
                EF context = new EF();
                users User = context.users.FirstOrDefault(a => a.UserName == username);
                byte[] result = Encoding.Default.GetBytes(password);    //tbPass为输入密码的文本框
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string passwordYZ = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
                if (User == null)
                {
                    return false;
                }
                else if (User.PassWord != passwordYZ.ToLower())
                {
                    return false;
                }           
                return true;
            }
            return false;
        }
    }
}