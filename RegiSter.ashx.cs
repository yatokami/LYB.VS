using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.IO;

namespace LYB
{
    /// <summary>
    /// RegiSter 的摘要说明
    /// 完成用户信息注册和更新
    /// </summary>
    public class RegiSter : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["action"] + "";
            string username = context.Request["account"] + "";
            string password = context.Request["password"] + "";
            string confirmpassword = context.Request["confirmpassword"] + "";
            string yzm = context.Request["YZM"] + "";
            if (action == "look")
            {
                var data = new { user = "", pass = "", hidden = 1 };
                string html = CommonHelper.RenderHtml("RegiSter.html", data);
                context.Response.Write(html);
            }
            else if (action == "codes")
            {
                string yzm1 = context.Session["Code"].ToString();
                if (yzm.ToUpper() == yzm1.ToUpper())
                {
                    context.Response.Write("OK");
                }
                else if (yzm.ToUpper() != yzm1.ToUpper())
                {
                    context.Response.Write("NO");
                }
                else
                {
                    context.Response.Write("Unexpected situation");
                }
            }
            else if (action == "users")
            {
                int count = (int)SqlHelper.ExecuteScalar("select count(*) from Users where name=@UserName", new SqlParameter("@UserName", username));
                if (count <= 0)
                {
                    context.Response.Write("OK");
                }
                else
                {
                    context.Response.Write("NO");
                }
            }
            else if (action == "edit")
            {
                if (username != "")
                {
                    int count = (int)SqlHelper.ExecuteScalar("select count(*) from Users where name=@UserName", new SqlParameter("@UserName", username));
                    if (count <= 0)
                    {
                        string yzm1 = context.Session["Code"].ToString();
                        if (yzm.ToUpper() == yzm1.ToUpper())
                        {
                            if (password == confirmpassword)
                            {
                                int zc = SqlHelper.ExecuteNonQuery("Insert into Users(name,password,date)values(@name,@password,GetDate())", new SqlParameter("@name", username), new SqlParameter("@password", password));
                                context.Response.Write("<script>alert('注册成功');location.href='lyb.ashx';</script>");
                            }
                            else
                            {
                                var data = new { user = username, pass = password, hidden = 3 };
                                string html = CommonHelper.RenderHtml("RegiSter.html", data);
                                context.Response.Write(html);
                            }
                        }
                        else
                        {
                            var data = new { user = username, pass = password, hidden = 4 };
                            string html = CommonHelper.RenderHtml("RegiSter.html", data);
                            context.Response.Write(html);
                        }
                    }
                    else
                    {
                        var data = new { user = "", pass = "", hidden = 2 };
                        string html = CommonHelper.RenderHtml("RegiSter.html", data);
                        context.Response.Write(html);
                    }
                }
                else
                {
                    var data = new { user = "", pass = "", hidden = 1 };
                    string html = CommonHelper.RenderHtml("Registers.html", data);
                    context.Response.Write(html);
                }
            }
            else if (action == "AddNew")
            {
                string FileExt = "";     //檢查扩展名
                int intFileSize = 0;     //檢查大小(KB)
                HttpPostedFile imgfile = context.Request.Files["iconimg"];
                string name = context.Session["User"].ToString();
                string Resume = context.Request["content"].ToString();
                if (imgfile != null)
                {
                    intFileSize = (imgfile.ContentLength / 1024);
                    if (imgfile.InputStream.Length != 0)
                    {
                        if (imgfile.InputStream.Length < 1048576)
                        {
                            FileExt = Path.GetFileNameWithoutExtension(imgfile.FileName);
                            FileExt = Path.GetExtension(imgfile.FileName);
                            if (FileExt.Equals(".jpg") || FileExt.Equals(".jpeg") || FileExt.Equals(".png") || FileExt.Equals(".gif") || FileExt.Equals(".bmp"))
                            {
                                string filename = name + Path.GetExtension(imgfile.FileName);//有bug的，一毫秒内多个人上传多个文件
                                imgfile.SaveAs(context.Server.MapPath("~/icon-img/" + filename));
                                filename = "icon-img/" + filename;
                                int count = SqlHelper.ExecuteNonQuery("update Users set iconimg=@Icon,Resume=@Resume where name=@Name ", new SqlParameter("@Icon", filename), new SqlParameter("@Resume", Resume), new SqlParameter("@Name", name));
                                if (count == 1)
                                {
                                    context.Response.Write("<script>alert('信息成功');location.href='lyb.ashx?action=login';</script>");
                                }
                            }
                        }
                        else
                        {
                            context.Response.Write("<script>alert('上传头像失败图片过大请在1M以内');location.href='lyb.ashx?action=login';</script>");
                        }
                    }
                    else
                    {
                        int count = SqlHelper.ExecuteNonQuery("update Users set Resume=@Resume where name=@Name ", new SqlParameter("@Resume", Resume), new SqlParameter("@Name", name));
                        if (count == 1)
                        {
                            context.Response.Write("<script>alert('信息成功');location.href='lyb.ashx?action=login';</script>");
                        }
                    }
                  
                }
                else
                {
                    int count = SqlHelper.ExecuteNonQuery("update Users set ,Resume=@Resume where name=@Name ", new SqlParameter("@Resume", Resume), new SqlParameter("@Name", name));
                    if (count == 1)
                    {
                        context.Response.Write("<script>alert('信息成功');location.href='lyb.ashx?action=login';</script>");
                    }
                }
            }
            else if(action=="exit")
            {
                context.Session["User"] =null;
                context.Response.Write("<script>alert('成功');location.href='lyb.ashx?action=login';</script>");
            }
            else
            {
                var data = new { user = "", pass = "", hidden = 1 };
                string html = CommonHelper.RenderHtml("Registers.html", data);
                context.Response.Write(html);
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