
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace LYB
{
    /// <summary>
    /// lyb 的摘要说明
    /// 留言板主界面显示
    /// </summary>
    public class lyb : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            string FileExt = "";     //檢查扩展名
            int intFileSize = 0;     //檢查大小(KB)
            if (action == "AddNew")
            {
                string title = context.Request["subject"];
                string msg = context.Request["content"];
                string ipAddress = context.Request.UserHostAddress;
                HttpPostedFile imgfile= context.Request.Files["imgcontent"];
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
                                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(imgfile.FileName);//有bug的，一毫秒内多个人上传多个文件
                                imgfile.SaveAs(context.Server.MapPath("~/imgcontent/" + filename));
                                filename = "imgcontent/" + filename;
                                int count = SqlHelper.ExecuteNonQuery("Insert into Lyb(name,title,msg,date,IPAddress,img)values(@Name,@Title,@Msg,GetDate(),@ipAddress,@img)", new SqlParameter("@Name", context.Session["User"].ToString()), new SqlParameter("@Title", title), new SqlParameter("@Msg", msg), new SqlParameter("@ipAddress", ipAddress), new SqlParameter("@img", filename));
                                if(count==1)
                                {
                                    context.Response.Write("<script>alert('发送成功');location.href='lyb.ashx?action=login';</script>");
                                }
                            }
                        }
                        else
                        {
                            context.Response.Write("<script>alert('发送失败图片过大请在1M以内');location.href='lyb.ashx?action=login';</script>");
                        }
                    }
                    else if (imgfile.InputStream.Length ==0)
                    {
                        int count = SqlHelper.ExecuteNonQuery("Insert into Lyb(name,title,msg,date,IPAddress)values(@Name,@Title,@Msg,GetDate(),@ipAddress)", new SqlParameter("@Name", context.Session["User"].ToString()), new SqlParameter("@Title", title), new SqlParameter("@Msg", msg), new SqlParameter("@ipAddress", ipAddress));
                        if (count == 1)
                        {
                            context.Response.Write("<script>alert('发送成功');location.href='lyb.ashx?action=login';</script>");
                        }
                        else
                        {
                            context.Response.Write("<script>alert('发送失败');location.href='lyb.ashx?action=login';</script>");
                        }
                    }
                }
            }
            else if(action=="login")
            {
                if (context.Session["User"] != null)
                {
                    string username = context.Session["User"].ToString();
                    if (!string.IsNullOrEmpty(username))
                    {
                        DataTable da = SqlHelper.ExecuteDataTable("select Top(10)* from Lyb order by id desc");
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from Users where name=@Name", new SqlParameter("@Name", username));
                        DataRow row = dt.Rows[0];
                        var data = new { das = da.Rows, name = username, dts = row["iconimg"], jj = row["Resume"] };
                        string html = CommonHelper.RenderHtml("lyb.html", data);
                        context.Response.Write(html);
                    }
                }
                else
                {
                    DataTable da = SqlHelper.ExecuteDataTable("select Top(10)* from Lyb order by id desc");
                    var data = new { das = da.Rows, name = "" };
                    string html = CommonHelper.RenderHtml("lyb.html", data);
                    context.Response.Write(html);
                }
            }
            else
            {
                DataTable da = SqlHelper.ExecuteDataTable("select Top(10)* from Lyb order by id desc");
                var data = new { das = da.Rows, name = "" };
                string html = CommonHelper.RenderHtml("lyb.html", data);
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