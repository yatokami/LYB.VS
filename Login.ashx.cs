using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace LYB
{
    /// <summary>
    /// Login 的摘要说明
    /// 用户登录操作
    /// </summary>
    public class Login : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string username = context.Request["UserName"].ToString() + "";
            string password = context.Request["Password"].ToString() + "";
            int count = (int)SqlHelper.ExecuteScalar("select count(*) from Users where name=@UserName and password=@PassWord", new SqlParameter("@UserName", username),new SqlParameter("@PassWord",password));
            if(count==1)
            {
                context.Session["User"] = username;
                context.Response.Write("OK");
            }
            else if(count<=0)
            {
                context.Response.Write("NO");
            }
            else
            {
                context.Response.Write("error");
            }
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