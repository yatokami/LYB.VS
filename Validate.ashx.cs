using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Web.SessionState;
using System.Web.Configuration;

namespace LYB
{
    /// <summary>
    /// verify_code 的摘要说明
    /// 验证码显示
    /// </summary>
    public class verify_code : IHttpHandler,IRequiresSessionState
    {
        public verify_code()
        {

        }

        public void ProcessRequest(HttpContext context)
        {
            string checkCode = GetValidation(5);  // 产生5位随机验证码字符
            context.Session["Code"] = checkCode; //将字符串保存到Session中，以便需要时进行验证
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(70, 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器

                Random random = new Random();

                //清空图片背景色

                g.Clear(Color.White);

                // 画图片的背景噪音线

                int i;
                for (i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点

                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 随即获取验证码

        /// </summary>
        /// <param name="num">位数</param>
        /// <returns>返回验证码</returns>
        public string GetValidation(int num)
        {
            string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //"或者写汉字也行"
            string validatecode = "";
            Random rd = new Random();
            for (int i = 0; i < num; i++)
            {
                validatecode += str.Substring(rd.Next(0, str.Length), 1);
            }
            return validatecode;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}